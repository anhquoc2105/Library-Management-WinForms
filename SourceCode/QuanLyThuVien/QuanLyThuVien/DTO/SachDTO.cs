using System;

namespace QuanLyThuVien.DTO
{
    public class SachDTO
    {
        public int MaSach { get; set; }
        public string TenSach { get; set; }
        public string ChuDe { get; set; }
        public int? MaTheLoai { get; set; }
        public string TenTG { get; set; }
        public int? MaTacGia { get; set; }
        public int NamXB { get; set; }
        public string NhaXB { get; set; }
        public DateTime NgayNhap { get; set; }
        public decimal TriGia { get; set; }
        public int SoLuongTon { get; set; }
        public string TinhTrang { get; set; }
    }
}
