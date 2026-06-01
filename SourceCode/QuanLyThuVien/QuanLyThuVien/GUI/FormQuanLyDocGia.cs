using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyThuVien.BUS;
using QuanLyThuVien.DTO;

namespace QuanLyThuVien.GUI
{
    public class FormQuanLyDocGia : Form
    {
        private readonly DocGiaBUS docGiaBUS = new DocGiaBUS();
        private Label lblTieuDe;
        private Label lblMoTa;
        private Label lblTenDG;
        private Label lblLoaiDG;
        private Label lblNgaySinh;
        private Label lblDiaChi;
        private Label lblEmail;
        private Label lblNgayLapThe;
        private TextBox txtTenDG;
        private ComboBox cboLoaiDG;
        private DateTimePicker dtpNgaySinh;
        private TextBox txtDiaChi;
        private TextBox txtEmail;
        private DateTimePicker dtpNgayLapThe;
        private DataGridView dgvDocGia;
        private Button btnLapThe;
        private Button btnTaiLai;
        private Button btnDong;

        public FormQuanLyDocGia()
        {
            TaoGiaoDien();
            Load += FormQuanLyDocGia_Load;
        }

        private void TaoGiaoDien()
        {
            Text = "Lập thẻ độc giả";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1340, 780);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.FromArgb(244, 247, 251);
            Font = new Font("Segoe UI", 11F);

            lblTieuDe = new Label();
            lblTieuDe.Text = "Lập thẻ độc giả";
            lblTieuDe.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTieuDe.ForeColor = Color.FromArgb(28, 77, 125);
            lblTieuDe.AutoSize = true;
            lblTieuDe.Location = new Point(34, 22);

            lblMoTa = new Label();
            lblMoTa.Text = "Quản lý thông tin độc giả, hạn thẻ và các điều kiện đăng ký theo quy định.";
            lblMoTa.ForeColor = Color.FromArgb(102, 117, 132);
            lblMoTa.Font = new Font("Segoe UI", 10.5F);
            lblMoTa.AutoSize = true;
            lblMoTa.Location = new Point(38, 62);

            lblTenDG = TaoLabel("Họ và tên", 40, 110);
            lblLoaiDG = TaoLabel("Loại độc giả", 400, 110);
            lblNgaySinh = TaoLabel("Ngày sinh", 880, 110);
            lblDiaChi = TaoLabel("Địa chỉ", 40, 180);
            lblEmail = TaoLabel("Email", 400, 180);
            lblNgayLapThe = TaoLabel("Ngày lập thẻ", 880, 180);

            txtTenDG = TaoTextBox(40, 136, 300);

            cboLoaiDG = new ComboBox();
            cboLoaiDG.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLoaiDG.Items.AddRange(new object[] { "X", "Y" });
            cboLoaiDG.Location = new Point(400, 136);
            cboLoaiDG.Size = new Size(300, 26);
            cboLoaiDG.SelectedIndex = 0;

            dtpNgaySinh = new DateTimePicker();
            dtpNgaySinh.Format = DateTimePickerFormat.Short;
            dtpNgaySinh.Location = new Point(880, 136);
            dtpNgaySinh.Size = new Size(400, 26);

            txtDiaChi = TaoTextBox(40, 206, 300);
            txtEmail = TaoTextBox(400, 206, 300);

            dtpNgayLapThe = new DateTimePicker();
            dtpNgayLapThe.Format = DateTimePickerFormat.Short;
            dtpNgayLapThe.Location = new Point(880, 206);
            dtpNgayLapThe.Size = new Size(400, 26);
            dtpNgayLapThe.Value = DateTime.Today;

            btnLapThe = TaoButton("Lập thẻ", 950, 268, Color.FromArgb(28, 77, 125), Color.White);
            btnTaiLai = TaoButton("Tải lại", 1066, 268, Color.FromArgb(230, 235, 241), Color.FromArgb(50, 60, 70));
            btnDong = TaoButton("Đóng", 1182, 268, Color.FromArgb(230, 235, 241), Color.FromArgb(50, 60, 70));

            btnLapThe.Click += btnLapThe_Click;
            btnTaiLai.Click += btnTaiLai_Click;
            btnDong.Click += (sender, e) => Close();

            dgvDocGia = new DataGridView();
            dgvDocGia.Location = new Point(24, 330);
            dgvDocGia.Size = new Size(1292, 420);
            dgvDocGia.ReadOnly = true;
            dgvDocGia.AllowUserToAddRows = false;
            dgvDocGia.AllowUserToDeleteRows = false;
            dgvDocGia.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDocGia.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDocGia.BackgroundColor = Color.White;
            dgvDocGia.BorderStyle = BorderStyle.None;
            dgvDocGia.RowHeadersVisible = false;
            dgvDocGia.EnableHeadersVisualStyles = false;
            dgvDocGia.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(237, 242, 247);
            dgvDocGia.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(36, 52, 71);
            dgvDocGia.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dgvDocGia.ColumnHeadersHeight = 46;
            dgvDocGia.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvDocGia.DefaultCellStyle.SelectionBackColor = Color.FromArgb(220, 232, 246);
            dgvDocGia.DefaultCellStyle.SelectionForeColor = Color.FromArgb(36, 52, 71);
            dgvDocGia.DefaultCellStyle.Padding = new Padding(2, 4, 2, 4);
            dgvDocGia.RowTemplate.Height = 40;

