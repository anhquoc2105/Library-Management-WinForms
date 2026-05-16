using System.Data;
using QuanLyThuVien.DAL;

namespace QuanLyThuVien.BUS
{
    public class TheLoaiBUS
    {
        private readonly TheLoaiDAL theLoaiDAL = new TheLoaiDAL();

        public DataTable LayDanhSachTheLoai()
        {
            return theLoaiDAL.LayDanhSachTheLoai();
        }

        public bool ThemTheLoai(string tenTheLoai, out string thongBao)
        {
            if (string.IsNullOrWhiteSpace(tenTheLoai))
            {
                thongBao = "Vui lòng nhập tên thể loại.";
                return false;
            }

            return theLoaiDAL.ThemTheLoai(tenTheLoai.Trim(), out thongBao);
        }

        public bool CapNhatTheLoai(int maTheLoai, string tenTheLoai, out string thongBao)
        {
            if (string.IsNullOrWhiteSpace(tenTheLoai))
            {
                thongBao = "Vui lòng nhập tên thể loại.";
                return false;
            }

            return theLoaiDAL.CapNhatTheLoai(maTheLoai, tenTheLoai.Trim(), out thongBao);
        }

        public bool XoaTheLoai(int maTheLoai, out string thongBao)
        {
            return theLoaiDAL.XoaTheLoai(maTheLoai, out thongBao);
        }
    }
}
