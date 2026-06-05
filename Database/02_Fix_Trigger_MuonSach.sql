USE QuanLyThuVien;
GO

CREATE OR ALTER TRIGGER dbo.TRG_ChiTietPM_UpdateSLMuon
ON dbo.ChiTietPM
AFTER INSERT, DELETE, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @SoSachMuonToiDa INT;
    SELECT TOP 1 @SoSachMuonToiDa = SoSachMuonToiDa
    FROM dbo.ThamSo
    ORDER BY MaThamSo;

    IF EXISTS
    (
        SELECT 1
        FROM inserted i
        INNER JOIN dbo.PhieuMuon pm ON i.MaPhieu = pm.MaPhieu
        INNER JOIN dbo.DocGia dg ON pm.MaDG = dg.MaDG
        WHERE dg.NgayHetHan IS NULL
           OR dg.NgayHetHan < pm.NgayMuon
    )
    BEGIN
        RAISERROR(N'The doc gia da het han hoac chua co ngay het han.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    IF EXISTS
    (
        SELECT 1
        FROM inserted i
        INNER JOIN dbo.PhieuMuon pm ON i.MaPhieu = pm.MaPhieu
        WHERE EXISTS
        (
            SELECT 1
            FROM dbo.PhieuMuon pm2
            WHERE pm2.MaDG = pm.MaDG
              AND pm2.NgayTra IS NULL
              AND pm2.NgayPhaiTra < pm.NgayMuon
              AND pm2.MaPhieu <> pm.MaPhieu
        )
    )
    BEGIN
        RAISERROR(N'Doc gia dang co sach muon qua han chua tra.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    IF EXISTS
    (
        SELECT 1
        FROM inserted i
        INNER JOIN dbo.PhieuMuon pm ON i.MaPhieu = pm.MaPhieu
        WHERE EXISTS
        (
            SELECT 1
            FROM dbo.PhieuMuon pm2
            INNER JOIN dbo.ChiTietPM ct2 ON pm2.MaPhieu = ct2.MaPhieu
            WHERE pm2.MaDG = pm.MaDG
              AND ct2.MaSach = i.MaSach
              AND ct2.NgayTra IS NULL
              AND ct2.MaCTPM <> i.MaCTPM
        )
    )
    BEGIN
        RAISERROR(N'Doc gia dang muon quyen sach nay, khong the muon trung.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    IF EXISTS
    (
        SELECT 1
        FROM inserted i
        INNER JOIN dbo.PhieuMuon pm ON i.MaPhieu = pm.MaPhieu
        GROUP BY pm.MaDG
        HAVING
        (
            SELECT COUNT(*)
            FROM dbo.ChiTietPM ct
            INNER JOIN dbo.PhieuMuon pm2 ON ct.MaPhieu = pm2.MaPhieu
            WHERE pm2.MaDG = pm.MaDG
              AND pm2.NgayTra IS NULL
        ) > @SoSachMuonToiDa
    )
    BEGIN
        RAISERROR(N'Doc gia vuot qua so sach muon toi da.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    IF EXISTS
    (
        SELECT 1
        FROM inserted i
        INNER JOIN dbo.Sach s ON i.MaSach = s.MaSach
        LEFT JOIN
        (
            SELECT MaSach, COUNT(*) AS SoLuongXoa
            FROM deleted
            GROUP BY MaSach
        ) d ON i.MaSach = d.MaSach
        WHERE s.SoLuongTon + ISNULL(d.SoLuongXoa, 0) - 1 < 0
    )
    BEGIN
        RAISERROR(N'Sach khong con trong kho.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    ;WITH DeltaSach AS
    (
        SELECT MaSach, SUM(Delta) AS SoLuongThayDoi
        FROM
        (
            SELECT i.MaSach, -COUNT(*) AS Delta
            FROM inserted i
            INNER JOIN dbo.PhieuMuon pm ON i.MaPhieu = pm.MaPhieu
            WHERE pm.NgayTra IS NULL
            GROUP BY i.MaSach

            UNION ALL

            SELECT d.MaSach, COUNT(*) AS Delta
            FROM deleted d
            INNER JOIN dbo.PhieuMuon pm ON d.MaPhieu = pm.MaPhieu
            WHERE pm.NgayTra IS NULL
            GROUP BY d.MaSach
        ) x
        GROUP BY MaSach
    )
    UPDATE s
    SET SoLuongTon = SoLuongTon + d.SoLuongThayDoi
    FROM dbo.Sach s
    INNER JOIN DeltaSach d ON s.MaSach = d.MaSach;

    ;WITH ChangedSach AS
    (
        SELECT MaSach FROM inserted
        UNION
        SELECT MaSach FROM deleted
    )
    UPDATE s
    SET TinhTrang = CASE WHEN s.SoLuongTon > 0 THEN N'Con' ELSE N'Dang muon' END
    FROM dbo.Sach s
    INNER JOIN ChangedSach cs ON s.MaSach = cs.MaSach
    WHERE s.TinhTrang IN (N'Con', N'Dang muon');

    ;WITH ChangedPhieu AS
    (
        SELECT MaPhieu FROM inserted
        UNION
        SELECT MaPhieu FROM deleted
    )
    UPDATE pm
    SET SLMuon = ISNULL(ct.SoLuong, 0)
    FROM dbo.PhieuMuon pm
    INNER JOIN ChangedPhieu cp ON pm.MaPhieu = cp.MaPhieu
    OUTER APPLY
    (
        SELECT COUNT(*) AS SoLuong
        FROM dbo.ChiTietPM c
        WHERE c.MaPhieu = pm.MaPhieu
    ) ct;
END
GO

;WITH SoLuongDangMuon AS
(
    SELECT ct.MaSach, COUNT(*) AS SoLuong
    FROM dbo.ChiTietPM ct
    INNER JOIN dbo.PhieuMuon pm ON ct.MaPhieu = pm.MaPhieu
    WHERE pm.NgayTra IS NULL
    GROUP BY ct.MaSach
)
UPDATE s
SET
    SoLuongTon =
        CASE
            WHEN s.SoLuongTon = 0 AND ISNULL(dm.SoLuong, 0) = 0 AND s.TinhTrang = N'Dang muon'
            THEN 1
            ELSE s.SoLuongTon
        END,
    TinhTrang =
        CASE
            WHEN s.TinhTrang IN (N'Hong', N'Mat') THEN s.TinhTrang
            WHEN
                CASE
                    WHEN s.SoLuongTon = 0 AND ISNULL(dm.SoLuong, 0) = 0 AND s.TinhTrang = N'Dang muon'
                    THEN 1
                    ELSE s.SoLuongTon
                END > 0
            THEN N'Con'
            ELSE N'Dang muon'
        END
FROM dbo.Sach s
LEFT JOIN SoLuongDangMuon dm ON s.MaSach = dm.MaSach;
GO
