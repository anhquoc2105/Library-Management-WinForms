IF DB_ID(N'QuanLyThuVien') IS NULL
BEGIN
    CREATE DATABASE QuanLyThuVien;
END
GO

USE QuanLyThuVien;
GO

IF OBJECT_ID(N'dbo.TRG_PhieuMuon_SetNgayPhaiTra', N'TR') IS NOT NULL
    DROP TRIGGER dbo.TRG_PhieuMuon_SetNgayPhaiTra;
GO

IF OBJECT_ID(N'dbo.TRG_PhieuMuon_HandleTraSach', N'TR') IS NOT NULL
    DROP TRIGGER dbo.TRG_PhieuMuon_HandleTraSach;
GO

IF OBJECT_ID(N'dbo.TRG_DocGia_SetNgayHetHan', N'TR') IS NOT NULL
    DROP TRIGGER dbo.TRG_DocGia_SetNgayHetHan;
GO

IF OBJECT_ID(N'dbo.TRG_DocGia_ValidateTuoi', N'TR') IS NOT NULL
    DROP TRIGGER dbo.TRG_DocGia_ValidateTuoi;
GO

IF OBJECT_ID(N'dbo.TRG_ChiTietPM_UpdateSLMuon', N'TR') IS NOT NULL
    DROP TRIGGER dbo.TRG_ChiTietPM_UpdateSLMuon;
GO

IF OBJECT_ID(N'dbo.TRG_Sach_ValidateNamXB', N'TR') IS NOT NULL
    DROP TRIGGER dbo.TRG_Sach_ValidateNamXB;
GO

IF OBJECT_ID(N'dbo.TRG_PhieuThuTienPhat_RecalculateTongNo', N'TR') IS NOT NULL
    DROP TRIGGER dbo.TRG_PhieuThuTienPhat_RecalculateTongNo;
GO

IF OBJECT_ID(N'dbo.PhieuThuTienPhat', N'U') IS NOT NULL
    DROP TABLE dbo.PhieuThuTienPhat;
GO

IF OBJECT_ID(N'dbo.ChiTietPM', N'U') IS NOT NULL
    DROP TABLE dbo.ChiTietPM;
GO

IF OBJECT_ID(N'dbo.PhieuMuon', N'U') IS NOT NULL
    DROP TABLE dbo.PhieuMuon;
GO

IF OBJECT_ID(N'dbo.Sach', N'U') IS NOT NULL
    DROP TABLE dbo.Sach;
GO

IF OBJECT_ID(N'dbo.ThuThu', N'U') IS NOT NULL
    DROP TABLE dbo.ThuThu;
GO

IF OBJECT_ID(N'dbo.DocGia', N'U') IS NOT NULL
    DROP TABLE dbo.DocGia;
GO

IF OBJECT_ID(N'dbo.TaiKhoan', N'U') IS NOT NULL
    DROP TABLE dbo.TaiKhoan;
GO

IF OBJECT_ID(N'dbo.TacGia', N'U') IS NOT NULL
    DROP TABLE dbo.TacGia;
GO

IF OBJECT_ID(N'dbo.TheLoai', N'U') IS NOT NULL
    DROP TABLE dbo.TheLoai;
GO

IF OBJECT_ID(N'dbo.ThamSo', N'U') IS NOT NULL
    DROP TABLE dbo.ThamSo;
GO

CREATE TABLE dbo.ThamSo
(
    MaThamSo INT IDENTITY(1,1) PRIMARY KEY,
    GiaTriThe INT NOT NULL,
    SoTuoiDG INT NOT NULL,
    SoTuoiDGToiDa INT NOT NULL,
    ThoiGianXB INT NOT NULL,
    SoSachMuonToiDa INT NOT NULL,
    SoNgayMuonToiDa INT NOT NULL,
    TienPhat DECIMAL(18,2) NOT NULL,
    MaTheLoai INT NULL,
    CONSTRAINT CK_ThamSo_GiaTriThe CHECK (GiaTriThe > 0),
    CONSTRAINT CK_ThamSo_SoTuoiDG CHECK (SoTuoiDG > 0),
    CONSTRAINT CK_ThamSo_SoTuoiDGToiDa CHECK (SoTuoiDGToiDa >= SoTuoiDG),
    CONSTRAINT CK_ThamSo_ThoiGianXB CHECK (ThoiGianXB >= 0),
    CONSTRAINT CK_ThamSo_SoSachMuonToiDa CHECK (SoSachMuonToiDa > 0),
    CONSTRAINT CK_ThamSo_SoNgayMuonToiDa CHECK (SoNgayMuonToiDa > 0),
    CONSTRAINT CK_ThamSo_TienPhat CHECK (TienPhat >= 0)
);
GO

