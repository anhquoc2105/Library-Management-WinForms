using System;
using System.Data;
using QuanLyThuVien.DAL;

namespace QuanLyThuVien.BUS
{
    public class BaoCaoBUS
    {
        private readonly BaoCaoDAL baoCaoDAL = new BaoCaoDAL();

        public DataTable LayBaoCaoSachMuon(DateTime ngayBaoCao)
        {
            return baoCaoDAL.LayBaoCaoSachMuon(ngayBaoCao);
        }

        public DataTable LayBaoCaoSachTra(DateTime ngayBaoCao)
        {
            return baoCaoDAL.LayBaoCaoSachTra(ngayBaoCao);
        }
    }
}
