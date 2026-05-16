namespace QuanLyThuVien.UTILS
{
    public static class SessionManager
    {
        public static int MaTaiKhoan { get; set; }
        public static string TenDangNhap { get; set; }
        public static string VaiTro { get; set; }
        public static int? MaDG { get; set; }
        public static int? MaTT { get; set; }

        public static void Clear()
        {
            MaTaiKhoan = 0;
            TenDangNhap = null;
            VaiTro = null;
            MaDG = null;
            MaTT = null;
        }
    }
}
