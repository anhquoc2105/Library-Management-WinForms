using System.Data;
using QuanLyThuVien.UTILS;

namespace QuanLyThuVien.DAL
{
    public class ThamSoDAL
    {
        public DataTable LayDanhSachThamSo()
        {
            const string query = @"
                SELECT
                    MaThamSo,
                    GiaTriThe,
                    SoTuoiDG,
                    SoTuoiDGToiDa,
                    ThoiGianXB,
                    SoSachMuonToiDa,
                    SoNgayMuonToiDa,
                    TienPhat
                FROM ThamSo
                ORDER BY MaThamSo";

            return DbHelper.ExecuteQuery(query);
        }

        public DataRow LayThamSoHienTai()
        {
            DataTable dataTable = LayDanhSachThamSo();
            return dataTable.Rows.Count > 0 ? dataTable.Rows[0] : null;
        }
    }
}
