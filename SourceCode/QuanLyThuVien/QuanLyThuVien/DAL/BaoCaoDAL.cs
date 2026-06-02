using System.Data;
using System.Data.SqlClient;
using QuanLyThuVien.UTILS;

namespace QuanLyThuVien.DAL
{
    public class BaoCaoDAL
    {
        public DataTable LayDanhSachThangBaoCao()
        {
            const string query = @"
                SELECT DISTINCT
                    YEAR(NgayMuon) AS Nam,
                    MONTH(NgayMuon) AS Thang,
                    RIGHT('0' + CAST(MONTH(NgayMuon) AS VARCHAR(2)), 2)
                        + '/' + CAST(YEAR(NgayMuon) AS VARCHAR(4)) AS ThangNam
                FROM PhieuMuon
                ORDER BY Nam DESC, Thang DESC";

            return DbHelper.ExecuteQuery(query);
        }

        public DataTable LayBaoCaoMuonTheoTheLoai(int nam, int thang)
        {
            const string query = @"
                WITH BaoCaoMuonTheoTheLoai AS
                (
                    SELECT
                        @Thang AS Thang,
                        @Nam AS Nam,
                        tl.TenTheLoai AS TenTheLoai,
                        COUNT(ct.MaCTPM) AS SoLuotMuon
                    FROM PhieuMuon pm
                    INNER JOIN ChiTietPM ct ON pm.MaPhieu = ct.MaPhieu
                    INNER JOIN Sach s ON ct.MaSach = s.MaSach
                    LEFT JOIN TheLoai tl ON s.MaTheLoai = tl.MaTheLoai
                    WHERE YEAR(pm.NgayMuon) = @Nam
                      AND MONTH(pm.NgayMuon) = @Thang
                    GROUP BY tl.TenTheLoai
                )
                SELECT
                    Nam,
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

            return DbHelper.ExecuteQuery(
                query,
                new SqlParameter("@Nam", nam),
                new SqlParameter("@Thang", thang));
        }

        public DataTable LayBaoCaoTraTre(int nam, int thang)
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
                  AND YEAR(pm.NgayMuon) = @Nam
                  AND MONTH(pm.NgayMuon) = @Thang
                ORDER BY ct.NgayTra DESC";

            return DbHelper.ExecuteQuery(
                query,
                new SqlParameter("@Nam", nam),
                new SqlParameter("@Thang", thang));
        }
    }
}
