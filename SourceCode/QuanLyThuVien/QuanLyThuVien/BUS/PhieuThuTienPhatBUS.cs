using System.Data;
using QuanLyThuVien.DAL;

namespace QuanLyThuVien.BUS
{
    public class PhieuThuTienPhatBUS
    {
        private readonly PhieuThuTienPhatDAL phieuThuTienPhatDAL = new PhieuThuTienPhatDAL();

        public DataTable LayDanhSachDocGiaNo()
        {
            return phieuThuTienPhatDAL.LayDanhSachDocGiaNo();
        }

        public bool LapPhieuThu(int maDG, decimal soTienThu, out string thongBao)
        {
            return phieuThuTienPhatDAL.LapPhieuThu(maDG, soTienThu, out thongBao);
        }
    }
}
