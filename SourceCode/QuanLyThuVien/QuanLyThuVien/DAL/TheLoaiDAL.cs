using System.Data;
using System.Data.SqlClient;
using QuanLyThuVien.UTILS;

namespace QuanLyThuVien.DAL
{
    public class TheLoaiDAL
    {
        public DataTable LayDanhSachTheLoai()
        {
            const string query = @"
                SELECT
                    MaTheLoai,
                    TenTheLoai
                FROM TheLoai
                ORDER BY MaTheLoai";

            return DbHelper.ExecuteQuery(query);
        }

        public bool ThemTheLoai(string tenTheLoai, out string thongBao)
        {
            const string checkQuery = "SELECT COUNT(*) FROM TheLoai WHERE TenTheLoai = @TenTheLoai";
            int soLuong = (int)DbHelper.ExecuteScalar(checkQuery, new SqlParameter("@TenTheLoai", tenTheLoai));

            if (soLuong > 0)
            {
                thongBao = "Thể loại đã tồn tại.";
                return false;
            }

            // Tìm mã thể loại nhỏ nhất chưa được sử dụng (lấp khoảng trống)
            const string nextIdQuery = @"
                SELECT MIN(t.NextId)
                FROM (
                    SELECT 1 AS NextId
                    WHERE NOT EXISTS (SELECT 1 FROM TheLoai WHERE MaTheLoai = 1)
                    UNION ALL
                    SELECT MaTheLoai + 1
                    FROM TheLoai tl
                    WHERE NOT EXISTS (SELECT 1 FROM TheLoai WHERE MaTheLoai = tl.MaTheLoai + 1)
                ) t";
            int nextId = (int)DbHelper.ExecuteScalar(nextIdQuery);

            const string insertQuery = @"
                SET IDENTITY_INSERT TheLoai ON;
                INSERT INTO TheLoai (MaTheLoai, TenTheLoai) VALUES (@MaTheLoai, @TenTheLoai);
                SET IDENTITY_INSERT TheLoai OFF;";
            int rows = DbHelper.ExecuteNonQuery(insertQuery,
                new SqlParameter("@MaTheLoai", nextId),
                new SqlParameter("@TenTheLoai", tenTheLoai));
            thongBao = rows > 0 ? "Thêm thể loại thành công." : "Không thể thêm thể loại.";
            return rows > 0;
        }

        public bool CapNhatTheLoai(int maTheLoai, string tenTheLoai, out string thongBao)
        {
            const string checkQuery = @"
                SELECT COUNT(*)
                FROM TheLoai
                WHERE TenTheLoai = @TenTheLoai
                  AND MaTheLoai <> @MaTheLoai";

            int soLuong = (int)DbHelper.ExecuteScalar(
                checkQuery,
                new SqlParameter("@TenTheLoai", tenTheLoai),
                new SqlParameter("@MaTheLoai", maTheLoai));

            if (soLuong > 0)
            {
                thongBao = "Tên thể loại đã tồn tại.";
                return false;
            }

            const string updateQuery = @"
                UPDATE TheLoai
                SET TenTheLoai = @TenTheLoai
                WHERE MaTheLoai = @MaTheLoai";

            int rows = DbHelper.ExecuteNonQuery(
                updateQuery,
                new SqlParameter("@TenTheLoai", tenTheLoai),
                new SqlParameter("@MaTheLoai", maTheLoai));

            thongBao = rows > 0 ? "Cập nhật thể loại thành công." : "Không thể cập nhật thể loại.";
            return rows > 0;
        }

        public bool XoaTheLoai(int maTheLoai, out string thongBao)
        {
            const string checkQuery = "SELECT COUNT(*) FROM Sach WHERE MaTheLoai = @MaTheLoai";
            int soLuongSach = (int)DbHelper.ExecuteScalar(checkQuery, new SqlParameter("@MaTheLoai", maTheLoai));

            if (soLuongSach > 0)
            {
                thongBao = "Không thể xóa thể loại đang được sử dụng trong sách.";
                return false;
            }

            const string deleteQuery = "DELETE FROM TheLoai WHERE MaTheLoai = @MaTheLoai";
            int rows = DbHelper.ExecuteNonQuery(deleteQuery, new SqlParameter("@MaTheLoai", maTheLoai));

            thongBao = rows > 0 ? "Xóa thể loại thành công." : "Không thể xóa thể loại.";
            return rows > 0;
        }
    }
}
