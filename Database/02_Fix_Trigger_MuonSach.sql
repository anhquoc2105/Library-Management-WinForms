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
            SELECT MaSach, -COUNT(*) AS Delta
            FROM inserted
            GROUP BY MaSach

            UNION ALL

            SELECT MaSach, COUNT(*) AS Delta
            FROM deleted
            GROUP BY MaSach
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

