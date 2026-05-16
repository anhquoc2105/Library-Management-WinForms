using System;

namespace QuanLyThuVien.DTO
{
    public class PhieuThuTienPhatDTO
    {
        public int MaPhieuThu { get; set; }
        public int MaDG { get; set; }
        public DateTime NgayThu { get; set; }
        public decimal TongNoLucThu { get; set; }
        public decimal SoTienThu { get; set; }
        public decimal ConLai { get; set; }
    }
}
