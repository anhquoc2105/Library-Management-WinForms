using System;
using System.Data;
using System.Data.SqlClient;
using QuanLyThuVien.DTO;
using QuanLyThuVien.UTILS;

namespace QuanLyThuVien.DAL
{
    public class DocGiaDAL
    {
        public DataTable LayDanhSachDocGia()
        {
            const string query = @"
                SELECT
                    ROW_NUMBER() OVER (ORDER BY MaDG) AS MaDGHienThi,
                    MaDG,
                    TenDG,
                    LoaiDG,
                    NgaySinhDG,
                    DiaChiDG,
                    EmailDG,
                    NgLapThe,
                    NgayHetHan,
                    TongNo,
                    MaTaiKhoan,
                    CASE
                        WHEN NgayHetHan IS NULL THEN N'Chưa có hạn thẻ'
                        WHEN NgayHetHan < CAST(GETDATE() AS DATE) THEN N'Hết hạn'
                        ELSE N'Còn hạn'
                    END AS TrangThaiThe
                FROM DocGia
                ORDER BY MaDG";

            return DbHelper.ExecuteQuery(query);
        }

        public DataTable LayDanhSachDocGiaChoCombo()
        {
            const string query = @"
                SELECT
                    MaDG,
                    TenDG
                FROM DocGia
                ORDER BY TenDG";

            return DbHelper.ExecuteQuery(query);
        }

        public DataTable LayThongTinTheoMaTaiKhoan(int maTaiKhoan)
        {
            const string query = @"
                SELECT
                    MaDG,
                    TenDG,
                    LoaiDG,
                    NgaySinhDG,
                    DiaChiDG,
                    EmailDG,
                    NgLapThe,
                    NgayHetHan,
                    TongNo,
                    MaTaiKhoan
                FROM DocGia
                WHERE MaTaiKhoan = @MaTaiKhoan";

            return DbHelper.ExecuteQuery(query, new SqlParameter("@MaTaiKhoan", maTaiKhoan));
        }

