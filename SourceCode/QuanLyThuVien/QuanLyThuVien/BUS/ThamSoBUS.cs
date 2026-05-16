using System.Data;
using QuanLyThuVien.DAL;

namespace QuanLyThuVien.BUS
{
    public class ThamSoBUS
    {
        private readonly ThamSoDAL thamSoDAL = new ThamSoDAL();

        public DataTable LayDanhSachThamSo()
        {
            return thamSoDAL.LayDanhSachThamSo();
        }
    }
}
