using System;
using System.Data;
using System.Data.SqlClient;
using QuanLyThuVien.UTILS;

namespace QuanLyThuVien.DAL
{
    public class BaoCaoDAL
    {
        public DataTable LayBaoCaoMuonTheoTheLoai(DateTime ngayBaoCao)
        {
            const string query = @"
                WITH BaoCaoMuonTheoTheLoai AS
                (
                    SELECT
                        @NgayBaoCao AS NgayBaoCao,
                        tl.TenTheLoai AS TenTheLoai,
                        COUNT(ct.MaCTPM) AS SoLuotMuon
                    FROM PhieuMuon pm
                    INNER JOIN ChiTietPM ct ON pm.MaPhieu = ct.MaPhieu
                    INNER JOIN Sach s ON ct.MaSach = s.MaSach
                    LEFT JOIN TheLoai tl ON s.MaTheLoai = tl.MaTheLoai
                    WHERE CAST(pm.NgayMuon AS DATE) = @NgayBaoCao
                    GROUP BY tl.TenTheLoai
                )
                SELECT
                    NgayBaoCao,
                    TenTheLoai,
                    SoLuotMuon,
                    CAST(
                        SoLuotMuon * 100.0 /
                        NULLIF(SUM(SoLuotMuon) OVER (), 0)
                        AS DECIMAL(10,2)
                    ) AS TiLe
                FROM BaoCaoMuonTheoTheLoai
                ORDER BY TenTheLoai";

            return DbHelper.ExecuteQuery(
                query,
                new SqlParameter("@NgayBaoCao", ngayBaoCao.Date));
        }

        public DataTable LayBaoCaoTraTre(DateTime ngayTra)
        {
            const string query = @"
                SELECT
                    s.TenSach,
                    pm.NgayMuon,
                    ct.NgayTra,
                    DATEDIFF(DAY, pm.NgayPhaiTra, ct.NgayTra) AS SoNgayTraTre
                FROM PhieuMuon pm
                INNER JOIN ChiTietPM ct ON pm.MaPhieu = ct.MaPhieu
                INNER JOIN Sach s ON ct.MaSach = s.MaSach
                WHERE ct.NgayTra IS NOT NULL
                  AND ct.NgayTra > pm.NgayPhaiTra
                  AND CAST(ct.NgayTra AS DATE) = @NgayTra
                ORDER BY s.TenSach";

            return DbHelper.ExecuteQuery(
                query,
                new SqlParameter("@NgayTra", ngayTra.Date));
        }
    }
}
