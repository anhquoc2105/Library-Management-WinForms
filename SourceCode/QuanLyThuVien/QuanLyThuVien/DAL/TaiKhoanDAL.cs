using System;
using System.Data;
using System.Data.SqlClient;
using QuanLyThuVien.DTO;
using QuanLyThuVien.UTILS;

namespace QuanLyThuVien.DAL
{
    public class TaiKhoanDAL
    {
        public TaiKhoanDTO DangNhap(string tenDangNhap, string matKhau)
        {
            const string query = @"
                SELECT TOP 1
                    tk.MaTaiKhoan,
                    tk.TenDangNhap,
                    tk.MatKhau,
                    dg.MaDG,
                    tt.MaTT,
                    CASE
                        WHEN dg.MaTaiKhoan IS NOT NULL THEN N'DocGia'
                        WHEN tt.MaTaiKhoan IS NOT NULL THEN N'ThuThu'
                        ELSE N'ChuaPhanQuyen'
                    END AS VaiTro
                FROM TaiKhoan tk
                LEFT JOIN DocGia dg ON tk.MaTaiKhoan = dg.MaTaiKhoan
                LEFT JOIN ThuThu tt ON tk.MaTaiKhoan = tt.MaTaiKhoan
                WHERE tk.TenDangNhap = @TenDangNhap
                  AND tk.MatKhau = @MatKhau";

            DataTable dataTable = DbHelper.ExecuteQuery(
                query,
                new SqlParameter("@TenDangNhap", tenDangNhap),
                new SqlParameter("@MatKhau", matKhau));

            if (dataTable.Rows.Count == 0)
            {
                return null;
            }

            DataRow row = dataTable.Rows[0];
            return new TaiKhoanDTO
            {
                MaTaiKhoan = (int)row["MaTaiKhoan"],
                TenDangNhap = row["TenDangNhap"].ToString(),
                MatKhau = row["MatKhau"].ToString(),
                VaiTro = row["VaiTro"].ToString(),
                MaDG = row["MaDG"] == DBNull.Value ? (int?)null : (int)row["MaDG"],
                MaTT = row["MaTT"] == DBNull.Value ? (int?)null : (int)row["MaTT"]
            };
        }
    }
}
