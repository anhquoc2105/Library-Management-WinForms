using System;
using System.Data;
using System.Data.SqlClient;
using QuanLyThuVien.UTILS;

namespace QuanLyThuVien.DAL
{
    public class PhieuThuTienPhatDAL
    {
        public DataTable LayDanhSachDocGiaNo()
        {
            const string query = @"
                SELECT MaDG, TenDG, TongNo
                FROM DocGia
                WHERE TongNo > 0
                ORDER BY TenDG";

            return DbHelper.ExecuteQuery(query);
        }

        public bool LapPhieuThu(int maDG, decimal soTienThu, out string thongBao)
        {
            using (SqlConnection connection = DbHelper.GetConnection())
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        decimal tongNo;
                        const string queryTongNo = "SELECT TongNo FROM DocGia WHERE MaDG = @MaDG";

                        using (SqlCommand command = new SqlCommand(queryTongNo, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@MaDG", maDG);
                            object result = command.ExecuteScalar();
                            if (result == null)
                            {
                                thongBao = "Không tìm thấy độc giả.";
                                transaction.Rollback();
                                return false;
                            }

                            tongNo = Convert.ToDecimal(result);
                        }

                        if (soTienThu <= 0)
                        {
                            thongBao = "Số tiền thu phải lớn hơn 0.";
                            transaction.Rollback();
                            return false;
                        }

                        if (soTienThu > tongNo)
                        {
                            thongBao = "Số tiền thu không được vượt quá tổng nợ.";
                            transaction.Rollback();
                            return false;
                        }

                        decimal conLai = tongNo - soTienThu;

                        const string insertQuery = @"
                            INSERT INTO PhieuThuTienPhat (MaDG, NgayThu, TongNoLucThu, SoTienThu, ConLai)
                            VALUES (@MaDG, GETDATE(), @TongNoLucThu, @SoTienThu, @ConLai)";

                        using (SqlCommand command = new SqlCommand(insertQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@MaDG", maDG);
                            command.Parameters.AddWithValue("@TongNoLucThu", tongNo);
                            command.Parameters.AddWithValue("@SoTienThu", soTienThu);
                            command.Parameters.AddWithValue("@ConLai", conLai);
                            command.ExecuteNonQuery();
                        }

                        const string updateNoQuery = "UPDATE DocGia SET TongNo = @ConLai WHERE MaDG = @MaDG";
                        using (SqlCommand command = new SqlCommand(updateNoQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@ConLai", conLai);
                            command.Parameters.AddWithValue("@MaDG", maDG);
                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        thongBao = "Lập phiếu thu tiền phạt thành công.";
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        thongBao = "Không thể lập phiếu thu tiền phạt. " + ex.Message;
                        return false;
                    }
                }
            }
        }
    }
}