CREATE TABLE dbo.TheLoai
(
    MaTheLoai INT IDENTITY(1,1) PRIMARY KEY,
    TenTheLoai NVARCHAR(50) NOT NULL,
    CONSTRAINT UQ_TheLoai_TenTheLoai UNIQUE (TenTheLoai)
);
GO

CREATE TABLE dbo.TacGia
(
    MaTacGia INT IDENTITY(1,1) PRIMARY KEY,
    TenTacGia NVARCHAR(100) NOT NULL,
    CONSTRAINT UQ_TacGia_TenTacGia UNIQUE (TenTacGia)
);
GO

CREATE TABLE dbo.TaiKhoan
(
    MaTaiKhoan INT IDENTITY(1,1) PRIMARY KEY,
    TenDangNhap VARCHAR(50) NOT NULL,
    MatKhau VARCHAR(255) NOT NULL,
    CONSTRAINT UQ_TaiKhoan_TenDangNhap UNIQUE (TenDangNhap)
);
GO

CREATE TABLE dbo.DocGia
(
    MaDG INT IDENTITY(1,1) PRIMARY KEY,
    TenDG NVARCHAR(100) NOT NULL,
    LoaiDG NVARCHAR(10) NOT NULL,
    NgaySinhDG DATE NOT NULL,
    DiaChiDG NVARCHAR(200) NULL,
    EmailDG VARCHAR(100) NULL,
    NgLapThe DATE NOT NULL DEFAULT GETDATE(),
    NgayHetHan DATE NULL,
    TongNo DECIMAL(18,2) NOT NULL DEFAULT 0,
    MaTaiKhoan INT NULL,
    CONSTRAINT FK_DocGia_TaiKhoan
        FOREIGN KEY (MaTaiKhoan) REFERENCES dbo.TaiKhoan(MaTaiKhoan),
    CONSTRAINT UQ_DocGia_EmailDG UNIQUE (EmailDG),
    CONSTRAINT UQ_DocGia_MaTaiKhoan UNIQUE (MaTaiKhoan),
    CONSTRAINT CK_DocGia_LoaiDG CHECK (LoaiDG IN (N'X', N'Y')),
    CONSTRAINT CK_DocGia_TongNo CHECK (TongNo >= 0)
);
GO

CREATE TABLE dbo.ThuThu
(
    MaTT INT IDENTITY(1,1) PRIMARY KEY,
    TenTT NVARCHAR(100) NOT NULL,
    GioiTinhTT NVARCHAR(10) NULL,
    NgaySinhTT DATE NULL,
    EmailTT VARCHAR(100) NULL,
    DiaChiTT NVARCHAR(200) NULL,
    GhiChu NVARCHAR(200) NULL,
    MaTaiKhoan INT NULL,
    CONSTRAINT FK_ThuThu_TaiKhoan
        FOREIGN KEY (MaTaiKhoan) REFERENCES dbo.TaiKhoan(MaTaiKhoan),
    CONSTRAINT UQ_ThuThu_EmailTT UNIQUE (EmailTT),
    CONSTRAINT UQ_ThuThu_MaTaiKhoan UNIQUE (MaTaiKhoan)
);
GO

