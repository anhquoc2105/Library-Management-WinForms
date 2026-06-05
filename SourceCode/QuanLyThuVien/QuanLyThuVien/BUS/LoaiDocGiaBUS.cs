using System.Data;
using QuanLyThuVien.DAL;

namespace QuanLyThuVien.BUS
{
    public class LoaiDocGiaBUS
    {
        private readonly LoaiDocGiaDAL loaiDocGiaDAL = new LoaiDocGiaDAL();

        public DataTable LayDanhSachLoaiDocGia()
        {
            return loaiDocGiaDAL.LayDanhSachLoaiDocGia();
        }

        public bool ThemLoaiDocGia(string tenLoaiDG, out string thongBao)
        {
            if (string.IsNullOrWhiteSpace(tenLoaiDG))
            {
                thongBao = "Vui lòng nhập tên loại độc giả.";
                return false;
            }

            return loaiDocGiaDAL.ThemLoaiDocGia(tenLoaiDG.Trim(), out thongBao);
        }

        public bool CapNhatLoaiDocGia(int maLoaiDG, string tenLoaiDG, out string thongBao)
        {
            if (maLoaiDG <= 0)
            {
                thongBao = "Vui lòng chọn loại độc giả cần sửa.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(tenLoaiDG))
            {
                thongBao = "Vui lòng nhập tên loại độc giả.";
                return false;
            }

            return loaiDocGiaDAL.CapNhatLoaiDocGia(maLoaiDG, tenLoaiDG.Trim(), out thongBao);
        }

        public bool XoaLoaiDocGia(int maLoaiDG, out string thongBao)
        {
            if (maLoaiDG <= 0)
            {
                thongBao = "Vui lòng chọn loại độc giả cần xóa.";
                return false;
            }

            return loaiDocGiaDAL.XoaLoaiDocGia(maLoaiDG, out thongBao);
        }
    }
}
