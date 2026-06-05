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
        private readonly LoaiDocGiaBUS loaiDocGiaBUS = new LoaiDocGiaBUS();
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
        private Button btnCapNhat;
        private Button btnGiaHan;
        private Button btnXoa;
        private Button btnTaiLai;
        private Button btnDong;
        private int maDocGiaDangChon;
        private bool dangTaiDanhSachDocGia;

        public FormQuanLyDocGia()
        {
            TaoGiaoDien();
            Load += FormQuanLyDocGia_Load;
        }

        private void TaoGiaoDien()
        {
            Text = "Quản lý thẻ độc giả";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1500, 860);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.FromArgb(244, 247, 251);
            Font = new Font("Segoe UI", 11F);

            lblTieuDe = new Label();
            lblTieuDe.Text = "Quản lý thẻ độc giả";
            lblTieuDe.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTieuDe.ForeColor = Color.FromArgb(28, 77, 125);
            lblTieuDe.AutoSize = true;
            lblTieuDe.Location = new Point(44, 28);

            lblMoTa = new Label();
            lblMoTa.Text = "Quản lý thông tin độc giả, hạn thẻ và các điều kiện đăng ký theo quy định.";
            lblMoTa.ForeColor = Color.FromArgb(102, 117, 132);
            lblMoTa.Font = new Font("Segoe UI", 10.5F);
            lblMoTa.AutoSize = true;
            lblMoTa.Location = new Point(48, 70);

            lblTenDG = TaoLabel("Họ và tên", 50, 126);
            lblLoaiDG = TaoLabel("Loại độc giả", 460, 126);
            lblNgaySinh = TaoLabel("Ngày sinh", 920, 126);
            lblDiaChi = TaoLabel("Địa chỉ", 50, 202);
            lblEmail = TaoLabel("Email", 460, 202);
            lblNgayLapThe = TaoLabel("Ngày lập thẻ", 920, 202);

            txtTenDG = TaoTextBox(50, 154, 330);

            cboLoaiDG = new ComboBox();
            cboLoaiDG.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLoaiDG.Location = new Point(460, 154);
            cboLoaiDG.Size = new Size(330, 30);

            dtpNgaySinh = new DateTimePicker();
            dtpNgaySinh.Format = DateTimePickerFormat.Short;
            dtpNgaySinh.Location = new Point(920, 154);
            dtpNgaySinh.Size = new Size(420, 30);

            txtDiaChi = TaoTextBox(50, 230, 330);
            txtEmail = TaoTextBox(460, 230, 330);

            dtpNgayLapThe = new DateTimePicker();
            dtpNgayLapThe.Format = DateTimePickerFormat.Short;
            dtpNgayLapThe.Location = new Point(920, 230);
            dtpNgayLapThe.Size = new Size(420, 30);
            dtpNgayLapThe.Value = DateTime.Today;

            btnXoa = TaoButton("Xóa", 684, 292, Color.FromArgb(190, 49, 68), Color.White);
            btnGiaHan = TaoButton("Gia hạn", 806, 292, Color.FromArgb(37, 132, 91), Color.White);
            btnCapNhat = TaoButton("Cập nhật", 928, 292, Color.FromArgb(28, 77, 125), Color.White);
            btnLapThe = TaoButton("Lập thẻ", 1050, 292, Color.FromArgb(28, 77, 125), Color.White);
            btnTaiLai = TaoButton("Tải lại", 1172, 292, Color.FromArgb(230, 235, 241), Color.FromArgb(50, 60, 70));
            btnDong = TaoButton("Đóng", 1294, 292, Color.FromArgb(230, 235, 241), Color.FromArgb(50, 60, 70));

            btnXoa.Click += btnXoa_Click;
            btnGiaHan.Click += btnGiaHan_Click;
            btnCapNhat.Click += btnCapNhat_Click;
            btnLapThe.Click += btnLapThe_Click;
            btnTaiLai.Click += btnTaiLai_Click;
            btnDong.Click += (sender, e) => Close();

            dgvDocGia = new DataGridView();
            dgvDocGia.Location = new Point(40, 360);
            dgvDocGia.Size = new Size(1420, 450);
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
            dgvDocGia.ColumnHeadersHeight = 48;
            dgvDocGia.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvDocGia.DefaultCellStyle.SelectionBackColor = Color.FromArgb(220, 232, 246);
            dgvDocGia.DefaultCellStyle.SelectionForeColor = Color.FromArgb(36, 52, 71);
            dgvDocGia.DefaultCellStyle.Padding = new Padding(2, 4, 2, 4);
            dgvDocGia.RowTemplate.Height = 42;
            dgvDocGia.SelectionChanged += dgvDocGia_SelectionChanged;

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
            Controls.Add(btnXoa);
            Controls.Add(btnGiaHan);
            Controls.Add(btnCapNhat);
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
            button.Size = new Size(110, 42);
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
            TaiDuLieu();
        }

        private void TaiDuLieu()
        {
            TaiDanhSachLoaiDocGia();
            TaiDanhSachDocGia();
        }

        private void TaiDanhSachLoaiDocGia()
        {
            cboLoaiDG.DataSource = loaiDocGiaBUS.LayDanhSachLoaiDocGia();
            cboLoaiDG.DisplayMember = "TenLoaiDG";
            cboLoaiDG.ValueMember = "TenLoaiDG";
        }

        private void btnLapThe_Click(object sender, EventArgs e)
        {
            DocGiaDTO docGia = new DocGiaDTO
            {
                TenDG = txtTenDG.Text.Trim(),
                LoaiDG = LayLoaiDocGiaDangChon(),
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

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (maDocGiaDangChon <= 0)
            {
                MessageBox.Show("Vui lòng chọn độc giả cần cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DocGiaDTO docGia = new DocGiaDTO
            {
                MaDG = maDocGiaDangChon,
                TenDG = txtTenDG.Text.Trim(),
                LoaiDG = LayLoaiDocGiaDangChon(),
                NgaySinhDG = dtpNgaySinh.Value.Date,
                DiaChiDG = txtDiaChi.Text.Trim(),
                EmailDG = txtEmail.Text.Trim(),
                NgLapThe = dtpNgayLapThe.Value.Date
            };

            string thongBao;
            bool thanhCong = docGiaBUS.CapNhatDocGia(docGia, out thongBao);

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

        private void btnGiaHan_Click(object sender, EventArgs e)
        {
            if (maDocGiaDangChon <= 0)
            {
                MessageBox.Show("Vui lòng chọn độc giả cần gia hạn thẻ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string tenDG = dgvDocGia.CurrentRow == null || dgvDocGia.CurrentRow.Cells["TenDG"].Value == null
                ? string.Empty
                : dgvDocGia.CurrentRow.Cells["TenDG"].Value.ToString();

            DialogResult result = MessageBox.Show(
                "Bạn có chắc muốn gia hạn thẻ cho độc giả \"" + tenDG + "\"?",
                "Xác nhận gia hạn",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
            {
                return;
            }

            string thongBao;
            bool thanhCong = docGiaBUS.GiaHanThe(maDocGiaDangChon, out thongBao);

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

        private string LayLoaiDocGiaDangChon()
        {
            return cboLoaiDG.SelectedValue == null
                ? string.Empty
                : cboLoaiDG.SelectedValue.ToString();
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            LamMoiNhapLieu();
            TaiDuLieu();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvDocGia.CurrentRow == null || dgvDocGia.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Vui lòng chọn độc giả cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            object maDGValue = dgvDocGia.CurrentRow.Cells["MaDG"].Value;
            if (maDGValue == null || maDGValue == DBNull.Value)
            {
                MessageBox.Show("Không xác định được mã độc giả cần xóa.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maDG = Convert.ToInt32(maDGValue);
            string tenDG = dgvDocGia.CurrentRow.Cells["TenDG"].Value == null
                ? string.Empty
                : dgvDocGia.CurrentRow.Cells["TenDG"].Value.ToString();

            DialogResult result = MessageBox.Show(
                "Bạn có chắc muốn xóa độc giả \"" + tenDG + "\"?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
            {
                return;
            }

            string thongBao;
            bool thanhCong = docGiaBUS.XoaDocGia(maDG, out thongBao);

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

        private void LamMoiNhapLieu()
        {
            maDocGiaDangChon = 0;
            txtTenDG.Clear();
            if (cboLoaiDG.Items.Count > 0)
            {
                cboLoaiDG.SelectedIndex = 0;
            }
            dtpNgaySinh.Value = DateTime.Today;
            txtDiaChi.Clear();
            txtEmail.Clear();
            dtpNgayLapThe.Value = DateTime.Today;
            txtTenDG.Focus();
        }

        private void dgvDocGia_SelectionChanged(object sender, EventArgs e)
        {
            if (dangTaiDanhSachDocGia)
            {
                return;
            }

            if (dgvDocGia.CurrentRow == null || dgvDocGia.CurrentRow.IsNewRow)
            {
                return;
            }

            object maDGValue = dgvDocGia.CurrentRow.Cells["MaDG"].Value;
            if (maDGValue == null || maDGValue == DBNull.Value)
            {
                return;
            }

            maDocGiaDangChon = Convert.ToInt32(maDGValue);
            txtTenDG.Text = LayGiaTriChuoi("TenDG");

            string loaiDG = LayGiaTriChuoi("LoaiDG");
            if (!string.IsNullOrWhiteSpace(loaiDG))
            {
                cboLoaiDG.SelectedValue = loaiDG;
            }

            DatNgayChoDateTimePicker(dtpNgaySinh, "NgaySinhDG");
            txtDiaChi.Text = LayGiaTriChuoi("DiaChiDG");
            txtEmail.Text = LayGiaTriChuoi("EmailDG");
            DatNgayChoDateTimePicker(dtpNgayLapThe, "NgLapThe");
        }

        private string LayGiaTriChuoi(string tenCot)
        {
            object value = dgvDocGia.CurrentRow.Cells[tenCot].Value;
            return value == null || value == DBNull.Value ? string.Empty : value.ToString();
        }

        private void DatNgayChoDateTimePicker(DateTimePicker dateTimePicker, string tenCot)
        {
            object value = dgvDocGia.CurrentRow.Cells[tenCot].Value;
            if (value != null && value != DBNull.Value)
            {
                dateTimePicker.Value = Convert.ToDateTime(value);
            }
        }

        private void TaiDanhSachDocGia()
        {
            try
            {
                dangTaiDanhSachDocGia = true;
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
                    dgvDocGia.Columns["TrangThaiThe"].HeaderText = "Trạng thái thẻ";
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
                    dgvDocGia.Columns["TrangThaiThe"].FillWeight = 110;
                }

                dgvDocGia.ClearSelection();
                maDocGiaDangChon = 0;
                dangTaiDanhSachDocGia = false;
            }
            catch (Exception ex)
            {
                dangTaiDanhSachDocGia = false;
                MessageBox.Show(
                    "Không thể tải danh sách độc giả.\nChi tiết: " + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
