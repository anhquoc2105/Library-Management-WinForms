using System;
using System.Data;
using System.Data.SqlClient;
using QuanLyThuVien.UTILS;

namespace QuanLyThuVien.DAL
{
    public class BaoCaoDAL
    {
        public DataTable LayBaoCaoSachMuon(DateTime ngayBaoCao)
        {
            const string query = @"
                SELECT
                    s.TenSach,
                    dg.TenDG,
                    pm.NgayMuon
                FROM PhieuMuon pm
                INNER JOIN DocGia dg ON pm.MaDG = dg.MaDG
                INNER JOIN ChiTietPM ct ON pm.MaPhieu = ct.MaPhieu
                INNER JOIN Sach s ON ct.MaSach = s.MaSach
                WHERE CAST(pm.NgayMuon AS DATE) = @NgayBaoCao
                ORDER BY pm.NgayMuon, s.TenSach";

            return DbHelper.ExecuteQuery(
                query,
                new SqlParameter("@NgayBaoCao", ngayBaoCao.Date));
        }

        public DataTable LayBaoCaoSachTra(DateTime ngayBaoCao)
        {
            const string query = @"
                SELECT
                    s.TenSach,
                    dg.TenDG,
                    pm.NgayMuon,
                    ct.NgayTra,
                    CASE
                        WHEN DATEDIFF(DAY, DATEADD(DAY, ISNULL(ts.SoNgayMuonToiDa, 0), CAST(pm.NgayMuon AS DATE)), CAST(ct.NgayTra AS DATE)) > 0
                        THEN DATEDIFF(DAY, DATEADD(DAY, ISNULL(ts.SoNgayMuonToiDa, 0), CAST(pm.NgayMuon AS DATE)), CAST(ct.NgayTra AS DATE))
                        ELSE 0
                    END AS SoNgayTre
                FROM PhieuMuon pm
                INNER JOIN DocGia dg ON pm.MaDG = dg.MaDG
                INNER JOIN ChiTietPM ct ON pm.MaPhieu = ct.MaPhieu
                INNER JOIN Sach s ON ct.MaSach = s.MaSach
                OUTER APPLY (
                    SELECT TOP 1 SoNgayMuonToiDa
                    FROM ThamSo
                    WHERE MaTheLoai IS NULL
                    ORDER BY MaThamSo
                ) ts
                WHERE ct.NgayTra IS NOT NULL
                  AND CAST(ct.NgayTra AS DATE) = @NgayBaoCao
                ORDER BY ct.NgayTra, s.TenSach";

            return DbHelper.ExecuteQuery(
                query,
                new SqlParameter("@NgayBaoCao", ngayBaoCao.Date));
        }
    }
}
