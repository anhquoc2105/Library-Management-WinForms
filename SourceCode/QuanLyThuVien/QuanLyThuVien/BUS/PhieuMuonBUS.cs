using System.Data;
using QuanLyThuVien.DAL;

namespace QuanLyThuVien.BUS
{
    public class PhieuMuonBUS
    {
        private readonly PhieuMuonDAL phieuMuonDAL = new PhieuMuonDAL();

        public bool LapPhieuMuon(int maDG, int maSach, out string thongBao)
        {
            return phieuMuonDAL.LapPhieuMuon(maDG, maSach, out thongBao);
        }

        public DataTable LayLichSuMuonTheoMaTaiKhoan(int maTaiKhoan)
        {
            return phieuMuonDAL.LayLichSuMuonTheoMaTaiKhoan(maTaiKhoan);
        }

        public DataTable LayDanhSachSachDangMuon()
        {
            return phieuMuonDAL.LayDanhSachSachDangMuon();
        }

        public bool TraSach(int maPhieu, int maSach, out string thongBao)
        {
            return phieuMuonDAL.TraSach(maPhieu, maSach, out thongBao);
        }
    }
}