            Controls.Add(lblTieuDe);
            Controls.Add(lblMoTa);
            Controls.Add(lblTenDG);
            Controls.Add(lblLoaiDG);
            Controls.Add(lblNgaySinh);
            Controls.Add(lblDiaChi);
            Controls.Add(lblEmail);
            Controls.Add(lblNgayLapThe);
            Controls.Add(txtTenDG);
            Controls.Add(cboLoaiDG);
            Controls.Add(dtpNgaySinh);
            Controls.Add(txtDiaChi);
            Controls.Add(txtEmail);
            Controls.Add(dtpNgayLapThe);
            Controls.Add(btnLapThe);
            Controls.Add(btnTaiLai);
            Controls.Add(btnDong);
            Controls.Add(dgvDocGia);
        }

        private Label TaoLabel(string text, int x, int y)
        {
            Label label = new Label();
            label.Text = text;
            label.AutoSize = true;
            label.Location = new Point(x, y);
            return label;
        }

        private TextBox TaoTextBox(int x, int y, int width)
        {
            TextBox textBox = new TextBox();
            textBox.Location = new Point(x, y);
            textBox.Size = new Size(width, 30);
            return textBox;
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

        private void FormQuanLyDocGia_Load(object sender, EventArgs e)
        {
            TaiDanhSachDocGia();
        }

        private void btnLapThe_Click(object sender, EventArgs e)
        {
            DocGiaDTO docGia = new DocGiaDTO
            {
                TenDG = txtTenDG.Text.Trim(),
                LoaiDG = cboLoaiDG.SelectedItem.ToString(),
                NgaySinhDG = dtpNgaySinh.Value.Date,
                DiaChiDG = txtDiaChi.Text.Trim(),
                EmailDG = txtEmail.Text.Trim(),
                NgLapThe = dtpNgayLapThe.Value.Date
            };

            string thongBao;
            bool thanhCong = docGiaBUS.ThemDocGia(docGia, out thongBao);

            MessageBox.Show(
                thongBao,
                thanhCong ? "Thông báo" : "Lỗi",
                MessageBoxButtons.OK,
                thanhCong ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            if (thanhCong)
            {
                LamMoiNhapLieu();
                TaiDanhSachDocGia();
            }
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            LamMoiNhapLieu();
            TaiDanhSachDocGia();
        }

        private void LamMoiNhapLieu()
        {
            txtTenDG.Clear();
            cboLoaiDG.SelectedIndex = 0;
            dtpNgaySinh.Value = DateTime.Today;
            txtDiaChi.Clear();
            txtEmail.Clear();
            dtpNgayLapThe.Value = DateTime.Today;
            txtTenDG.Focus();
        }

        private void TaiDanhSachDocGia()
        {
            try
            {
                dgvDocGia.DataSource = docGiaBUS.LayDanhSachDocGia();

                if (dgvDocGia.Columns.Count > 0)
                {
                    dgvDocGia.Columns["MaDGHienThi"].HeaderText = "Mã độc giả";
                    dgvDocGia.Columns["TenDG"].HeaderText = "Họ và tên";
                    dgvDocGia.Columns["LoaiDG"].HeaderText = "Loại độc giả";
                    dgvDocGia.Columns["NgaySinhDG"].HeaderText = "Ngày sinh";
                    dgvDocGia.Columns["DiaChiDG"].HeaderText = "Địa chỉ";
                    dgvDocGia.Columns["EmailDG"].HeaderText = "Email";
                    dgvDocGia.Columns["NgLapThe"].HeaderText = "Ngày lập thẻ";
                    dgvDocGia.Columns["NgayHetHan"].HeaderText = "Ngày hết hạn";
                    dgvDocGia.Columns["MaDG"].Visible = false;
                    dgvDocGia.Columns["TongNo"].Visible = false;
                    dgvDocGia.Columns["MaTaiKhoan"].Visible = false;

                    dgvDocGia.Columns["MaDGHienThi"].FillWeight = 80;
                    dgvDocGia.Columns["TenDG"].FillWeight = 190;
                    dgvDocGia.Columns["LoaiDG"].FillWeight = 95;
                    dgvDocGia.Columns["NgaySinhDG"].FillWeight = 105;
                    dgvDocGia.Columns["DiaChiDG"].FillWeight = 160;
                    dgvDocGia.Columns["EmailDG"].FillWeight = 170;
                    dgvDocGia.Columns["NgLapThe"].FillWeight = 105;
                    dgvDocGia.Columns["NgayHetHan"].FillWeight = 110;
                }

                dgvDocGia.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải danh sách độc giả.\nChi tiết: " + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
