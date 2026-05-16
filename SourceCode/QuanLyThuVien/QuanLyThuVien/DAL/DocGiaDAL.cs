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
                new SqlParameter("@EmailDG", string.IsNullOrWhiteSpace(docGia.EmailDG)
                    ? (object)DBNull.Value
                    : docGia.EmailDG));

            if (emailCount != null && Convert.ToInt32(emailCount) > 0)
            {
                thongBao = "Email đã tồn tại.";
                return false;
            }

            const string query = @"
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
                    NULL
                )";

            int rowsAffected = DbHelper.ExecuteNonQuery(
                query,
                new SqlParameter("@TenDG", docGia.TenDG),
                new SqlParameter("@LoaiDG", docGia.LoaiDG),
                new SqlParameter("@NgaySinhDG", docGia.NgaySinhDG),
                new SqlParameter("@DiaChiDG", string.IsNullOrWhiteSpace(docGia.DiaChiDG)
                    ? (object)DBNull.Value
                    : docGia.DiaChiDG),
                new SqlParameter("@EmailDG", string.IsNullOrWhiteSpace(docGia.EmailDG)
                    ? (object)DBNull.Value
                    : docGia.EmailDG),
                new SqlParameter("@NgLapThe", docGia.NgLapThe),
                new SqlParameter("@TongNo", docGia.TongNo));

            thongBao = rowsAffected > 0
                ? "Lập thẻ độc giả thành công."
                : "Không thể lập thẻ độc giả.";

            return rowsAffected > 0;
        }
    }
}
