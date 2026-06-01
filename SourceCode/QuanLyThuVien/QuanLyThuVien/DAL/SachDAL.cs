using System;
using System.Data;
using System.Data.SqlClient;
using QuanLyThuVien.DTO;
using QuanLyThuVien.UTILS;

namespace QuanLyThuVien.DAL
{
    public class SachDAL
    {
        public DataTable LayDanhSachSach()
        {
            const string query = @"
                SELECT
                    s.MaSach,
                    s.TenSach,
                    tl.TenTheLoai,
                    s.TenTG,
                    s.NamXB,
                    s.NhaXB,
                    s.NgayNhap,
                    s.TriGia,
                    s.SoLuongTon,
                    CASE
                        WHEN s.TinhTrang = N'Con' THEN N'Còn'
                        WHEN s.TinhTrang = N'Dang muon' THEN N'Đang mượn'
                        WHEN s.TinhTrang = N'Hong' THEN N'Hỏng'
                        WHEN s.TinhTrang = N'Mat' THEN N'Mất'
                        ELSE s.TinhTrang
                    END AS TinhTrang
                FROM Sach s
                LEFT JOIN TheLoai tl ON s.MaTheLoai = tl.MaTheLoai
                ORDER BY s.MaSach";

            return DbHelper.ExecuteQuery(query);
        }

        public DataTable LayDanhSachSachCon()
        {
            const string query = @"
                SELECT
                    s.MaSach,
                    s.TenSach,
                    tl.TenTheLoai,
                    s.TenTG,
                    s.NamXB,
                    s.NhaXB,
                    s.NgayNhap,
                    s.TriGia,
                    s.SoLuongTon,
                    CASE
                        WHEN s.TinhTrang = N'Con' THEN N'Còn'
                        WHEN s.TinhTrang = N'Dang muon' THEN N'Đang mượn'
                        WHEN s.TinhTrang = N'Hong' THEN N'Hỏng'
                        WHEN s.TinhTrang = N'Mat' THEN N'Mất'
                        ELSE s.TinhTrang
                    END AS TinhTrang
                FROM Sach s
                LEFT JOIN TheLoai tl ON s.MaTheLoai = tl.MaTheLoai
                WHERE s.TinhTrang = N'Con' AND s.SoLuongTon > 0
                ORDER BY s.MaSach";

            return DbHelper.ExecuteQuery(query);
        }

        public DataTable LayDanhSachTheLoai()
        {
            return DbHelper.ExecuteQuery("SELECT MaTheLoai, TenTheLoai FROM TheLoai ORDER BY TenTheLoai");
        }

        public DataTable LayDanhSachTacGia()
        {
            return DbHelper.ExecuteQuery("SELECT MaTacGia, TenTacGia FROM TacGia ORDER BY TenTacGia");
        }

        public DataTable TimKiemSach(string maSach, string tenSach, string theLoai, string tacGia)
        {
            const string query = @"
                SELECT
                    s.MaSach,
                    s.TenSach,
                    tl.TenTheLoai,
                    s.TenTG,
                    s.NamXB,
                    s.NhaXB,
                    s.NgayNhap,
                    s.TriGia,
                    s.SoLuongTon,
                    CASE
                        WHEN s.TinhTrang = N'Con' THEN N'Còn'
                        WHEN s.TinhTrang = N'Dang muon' THEN N'Đang mượn'
                        WHEN s.TinhTrang = N'Hong' THEN N'Hỏng'
                        WHEN s.TinhTrang = N'Mat' THEN N'Mất'
                        ELSE s.TinhTrang
                    END AS TinhTrang
                FROM Sach s
                LEFT JOIN TheLoai tl ON s.MaTheLoai = tl.MaTheLoai
                WHERE (@MaSach = ''
                       OR CAST(s.MaSach AS NVARCHAR(20)) LIKE N'%' + @MaSach + N'%'
                       OR RIGHT('00000' + CAST(s.MaSach AS VARCHAR(5)), 5) LIKE '%' + @MaSach + '%')
                  AND (@TenSach = '' OR s.TenSach LIKE N'%' + @TenSach + N'%')
                  AND (@TheLoai = '' OR tl.TenTheLoai LIKE N'%' + @TheLoai + N'%')
                  AND (@TacGia = '' OR s.TenTG LIKE N'%' + @TacGia + N'%')
                ORDER BY s.MaSach";

            return DbHelper.ExecuteQuery(
                query,
                new SqlParameter("@MaSach", maSach ?? string.Empty),
                new SqlParameter("@TenSach", tenSach ?? string.Empty),
                new SqlParameter("@TheLoai", theLoai ?? string.Empty),
                new SqlParameter("@TacGia", tacGia ?? string.Empty));
        }

