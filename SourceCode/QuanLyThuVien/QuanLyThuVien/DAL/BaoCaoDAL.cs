using System.Data;
using QuanLyThuVien.UTILS;

namespace QuanLyThuVien.DAL
{
    public class BaoCaoDAL
    {
        public DataTable LayBaoCaoMuonTheoTheLoai()
        {
            const string query = @"
                SELECT
                    tl.TenTheLoai AS TenTheLoai,
                    COUNT(ct.MaCTPM) AS SoLuotMuon,
                    CAST(
                        COUNT(ct.MaCTPM) * 100.0 /
                        NULLIF((SELECT COUNT(*) FROM ChiTietPM), 0)
                        AS DECIMAL(10,2)
                    ) AS TiLe
                FROM ChiTietPM ct
                INNER JOIN Sach s ON ct.MaSach = s.MaSach
                LEFT JOIN TheLoai tl ON s.MaTheLoai = tl.MaTheLoai
                GROUP BY tl.TenTheLoai
                ORDER BY tl.TenTheLoai";

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
