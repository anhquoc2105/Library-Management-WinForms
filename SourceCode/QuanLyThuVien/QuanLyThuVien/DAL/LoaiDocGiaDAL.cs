using System;
using System.Data;
using System.Data.SqlClient;
using QuanLyThuVien.UTILS;

namespace QuanLyThuVien.DAL
{
    public class LoaiDocGiaDAL
    {
        public DataTable LayDanhSachLoaiDocGia()
        {
            DamBaoBangLoaiDocGia();

            const string query = @"
                SELECT
                    MaLoaiDG,
                    TenLoaiDG
                FROM LoaiDocGia
                ORDER BY MaLoaiDG";

            return DbHelper.ExecuteQuery(query);
        }

        public bool ThemLoaiDocGia(string tenLoaiDG, out string thongBao)
        {
            DamBaoBangLoaiDocGia();

            const string checkQuery = "SELECT COUNT(*) FROM LoaiDocGia WHERE TenLoaiDG = @TenLoaiDG";
            int soLuong = Convert.ToInt32(DbHelper.ExecuteScalar(checkQuery, new SqlParameter("@TenLoaiDG", tenLoaiDG)));
            if (soLuong > 0)
            {
                thongBao = "Loại độc giả đã tồn tại.";
                return false;
            }

            const string insertQuery = "INSERT INTO LoaiDocGia (TenLoaiDG) VALUES (@TenLoaiDG)";
            int rows = DbHelper.ExecuteNonQuery(insertQuery, new SqlParameter("@TenLoaiDG", tenLoaiDG));

            thongBao = rows > 0 ? "Thêm loại độc giả thành công." : "Không thể thêm loại độc giả.";
            return rows > 0;
        }

        public bool CapNhatLoaiDocGia(int maLoaiDG, string tenLoaiDG, out string thongBao)
        {
            DamBaoBangLoaiDocGia();

            const string checkQuery = @"
                SELECT COUNT(*)
                FROM LoaiDocGia
                WHERE TenLoaiDG = @TenLoaiDG
                  AND MaLoaiDG <> @MaLoaiDG";

            int soLuong = Convert.ToInt32(DbHelper.ExecuteScalar(
                checkQuery,
                new SqlParameter("@TenLoaiDG", tenLoaiDG),
                new SqlParameter("@MaLoaiDG", maLoaiDG)));

            if (soLuong > 0)
            {
                thongBao = "Tên loại độc giả đã tồn tại.";
                return false;
            }

            const string selectOldQuery = "SELECT TenLoaiDG FROM LoaiDocGia WHERE MaLoaiDG = @MaLoaiDG";
            object oldValue = DbHelper.ExecuteScalar(selectOldQuery, new SqlParameter("@MaLoaiDG", maLoaiDG));
            if (oldValue == null || oldValue == DBNull.Value)
            {
                thongBao = "Không tìm thấy loại độc giả cần sửa.";
                return false;
            }

            using (SqlConnection connection = DbHelper.GetConnection())
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string tenCu = oldValue.ToString();

                        using (SqlCommand command = new SqlCommand(
                            "UPDATE LoaiDocGia SET TenLoaiDG = @TenLoaiDG WHERE MaLoaiDG = @MaLoaiDG",
                            connection,
                            transaction))
                        {
                            command.Parameters.AddWithValue("@TenLoaiDG", tenLoaiDG);
                            command.Parameters.AddWithValue("@MaLoaiDG", maLoaiDG);
                            command.ExecuteNonQuery();
                        }

                        using (SqlCommand command = new SqlCommand(
                            "UPDATE DocGia SET LoaiDG = @TenLoaiDGMoi WHERE LoaiDG = @TenLoaiDGOld",
                            connection,
                            transaction))
                        {
                            command.Parameters.AddWithValue("@TenLoaiDGMoi", tenLoaiDG);
                            command.Parameters.AddWithValue("@TenLoaiDGOld", tenCu);
                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        thongBao = "Cập nhật loại độc giả thành công.";
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public bool XoaLoaiDocGia(int maLoaiDG, out string thongBao)
        {
            DamBaoBangLoaiDocGia();

            const string selectQuery = "SELECT TenLoaiDG FROM LoaiDocGia WHERE MaLoaiDG = @MaLoaiDG";
            object tenLoaiValue = DbHelper.ExecuteScalar(selectQuery, new SqlParameter("@MaLoaiDG", maLoaiDG));
            if (tenLoaiValue == null || tenLoaiValue == DBNull.Value)
            {
                thongBao = "Không tìm thấy loại độc giả cần xóa.";
                return false;
            }

            string tenLoaiDG = tenLoaiValue.ToString();
            const string checkUsedQuery = "SELECT COUNT(*) FROM DocGia WHERE LoaiDG = @TenLoaiDG";
            int soDocGia = Convert.ToInt32(DbHelper.ExecuteScalar(checkUsedQuery, new SqlParameter("@TenLoaiDG", tenLoaiDG)));
            if (soDocGia > 0)
            {
                thongBao = "Không thể xóa loại độc giả đang được sử dụng.";
                return false;
            }

            const string deleteQuery = "DELETE FROM LoaiDocGia WHERE MaLoaiDG = @MaLoaiDG";
            int rows = DbHelper.ExecuteNonQuery(deleteQuery, new SqlParameter("@MaLoaiDG", maLoaiDG));

            thongBao = rows > 0 ? "Xóa loại độc giả thành công." : "Không thể xóa loại độc giả.";
            return rows > 0;
        }

        private void DamBaoBangLoaiDocGia()
        {
            const string createTableQuery = @"
                IF OBJECT_ID(N'dbo.LoaiDocGia', N'U') IS NULL
                BEGIN
                    CREATE TABLE dbo.LoaiDocGia
                    (
                        MaLoaiDG INT IDENTITY(1,1) PRIMARY KEY,
                        TenLoaiDG NVARCHAR(50) NOT NULL UNIQUE
                    );
                END;";

            DbHelper.ExecuteNonQuery(createTableQuery);

            const string dropConstraintQuery = @"
                IF OBJECT_ID(N'dbo.DocGia', N'U') IS NOT NULL
                   AND OBJECT_ID(N'dbo.CK_DocGia_LoaiDG', N'C') IS NOT NULL
                BEGIN
                    ALTER TABLE dbo.DocGia DROP CONSTRAINT CK_DocGia_LoaiDG;
                END;";

            DbHelper.ExecuteNonQuery(dropConstraintQuery);

            const string seedFromDocGiaQuery = @"
                IF OBJECT_ID(N'dbo.DocGia', N'U') IS NOT NULL
                BEGIN
                    INSERT INTO dbo.LoaiDocGia (TenLoaiDG)
                    SELECT DISTINCT dg.LoaiDG
                    FROM dbo.DocGia dg
                    WHERE dg.LoaiDG IS NOT NULL
                      AND LTRIM(RTRIM(dg.LoaiDG)) <> N''
                      AND NOT EXISTS
                      (
                          SELECT 1
                          FROM dbo.LoaiDocGia ldg
                          WHERE ldg.TenLoaiDG = dg.LoaiDG
                      );
                END;";

            DbHelper.ExecuteNonQuery(seedFromDocGiaQuery);

            const string seedDefaultQuery = @"
                IF NOT EXISTS (SELECT 1 FROM dbo.LoaiDocGia)
                BEGIN
                    INSERT INTO dbo.LoaiDocGia (TenLoaiDG)
                    VALUES (N'X'), (N'Y');
                END;";

            DbHelper.ExecuteNonQuery(seedDefaultQuery);
        }
    }
}
