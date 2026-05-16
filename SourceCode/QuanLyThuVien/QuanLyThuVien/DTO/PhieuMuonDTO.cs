using System;

namespace QuanLyThuVien.DTO
{
    public class PhieuMuonDTO
    {
        public int MaPhieu { get; set; }
        public int MaDG { get; set; }
        public DateTime NgayMuon { get; set; }
        public DateTime? NgayTra { get; set; }
        public int SLMuon { get; set; }
    }
}