CREATE TABLE dbo.Sach
(
    MaSach INT IDENTITY(1,1) PRIMARY KEY,
    TenSach NVARCHAR(200) NOT NULL,
    ChuDe NVARCHAR(100) NOT NULL,
    MaTheLoai INT NULL,
    TenTG NVARCHAR(100) NOT NULL,
    MaTacGia INT NULL,
    NamXB INT NOT NULL,
    NhaXB NVARCHAR(100) NOT NULL,
    NgayNhap DATE NOT NULL DEFAULT GETDATE(),
    TriGia DECIMAL(18,2) NOT NULL DEFAULT 0,
    SoLuongTon INT NOT NULL DEFAULT 1,
    TinhTrang NVARCHAR(30) NOT NULL DEFAULT N'Con',
    CONSTRAINT FK_Sach_TheLoai
        FOREIGN KEY (MaTheLoai) REFERENCES dbo.TheLoai(MaTheLoai),
    CONSTRAINT FK_Sach_TacGia
        FOREIGN KEY (MaTacGia) REFERENCES dbo.TacGia(MaTacGia),
    CONSTRAINT CK_Sach_TriGia CHECK (TriGia >= 0),
    CONSTRAINT CK_Sach_SoLuongTon CHECK (SoLuongTon >= 0),
    CONSTRAINT CK_Sach_NamXB CHECK (NamXB >= 1900),
    CONSTRAINT CK_Sach_TinhTrang CHECK (TinhTrang IN (N'Con', N'Dang muon', N'Hong', N'Mat'))
);
GO

CREATE TABLE dbo.PhieuMuon
(
    MaPhieu INT IDENTITY(1,1) PRIMARY KEY,
    MaDG INT NOT NULL,
    NgayMuon DATE NOT NULL DEFAULT GETDATE(),
    NgayPhaiTra DATE NULL,
    NgayTra DATE NULL,
    SLMuon INT NOT NULL DEFAULT 0,
    TienPhatKyNay DECIMAL(18,2) NOT NULL DEFAULT 0,
    CONSTRAINT FK_PhieuMuon_DocGia
        FOREIGN KEY (MaDG) REFERENCES dbo.DocGia(MaDG),
    CONSTRAINT CK_PhieuMuon_SLMuon CHECK (SLMuon >= 0),
    CONSTRAINT CK_PhieuMuon_TienPhatKyNay CHECK (TienPhatKyNay >= 0),
    CONSTRAINT CK_PhieuMuon_NgayTra CHECK (NgayTra IS NULL OR NgayTra >= NgayMuon)
);
GO

CREATE TABLE dbo.ChiTietPM
(
    MaCTPM INT IDENTITY(1,1) PRIMARY KEY,
    MaPhieu INT NOT NULL,
    MaSach INT NOT NULL,
    NgayThang DATE NOT NULL DEFAULT GETDATE(),
    NgayTra DATE NULL,
    TienPhatKyNay DECIMAL(18,2) NOT NULL DEFAULT 0,
    SoLanMuon INT NOT NULL DEFAULT 1,
    CONSTRAINT FK_ChiTietPM_PhieuMuon
        FOREIGN KEY (MaPhieu) REFERENCES dbo.PhieuMuon(MaPhieu),
    CONSTRAINT FK_ChiTietPM_Sach
        FOREIGN KEY (MaSach) REFERENCES dbo.Sach(MaSach),
    CONSTRAINT UQ_ChiTietPM_MaPhieu_MaSach UNIQUE (MaPhieu, MaSach),
    CONSTRAINT CK_ChiTietPM_SoLanMuon CHECK (SoLanMuon > 0),
    CONSTRAINT CK_ChiTietPM_TienPhatKyNay CHECK (TienPhatKyNay >= 0),
    CONSTRAINT CK_ChiTietPM_NgayTra CHECK (NgayTra IS NULL OR NgayTra >= NgayThang)
);
GO

