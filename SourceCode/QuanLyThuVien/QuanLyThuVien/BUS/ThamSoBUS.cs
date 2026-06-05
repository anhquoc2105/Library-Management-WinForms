using System.Data;
using System.Data.SqlClient;
using QuanLyThuVien.DAL;
using QuanLyThuVien.UTILS;

namespace QuanLyThuVien.BUS
{
    public class ThamSoBUS
    {
        private readonly ThamSoDAL thamSoDAL = new ThamSoDAL();

        public DataTable LayDanhSachThamSo()
        {
            return thamSoDAL.LayDanhSachThamSo();
        }

        public DataRow LayThamSoTheoTheLoai(int maTheLoai)
        {
            return thamSoDAL.LayThamSoTheoTheLoai(maTheLoai);
        }

        public bool CapNhatThamSo(
            int maThamSo,
            int giaTriThe,
            int soTuoiToiThieu,
            int soTuoiToiDa,
            int thoiGianXB,
            int soSachMuonToiDa,
            int soNgayMuonToiDa,
            decimal tienPhat,
            out string thongBao)
        {
            if (soTuoiToiThieu <= 0 || soTuoiToiDa < soTuoiToiThieu)
            {
                thongBao = "Tuổi tối thiểu/tối đa không hợp lệ.";
                return false;
            }

            if (giaTriThe <= 0 || thoiGianXB < 0 || soSachMuonToiDa <= 0 || soNgayMuonToiDa <= 0 || tienPhat < 0)
            {
                thongBao = "Tham số nhập vào không hợp lệ.";
                return false;
            }

            const string query = @"
                UPDATE ThamSo
                SET GiaTriThe = @GiaTriThe,
                    SoTuoiDG = @SoTuoiDG,
                    SoTuoiDGToiDa = @SoTuoiDGToiDa,
                    ThoiGianXB = @ThoiGianXB,
                    SoSachMuonToiDa = @SoSachMuonToiDa,
                    SoNgayMuonToiDa = @SoNgayMuonToiDa,
                    TienPhat = @TienPhat
                WHERE MaThamSo = @MaThamSo";

            int rows = DbHelper.ExecuteNonQuery(
                query,
                new SqlParameter("@GiaTriThe", giaTriThe),
                new SqlParameter("@SoTuoiDG", soTuoiToiThieu),
                new SqlParameter("@SoTuoiDGToiDa", soTuoiToiDa),
                new SqlParameter("@ThoiGianXB", thoiGianXB),
                new SqlParameter("@SoSachMuonToiDa", soSachMuonToiDa),
                new SqlParameter("@SoNgayMuonToiDa", soNgayMuonToiDa),
                new SqlParameter("@TienPhat", tienPhat),
                new SqlParameter("@MaThamSo", maThamSo));

            thongBao = rows > 0 ? "Cập nhật tham số thành công." : "Không thể cập nhật tham số.";
            return rows > 0;
        }
    }
}
