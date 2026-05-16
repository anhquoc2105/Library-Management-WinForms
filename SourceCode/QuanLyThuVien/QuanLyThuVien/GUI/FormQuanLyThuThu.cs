using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyThuVien.BUS;

namespace QuanLyThuVien.GUI
{
    public class FormQuanLyThuThu : Form
    {
        private readonly ThuThuBUS thuThuBUS = new ThuThuBUS();
        private Label lblTieuDe;
        private Label lblMoTa;
        private DataGridView dgvThuThu;
        private Button btnTaiLai;
        private Button btnDong;

        public FormQuanLyThuThu()
        {
            TaoGiaoDien();
            Load += FormQuanLyThuThu_Load;
        }

        private void TaoGiaoDien()
        {
            Text = "Quản lý thủ thư";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1020, 600);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.FromArgb(244, 247, 251);
            Font = new Font("Segoe UI", 11F);

            lblTieuDe = new Label();
            lblTieuDe.Text = "Quản lý thủ thư";
            lblTieuDe.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTieuDe.ForeColor = Color.FromArgb(28, 77, 125);
            lblTieuDe.AutoSize = true;
            lblTieuDe.Location = new Point(34, 24);

            lblMoTa = new Label();
            lblMoTa.Text = "Theo dõi hồ sơ, liên hệ và tài khoản của đội ngũ thủ thư trong hệ thống.";
            lblMoTa.ForeColor = Color.FromArgb(102, 117, 132);
            lblMoTa.AutoSize = true;
            lblMoTa.Location = new Point(38, 60);

            btnTaiLai = TaoButton("Tải lại", 790, 104, Color.FromArgb(230, 235, 241), Color.FromArgb(50, 60, 70));
            btnDong = TaoButton("Đóng", 906, 104, Color.FromArgb(230, 235, 241), Color.FromArgb(50, 60, 70));
            btnTaiLai.Click += btnTaiLai_Click;
            btnDong.Click += (sender, e) => Close();

            dgvThuThu = new DataGridView();
            dgvThuThu.Location = new Point(24, 170);
            dgvThuThu.Size = new Size(960, 380);
            dgvThuThu.ReadOnly = true;
            dgvThuThu.AllowUserToAddRows = false;
            dgvThuThu.AllowUserToDeleteRows = false;
            dgvThuThu.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvThuThu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvThuThu.MultiSelect = false;
            dgvThuThu.BackgroundColor = Color.White;
            dgvThuThu.BorderStyle = BorderStyle.None;
            dgvThuThu.RowHeadersVisible = false;
            dgvThuThu.EnableHeadersVisualStyles = false;
            dgvThuThu.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(237, 242, 247);
            dgvThuThu.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(36, 52, 71);
            dgvThuThu.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dgvThuThu.ColumnHeadersHeight = 42;
            dgvThuThu.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvThuThu.DefaultCellStyle.SelectionBackColor = Color.FromArgb(220, 232, 246);
            dgvThuThu.DefaultCellStyle.SelectionForeColor = Color.FromArgb(36, 52, 71);
            dgvThuThu.DefaultCellStyle.Padding = new Padding(2, 4, 2, 4);
            dgvThuThu.RowTemplate.Height = 36;

            Controls.Add(lblTieuDe);
            Controls.Add(lblMoTa);
            Controls.Add(btnTaiLai);
            Controls.Add(btnDong);
            Controls.Add(dgvThuThu);
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

        private void FormQuanLyThuThu_Load(object sender, EventArgs e)
        {
            TaiDanhSachThuThu();
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            TaiDanhSachThuThu();
        }

        private void TaiDanhSachThuThu()
        {
            try
            {
                dgvThuThu.DataSource = thuThuBUS.LayDanhSachThuThu();

                if (dgvThuThu.Columns.Count > 0)
                {
                    dgvThuThu.Columns["MaTT"].HeaderText = "Mã thủ thư";
                    dgvThuThu.Columns["TenTT"].HeaderText = "Tên thủ thư";
                    dgvThuThu.Columns["GioiTinhTT"].HeaderText = "Giới tính";
                    dgvThuThu.Columns["NgaySinhTT"].HeaderText = "Ngày sinh";
                    dgvThuThu.Columns["EmailTT"].HeaderText = "Email";
                    dgvThuThu.Columns["DiaChiTT"].HeaderText = "Địa chỉ";
                    dgvThuThu.Columns["GhiChu"].HeaderText = "Ghi chú";
                    dgvThuThu.Columns["MaTaiKhoan"].HeaderText = "Mã tài khoản";
                }

                dgvThuThu.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải danh sách thủ thư.\nChi tiết: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
