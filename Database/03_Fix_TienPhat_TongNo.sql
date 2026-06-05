USE QuanLyThuVien;
GO

CREATE OR ALTER TRIGGER dbo.TRG_PhieuMuon_HandleTraSach
ON dbo.PhieuMuon
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF TRIGGER_NESTLEVEL() > 1
        RETURN;

    DECLARE @TienPhat DECIMAL(18,2);
    SELECT TOP 1 @TienPhat = TienPhat
    FROM dbo.ThamSo
    WHERE MaTheLoai IS NULL
    ORDER BY MaThamSo;

    ;WITH ReturnedPhieu AS
    (
        SELECT i.MaPhieu, i.MaDG, i.NgayTra, i.NgayPhaiTra
        FROM inserted i
        INNER JOIN deleted d ON i.MaPhieu = d.MaPhieu
        WHERE d.NgayTra IS NULL
          AND i.NgayTra IS NOT NULL
    )
    UPDATE ct
    SET NgayTra = rp.NgayTra,
        TienPhatKyNay =
            CASE
                WHEN rp.NgayTra > rp.NgayPhaiTra
                THEN DATEDIFF(DAY, rp.NgayPhaiTra, rp.NgayTra) * @TienPhat
                ELSE 0
            END
    FROM dbo.ChiTietPM ct
    INNER JOIN ReturnedPhieu rp ON ct.MaPhieu = rp.MaPhieu
    WHERE ct.NgayTra IS NULL;

    ;WITH ChangedPhieu AS
    (
        SELECT MaPhieu FROM inserted
        UNION
        SELECT MaPhieu FROM deleted
    )
    UPDATE pm
    SET TienPhatKyNay = ISNULL(ct.TongTienPhatKyNay, 0)
    FROM dbo.PhieuMuon pm
    INNER JOIN ChangedPhieu cp ON pm.MaPhieu = cp.MaPhieu
    OUTER APPLY
    (
        SELECT SUM(TienPhatKyNay) AS TongTienPhatKyNay
        FROM dbo.ChiTietPM c
        WHERE c.MaPhieu = pm.MaPhieu
    ) ct;

    ;WITH AffectedDocGia AS
    (
        SELECT MaDG FROM inserted
        UNION
        SELECT MaDG FROM deleted
    )
    UPDATE dg
    SET TongNo =
        CASE
            WHEN ISNULL(f.TongTienPhat, 0) - ISNULL(pt.TongTienThu, 0) < 0 THEN 0
            ELSE ISNULL(f.TongTienPhat, 0) - ISNULL(pt.TongTienThu, 0)
        END
    FROM dbo.DocGia dg
    INNER JOIN AffectedDocGia adg ON dg.MaDG = adg.MaDG
    OUTER APPLY
    (
        SELECT SUM(ct.TienPhatKyNay) AS TongTienPhat
        FROM dbo.PhieuMuon pm
        INNER JOIN dbo.ChiTietPM ct ON pm.MaPhieu = ct.MaPhieu
        WHERE pm.MaDG = dg.MaDG
    ) f
    OUTER APPLY
    (
        SELECT SUM(SoTienThu) AS TongTienThu
        FROM dbo.PhieuThuTienPhat p
        WHERE p.MaDG = dg.MaDG
    ) pt;
END
GO

CREATE OR ALTER TRIGGER dbo.TRG_PhieuThuTienPhat_RecalculateTongNo
ON dbo.PhieuThuTienPhat
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH AffectedDocGia AS
    (
        SELECT MaDG FROM inserted
        UNION
        SELECT MaDG FROM deleted
    )
    UPDATE dg
    SET TongNo =
        CASE
            WHEN ISNULL(f.TongTienPhat, 0) - ISNULL(pt.TongTienThu, 0) < 0 THEN 0
            ELSE ISNULL(f.TongTienPhat, 0) - ISNULL(pt.TongTienThu, 0)
        END
    FROM dbo.DocGia dg
    INNER JOIN AffectedDocGia adg ON dg.MaDG = adg.MaDG
    OUTER APPLY
    (
        SELECT SUM(ct.TienPhatKyNay) AS TongTienPhat
        FROM dbo.PhieuMuon pm
        INNER JOIN dbo.ChiTietPM ct ON pm.MaPhieu = ct.MaPhieu
        WHERE pm.MaDG = dg.MaDG
    ) f
    OUTER APPLY
    (
        SELECT SUM(SoTienThu) AS TongTienThu
        FROM dbo.PhieuThuTienPhat p
        WHERE p.MaDG = dg.MaDG
    ) pt;
END
GO

UPDATE pm
SET TienPhatKyNay = ISNULL(ct.TongTienPhatKyNay, 0)
FROM dbo.PhieuMuon pm
OUTER APPLY
(
    SELECT SUM(TienPhatKyNay) AS TongTienPhatKyNay
    FROM dbo.ChiTietPM c
    WHERE c.MaPhieu = pm.MaPhieu
) ct;
GO

UPDATE dg
SET TongNo =
    CASE
        WHEN ISNULL(f.TongTienPhat, 0) - ISNULL(pt.TongTienThu, 0) < 0 THEN 0
        ELSE ISNULL(f.TongTienPhat, 0) - ISNULL(pt.TongTienThu, 0)
    END
FROM dbo.DocGia dg
OUTER APPLY
(
    SELECT SUM(ct.TienPhatKyNay) AS TongTienPhat
    FROM dbo.PhieuMuon pm
    INNER JOIN dbo.ChiTietPM ct ON pm.MaPhieu = ct.MaPhieu
    WHERE pm.MaDG = dg.MaDG
) f
OUTER APPLY
(
    SELECT SUM(SoTienThu) AS TongTienThu
    FROM dbo.PhieuThuTienPhat p
    WHERE p.MaDG = dg.MaDG
) pt;
GO
