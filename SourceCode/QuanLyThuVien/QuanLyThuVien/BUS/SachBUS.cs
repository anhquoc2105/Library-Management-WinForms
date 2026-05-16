using System;
using System.Data;
using QuanLyThuVien.DAL;
using QuanLyThuVien.DTO;

namespace QuanLyThuVien.BUS
{
    public class SachBUS
    {
        private readonly SachDAL sachDAL = new SachDAL();
        private readonly ThamSoDAL thamSoDAL = new ThamSoDAL();

        public DataTable LayDanhSachSach()
        {
            return sachDAL.LayDanhSachSach();
        }

        public DataTable LayDanhSachSachCon()
        {
            return sachDAL.LayDanhSachSachCon();
        }

        public DataTable LayDanhSachTheLoai()
        {
            return sachDAL.LayDanhSachTheLoai();
        }

        public DataTable LayDanhSachTacGia()
        {
            return sachDAL.LayDanhSachTacGia();
        }

        public DataTable TimKiemSach(string maSach, string tenSach, string theLoai, string tacGia)
        {
            return sachDAL.TimKiemSach(maSach, tenSach, theLoai, tacGia);
        }

        public bool ThemSach(SachDTO sach, out string thongBao)
        {
            DataRow thamSo = thamSoDAL.LayThamSoHienTai();
            if (thamSo == null)
            {
                thongBao = "Không tìm thấy tham số hệ thống.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(sach.TenSach))
            {
                thongBao = "Vui lòng nhập tên sách.";
                return false;
            }

            int khoangNamXB = Convert.ToInt32(thamSo["ThoiGianXB"]);
            int namHienTai = DateTime.Today.Year;
            if (sach.NamXB < namHienTai - khoangNamXB || sach.NamXB > namHienTai)
            {
                thongBao = "Chỉ nhận sách xuất bản trong vòng " + khoangNamXB + " năm.";
                return false;
            }

            if (sach.ChuDe != "A" && sach.ChuDe != "B" && sach.ChuDe != "C")
            {
                thongBao = "Thể loại chỉ được là A, B hoặc C.";
                return false;
            }

            sach.TinhTrang = sach.SoLuongTon > 0 ? "Con" : "Mat";
            return sachDAL.ThemSach(sach, out thongBao);
        }
    }
}
