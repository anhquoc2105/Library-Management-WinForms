using System;
using System.Data;
using QuanLyThuVien.DAL;

namespace QuanLyThuVien.BUS
{
    public class BaoCaoBUS
    {
        private readonly BaoCaoDAL baoCaoDAL = new BaoCaoDAL();

        public DataTable LayBaoCaoMuonTheoTheLoai(DateTime ngayBaoCao)
        {
            return baoCaoDAL.LayBaoCaoMuonTheoTheLoai(ngayBaoCao);
        }

        public DataTable LayBaoCaoTraTre(DateTime ngayTra)
        {
            return baoCaoDAL.LayBaoCaoTraTre(ngayTra);
        }
    }
}
