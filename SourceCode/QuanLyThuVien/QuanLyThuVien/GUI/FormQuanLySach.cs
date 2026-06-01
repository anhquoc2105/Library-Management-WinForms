using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyThuVien.BUS;
using QuanLyThuVien.DTO;

namespace QuanLyThuVien.GUI
{
    public class FormQuanLySach : Form
    {
        private readonly SachBUS sachBUS = new SachBUS();
        private Label lblTieuDe;
        private Label lblMoTa;
        private Label lblTenSach;
        private Label lblTheLoai;
        private Label lblTacGia;
        private Label lblNamXB;
        private Label lblNhaXB;
        private Label lblNgayNhap;
        private Label lblTriGia;
        private TextBox txtTenSach;
        private ComboBox cboTheLoai;
        private ComboBox cboTacGia;
        private TextBox txtNamXB;
        private TextBox txtNhaXB;
        private DateTimePicker dtpNgayNhap;
        private TextBox txtTriGia;
        private DataGridView dgvSach;
        private Button btnTiepNhan;
        private Button btnTaiLai;
        private Button btnDong;

        public FormQuanLySach()
        {
            TaoGiaoDien();
            Load += FormQuanLySach_Load;
        }

        private void TaoGiaoDien()
        {
            Text = "Tiếp nhận sách mới";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1340, 780);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.FromArgb(244, 247, 251);
            Font = new Font("Segoe UI", 11F);

            lblTieuDe = new Label();
            lblTieuDe.Text = "Tiếp nhận sách mới";
            lblTieuDe.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTieuDe.ForeColor = Color.FromArgb(28, 77, 125);
            lblTieuDe.AutoSize = true;
            lblTieuDe.Location = new Point(34, 22);

            lblMoTa = new Label();
            lblMoTa.Text = "Quản lý thông tin sách, thể loại, tác giả và tình trạng hiện có trong thư viện.";
            lblMoTa.ForeColor = Color.FromArgb(102, 117, 132);
            lblMoTa.Font = new Font("Segoe UI", 10.5F);
            lblMoTa.AutoSize = true;
            lblMoTa.Location = new Point(38, 62);

            lblTenSach = TaoLabel("Tên sách", 40, 110);
            lblTheLoai = TaoLabel("Thể loại", 400, 110);
            lblTacGia = TaoLabel("Tác giả", 880, 110);
            lblNamXB = TaoLabel("Năm xuất bản", 40, 180);
            lblNhaXB = TaoLabel("Nhà xuất bản", 400, 180);
            lblNgayNhap = TaoLabel("Ngày nhập", 880, 180);
            lblTriGia = TaoLabel("Trị giá", 40, 250);

            txtTenSach = TaoTextBox(40, 136, 300);

            cboTheLoai = new ComboBox();
            cboTheLoai.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTheLoai.Location = new Point(400, 136);
            cboTheLoai.Size = new Size(300, 26);

            cboTacGia = new ComboBox();
            cboTacGia.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTacGia.Location = new Point(880, 136);
            cboTacGia.Size = new Size(400, 26);

            txtNamXB = TaoTextBox(40, 206, 300);
            txtNhaXB = TaoTextBox(400, 206, 300);

            dtpNgayNhap = new DateTimePicker();
            dtpNgayNhap.Format = DateTimePickerFormat.Short;
            dtpNgayNhap.Location = new Point(880, 206);
            dtpNgayNhap.Size = new Size(400, 26);
            dtpNgayNhap.Value = DateTime.Today;

            txtTriGia = TaoTextBox(40, 276, 300);

            btnTiepNhan = TaoButton("Tiếp nhận", 950, 268, Color.FromArgb(28, 77, 125), Color.White);
            btnTaiLai = TaoButton("Tải lại", 1066, 268, Color.FromArgb(230, 235, 241), Color.FromArgb(50, 60, 70));
            btnDong = TaoButton("Đóng", 1182, 268, Color.FromArgb(230, 235, 241), Color.FromArgb(50, 60, 70));

            btnTiepNhan.Click += btnTiepNhan_Click;
            btnTaiLai.Click += btnTaiLai_Click;
            btnDong.Click += (sender, e) => Close();

            dgvSach = new DataGridView();
            dgvSach.Location = new Point(24, 330);
            dgvSach.Size = new Size(1292, 420);
            dgvSach.ReadOnly = true;
            dgvSach.AllowUserToAddRows = false;
            dgvSach.AllowUserToDeleteRows = false;
            dgvSach.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSach.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSach.BackgroundColor = Color.White;
            dgvSach.BorderStyle = BorderStyle.None;
            dgvSach.RowHeadersVisible = false;
            dgvSach.EnableHeadersVisualStyles = false;
            dgvSach.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(237, 242, 247);
            dgvSach.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(36, 52, 71);
            dgvSach.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dgvSach.ColumnHeadersHeight = 46;
            dgvSach.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvSach.DefaultCellStyle.SelectionBackColor = Color.FromArgb(220, 232, 246);
            dgvSach.DefaultCellStyle.SelectionForeColor = Color.FromArgb(36, 52, 71);
            dgvSach.DefaultCellStyle.Padding = new Padding(2, 4, 2, 4);
            dgvSach.RowTemplate.Height = 40;