CREATE TABLE dbo.PhieuThuTienPhat
(
    MaPhieuThu INT IDENTITY(1,1) PRIMARY KEY,
    MaDG INT NOT NULL,
    NgayThu DATE NOT NULL DEFAULT GETDATE(),
    TongNoLucThu DECIMAL(18,2) NOT NULL,
    SoTienThu DECIMAL(18,2) NOT NULL,
    ConLai DECIMAL(18,2) NOT NULL,
    CONSTRAINT FK_PhieuThuTienPhat_DocGia
        FOREIGN KEY (MaDG) REFERENCES dbo.DocGia(MaDG),
    CONSTRAINT CK_PhieuThuTienPhat_TongNo CHECK (TongNoLucThu >= 0),
    CONSTRAINT CK_PhieuThuTienPhat_SoTienThu CHECK (SoTienThu >= 0),
    CONSTRAINT CK_PhieuThuTienPhat_ConLai CHECK (ConLai >= 0),
    CONSTRAINT CK_PhieuThuTienPhat_KhongVuotNo CHECK (SoTienThu <= TongNoLucThu)
);
GO

CREATE INDEX IX_DocGia_MaTaiKhoan ON dbo.DocGia(MaTaiKhoan);
CREATE INDEX IX_ThuThu_MaTaiKhoan ON dbo.ThuThu(MaTaiKhoan);
CREATE INDEX IX_Sach_MaTheLoai ON dbo.Sach(MaTheLoai);
CREATE INDEX IX_Sach_MaTacGia ON dbo.Sach(MaTacGia);
CREATE INDEX IX_PhieuMuon_MaDG ON dbo.PhieuMuon(MaDG);
CREATE INDEX IX_ChiTietPM_MaPhieu ON dbo.ChiTietPM(MaPhieu);
CREATE INDEX IX_ChiTietPM_MaSach ON dbo.ChiTietPM(MaSach);
CREATE INDEX IX_PhieuThuTienPhat_MaDG ON dbo.PhieuThuTienPhat(MaDG);
GO

CREATE TRIGGER dbo.TRG_ChiTietPM_UpdateSLMuon
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
        RAISERROR(N'Thẻ độc giả đã hết hạn hoặc chưa có ngày hết hạn.', 16, 1);
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
              AND ct2.NgayTra IS NULL
              AND pm2.NgayPhaiTra < pm.NgayMuon
              AND pm2.MaPhieu <> pm.MaPhieu
        )
    )
    BEGIN
        RAISERROR(N'Độc giả đang có sách mượn quá hạn chưa trả.', 16, 1);
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
              AND ct.NgayTra IS NULL
        ) > @SoSachMuonToiDa
    )
    BEGIN
        RAISERROR(N'Độc giả vượt quá số sách mượn tối đa.', 16, 1);
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
          AND i.NgayTra IS NULL
    )
    BEGIN
        RAISERROR(N'Sách không còn trong kho.', 16, 1);
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
            WHERE i.NgayTra IS NULL
            GROUP BY i.MaSach

            UNION ALL

            SELECT d.MaSach, COUNT(*) AS Delta
            FROM deleted d
            WHERE d.NgayTra IS NULL
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
          AND c.NgayTra IS NULL
    ) ct;
END
GO

CREATE TRIGGER dbo.TRG_DocGia_SetNgayHetHan
ON dbo.DocGia
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @GiaTriThe INT;
    SELECT TOP 1 @GiaTriThe = GiaTriThe
    FROM dbo.ThamSo
    ORDER BY MaThamSo;

    UPDATE dg
    SET NgayHetHan = DATEADD(MONTH, @GiaTriThe, dg.NgLapThe)
    FROM dbo.DocGia dg
    INNER JOIN inserted i ON dg.MaDG = i.MaDG
    WHERE dg.NgLapThe IS NOT NULL;
END
GO

