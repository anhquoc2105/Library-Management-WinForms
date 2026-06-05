using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using QuanLyThuVien.DTO;
using QuanLyThuVien.UTILS;

namespace QuanLyThuVien.DAL
{
    public class PhieuMuonDAL
    {
        private bool TraPhieuNoiBo(
            SqlConnection connection,
            SqlTransaction transaction,
            int maPhieu,
            List<int> danhSachMaSach,
            decimal tienPhatMoiNgay,
            out decimal tienPhatKyNay,
            out string thongBao)
        {
            const string selectPhieuQuery = @"
                SELECT
                    pm.NgayPhaiTra,
                    ct.MaSach
                FROM PhieuMuon pm
                INNER JOIN ChiTietPM ct ON pm.MaPhieu = ct.MaPhieu
                WHERE pm.MaPhieu = @MaPhieu
                  AND ct.NgayTra IS NULL
                  AND ct.MaSach IN ({0})";

            List<int> danhSachMaSachKhongTrung = danhSachMaSach.Distinct().ToList();
            string parameterNames = string.Join(",", danhSachMaSachKhongTrung.Select((_, index) => "@MaSach" + index));
            string selectQuery = string.Format(selectPhieuQuery, parameterNames);

            List<int> sachCanTra = new List<int>();
            DateTime? ngayPhaiTra = null;

            using (SqlCommand command = new SqlCommand(selectQuery, connection, transaction))
            {
                command.Parameters.AddWithValue("@MaPhieu", maPhieu);
                for (int i = 0; i < danhSachMaSachKhongTrung.Count; i++)
                {
                    command.Parameters.AddWithValue("@MaSach" + i, danhSachMaSachKhongTrung[i]);
                }

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!ngayPhaiTra.HasValue)
                        {
                            ngayPhaiTra = Convert.ToDateTime(reader["NgayPhaiTra"]);
                        }

                        sachCanTra.Add(Convert.ToInt32(reader["MaSach"]));
                    }
                }
            }

            if (!ngayPhaiTra.HasValue || sachCanTra.Count == 0)
            {
                tienPhatKyNay = 0;
                thongBao = "Sách đã được trả hoặc không tồn tại trong phiếu mượn.";
                return false;
            }

            DateTime ngayTra = DateTime.Today;
            int soNgayTre = ngayTra > ngayPhaiTra.Value ? (ngayTra - ngayPhaiTra.Value).Days : 0;
            decimal tienPhatMoiSach = soNgayTre * tienPhatMoiNgay;
            tienPhatKyNay = tienPhatMoiSach * sachCanTra.Count;

            const string updateChiTietQueryTemplate = @"
                UPDATE ChiTietPM
                SET NgayTra = @NgayTra,
                    TienPhatKyNay = @TienPhatMoiSach
                WHERE MaPhieu = @MaPhieu
                  AND NgayTra IS NULL
                  AND MaSach IN ({0})";

            using (SqlCommand command = new SqlCommand(string.Format(updateChiTietQueryTemplate, parameterNames), connection, transaction))
            {
                command.Parameters.AddWithValue("@NgayTra", ngayTra);
                command.Parameters.AddWithValue("@TienPhatMoiSach", tienPhatMoiSach);
                command.Parameters.AddWithValue("@MaPhieu", maPhieu);
                for (int i = 0; i < danhSachMaSachKhongTrung.Count; i++)
                {
                    command.Parameters.AddWithValue("@MaSach" + i, danhSachMaSachKhongTrung[i]);
                }

                if (command.ExecuteNonQuery() != sachCanTra.Count)
                {
                    thongBao = "Không thể cập nhật trạng thái trả sách.";
                    return false;
                }
            }

            const string updatePhieuQuery = @"
                UPDATE PhieuMuon
                SET NgayTra =
                    CASE
                        WHEN NOT EXISTS
                        (
                            SELECT 1
                            FROM ChiTietPM
                            WHERE MaPhieu = @MaPhieu
                              AND NgayTra IS NULL
                        )
                        THEN @NgayTra
                        ELSE NULL
                    END,
                    TienPhatKyNay =
                    (
                        SELECT ISNULL(SUM(TienPhatKyNay), 0)
                        FROM ChiTietPM
                        WHERE MaPhieu = @MaPhieu
                    )
                WHERE MaPhieu = @MaPhieu";

            using (SqlCommand command = new SqlCommand(updatePhieuQuery, connection, transaction))
            {
                command.Parameters.AddWithValue("@NgayTra", ngayTra);
                command.Parameters.AddWithValue("@MaPhieu", maPhieu);
                command.ExecuteNonQuery();
            }

            const string dongBoTienPhatQuery = @"
                UPDATE PhieuMuon
                SET TienPhatKyNay =
                (
                    SELECT ISNULL(SUM(TienPhatKyNay), 0)
                    FROM ChiTietPM
                    WHERE MaPhieu = @MaPhieu
                )
                WHERE MaPhieu = @MaPhieu;

                ;WITH AffectedDocGia AS
                (
                    SELECT MaDG
                    FROM PhieuMuon
                    WHERE MaPhieu = @MaPhieu
                )
                UPDATE dg
                SET TongNo =
                    CASE
                        WHEN ISNULL(f.TongTienPhat, 0) - ISNULL(pt.TongTienThu, 0) < 0 THEN 0
                        ELSE ISNULL(f.TongTienPhat, 0) - ISNULL(pt.TongTienThu, 0)
                    END
                FROM DocGia dg
                INNER JOIN AffectedDocGia adg ON dg.MaDG = adg.MaDG
                OUTER APPLY
                (
                    SELECT SUM(ct.TienPhatKyNay) AS TongTienPhat
                    FROM PhieuMuon pm
                    INNER JOIN ChiTietPM ct ON pm.MaPhieu = ct.MaPhieu
                    WHERE pm.MaDG = dg.MaDG
                ) f
                OUTER APPLY
                (
                    SELECT SUM(SoTienThu) AS TongTienThu
                    FROM PhieuThuTienPhat p
                    WHERE p.MaDG = dg.MaDG
                ) pt;";

            using (SqlCommand command = new SqlCommand(dongBoTienPhatQuery, connection, transaction))
            {
                command.Parameters.AddWithValue("@MaPhieu", maPhieu);
                command.ExecuteNonQuery();
            }

            thongBao = "Trả sách thành công. Tiền phạt kỳ này: " + tienPhatKyNay.ToString("N0") + "đ";
            return true;
        }

        private DataRow LayThamSo(SqlConnection connection, SqlTransaction transaction)
        {
            const string query = @"
                SELECT TOP 1
                    SoSachMuonToiDa,
                    SoNgayMuonToiDa,
                    TienPhat
                FROM ThamSo
                WHERE MaTheLoai IS NULL
                ORDER BY MaThamSo";

            using (SqlCommand command = new SqlCommand(query, connection, transaction))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable.Rows[0];
            }
        }

        public bool LapPhieuMuon(int maDG, int maSach, out string thongBao)
        {
            using (SqlConnection connection = DbHelper.GetConnection())
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        DataRow thamSo = LayThamSo(connection, transaction);
                        int soSachMuonToiDa = Convert.ToInt32(thamSo["SoSachMuonToiDa"]);

                        const string checkDocGiaQuery = @"
                            SELECT NgayHetHan, TongNo
                            FROM DocGia
                            WHERE MaDG = @MaDG";

                        DateTime? ngayHetHan = null;
                        decimal tongNo = 0;

                        using (SqlCommand command = new SqlCommand(checkDocGiaQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@MaDG", maDG);
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (!reader.Read())
                                {
                                    thongBao = "Không tìm thấy độc giả.";
                                    transaction.Rollback();
                                    return false;
                                }

                                ngayHetHan = reader["NgayHetHan"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["NgayHetHan"]);
                                tongNo = Convert.ToDecimal(reader["TongNo"]);
                            }
                        }

                        if (ngayHetHan == null || ngayHetHan.Value.Date < DateTime.Today)
                        {
                            thongBao = "Thẻ độc giả đã hết hạn.";
                            transaction.Rollback();
                            return false;
                        }

                        if (tongNo > 0)
                        {
                            thongBao = "Độc giả đang còn nợ tiền phạt.";
                            transaction.Rollback();
                            return false;
                        }

                        const string checkQuaHanQuery = @"
                            SELECT COUNT(*)
                            FROM PhieuMuon pm
                            INNER JOIN ChiTietPM ct ON pm.MaPhieu = ct.MaPhieu
                            WHERE pm.MaDG = @MaDG
                              AND ct.NgayTra IS NULL
                              AND pm.NgayPhaiTra < CAST(GETDATE() AS DATE)";

                        using (SqlCommand command = new SqlCommand(checkQuaHanQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@MaDG", maDG);
                            int soPhieuQuaHan = Convert.ToInt32(command.ExecuteScalar());
                            if (soPhieuQuaHan > 0)
                            {
                                thongBao = "Độc giả đang có sách mượn quá hạn.";
                                transaction.Rollback();
                                return false;
                            }
                        }

                        const string checkDangMuonQuery = @"
                            SELECT COUNT(*)
                            FROM PhieuMuon pm
                            INNER JOIN ChiTietPM ct ON pm.MaPhieu = ct.MaPhieu
                            WHERE pm.MaDG = @MaDG
                              AND ct.NgayTra IS NULL";

                        using (SqlCommand command = new SqlCommand(checkDangMuonQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@MaDG", maDG);
                            int soSachDangMuon = Convert.ToInt32(command.ExecuteScalar());
                            if (soSachDangMuon >= soSachMuonToiDa)
                            {
                                thongBao = "Độc giả đã mượn tối đa " + soSachMuonToiDa + " quyển.";
                                transaction.Rollback();
                                return false;
                            }
                        }

                        const string checkSachTrungQuery = @"
                            SELECT COUNT(*)
                            FROM PhieuMuon pm
                            INNER JOIN ChiTietPM ct ON pm.MaPhieu = ct.MaPhieu
                            WHERE pm.MaDG = @MaDG
                              AND ct.MaSach = @MaSach
                              AND ct.NgayTra IS NULL";

                        using (SqlCommand command = new SqlCommand(checkSachTrungQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@MaDG", maDG);
                            command.Parameters.AddWithValue("@MaSach", maSach);
                            int soLanDangMuonSachNay = Convert.ToInt32(command.ExecuteScalar());
                            if (soLanDangMuonSachNay > 0)
                            {
                                thongBao = "Độc giả đang mượn quyển sách này, không thể mượn trùng.";
                                transaction.Rollback();
                                return false;
                            }
                        }

                        const string checkSachQuery = @"
                            SELECT TinhTrang, SoLuongTon
                            FROM Sach
                            WHERE MaSach = @MaSach";

                        string tinhTrang;
                        int soLuongTon;

                        using (SqlCommand command = new SqlCommand(checkSachQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@MaSach", maSach);
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (!reader.Read())
                                {
                                    thongBao = "Không tìm thấy sách.";
                                    transaction.Rollback();
                                    return false;
                                }

                                tinhTrang = reader["TinhTrang"].ToString();
                                soLuongTon = Convert.ToInt32(reader["SoLuongTon"]);
                            }
                        }

                        if (!string.Equals(tinhTrang, "Con", StringComparison.OrdinalIgnoreCase) || soLuongTon <= 0)
                        {
                            thongBao = "Sách hiện không sẵn sàng để mượn.";
                            transaction.Rollback();
                            return false;
                        }

                        const string insertPhieuQuery = @"
                            INSERT INTO PhieuMuon (MaDG, NgayMuon, NgayTra, TienPhatKyNay)
                            VALUES (@MaDG, GETDATE(), NULL, 0);
                            SELECT CAST(SCOPE_IDENTITY() AS INT);";

                        int maPhieu;
                        using (SqlCommand command = new SqlCommand(insertPhieuQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@MaDG", maDG);
                            maPhieu = (int)command.ExecuteScalar();
                        }

                        const string soLanMuonQuery = @"
                            SELECT ISNULL(MAX(SoLanMuon), 0) + 1
                            FROM ChiTietPM
                            WHERE MaSach = @MaSach";

                        int soLanMuon;
                        using (SqlCommand command = new SqlCommand(soLanMuonQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@MaSach", maSach);
                            soLanMuon = Convert.ToInt32(command.ExecuteScalar());
                        }

                        const string insertChiTietQuery = @"
                            INSERT INTO ChiTietPM (MaPhieu, MaSach, NgayThang, SoLanMuon)
                            VALUES (@MaPhieu, @MaSach, GETDATE(), @SoLanMuon)";

                        using (SqlCommand command = new SqlCommand(insertChiTietQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@MaPhieu", maPhieu);
                            command.Parameters.AddWithValue("@MaSach", maSach);
                            command.Parameters.AddWithValue("@SoLanMuon", soLanMuon);
                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        thongBao = "Lập phiếu mượn thành công.";
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        thongBao = "Không thể lập phiếu mượn. " + ex.Message;
                        return false;
                    }
                }
            }
        }

        public DataTable LayLichSuMuonTheoMaTaiKhoan(int maTaiKhoan)
        {
            const string query = @"
                ;WITH LichSuMuon AS
                (
                    SELECT
                        pm.MaPhieu,
                        s.TenSach,
                        pm.NgayMuon,
                        pm.NgayPhaiTra,
                        ct.NgayTra,
                        ct.TienPhatKyNay,
                        CASE
                            WHEN ct.NgayTra IS NULL THEN N'Đang mượn'
                            ELSE N'Đã trả'
                        END AS TrangThaiMuon
                    FROM DocGia dg
                    INNER JOIN PhieuMuon pm ON dg.MaDG = pm.MaDG
                    INNER JOIN ChiTietPM ct ON pm.MaPhieu = ct.MaPhieu
                    INNER JOIN Sach s ON ct.MaSach = s.MaSach
                    WHERE dg.MaTaiKhoan = @MaTaiKhoan
                )
                SELECT
                    MIN(MaPhieu) AS MaPhieu,
                    TenSach,
                    NgayMuon,
                    NgayPhaiTra,
                    NgayTra,
                    TienPhatKyNay,
                    TrangThaiMuon,
                    COUNT(*) AS SoLanMuon
                FROM LichSuMuon
                GROUP BY
                    TenSach,
                    NgayMuon,
                    NgayPhaiTra,
                    NgayTra,
                    TienPhatKyNay,
                    TrangThaiMuon
                ORDER BY MAX(MaPhieu) DESC, TenSach";

            return DbHelper.ExecuteQuery(query, new SqlParameter("@MaTaiKhoan", maTaiKhoan));
        }

        public DataTable LayDanhSachSachDangMuon()
        {
            const string query = @"
                SELECT
                    pm.MaPhieu,
                    dg.MaDG,
                    dg.TenDG,
                    dg.TongNo,
                    s.MaSach,
                    s.TenSach,
                    pm.NgayMuon,
                    pm.NgayPhaiTra,
                    ct.NgayTra,
                    ct.TienPhatKyNay,
                    DATEDIFF(DAY, pm.NgayMuon, CAST(GETDATE() AS DATE)) AS SoNgayMuon,
                    CASE
                        WHEN CAST(GETDATE() AS DATE) > pm.NgayPhaiTra
                        THEN DATEDIFF(DAY, pm.NgayPhaiTra, CAST(GETDATE() AS DATE)) *
                             (
                                 SELECT TOP 1 TienPhat
                                 FROM ThamSo
                                 WHERE MaTheLoai IS NULL
                                 ORDER BY MaThamSo
                             )
                        ELSE 0
                    END AS TienPhatDuKien,
                    CASE
                        WHEN s.TinhTrang = N'Con' THEN N'Còn'
                        WHEN s.TinhTrang = N'Dang muon' THEN N'Đang mượn'
                        WHEN s.TinhTrang = N'Hong' THEN N'Hỏng'
                        WHEN s.TinhTrang = N'Mat' THEN N'Mất'
                        ELSE s.TinhTrang
                    END AS TinhTrang
                FROM PhieuMuon pm
                INNER JOIN DocGia dg ON pm.MaDG = dg.MaDG
                INNER JOIN ChiTietPM ct ON pm.MaPhieu = ct.MaPhieu
                INNER JOIN Sach s ON ct.MaSach = s.MaSach
                WHERE ct.NgayTra IS NULL
                ORDER BY pm.MaPhieu, s.TenSach";

            return DbHelper.ExecuteQuery(query);
        }

        public bool TraSach(int maPhieu, int maSach, out string thongBao)
        {
            List<ChiTietPMDTO> danhSachTra = new List<ChiTietPMDTO>
            {
                new ChiTietPMDTO
                {
                    MaPhieu = maPhieu,
                    MaSach = maSach
                }
            };

            return TraNhieuSach(danhSachTra, out thongBao);
        }

        public bool TraNhieuSach(List<ChiTietPMDTO> danhSachTra, out string thongBao)
        {
            if (danhSachTra == null || danhSachTra.Count == 0)
            {
                thongBao = "Danh sách sách cần trả không hợp lệ.";
                return false;
            }

            List<ChiTietPMDTO> danhSachKhongTrung = danhSachTra
                .GroupBy(item => new { item.MaPhieu, item.MaSach })
                .Select(group => group.First())
                .ToList();

            using (SqlConnection connection = DbHelper.GetConnection())
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        DataRow thamSo = LayThamSo(connection, transaction);
                        decimal tienPhatMoiNgay = Convert.ToDecimal(thamSo["TienPhat"]);
                        decimal tongTienPhatKyNay = 0;

                        foreach (var nhomPhieu in danhSachKhongTrung.GroupBy(item => item.MaPhieu))
                        {
                            decimal tienPhatKyNay;
                            string thongBaoLoi;
                            List<int> danhSachMaSach = nhomPhieu.Select(item => item.MaSach).ToList();

                            bool thanhCong = TraPhieuNoiBo(
                                connection,
                                transaction,
                                nhomPhieu.Key,
                                danhSachMaSach,
                                tienPhatMoiNgay,
                                out tienPhatKyNay,
                                out thongBaoLoi);

                            if (!thanhCong)
                            {
                                transaction.Rollback();
                                thongBao = thongBaoLoi;
                                return false;
                            }

                            tongTienPhatKyNay += tienPhatKyNay;
                        }

                        transaction.Commit();
                        thongBao = "Trả " + danhSachKhongTrung.Count + " sách thành công. Tổng tiền phạt kỳ này: " + tongTienPhatKyNay.ToString("N0") + "đ";
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        thongBao = "Không thể trả sách. " + ex.Message;
                        return false;
                    }
                }
            }
        }
    }
}