            Controls.Add(lblTieuDe);
            Controls.Add(lblMoTa);
            Controls.Add(lblTenSach);
            Controls.Add(lblTheLoai);
            Controls.Add(lblTacGia);
            Controls.Add(lblNamXB);
            Controls.Add(lblNhaXB);
            Controls.Add(lblNgayNhap);
            Controls.Add(lblTriGia);
            Controls.Add(txtTenSach);
            Controls.Add(cboTheLoai);
            Controls.Add(cboTacGia);
            Controls.Add(txtNamXB);
            Controls.Add(txtNhaXB);
            Controls.Add(dtpNgayNhap);
            Controls.Add(txtTriGia);
            Controls.Add(btnTiepNhan);
            Controls.Add(btnTaiLai);
            Controls.Add(btnDong);
            Controls.Add(dgvSach);
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

        private void FormQuanLySach_Load(object sender, EventArgs e)
        {
            TaiDuLieu();
        }

        private void btnTiepNhan_Click(object sender, EventArgs e)
        {
            int namXB;
            decimal triGia;

            if (!int.TryParse(txtNamXB.Text.Trim(), out namXB))
            {
                MessageBox.Show("Năm xuất bản không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNamXB.Focus();
                return;
            }

            if (!decimal.TryParse(txtTriGia.Text.Trim(), out triGia))
            {
                MessageBox.Show("Trị giá không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTriGia.Focus();
                return;
            }

            DataRowView theLoai = cboTheLoai.SelectedItem as DataRowView;
            DataRowView tacGia = cboTacGia.SelectedItem as DataRowView;

            SachDTO sach = new SachDTO
            {
                TenSach = txtTenSach.Text.Trim(),
                ChuDe = theLoai["TenTheLoai"].ToString(),
                MaTheLoai = Convert.ToInt32(theLoai["MaTheLoai"]),
                TenTG = tacGia["TenTacGia"].ToString(),
                MaTacGia = Convert.ToInt32(tacGia["MaTacGia"]),
                NamXB = namXB,
                NhaXB = txtNhaXB.Text.Trim(),
                NgayNhap = dtpNgayNhap.Value.Date,
                TriGia = triGia,
                SoLuongTon = 1
            };

            string thongBao;
            bool thanhCong = sachBUS.ThemSach(sach, out thongBao);

            MessageBox.Show(
                thongBao,
                thanhCong ? "Thông báo" : "Lỗi",
                MessageBoxButtons.OK,
                thanhCong ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            if (thanhCong)
            {
                LamMoiNhapLieu();
                TaiDanhSachSach();
            }
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            LamMoiNhapLieu();
            TaiDuLieu();
        }

        private void TaiDuLieu()
        {
            cboTheLoai.DataSource = sachBUS.LayDanhSachTheLoai();
            cboTheLoai.DisplayMember = "TenTheLoai";
            cboTheLoai.ValueMember = "MaTheLoai";

            cboTacGia.DataSource = sachBUS.LayDanhSachTacGia();
            cboTacGia.DisplayMember = "TenTacGia";
            cboTacGia.ValueMember = "MaTacGia";

            TaiDanhSachSach();
        }

        private void LamMoiNhapLieu()
        {
            txtTenSach.Clear();
            if (cboTheLoai.Items.Count > 0) cboTheLoai.SelectedIndex = 0;
            if (cboTacGia.Items.Count > 0) cboTacGia.SelectedIndex = 0;
            txtNamXB.Clear();
            txtNhaXB.Clear();
            dtpNgayNhap.Value = DateTime.Today;
            txtTriGia.Clear();
            txtTenSach.Focus();
        }

        private void TaiDanhSachSach()
        {
            try
            {
                dgvSach.DataSource = sachBUS.LayDanhSachSach();

                if (dgvSach.Columns.Count > 0)
                {
                    dgvSach.Columns["MaSach"].HeaderText = "Mã sách";
                    dgvSach.Columns["TenSach"].HeaderText = "Tên sách";
                    dgvSach.Columns["TenTheLoai"].HeaderText = "Thể loại";
                    dgvSach.Columns["TenTG"].HeaderText = "Tác giả";
                    dgvSach.Columns["NamXB"].HeaderText = "Năm XB";
                    dgvSach.Columns["NhaXB"].HeaderText = "Nhà XB";
                    dgvSach.Columns["NgayNhap"].HeaderText = "Ngày nhập";
                    dgvSach.Columns["TriGia"].HeaderText = "Trị giá";
                    dgvSach.Columns["SoLuongTon"].HeaderText = "Số lượng còn";
                    dgvSach.Columns["TinhTrang"].HeaderText = "Tình trạng";

                    dgvSach.Columns["TriGia"].DefaultCellStyle.Format = "N0";

                    dgvSach.Columns["MaSach"].FillWeight = 70;
                    dgvSach.Columns["TenSach"].FillWeight = 190;
                    dgvSach.Columns["TenTheLoai"].FillWeight = 75;
                    dgvSach.Columns["TenTG"].FillWeight = 105;
                    dgvSach.Columns["NamXB"].FillWeight = 75;
                    dgvSach.Columns["NhaXB"].FillWeight = 140;
                    dgvSach.Columns["NgayNhap"].FillWeight = 105;
                    dgvSach.Columns["TriGia"].FillWeight = 90;
                    dgvSach.Columns["SoLuongTon"].FillWeight = 105;
                    dgvSach.Columns["TinhTrang"].FillWeight = 105;
                }

                dgvSach.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải danh sách sách.\nChi tiết: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