CREATE TRIGGER dbo.TRG_DocGia_ValidateTuoi
ON dbo.DocGia
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @SoTuoiDG INT;
    DECLARE @SoTuoiDGToiDa INT;

    SELECT TOP 1
        @SoTuoiDG = SoTuoiDG,
        @SoTuoiDGToiDa = SoTuoiDGToiDa
    FROM dbo.ThamSo
    ORDER BY MaThamSo;

    IF EXISTS
    (
        SELECT 1
        FROM inserted i
        CROSS APPLY
        (
            SELECT
                DATEDIFF(YEAR, i.NgaySinhDG, i.NgLapThe)
                - CASE
                    WHEN DATEADD(YEAR, DATEDIFF(YEAR, i.NgaySinhDG, i.NgLapThe), i.NgaySinhDG) > i.NgLapThe
                    THEN 1 ELSE 0
                  END AS Tuoi
        ) t
        WHERE t.Tuoi < @SoTuoiDG OR t.Tuoi > @SoTuoiDGToiDa
    )
    BEGIN
        RAISERROR(N'Tuổi độc giả phải nằm trong khoảng quy định.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
END
GO

CREATE TRIGGER dbo.TRG_PhieuMuon_SetNgayPhaiTra
ON dbo.PhieuMuon
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF TRIGGER_NESTLEVEL() > 1
        RETURN;

    DECLARE @SoNgayMuonToiDa INT;
    SELECT TOP 1 @SoNgayMuonToiDa = SoNgayMuonToiDa
    FROM dbo.ThamSo
    ORDER BY MaThamSo;

    UPDATE pm
    SET NgayPhaiTra = DATEADD(DAY, @SoNgayMuonToiDa, pm.NgayMuon)
    FROM dbo.PhieuMuon pm
    INNER JOIN inserted i ON pm.MaPhieu = i.MaPhieu
    WHERE pm.NgayMuon IS NOT NULL;
END
GO

CREATE TRIGGER dbo.TRG_PhieuMuon_HandleTraSach
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
    ORDER BY MaThamSo;

    ;WITH ReturnedPhieu AS
    (
        SELECT i.MaPhieu, i.MaDG, i.NgayTra, i.NgayPhaiTra
        FROM inserted i
        INNER JOIN deleted d ON i.MaPhieu = d.MaPhieu
        WHERE d.NgayTra IS NULL
          AND i.NgayTra IS NOT NULL
    )
    UPDATE pm
    SET TienPhatKyNay =
        CASE
            WHEN rp.NgayTra > rp.NgayPhaiTra
            THEN DATEDIFF(DAY, rp.NgayPhaiTra, rp.NgayTra) * @TienPhat
            ELSE 0
        END
    FROM dbo.PhieuMuon pm
    INNER JOIN ReturnedPhieu rp ON pm.MaPhieu = rp.MaPhieu;

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
        SELECT SUM(TienPhatKyNay) AS TongTienPhat
        FROM dbo.PhieuMuon pm
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

CREATE TRIGGER dbo.TRG_Sach_ValidateNamXB
ON dbo.Sach
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ThoiGianXB INT;
    SELECT TOP 1 @ThoiGianXB = ThoiGianXB
    FROM dbo.ThamSo
    WHERE MaTheLoai IS NULL
    ORDER BY MaThamSo;

    IF EXISTS
    (
        SELECT 1
        FROM inserted i
        WHERE YEAR(GETDATE()) - i.NamXB > @ThoiGianXB
           OR i.NamXB > YEAR(GETDATE())
    )
    BEGIN
        RAISERROR(N'Chỉ nhận sách xuất bản trong khoảng năm hợp lệ.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
END
GO

CREATE TRIGGER dbo.TRG_PhieuThuTienPhat_RecalculateTongNo
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
        SELECT SUM(TienPhatKyNay) AS TongTienPhat
        FROM dbo.PhieuMuon pm
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

INSERT INTO dbo.ThamSo
(
    GiaTriThe,
    SoTuoiDG,
    SoTuoiDGToiDa,
    ThoiGianXB,
    SoSachMuonToiDa,
    SoNgayMuonToiDa,
    TienPhat
)
VALUES
    (6, 18, 55, 8, 5, 4, 1000);
GO

INSERT INTO dbo.TheLoai (TenTheLoai)
VALUES
    (N'A'),
    (N'B'),
    (N'C'),
    (N'D'),
    (N'E');
GO

;WITH Numbers AS
(
    SELECT 1 AS N
    UNION ALL
    SELECT N + 1
    FROM Numbers
    WHERE N < 100
)
INSERT INTO dbo.TacGia (TenTacGia)
SELECT N'Tác giả ' + RIGHT('000' + CAST(N AS NVARCHAR(3)), 3)
FROM Numbers
OPTION (MAXRECURSION 100);
GO

INSERT INTO dbo.TaiKhoan (TenDangNhap, MatKhau)
VALUES
    ('docgia01', '123456'),
    ('docgia02', '123456'),
    ('docgia03', '123456'),
    ('docgia04', '123456'),
    ('docgia05', '123456'),
    ('docgia06', '123456'),
    ('thuthu01', 'admin123'),
    ('thuthu02', 'admin123'),
    ('docgia07', '123456'),
    ('docgia08', '123456'),
    ('docgia09', '123456');
GO

INSERT INTO dbo.DocGia
(
    TenDG,
    LoaiDG,
    NgaySinhDG,
    DiaChiDG,
    EmailDG,
    NgLapThe,
    NgayHetHan,
    TongNo,
    MaTaiKhoan
)
VALUES
    (N'Nguyễn Văn A', N'X', '2004-03-15', N'98 Yên Đỗ', 'a@gmail.com', '2026-05-01', NULL, 0, 1),
    (N'Trần Thị B', N'Y', '1990-09-20', N'12 Lê Lợi', 'b@gmail.com', '2026-05-02', NULL, 2000, 2),
    (N'Lê Minh Quốc', N'X', '2000-05-16', N'Dĩ An, Bình Dương', 'quoc@gmail.com', '2026-05-16', NULL, 0, 3),
    (N'Phạm Hoàng An', N'X', '1998-11-02', N'Thủ Đức, TP.HCM', 'an@gmail.com', '2026-05-18', NULL, 0, 4),
    (N'Võ Ngọc Nhi', N'Y', '2002-08-21', N'Biên Hòa, Đồng Nai', 'nhi@gmail.com', '2026-05-20', NULL, 0, 5),
    (N'Đặng Thành Tín', N'Y', '1995-12-09', N'Quận 1, TP.HCM', 'tin@gmail.com', '2026-05-22', NULL, 1000, 6),
    (N'Nguyễn Thị Mai', N'X', '2001-01-10', N'TP.HCM', 'mai@gmail.com', '2026-06-01', NULL, 0, 9),
    (N'Trần Quốc Huy', N'Y', '1999-02-20', N'Đồng Nai', 'huy@gmail.com', '2026-06-01', NULL, 0, 10),
    (N'Hoàng Gia Bảo', N'X', '2003-03-30', N'Bình Dương', 'bao@gmail.com', '2026-06-01', NULL, 0, 11);
GO

INSERT INTO dbo.ThuThu
(
    TenTT,
    GioiTinhTT,
    NgaySinhTT,
    EmailTT,
    DiaChiTT,
    GhiChu,
    MaTaiKhoan
)
VALUES
    (N'Lê Thị Thu', N'Nữ', '1998-07-10', 'thuthu01@gmail.com', N'Đồng Nai', N'Thủ thư chính', 7),
    (N'Nguyễn Minh Khang', N'Nam', '1996-04-12', 'thuthu02@gmail.com', N'TP.HCM', N'Hỗ trợ ca chiều', 8);
GO

INSERT INTO dbo.Sach
(
    TenSach,
    ChuDe,
    MaTheLoai,
    TenTG,
    MaTacGia,
    NamXB,
    NhaXB,
    NgayNhap,
    TriGia,
    SoLuongTon,
    TinhTrang
)
VALUES
    (
        N'CNPM',
        N'A',
        (SELECT MaTheLoai FROM dbo.TheLoai WHERE TenTheLoai = N'A'),
        N'Tác giả 001',
        (SELECT MaTacGia FROM dbo.TacGia WHERE TenTacGia = N'Tác giả 001'),
        2023,
        N'NXB Trẻ',
        '2026-05-01',
        30000,
        2,
        N'Con'
    ),
    (
        N'Lập Trình Cơ Bản',
        N'B',
        (SELECT MaTheLoai FROM dbo.TheLoai WHERE TenTheLoai = N'B'),
        N'Tác giả 002',
        (SELECT MaTacGia FROM dbo.TacGia WHERE TenTacGia = N'Tác giả 002'),
        2022,
        N'NXB Giáo Dục',
        '2026-05-03',
        85000,
        2,
        N'Con'
    ),
    (
        N'Thuật Toán',
        N'C',
        (SELECT MaTheLoai FROM dbo.TheLoai WHERE TenTheLoai = N'C'),
        N'Tác giả 003',
        (SELECT MaTacGia FROM dbo.TacGia WHERE TenTacGia = N'Tác giả 003'),
        2021,
        N'NXB Lao Động',
        '2026-05-04',
        92000,
        2,
        N'Con'
    ),
    (
        N'SQL Server Thực Hành',
        N'A',
        (SELECT MaTheLoai FROM dbo.TheLoai WHERE TenTheLoai = N'A'),
        N'Tác giả 004',
        (SELECT MaTacGia FROM dbo.TacGia WHERE TenTacGia = N'Tác giả 004'),
        2024,
        N'NXB Thanh Niên',
        '2026-05-05',
        96000,
        1,
        N'Con'
    ),
    (
        N'Cấu Trúc Dữ Liệu',
        N'B',
        (SELECT MaTheLoai FROM dbo.TheLoai WHERE TenTheLoai = N'B'),
        N'Tác giả 005',
        (SELECT MaTacGia FROM dbo.TacGia WHERE TenTacGia = N'Tác giả 005'),
        2020,
        N'NXB Đại Học Quốc Gia',
        '2026-05-08',
        78000,
        3,
        N'Con'
    ),
    (
        N'Lập Trình C# Nâng Cao',
        N'A',
        (SELECT MaTheLoai FROM dbo.TheLoai WHERE TenTheLoai = N'A'),
        N'Tác giả 006',
        (SELECT MaTacGia FROM dbo.TacGia WHERE TenTacGia = N'Tác giả 006'),
        2021,
        N'NXB Trẻ',
        '2026-05-09',
        105000,
        2,
        N'Con'
    ),
    (
        N'Nhập Môn Cơ Sở Dữ Liệu',
        N'C',
        (SELECT MaTheLoai FROM dbo.TheLoai WHERE TenTheLoai = N'C'),
        N'Tác giả 007',
        (SELECT MaTacGia FROM dbo.TacGia WHERE TenTacGia = N'Tác giả 007'),
        2019,
        N'NXB Giáo Dục',
        '2026-05-11',
        68000,
        2,
        N'Con'
    ),
    (
        N'Thiết Kế Giao Diện WinForms',
        N'D',
        (SELECT MaTheLoai FROM dbo.TheLoai WHERE TenTheLoai = N'D'),
        N'Tác giả 008',
        (SELECT MaTacGia FROM dbo.TacGia WHERE TenTacGia = N'Tác giả 008'),
        2023,
        N'NXB Thanh Niên',
        '2026-05-12',
        72000,
        1,
        N'Con'
    ),
    (
        N'Phân Tích Thiết Kế Hệ Thống',
        N'D',
        (SELECT MaTheLoai FROM dbo.TheLoai WHERE TenTheLoai = N'D'),
        N'Tác giả 009',
        (SELECT MaTacGia FROM dbo.TacGia WHERE TenTacGia = N'Tác giả 009'),
        2022,
        N'NXB Lao Động',
        '2026-05-13',
        88000,
        2,
        N'Con'
    ),
    (
        N'Toán Rời Rạc',
        N'E',
        (SELECT MaTheLoai FROM dbo.TheLoai WHERE TenTheLoai = N'E'),
        N'Tác giả 010',
        (SELECT MaTacGia FROM dbo.TacGia WHERE TenTacGia = N'Tác giả 010'),
        2020,
        N'NXB Khoa Học',
        '2026-05-14',
        64000,
        2,
        N'Con'
    ),
    (
        N'Mạng Máy Tính',
        N'E',
        (SELECT MaTheLoai FROM dbo.TheLoai WHERE TenTheLoai = N'E'),
        N'Tác giả 011',
        (SELECT MaTacGia FROM dbo.TacGia WHERE TenTacGia = N'Tác giả 011'),
        2021,
        N'NXB Bưu Điện',
        '2026-05-15',
        83000,
        2,
        N'Con'
    ),
    (
        N'Kỹ Thuật Lập Trình',
        N'A',
        (SELECT MaTheLoai FROM dbo.TheLoai WHERE TenTheLoai = N'A'),
        N'Tác giả 012',
        (SELECT MaTacGia FROM dbo.TacGia WHERE TenTacGia = N'Tác giả 012'),
        2024,
        N'NXB Trẻ',
        '2026-05-16',
        99000,
        2,
        N'Con'
    ),
    (
        N'Tin Học Văn Phòng',
        N'A',
        (SELECT MaTheLoai FROM dbo.TheLoai WHERE TenTheLoai = N'A'),
        N'Tác giả 013',
        (SELECT MaTacGia FROM dbo.TacGia WHERE TenTacGia = N'Tác giả 013'),
        2024,
        N'NXB Tổng Hợp',
        '2026-06-01',
        45000,
        1,
        N'Con'
    ),
    (
        N'Lập Trình Web Cơ Bản',
        N'B',
        (SELECT MaTheLoai FROM dbo.TheLoai WHERE TenTheLoai = N'B'),
        N'Tác giả 014',
        (SELECT MaTacGia FROM dbo.TacGia WHERE TenTacGia = N'Tác giả 014'),
        2023,
        N'NXB Công Nghệ',
        '2026-06-01',
        52000,
        1,
        N'Con'
    ),
    (
        N'Quản Trị Cơ Sở Dữ Liệu',
        N'C',
        (SELECT MaTheLoai FROM dbo.TheLoai WHERE TenTheLoai = N'C'),
        N'Tác giả 015',
        (SELECT MaTacGia FROM dbo.TacGia WHERE TenTacGia = N'Tác giả 015'),
        2022,
        N'NXB Giáo Dục',
        '2026-06-01',
        60000,
        1,
        N'Con'
    );
GO

INSERT INTO dbo.PhieuMuon (MaDG, NgayMuon, NgayTra, TienPhatKyNay)
VALUES
    (1, '2026-05-10', NULL, 0),
    (2, '2026-05-11', '2026-05-17', 2000),
    (3, '2026-05-18', NULL, 0),
    (4, '2026-05-20', '2026-05-23', 0),
    (5, '2026-05-24', NULL, 0),
    (6, '2026-05-25', '2026-05-30', 1000),
    (2, '2026-06-01', NULL, 0),
    (4, '2026-06-01', '2026-06-04', 0),
    (6, '2026-06-02', '2026-06-08', 2000);
GO

INSERT INTO dbo.ChiTietPM (MaPhieu, MaSach, NgayThang, NgayTra, TienPhatKyNay, SoLanMuon)
VALUES
    (1, 1, '2026-05-10', NULL, 0, 1),
    (1, 5, '2026-05-10', NULL, 0, 1),
    (2, 2, '2026-05-11', '2026-05-17', 2000, 1),
    (3, 6, '2026-05-18', NULL, 0, 1),
    (3, 7, '2026-05-18', NULL, 0, 1),
    (4, 3, '2026-05-20', '2026-05-23', 0, 1),
    (5, 8, '2026-05-24', NULL, 0, 1),
    (6, 10, '2026-05-25', '2026-05-30', 1000, 1),
    (7, 4, '2026-06-01', NULL, 0, 1),
    (7, 9, '2026-06-01', NULL, 0, 1),
    (8, 11, '2026-06-01', '2026-06-04', 0, 1),
    (9, 12, '2026-06-02', '2026-06-08', 2000, 1);
GO

INSERT INTO dbo.PhieuThuTienPhat (MaDG, NgayThu, TongNoLucThu, SoTienThu, ConLai)
VALUES
    (2, '2026-05-18', 2000, 0, 2000),
    (6, '2026-05-31', 1000, 0, 1000),
    (6, '2026-06-09', 2000, 0, 2000);
GO
