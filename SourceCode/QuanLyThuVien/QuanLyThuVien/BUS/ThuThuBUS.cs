using System.Data;
using QuanLyThuVien.DAL;

namespace QuanLyThuVien.BUS
{
    public class ThuThuBUS
    {
        private readonly ThuThuDAL thuThuDAL = new ThuThuDAL();

        public DataTable LayDanhSachThuThu()
        {
            return thuThuDAL.LayDanhSachThuThu();
        }
    }
}
