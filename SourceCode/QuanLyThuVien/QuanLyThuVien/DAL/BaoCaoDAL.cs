using System.Data;
using QuanLyThuVien.UTILS;

namespace QuanLyThuVien.DAL
{
    public class BaoCaoDAL
    {
        public DataTable LayBaoCaoMuonTheoTheLoai()
        {
            const string query = @"
                WITH ThangBaoCao AS
                (
                    SELECT
                        TOP 1
                        YEAR(pm.NgayMuon) AS Nam,
                        MONTH(pm.NgayMuon) AS Thang
                    FROM PhieuMuon pm
                    ORDER BY YEAR(pm.NgayMuon) DESC, MONTH(pm.NgayMuon) DESC
                ),
                BaoCaoMuonTheoTheLoai AS
                (
                    SELECT
                        tb.Thang,
                        tl.TenTheLoai AS TenTheLoai,
                        COUNT(ct.MaCTPM) AS SoLuotMuon
                    FROM ThangBaoCao tb
                    INNER JOIN PhieuMuon pm
                        ON YEAR(pm.NgayMuon) = tb.Nam
                       AND MONTH(pm.NgayMuon) = tb.Thang
                    INNER JOIN ChiTietPM ct ON pm.MaPhieu = ct.MaPhieu
                    INNER JOIN Sach s ON ct.MaSach = s.MaSach
                    LEFT JOIN TheLoai tl ON s.MaTheLoai = tl.MaTheLoai
                    GROUP BY tb.Thang, tl.TenTheLoai
                )
                SELECT
                    Thang,
                    TenTheLoai,
                    SoLuotMuon,
                    CAST(
                        SoLuotMuon * 100.0 /
                        NULLIF(SUM(SoLuotMuon) OVER (), 0)
                        AS DECIMAL(10,2)
                    ) AS TiLe
                FROM BaoCaoMuonTheoTheLoai
                ORDER BY TenTheLoai";

            return DbHelper.ExecuteQuery(query);
        }

        public DataTable LayBaoCaoTraTre()
        {
            const string query = @"
                SELECT
                    s.TenSach,
                    pm.NgayMuon,
                    DATEDIFF(DAY, pm.NgayPhaiTra, pm.NgayTra) AS SoNgayTraTre
                FROM PhieuMuon pm
                INNER JOIN ChiTietPM ct ON pm.MaPhieu = ct.MaPhieu
                INNER JOIN Sach s ON ct.MaSach = s.MaSach
                WHERE pm.NgayTra IS NOT NULL
                  AND pm.NgayTra > pm.NgayPhaiTra
                ORDER BY pm.NgayTra DESC";

            return DbHelper.ExecuteQuery(query);
        }
    }
}