        public bool ThemDocGia(DocGiaDTO docGia, out string thongBao)
        {
            const string checkEmailQuery = @"
                SELECT COUNT(*)
                FROM DocGia
                WHERE EmailDG = @EmailDG";

            object emailCount = DbHelper.ExecuteScalar(
                checkEmailQuery,
                new SqlParameter(
                    "@EmailDG",
                    string.IsNullOrWhiteSpace(docGia.EmailDG) ? (object)DBNull.Value : docGia.EmailDG));

            if (emailCount != null && Convert.ToInt32(emailCount) > 0)
            {
                thongBao = "Email da ton tai.";
                return false;
            }

            const string insertTaiKhoanQuery = @"
                INSERT INTO TaiKhoan (TenDangNhap, MatKhau)
                VALUES (@TenDangNhap, @MatKhau);

                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            const string insertDocGiaQuery = @"
                INSERT INTO DocGia
                (
                    TenDG,
                    LoaiDG,
                    NgaySinhDG,
                    DiaChiDG,
                    EmailDG,
                    NgLapThe,
                    TongNo,
                    MaTaiKhoan
                )
                VALUES
                (
                    @TenDG,
                    @LoaiDG,
                    @NgaySinhDG,
                    @DiaChiDG,
                    @EmailDG,
                    @NgLapThe,
                    @TongNo,
                    @MaTaiKhoan
                )";

            using (SqlConnection connection = DbHelper.GetConnection())
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int maTaiKhoanDuKien = LayMaTaiKhoanTiepTheo(connection, transaction);
                        string tenDangNhap = TaoTenDangNhapDocGia(maTaiKhoanDuKien);
                        const string matKhauMacDinh = "123456";

                        int maTaiKhoan;
                        using (SqlCommand insertTaiKhoanCommand = new SqlCommand(insertTaiKhoanQuery, connection, transaction))
                        {
                            insertTaiKhoanCommand.Parameters.AddWithValue("@TenDangNhap", tenDangNhap);
                            insertTaiKhoanCommand.Parameters.AddWithValue("@MatKhau", matKhauMacDinh);
                            maTaiKhoan = Convert.ToInt32(insertTaiKhoanCommand.ExecuteScalar());
                        }

                        using (SqlCommand insertDocGiaCommand = new SqlCommand(insertDocGiaQuery, connection, transaction))
                        {
                            insertDocGiaCommand.Parameters.AddWithValue("@TenDG", docGia.TenDG);
                            insertDocGiaCommand.Parameters.AddWithValue("@LoaiDG", docGia.LoaiDG);
                            insertDocGiaCommand.Parameters.AddWithValue("@NgaySinhDG", docGia.NgaySinhDG);
                            insertDocGiaCommand.Parameters.AddWithValue(
                                "@DiaChiDG",
                                string.IsNullOrWhiteSpace(docGia.DiaChiDG) ? (object)DBNull.Value : docGia.DiaChiDG);
                            insertDocGiaCommand.Parameters.AddWithValue(
                                "@EmailDG",
                                string.IsNullOrWhiteSpace(docGia.EmailDG) ? (object)DBNull.Value : docGia.EmailDG);
                            insertDocGiaCommand.Parameters.AddWithValue("@NgLapThe", docGia.NgLapThe);
                            insertDocGiaCommand.Parameters.AddWithValue("@TongNo", docGia.TongNo);
                            insertDocGiaCommand.Parameters.AddWithValue("@MaTaiKhoan", maTaiKhoan);
                            insertDocGiaCommand.ExecuteNonQuery();
                        }

                        transaction.Commit();

                        thongBao =
                            "Lap the doc gia thanh cong.\n" +
                            "Tai khoan dang nhap: " + tenDangNhap + "\n" +
                            "Mat khau mac dinh: " + matKhauMacDinh;
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        thongBao = "Không thể lập thẻ độc giả. Chi tiết: " + ex.Message;
                        return false;
                    }
                }
            }
        }

        public bool CapNhatDocGia(DocGiaDTO docGia, out string thongBao)
        {
            const string checkEmailQuery = @"
                SELECT COUNT(*)
                FROM DocGia
                WHERE EmailDG = @EmailDG
                  AND MaDG <> @MaDG";

            object emailCount = DbHelper.ExecuteScalar(
                checkEmailQuery,
                new SqlParameter(
                    "@EmailDG",
                    string.IsNullOrWhiteSpace(docGia.EmailDG) ? (object)DBNull.Value : docGia.EmailDG),
                new SqlParameter("@MaDG", docGia.MaDG));

            if (emailCount != null && Convert.ToInt32(emailCount) > 0)
            {
                thongBao = "Email đã tồn tại.";
                return false;
            }

            const string updateQuery = @"
                UPDATE DocGia
                SET TenDG = @TenDG,
                    LoaiDG = @LoaiDG,
                    NgaySinhDG = @NgaySinhDG,
                    DiaChiDG = @DiaChiDG,
                    EmailDG = @EmailDG,
                    NgLapThe = @NgLapThe
                WHERE MaDG = @MaDG";

            int rows = DbHelper.ExecuteNonQuery(
                updateQuery,
                new SqlParameter("@TenDG", docGia.TenDG),
                new SqlParameter("@LoaiDG", docGia.LoaiDG),
                new SqlParameter("@NgaySinhDG", docGia.NgaySinhDG),
                new SqlParameter(
                    "@DiaChiDG",
                    string.IsNullOrWhiteSpace(docGia.DiaChiDG) ? (object)DBNull.Value : docGia.DiaChiDG),
                new SqlParameter(
                    "@EmailDG",
                    string.IsNullOrWhiteSpace(docGia.EmailDG) ? (object)DBNull.Value : docGia.EmailDG),
                new SqlParameter("@NgLapThe", docGia.NgLapThe),
                new SqlParameter("@MaDG", docGia.MaDG));

            thongBao = rows > 0 ? "Cập nhật thẻ độc giả thành công." : "Không tìm thấy độc giả cần cập nhật.";
            return rows > 0;
        }

        public bool GiaHanThe(int maDG, out string thongBao)
        {
            const string query = @"
                UPDATE DocGia
                SET NgLapThe = CAST(GETDATE() AS DATE)
                WHERE MaDG = @MaDG";

            int rows = DbHelper.ExecuteNonQuery(query, new SqlParameter("@MaDG", maDG));

            thongBao = rows > 0 ? "Gia hạn thẻ độc giả thành công." : "Không tìm thấy độc giả cần gia hạn.";
            return rows > 0;
        }

        public bool XoaDocGia(int maDG, out string thongBao)
        {
            const string selectTaiKhoanQuery = "SELECT MaTaiKhoan FROM DocGia WHERE MaDG = @MaDG";
            const string deleteDocGiaQuery = "DELETE FROM DocGia WHERE MaDG = @MaDG";
            const string deleteTaiKhoanQuery = "DELETE FROM TaiKhoan WHERE MaTaiKhoan = @MaTaiKhoan";

            using (SqlConnection connection = DbHelper.GetConnection())
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        object maTaiKhoanValue;
                        using (SqlCommand selectTaiKhoanCommand = new SqlCommand(selectTaiKhoanQuery, connection, transaction))
                        {
                            selectTaiKhoanCommand.Parameters.AddWithValue("@MaDG", maDG);
                            maTaiKhoanValue = selectTaiKhoanCommand.ExecuteScalar();
                        }

                        if (maTaiKhoanValue == null || maTaiKhoanValue == DBNull.Value)
                        {
                            transaction.Rollback();
                            thongBao = "Không tìm thấy độc giả cần xóa.";
                            return false;
                        }

                        int rows;
                        using (SqlCommand deleteDocGiaCommand = new SqlCommand(deleteDocGiaQuery, connection, transaction))
                        {
                            deleteDocGiaCommand.Parameters.AddWithValue("@MaDG", maDG);
                            rows = deleteDocGiaCommand.ExecuteNonQuery();
                        }

                        if (rows == 0)
                        {
                            transaction.Rollback();
                            thongBao = "Không tìm thấy độc giả cần xóa.";
                            return false;
                        }

                        using (SqlCommand deleteTaiKhoanCommand = new SqlCommand(deleteTaiKhoanQuery, connection, transaction))
                        {
                            deleteTaiKhoanCommand.Parameters.AddWithValue("@MaTaiKhoan", Convert.ToInt32(maTaiKhoanValue));
                            deleteTaiKhoanCommand.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        thongBao = "Xóa độc giả thành công.";
                        return true;
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();

                        if (ex.Number == 547)
                        {
                            thongBao = "Không thể xóa độc giả đã phát sinh phiếu mượn hoặc phiếu thu tiền phạt.";
                            return false;
                        }

                        throw;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        private static string TaoTenDangNhapDocGia(int maDocGia)
        {
            return "docgia" + maDocGia.ToString("D2");
        }

        private static int LayMaTaiKhoanTiepTheo(SqlConnection connection, SqlTransaction transaction)
        {
            const string query = @"
                SELECT IDENT_CURRENT('TaiKhoan') + IDENT_INCR('TaiKhoan')";

            using (SqlCommand command = new SqlCommand(query, connection, transaction))
            {
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }
    }
}
