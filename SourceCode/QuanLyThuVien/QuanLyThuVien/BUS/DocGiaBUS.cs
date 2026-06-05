using System;
using System.Data;
using QuanLyThuVien.DAL;
using QuanLyThuVien.DTO;

namespace QuanLyThuVien.BUS
{
    public class DocGiaBUS
    {
        private readonly DocGiaDAL docGiaDAL = new DocGiaDAL();
        private readonly ThamSoDAL thamSoDAL = new ThamSoDAL();

        public DataTable LayDanhSachDocGia()
        {
            return docGiaDAL.LayDanhSachDocGia();
        }

        public DataTable LayDanhSachDocGiaChoCombo()
        {
            return docGiaDAL.LayDanhSachDocGiaChoCombo();
        }

        public DataTable LayThongTinTheoMaTaiKhoan(int maTaiKhoan)
        {
            return docGiaDAL.LayThongTinTheoMaTaiKhoan(maTaiKhoan);
        }

        public bool XoaDocGia(int maDG, out string thongBao)
        {
            if (maDG <= 0)
            {
                thongBao = "Mã độc giả không hợp lệ.";
                return false;
            }

            return docGiaDAL.XoaDocGia(maDG, out thongBao);
        }

        public bool ThemDocGia(DocGiaDTO docGia, out string thongBao)
        {
            if (!KiemTraThongTinDocGia(docGia, out thongBao))
            {
                return false;
            }

            docGia.TongNo = 0;
            return docGiaDAL.ThemDocGia(docGia, out thongBao);
        }

        public bool CapNhatDocGia(DocGiaDTO docGia, out string thongBao)
        {
            if (docGia.MaDG <= 0)
            {
                thongBao = "Vui lòng chọn độc giả cần cập nhật.";
                return false;
            }

            if (!KiemTraThongTinDocGia(docGia, out thongBao))
            {
                return false;
            }

            return docGiaDAL.CapNhatDocGia(docGia, out thongBao);
        }

        public bool GiaHanThe(int maDG, out string thongBao)
        {
            if (maDG <= 0)
            {
                thongBao = "Vui lòng chọn độc giả cần gia hạn thẻ.";
                return false;
            }

            return docGiaDAL.GiaHanThe(maDG, out thongBao);
        }

        private bool KiemTraThongTinDocGia(DocGiaDTO docGia, out string thongBao)
        {
            DataRow thamSo = thamSoDAL.LayThamSoHienTai();
            if (thamSo == null)
            {
                thongBao = "Không tìm thấy tham số hệ thống.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(docGia.TenDG))
            {
                thongBao = "Vui lòng nhập họ và tên độc giả.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(docGia.LoaiDG))
            {
                thongBao = "Vui lòng chọn loại độc giả.";
                return false;
            }

            int tuoiToiThieu = Convert.ToInt32(thamSo["SoTuoiDG"]);
            int tuoiToiDa = Convert.ToInt32(thamSo["SoTuoiDGToiDa"]);
            int tuoi = TinhTuoi(docGia.NgaySinhDG, docGia.NgLapThe);

            if (tuoi < tuoiToiThieu || tuoi > tuoiToiDa)
            {
                thongBao = "Tuổi độc giả phải từ " + tuoiToiThieu + " đến " + tuoiToiDa + ".";
                return false;
            }

            thongBao = string.Empty;
            return true;
        }

        private int TinhTuoi(DateTime ngaySinh, DateTime ngayThamChieu)
        {
            int tuoi = ngayThamChieu.Year - ngaySinh.Year;
            if (ngayThamChieu < ngaySinh.AddYears(tuoi))
            {
                tuoi--;
            }

            return tuoi;
        }
    }
}
