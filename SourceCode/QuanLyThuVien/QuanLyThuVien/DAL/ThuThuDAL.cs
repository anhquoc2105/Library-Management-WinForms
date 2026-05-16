using System.Data;
using QuanLyThuVien.UTILS;

namespace QuanLyThuVien.DAL
{
    public class ThuThuDAL
    {
        public DataTable LayDanhSachThuThu()
        {
            const string query = @"
                SELECT
                    MaTT,
                    TenTT,
                    GioiTinhTT,
                    NgaySinhTT,
                    EmailTT,
                    DiaChiTT,
                    GhiChu,
                    MaTaiKhoan
                FROM ThuThu
                ORDER BY MaTT";

            return DbHelper.ExecuteQuery(query);
        }
    }
}
