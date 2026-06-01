using System.Data;
using System.Data.SqlClient;
using QuanLyThuVien.UTILS;

namespace QuanLyThuVien.DAL
{
    public class ThamSoDAL
    {
        private const string SelectThamSoColumns = @"
                    MaThamSo,
                    GiaTriThe,
                    SoTuoiDG,
                    SoTuoiDGToiDa,
                    ThoiGianXB,
                    SoSachMuonToiDa,
                    SoNgayMuonToiDa,
                    TienPhat";

        public DataTable LayDanhSachThamSo()
        {
            const string query = @"
                SELECT
                    " + SelectThamSoColumns + @"
                FROM ThamSo
                ORDER BY MaThamSo";

            return DbHelper.ExecuteQuery(query);
        }

        public DataRow LayThamSoHienTai()
        {
            DataTable dataTable = LayDanhSachThamSo();
            return dataTable.Rows.Count > 0 ? dataTable.Rows[0] : null;
        }

        public DataRow LayThamSoTheoTheLoai(int maTheLoai)
        {
            DamBaoThamSoTheoTheLoai();

            const string query = @"
                SELECT TOP 1
                    " + SelectThamSoColumns + @"
                FROM ThamSo
                WHERE MaTheLoai = @MaTheLoai
                ORDER BY MaThamSo";

            DataTable dataTable = DbHelper.ExecuteQuery(
                query,
                new SqlParameter("@MaTheLoai", maTheLoai));

            return dataTable.Rows.Count > 0 ? dataTable.Rows[0] : null;
        }

        private void DamBaoThamSoTheoTheLoai()
        {
            // Bước 1: Thêm cột MaTheLoai nếu chưa có
            const string alterQuery = @"
                IF COL_LENGTH('dbo.ThamSo', 'MaTheLoai') IS NULL
                BEGIN
                    ALTER TABLE dbo.ThamSo ADD MaTheLoai INT NULL;
                END;";

            DbHelper.ExecuteNonQuery(alterQuery);

            // Bước 2: Tạo tham số mặc định cho các thể loại chưa có (dùng EXEC để tránh lỗi compile)
            const string insertQuery = @"
                EXEC('
                    DECLARE @GiaTriThe INT;
                    DECLARE @SoTuoiDG INT;
                    DECLARE @SoTuoiDGToiDa INT;
                    DECLARE @ThoiGianXB INT;
                    DECLARE @SoSachMuonToiDa INT;
                    DECLARE @SoNgayMuonToiDa INT;
                    DECLARE @TienPhat DECIMAL(18,2);

                    SELECT TOP 1
                        @GiaTriThe = GiaTriThe,
                        @SoTuoiDG = SoTuoiDG,
                        @SoTuoiDGToiDa = SoTuoiDGToiDa,
                        @ThoiGianXB = ThoiGianXB,
                        @SoSachMuonToiDa = SoSachMuonToiDa,
                        @SoNgayMuonToiDa = SoNgayMuonToiDa,
                        @TienPhat = TienPhat
                    FROM dbo.ThamSo
                    ORDER BY MaThamSo;

                    INSERT INTO dbo.ThamSo
                    (
                        GiaTriThe,
                        SoTuoiDG,
                        SoTuoiDGToiDa,
                        ThoiGianXB,
                        SoSachMuonToiDa,
                        SoNgayMuonToiDa,
                        TienPhat,
                        MaTheLoai
                    )
                    SELECT
                        @GiaTriThe,
                        @SoTuoiDG,
                        @SoTuoiDGToiDa,
                        @ThoiGianXB,
                        @SoSachMuonToiDa,
                        @SoNgayMuonToiDa,
                        @TienPhat,
                        tl.MaTheLoai
                    FROM dbo.TheLoai tl
                    WHERE NOT EXISTS
                    (
                        SELECT 1
                        FROM dbo.ThamSo ts
                        WHERE ts.MaTheLoai = tl.MaTheLoai
                    );
                ')";

            DbHelper.ExecuteNonQuery(insertQuery);
        }
    }
}
