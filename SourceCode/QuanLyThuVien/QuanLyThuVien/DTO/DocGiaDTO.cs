using System;

namespace QuanLyThuVien.DTO
{
    public class DocGiaDTO
    {
        public int MaDG { get; set; }
        public string TenDG { get; set; }
        public string LoaiDG { get; set; }
        public DateTime NgaySinhDG { get; set; }
        public string DiaChiDG { get; set; }
        public string EmailDG { get; set; }
        public DateTime NgLapThe { get; set; }
        public DateTime? NgayHetHan { get; set; }
        public decimal TongNo { get; set; }
        public int? MaTaiKhoan { get; set; }
    }
}
