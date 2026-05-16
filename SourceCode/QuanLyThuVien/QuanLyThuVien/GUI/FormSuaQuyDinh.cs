using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyThuVien.DAL;
using QuanLyThuVien.UTILS;

namespace QuanLyThuVien.GUI
{
    public class FormSuaQuyDinh : Form
    {
        private readonly ThamSoDAL thamSoDAL = new ThamSoDAL();
        private DataGridView dgvThamSo;
        private Label lblTieuDe;
        private Label lblMoTa;
        private Button btnLuu;
        private Button btnDong;

        public FormSuaQuyDinh()
        {
            TaoGiaoDien();
            Load += FormSuaQuyDinh_Load;
        }

        private void TaoGiaoDien()
        {
            Text = "Thay đổi quy định";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(940, 480);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.FromArgb(244, 247, 251);
            Font = new Font("Segoe UI", 11F);

            lblTieuDe = new Label();
            lblTieuDe.Text = "Thay đổi quy định";
            lblTieuDe.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTieuDe.ForeColor = Color.FromArgb(28, 77, 125);
            lblTieuDe.AutoSize = true;
            lblTieuDe.Location = new Point(34, 24);

            lblMoTa = new Label();
            lblMoTa.Text = "Điều chỉnh tuổi độc giả, thời hạn thẻ, số sách được mượn và các giới hạn vận hành khác.";
            lblMoTa.ForeColor = Color.FromArgb(102, 117, 132);
            lblMoTa.AutoSize = true;
            lblMoTa.Location = new Point(38, 60);

            dgvThamSo = new DataGridView();
            dgvThamSo.Location = new Point(24, 120);
            dgvThamSo.Size = new Size(880, 250);
            dgvThamSo.AllowUserToAddRows = false;
            dgvThamSo.AllowUserToDeleteRows = false;
            dgvThamSo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvThamSo.BackgroundColor = Color.White;
            dgvThamSo.BorderStyle = BorderStyle.None;
            dgvThamSo.RowHeadersVisible = false;
            dgvThamSo.EnableHeadersVisualStyles = false;
            dgvThamSo.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(237, 242, 247);
            dgvThamSo.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(36, 52, 71);
            dgvThamSo.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dgvThamSo.ColumnHeadersHeight = 42;
            dgvThamSo.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvThamSo.DefaultCellStyle.Padding = new Padding(2, 4, 2, 4);
            dgvThamSo.RowTemplate.Height = 36;

            btnLuu = TaoButton("Lưu", 690, 400, Color.FromArgb(28, 77, 125), Color.White);
            btnDong = TaoButton("Đóng", 806, 400, Color.FromArgb(230, 235, 241), Color.FromArgb(50, 60, 70));

            btnLuu.Click += btnLuu_Click;
            btnDong.Click += (sender, e) => Close();

            Controls.Add(lblTieuDe);
            Controls.Add(lblMoTa);
            Controls.Add(dgvThamSo);
            Controls.Add(btnLuu);
            Controls.Add(btnDong);
        }

        private Button TaoButton(string text, int x, int y, Color backColor, Color foreColor)
        {
            Button button = new Button();
            button.Text = text;
            button.Size = new Size(108, 40);
            button.Location = new Point(x, y);
            button.BackColor = backColor;
            button.ForeColor = foreColor;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            return button;
        }

        private void FormSuaQuyDinh_Load(object sender, System.EventArgs e)
        {
            dgvThamSo.DataSource = thamSoDAL.LayDanhSachThamSo();
        }

        private void btnLuu_Click(object sender, System.EventArgs e)
        {
            if (dgvThamSo.Rows.Count == 0)
            {
                return;
            }

            DataGridViewRow row = dgvThamSo.Rows[0];
            string query = @"
                UPDATE ThamSo
                SET GiaTriThe = @GiaTriThe,
                    SoTuoiDG = @SoTuoiDG,
                    SoTuoiDGToiDa = @SoTuoiDGToiDa,
                    ThoiGianXB = @ThoiGianXB,
                    SoSachMuonToiDa = @SoSachMuonToiDa,
                    SoNgayMuonToiDa = @SoNgayMuonToiDa,
                    TienPhat = @TienPhat
                WHERE MaThamSo = @MaThamSo";

            DbHelper.ExecuteNonQuery(
                query,
                new System.Data.SqlClient.SqlParameter("@GiaTriThe", row.Cells["GiaTriThe"].Value),
                new System.Data.SqlClient.SqlParameter("@SoTuoiDG", row.Cells["SoTuoiDG"].Value),
                new System.Data.SqlClient.SqlParameter("@SoTuoiDGToiDa", row.Cells["SoTuoiDGToiDa"].Value),
                new System.Data.SqlClient.SqlParameter("@ThoiGianXB", row.Cells["ThoiGianXB"].Value),
                new System.Data.SqlClient.SqlParameter("@SoSachMuonToiDa", row.Cells["SoSachMuonToiDa"].Value),
                new System.Data.SqlClient.SqlParameter("@SoNgayMuonToiDa", row.Cells["SoNgayMuonToiDa"].Value),
                new System.Data.SqlClient.SqlParameter("@TienPhat", row.Cells["TienPhat"].Value),
                new System.Data.SqlClient.SqlParameter("@MaThamSo", row.Cells["MaThamSo"].Value));

            MessageBox.Show("Cập nhật quy định thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