        public bool ThemSach(SachDTO sach, out string thongBao)
        {
            const string query = @"
                DECLARE @KetQua INT = 0;

                BEGIN TRY
                    BEGIN TRANSACTION;

                    ;WITH SachTrung AS
                    (
                        SELECT TOP (1) *
                        FROM Sach WITH (UPDLOCK, HOLDLOCK)
                        WHERE LOWER(LTRIM(RTRIM(TenSach))) = LOWER(LTRIM(RTRIM(@TenSach)))
                          AND LOWER(LTRIM(RTRIM(ChuDe))) = LOWER(LTRIM(RTRIM(@ChuDe)))
                          AND ((MaTheLoai = @MaTheLoai) OR (MaTheLoai IS NULL AND @MaTheLoai IS NULL))
                          AND LOWER(LTRIM(RTRIM(TenTG))) = LOWER(LTRIM(RTRIM(@TenTG)))
                          AND ((MaTacGia = @MaTacGia) OR (MaTacGia IS NULL AND @MaTacGia IS NULL))
                          AND NamXB = @NamXB
                          AND LOWER(LTRIM(RTRIM(NhaXB))) = LOWER(LTRIM(RTRIM(@NhaXB)))
                          AND TriGia = @TriGia
                        ORDER BY MaSach
                    )
                    UPDATE SachTrung
                    SET SoLuongTon = SoLuongTon + @SoLuongTon,
                        TinhTrang = CASE
                            WHEN TinhTrang IN (N'Con', N'Dang muon') THEN N'Con'
                            ELSE TinhTrang
                        END;

                    IF @@ROWCOUNT > 0
                    BEGIN
                        SET @KetQua = 2;
                    END
                    ELSE
                    BEGIN
                        INSERT INTO Sach
                        (
                            TenSach,
                            ChuDe,
                            MaTheLoai,
                            TenTG,
                            MaTacGia,
                            NamXB,
                            NhaXB,
                            NgayNhap,
                            TriGia,
                            SoLuongTon,
                            TinhTrang
                        )
                        VALUES
                        (
                            @TenSach,
                            @ChuDe,
                            @MaTheLoai,
                            @TenTG,
                            @MaTacGia,
                            @NamXB,
                            @NhaXB,
                            @NgayNhap,
                            @TriGia,
                            @SoLuongTon,
                            @TinhTrang
                        );

                        SET @KetQua = 1;
                    END

                    COMMIT TRANSACTION;
                END TRY
                BEGIN CATCH
                    IF @@TRANCOUNT > 0
                        ROLLBACK TRANSACTION;

                    THROW;
                END CATCH

                SELECT @KetQua;";

            int ketQua = Convert.ToInt32(DbHelper.ExecuteScalar(
                query,
                new SqlParameter("@TenSach", sach.TenSach),
                new SqlParameter("@ChuDe", sach.ChuDe),
                new SqlParameter("@MaTheLoai", (object)sach.MaTheLoai ?? DBNull.Value),
                new SqlParameter("@TenTG", sach.TenTG),
                new SqlParameter("@MaTacGia", (object)sach.MaTacGia ?? DBNull.Value),
                new SqlParameter("@NamXB", sach.NamXB),
                new SqlParameter("@NhaXB", sach.NhaXB),
                new SqlParameter("@NgayNhap", sach.NgayNhap),
                new SqlParameter("@TriGia", sach.TriGia),
                new SqlParameter("@SoLuongTon", sach.SoLuongTon),
                new SqlParameter("@TinhTrang", sach.TinhTrang)));

            if (ketQua == 2)
            {
                thongBao = "Sách đã tồn tại, số lượng còn đã được cộng thêm.";
                return true;
            }

            int rowsAffected = ketQua;

            thongBao = rowsAffected > 0 ? "Tiếp nhận sách mới thành công." : "Không thể tiếp nhận sách mới.";
            return ketQua == 1;
        }
    }
}
