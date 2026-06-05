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
                        WHEN s.TinhTrang = N'Hong' THEN N'Hỏng'
                        WHEN s.TinhTrang = N'Mat' THEN N'Mất'
                        WHEN s.SoLuongTon > 0 THEN N'Còn'
                        ELSE N'Đang mượn'
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
                        WHEN s.TinhTrang = N'Hong' THEN N'Hỏng'
                        WHEN s.TinhTrang = N'Mat' THEN N'Mất'
                        WHEN s.SoLuongTon > 0 THEN N'Còn'
                        ELSE N'Đang mượn'
                    END AS TinhTrang
                FROM Sach s
                LEFT JOIN TheLoai tl ON s.MaTheLoai = tl.MaTheLoai
                WHERE s.TinhTrang NOT IN (N'Hong', N'Mat') AND s.SoLuongTon > 0
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

        public DataTable TimKiemSach(string maSach, string tenSach, string theLoai, string tacGia, string nhaXB, string namXB)
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
                        WHEN s.TinhTrang = N'Hong' THEN N'Hỏng'
                        WHEN s.TinhTrang = N'Mat' THEN N'Mất'
                        WHEN s.SoLuongTon > 0 THEN N'Còn'
                        ELSE N'Đang mượn'
                    END AS TinhTrang
                FROM Sach s
                LEFT JOIN TheLoai tl ON s.MaTheLoai = tl.MaTheLoai
                WHERE (@MaSach = ''
                       OR CAST(s.MaSach AS NVARCHAR(20)) LIKE N'%' + @MaSach + N'%'
                       OR RIGHT('00000' + CAST(s.MaSach AS VARCHAR(5)), 5) LIKE '%' + @MaSach + '%')
                  AND (@TenSach = '' OR s.TenSach LIKE N'%' + @TenSach + N'%')
                  AND (@TheLoai = '' OR tl.TenTheLoai LIKE N'%' + @TheLoai + N'%')
                  AND (@TacGia = '' OR s.TenTG LIKE N'%' + @TacGia + N'%')
                  AND (@NhaXB = '' OR s.NhaXB LIKE N'%' + @NhaXB + N'%')
                  AND (@NamXB = '' OR CAST(s.NamXB AS NVARCHAR(20)) LIKE N'%' + @NamXB + N'%')
                ORDER BY s.MaSach";

            return DbHelper.ExecuteQuery(
                query,
                new SqlParameter("@MaSach", maSach ?? string.Empty),
                new SqlParameter("@TenSach", tenSach ?? string.Empty),
                new SqlParameter("@TheLoai", theLoai ?? string.Empty),
                new SqlParameter("@TacGia", tacGia ?? string.Empty),
                new SqlParameter("@NhaXB", nhaXB ?? string.Empty),
                new SqlParameter("@NamXB", namXB ?? string.Empty));
        }

        public bool ThemSach(SachDTO sach, out string thongBao)
        {
            DamBaoTriggerKiemTraNamXB();

            const string query = @"
                DECLARE @KetQua INT = 0;
                DECLARE @DaBatIdentityInsert BIT = 0;

                BEGIN TRY
                    BEGIN TRANSACTION;

                    IF @MaSach > 0
                    BEGIN
                        IF EXISTS (SELECT 1 FROM Sach WITH (UPDLOCK, HOLDLOCK) WHERE MaSach = @MaSach)
                        BEGIN
                            UPDATE Sach
                            SET TenSach = @TenSach,
                                ChuDe = @ChuDe,
                                MaTheLoai = @MaTheLoai,
                                TenTG = @TenTG,
                                MaTacGia = @MaTacGia,
                                NamXB = @NamXB,
                                NhaXB = @NhaXB,
                                NgayNhap = @NgayNhap,
                                TriGia = @TriGia,
                                TinhTrang = CASE
                                    WHEN TinhTrang IN (N'Con', N'Dang muon') AND SoLuongTon > 0 THEN N'Con'
                                    WHEN TinhTrang IN (N'Con', N'Dang muon') AND SoLuongTon = 0 THEN N'Dang muon'
                                    ELSE TinhTrang
                                END
                            WHERE MaSach = @MaSach;

                            SET @KetQua = 3;
                        END
                        ELSE
                        BEGIN
                            SET IDENTITY_INSERT Sach ON;
                            SET @DaBatIdentityInsert = 1;

                            INSERT INTO Sach
                            (
                                MaSach,
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
                                @MaSach,
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

                            SET IDENTITY_INSERT Sach OFF;
                            SET @DaBatIdentityInsert = 0;
                            SET @KetQua = 1;
                        END
                    END
                    ELSE
                    BEGIN
                        ;WITH SachTrung AS
                        (
                            SELECT TOP (1) *
                            FROM Sach WITH (UPDLOCK, HOLDLOCK)
                            WHERE LOWER(LTRIM(RTRIM(TenSach))) = LOWER(LTRIM(RTRIM(@TenSach)))
                              AND ((MaTheLoai = @MaTheLoai) OR (MaTheLoai IS NULL AND @MaTheLoai IS NULL))
                              AND LOWER(LTRIM(RTRIM(TenTG))) = LOWER(LTRIM(RTRIM(@TenTG)))
                              AND NamXB = @NamXB
                              AND LOWER(LTRIM(RTRIM(NhaXB))) = LOWER(LTRIM(RTRIM(@NhaXB)))
                              AND TriGia = @TriGia
                            ORDER BY MaSach DESC
                        )
                        UPDATE SachTrung
                        SET SoLuongTon = SoLuongTon + @SoLuongTon,
                            NgayNhap = @NgayNhap,
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
                    END

                    COMMIT TRANSACTION;
                END TRY
                BEGIN CATCH
                    IF @@TRANCOUNT > 0
                        ROLLBACK TRANSACTION;

                    IF @DaBatIdentityInsert = 1
                        SET IDENTITY_INSERT Sach OFF;

                    THROW;
                END CATCH

                SELECT @KetQua;";

            int ketQua = Convert.ToInt32(DbHelper.ExecuteScalar(
                query,
                new SqlParameter("@MaSach", sach.MaSach),
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

            if (ketQua == 3)
            {
                thongBao = "Cập nhật sách theo mã sách thành công.";
                return true;
            }

            int rowsAffected = ketQua;

            thongBao = rowsAffected > 0 ? "Tiếp nhận sách mới thành công." : "Không thể tiếp nhận sách mới.";
            return ketQua == 1;
        }

        public bool XoaSach(int maSach, out string thongBao)
        {
            const string query = "DELETE FROM Sach WHERE MaSach = @MaSach";

            try
            {
                int rows = DbHelper.ExecuteNonQuery(
                    query,
                    new SqlParameter("@MaSach", maSach));

                thongBao = rows > 0 ? "Xóa sách thành công." : "Không tìm thấy sách cần xóa.";
                return rows > 0;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                {
                    thongBao = "Không thể xóa sách đã phát sinh phiếu mượn/trả.";
                    return false;
                }

                throw;
            }
        }

        private void DamBaoTriggerKiemTraNamXB()
        {
            const string query = @"
                CREATE OR ALTER TRIGGER dbo.TRG_Sach_ValidateNamXB
                ON dbo.Sach
                AFTER INSERT, UPDATE
                AS
                BEGIN
                    SET NOCOUNT ON;

                    DECLARE @ThoiGianXB INT;
                    SELECT TOP 1 @ThoiGianXB = ThoiGianXB
                    FROM dbo.ThamSo
                    WHERE MaTheLoai IS NULL
                    ORDER BY MaThamSo;

                    IF EXISTS
                    (
                        SELECT 1
                        FROM inserted i
                        WHERE YEAR(GETDATE()) - i.NamXB > @ThoiGianXB
                           OR i.NamXB > YEAR(GETDATE())
                    )
                    BEGIN
                        RAISERROR(N'Chỉ nhận sách xuất bản trong khoảng năm hợp lệ.', 16, 1);
                        ROLLBACK TRANSACTION;
                        RETURN;
                    END
                END";

            DbHelper.ExecuteNonQuery(query);
        }
    }
}
