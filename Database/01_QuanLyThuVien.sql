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

IF OBJECT_ID(N'dbo.TRG_DocGia_SetNgayHetHan', N'TR') IS NOT NULL
    DROP TRIGGER dbo.TRG_DocGia_SetNgayHetHan;
GO

IF OBJECT_ID(N'dbo.TRG_ChiTietPM_UpdateSLMuon', N'TR') IS NOT NULL
    DROP TRIGGER dbo.TRG_ChiTietPM_UpdateSLMuon;
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
    SoLanMuon INT NOT NULL DEFAULT 1,
    CONSTRAINT FK_ChiTietPM_PhieuMuon
        FOREIGN KEY (MaPhieu) REFERENCES dbo.PhieuMuon(MaPhieu),
    CONSTRAINT FK_ChiTietPM_Sach
        FOREIGN KEY (MaSach) REFERENCES dbo.Sach(MaSach),
    CONSTRAINT UQ_ChiTietPM_MaPhieu_MaSach UNIQUE (MaPhieu, MaSach),
    CONSTRAINT CK_ChiTietPM_SoLanMuon CHECK (SoLanMuon > 0)
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

CREATE TRIGGER dbo.TRG_PhieuMuon_SetNgayPhaiTra
ON dbo.PhieuMuon
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

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
    (N'C');
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
    ('thuthu01', 'admin123');
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
    (N'Trần Thị B', N'Y', '1990-09-20', N'12 Lê Lợi', 'b@gmail.com', '2026-05-02', NULL, 2000, 2);
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
    (N'Lê Thị Thu', N'Nữ', '1998-07-10', 'thuthu01@gmail.com', N'Đồng Nai', N'Thủ thư chính', 3);
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
        1,
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
        1,
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
        1,
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
    );
GO

INSERT INTO dbo.PhieuMuon (MaDG, NgayMuon, NgayPhaiTra, NgayTra, TienPhatKyNay)
VALUES
    (1, '2026-05-10', '2026-05-14', NULL, 0),
    (2, '2026-05-11', '2026-05-15', '2026-05-17', 2000);
GO

INSERT INTO dbo.ChiTietPM (MaPhieu, MaSach, NgayThang, SoLanMuon)
VALUES
    (1, 1, '2026-05-10', 1),
    (2, 2, '2026-05-11', 1);
GO

UPDATE dbo.Sach
SET TinhTrang = N'Dang muon'
WHERE MaSach IN
(
    SELECT ct.MaSach
    FROM dbo.ChiTietPM ct
    INNER JOIN dbo.PhieuMuon pm ON ct.MaPhieu = pm.MaPhieu
    WHERE pm.NgayTra IS NULL
);
GO

INSERT INTO dbo.PhieuThuTienPhat (MaDG, NgayThu, TongNoLucThu, SoTienThu, ConLai)
VALUES
    (2, '2026-05-18', 2000, 0, 2000);
GO

USE QuanLyThuVien;
SELECT * FROM ThamSo;
SELECT * FROM TheLoai;
SELECT * FROM TacGia;
SELECT * FROM DocGia;
SELECT * FROM Sach;
SELECT * FROM PhieuMuon;
SELECT * FROM PhieuThuTienPhat;
SELECT * FROM TaiKhoan;
