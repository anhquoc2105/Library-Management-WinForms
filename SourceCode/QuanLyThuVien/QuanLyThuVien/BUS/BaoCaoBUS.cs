using System.Data;
using QuanLyThuVien.DAL;

namespace QuanLyThuVien.BUS
{
    public class BaoCaoBUS
    {
        private readonly BaoCaoDAL baoCaoDAL = new BaoCaoDAL();

        public DataTable LayDanhSachThangBaoCao()
        {
            return baoCaoDAL.LayDanhSachThangBaoCao();
        }

        public DataTable LayBaoCaoMuonTheoTheLoai(int nam, int thang)
        {
            return baoCaoDAL.LayBaoCaoMuonTheoTheLoai(nam, thang);
        }

        public DataTable LayBaoCaoTraTre(int nam, int thang)
        {
            return baoCaoDAL.LayBaoCaoTraTre(nam, thang);
        }
    }
}
