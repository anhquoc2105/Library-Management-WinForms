using QuanLyThuVien.DAL;
using QuanLyThuVien.DTO;

namespace QuanLyThuVien.BUS
{
    public class TaiKhoanBUS
    {
        private readonly TaiKhoanDAL taiKhoanDAL = new TaiKhoanDAL();

        public TaiKhoanDTO DangNhap(string tenDangNhap, string matKhau, out string thongBao)
        {
            if (string.IsNullOrWhiteSpace(tenDangNhap))
            {
                thongBao = "Vui lòng nhập tên đăng nhập.";
                return null;
            }

            if (string.IsNullOrWhiteSpace(matKhau))
            {
                thongBao = "Vui lòng nhập mật khẩu.";
                return null;
            }

            TaiKhoanDTO taiKhoan = taiKhoanDAL.DangNhap(tenDangNhap.Trim(), matKhau.Trim());

            if (taiKhoan == null)
            {
                thongBao = "Tên đăng nhập hoặc mật khẩu không đúng.";
                return null;
            }

            thongBao = "Đăng nhập thành công.";
            return taiKhoan;
        }
    }
}
