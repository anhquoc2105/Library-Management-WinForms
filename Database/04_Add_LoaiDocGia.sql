USE QuanLyThuVien;
GO

IF OBJECT_ID(N'dbo.LoaiDocGia', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.LoaiDocGia
    (
        MaLoaiDG INT IDENTITY(1,1) PRIMARY KEY,
        TenLoaiDG NVARCHAR(50) NOT NULL,
        CONSTRAINT UQ_LoaiDocGia_TenLoaiDG UNIQUE (TenLoaiDG)
    );
END
GO

IF OBJECT_ID(N'dbo.DocGia', N'U') IS NOT NULL
   AND OBJECT_ID(N'dbo.CK_DocGia_LoaiDG', N'C') IS NOT NULL
BEGIN
    ALTER TABLE dbo.DocGia DROP CONSTRAINT CK_DocGia_LoaiDG;
END
GO

IF OBJECT_ID(N'dbo.DocGia', N'U') IS NOT NULL
BEGIN
    INSERT INTO dbo.LoaiDocGia (TenLoaiDG)
    SELECT DISTINCT dg.LoaiDG
    FROM dbo.DocGia dg
    WHERE dg.LoaiDG IS NOT NULL
      AND LTRIM(RTRIM(dg.LoaiDG)) <> N''
      AND NOT EXISTS
      (
          SELECT 1
          FROM dbo.LoaiDocGia ldg
          WHERE ldg.TenLoaiDG = dg.LoaiDG
      );
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.LoaiDocGia)
BEGIN
    INSERT INTO dbo.LoaiDocGia (TenLoaiDG)
    VALUES (N'X'), (N'Y');
END
GO
