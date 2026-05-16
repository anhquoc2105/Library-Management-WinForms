using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyThuVien.BUS;
using QuanLyThuVien.UTILS;

namespace QuanLyThuVien.GUI
{
    public class FormThongTinCaNhan : Form
    {
        private readonly DocGiaBUS docGiaBUS = new DocGiaBUS();
        private readonly PhieuMuonBUS phieuMuonBUS = new PhieuMuonBUS();
        private Label lblTieuDe;
        private Label lblTenDG;
        private Label lblLoaiDG;
        private Label lblEmailDG;
        private Label lblNgayLapThe;
        private DataGridView dgvLichSuMuon;
        private Button btnTaiLai;
        private Button btnDong;

        public FormThongTinCaNhan()
        {
            TaoGiaoDien();
            Load += FormThongTinCaNhan_Load;
        }

        private void TaoGiaoDien()
        {
            Text = "Thông tin cá nhân";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1000, 600);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.FromArgb(244, 247, 251);
            Font = new Font("Segoe UI", 11F);

            lblTieuDe = new Label();
            lblTieuDe.Text = "Thông tin cá nhân";
            lblTieuDe.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTieuDe.ForeColor = Color.FromArgb(28, 77, 125);
            lblTieuDe.AutoSize = true;
            lblTieuDe.Location = new Point(34, 24);

            lblTenDG = TaoLabel("Tên độc giả: ", 40, 100);
            lblLoaiDG = TaoLabel("Loại độc giả: ", 40, 134);
            lblEmailDG = TaoLabel("Email: ", 40, 168);
            lblNgayLapThe = TaoLabel("Ngày lập thẻ: ", 40, 202);

            btnTaiLai = TaoButton("Tải lại", 770, 100, Color.FromArgb(230, 235, 241), Color.FromArgb(50, 60, 70));
            btnDong = TaoButton("Đóng", 886, 100, Color.FromArgb(230, 235, 241), Color.FromArgb(50, 60, 70));
            btnTaiLai.Click += btnTaiLai_Click;
            btnDong.Click += (sender, e) => Close();

            dgvLichSuMuon = new DataGridView();
            dgvLichSuMuon.Location = new Point(24, 260);
            dgvLichSuMuon.Size = new Size(940, 300);
            dgvLichSuMuon.ReadOnly = true;
            dgvLichSuMuon.AllowUserToAddRows = false;
            dgvLichSuMuon.AllowUserToDeleteRows = false;
            dgvLichSuMuon.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLichSuMuon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLichSuMuon.BackgroundColor = Color.White;
            dgvLichSuMuon.BorderStyle = BorderStyle.None;
            dgvLichSuMuon.RowHeadersVisible = false;
            dgvLichSuMuon.EnableHeadersVisualStyles = false;
            dgvLichSuMuon.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(237, 242, 247);
            dgvLichSuMuon.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(36, 52, 71);
            dgvLichSuMuon.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dgvLichSuMuon.ColumnHeadersHeight = 42;
            dgvLichSuMuon.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvLichSuMuon.DefaultCellStyle.SelectionBackColor = Color.FromArgb(220, 232, 246);
            dgvLichSuMuon.DefaultCellStyle.SelectionForeColor = Color.FromArgb(36, 52, 71);
            dgvLichSuMuon.DefaultCellStyle.Padding = new Padding(2, 4, 2, 4);
            dgvLichSuMuon.RowTemplate.Height = 36;

            Controls.Add(lblTieuDe);
            Controls.Add(lblTenDG);
            Controls.Add(lblLoaiDG);
            Controls.Add(lblEmailDG);
            Controls.Add(lblNgayLapThe);
            Controls.Add(btnTaiLai);
            Controls.Add(btnDong);
            Controls.Add(dgvLichSuMuon);
        }

        private Label TaoLabel(string text, int x, int y)
        {
            Label label = new Label();
            label.Text = text;
            label.AutoSize = true;
            label.Location = new Point(x, y);
            return label;
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

        private void FormThongTinCaNhan_Load(object sender, EventArgs e)
        {
            TaiThongTin();
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            TaiThongTin();
        }

        private void TaiThongTin()
        {
            if (!string.Equals(SessionManager.VaiTro, "DocGia", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Chức năng này chỉ dành cho độc giả.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
                return;
            }

            DataTable thongTin = docGiaBUS.LayThongTinTheoMaTaiKhoan(SessionManager.MaTaiKhoan);

            if (thongTin.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy thông tin độc giả.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DataRow row = thongTin.Rows[0];
            lblTenDG.Text = "Tên độc giả: " + row["TenDG"];
            lblLoaiDG.Text = "Loại độc giả: " + row["LoaiDG"];
            lblEmailDG.Text = "Email: " + row["EmailDG"];
            lblNgayLapThe.Text = "Ngày lập thẻ: " + Convert.ToDateTime(row["NgLapThe"]).ToString("dd/MM/yyyy");

            dgvLichSuMuon.DataSource = phieuMuonBUS.LayLichSuMuonTheoMaTaiKhoan(SessionManager.MaTaiKhoan);
            DinhDangCot();
        }

        private void DinhDangCot()
        {
            if (dgvLichSuMuon.Columns.Count == 0)
            {
                return;
            }

            dgvLichSuMuon.Columns["MaPhieu"].HeaderText = "Mã phiếu";
            dgvLichSuMuon.Columns["TenSach"].HeaderText = "Tên sách";
            dgvLichSuMuon.Columns["NgayMuon"].HeaderText = "Ngày mượn";
            dgvLichSuMuon.Columns["NgayPhaiTra"].HeaderText = "Ngày phải trả";
            dgvLichSuMuon.Columns["NgayTra"].HeaderText = "Ngày trả";
            dgvLichSuMuon.Columns["TienPhatKyNay"].HeaderText = "Tiền phạt kỳ này";
            dgvLichSuMuon.Columns["TrangThaiMuon"].HeaderText = "Trạng thái mượn";

            dgvLichSuMuon.Columns["MaPhieu"].FillWeight = 75;
            dgvLichSuMuon.Columns["TenSach"].FillWeight = 130;
            dgvLichSuMuon.Columns["NgayMuon"].FillWeight = 95;
            dgvLichSuMuon.Columns["NgayPhaiTra"].FillWeight = 105;
            dgvLichSuMuon.Columns["NgayTra"].FillWeight = 95;
            dgvLichSuMuon.Columns["TienPhatKyNay"].FillWeight = 95;
            dgvLichSuMuon.Columns["TrangThaiMuon"].FillWeight = 95;

            dgvLichSuMuon.ClearSelection();
        }
    }
}
