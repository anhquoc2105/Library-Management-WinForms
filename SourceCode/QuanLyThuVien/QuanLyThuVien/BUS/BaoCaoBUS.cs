using System.Data;
using QuanLyThuVien.DAL;

namespace QuanLyThuVien.BUS
{
    public class BaoCaoBUS
    {
        private readonly BaoCaoDAL baoCaoDAL = new BaoCaoDAL();

        public DataTable LayBaoCaoMuonTheoTheLoai()
        {
            return baoCaoDAL.LayBaoCaoMuonTheoTheLoai();
        }

        public DataTable LayBaoCaoTraTre()
        {
            return baoCaoDAL.LayBaoCaoTraTre();
        }
    }
}
