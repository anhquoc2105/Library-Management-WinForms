using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyThuVien.BUS;

namespace QuanLyThuVien.GUI
{
    public class FormTraSach : Form
    {
        private readonly PhieuMuonBUS phieuMuonBUS = new PhieuMuonBUS();
        private Label lblTieuDe;
        private Label lblMoTa;
        private DataGridView dgvSachDangMuon;
        private Button btnTraSach;
        private Button btnTaiLai;
        private Button btnDong;

        public FormTraSach()
        {
            TaoGiaoDien();
            Load += FormTraSach_Load;
        }

        private void TaoGiaoDien()
        {
            Text = "Nhận trả sách";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1120, 620);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.FromArgb(244, 247, 251);
            Font = new Font("Segoe UI", 11F);

            lblTieuDe = new Label();
            lblTieuDe.Text = "Nhận trả sách";
            lblTieuDe.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTieuDe.ForeColor = Color.FromArgb(28, 77, 125);
            lblTieuDe.AutoSize = true;
            lblTieuDe.Location = new Point(34, 24);

            lblMoTa = new Label();
            lblMoTa.Text = "Theo dõi sách đang mượn, ngày phải trả và tiền phạt phát sinh khi trả trễ.";
            lblMoTa.ForeColor = Color.FromArgb(102, 117, 132);
            lblMoTa.Font = new Font("Segoe UI", 10.5F);
            lblMoTa.AutoSize = true;
            lblMoTa.Location = new Point(38, 64);

            btnTraSach = TaoButton("Trả sách", 760, 104, Color.FromArgb(28, 77, 125), Color.White);
            btnTaiLai = TaoButton("Tải lại", 876, 104, Color.FromArgb(230, 235, 241), Color.FromArgb(50, 60, 70));
            btnDong = TaoButton("Đóng", 992, 104, Color.FromArgb(230, 235, 241), Color.FromArgb(50, 60, 70));

            btnTraSach.Click += btnTraSach_Click;
            btnTaiLai.Click += btnTaiLai_Click;
            btnDong.Click += (sender, e) => Close();

            dgvSachDangMuon = new DataGridView();
            dgvSachDangMuon.Location = new Point(24, 170);
            dgvSachDangMuon.Size = new Size(1060, 400);
            dgvSachDangMuon.ReadOnly = true;
            dgvSachDangMuon.AllowUserToAddRows = false;
            dgvSachDangMuon.AllowUserToDeleteRows = false;
            dgvSachDangMuon.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSachDangMuon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSachDangMuon.MultiSelect = false;
            dgvSachDangMuon.BackgroundColor = Color.White;
            dgvSachDangMuon.BorderStyle = BorderStyle.None;
            dgvSachDangMuon.RowHeadersVisible = false;
            dgvSachDangMuon.EnableHeadersVisualStyles = false;
            dgvSachDangMuon.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(237, 242, 247);
            dgvSachDangMuon.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(36, 52, 71);
            dgvSachDangMuon.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dgvSachDangMuon.ColumnHeadersHeight = 40;
            dgvSachDangMuon.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvSachDangMuon.DefaultCellStyle.SelectionBackColor = Color.FromArgb(220, 232, 246);
            dgvSachDangMuon.DefaultCellStyle.SelectionForeColor = Color.FromArgb(36, 52, 71);
            dgvSachDangMuon.RowTemplate.Height = 34;

            Controls.Add(lblTieuDe);
            Controls.Add(lblMoTa);
            Controls.Add(btnTraSach);
            Controls.Add(btnTaiLai);
            Controls.Add(btnDong);
            Controls.Add(dgvSachDangMuon);
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

        private void FormTraSach_Load(object sender, EventArgs e)
        {
            TaiDanhSachSachDangMuon();
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            TaiDanhSachSachDangMuon();
        }

        private void btnTraSach_Click(object sender, EventArgs e)
        {
            if (dgvSachDangMuon.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn sách cần trả.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maPhieu = Convert.ToInt32(dgvSachDangMuon.CurrentRow.Cells["MaPhieu"].Value);
            int maSach = Convert.ToInt32(dgvSachDangMuon.CurrentRow.Cells["MaSach"].Value);
            string thongBao;

            bool thanhCong = phieuMuonBUS.TraSach(maPhieu, maSach, out thongBao);
            MessageBox.Show(
                thongBao,
                thanhCong ? "Thông báo" : "Lỗi",
                MessageBoxButtons.OK,
                thanhCong ? MessageBoxIcon.Information : MessageBoxIcon.Error);

            if (thanhCong)
            {
                TaiDanhSachSachDangMuon();
            }
        }

        private void TaiDanhSachSachDangMuon()
        {
            try
            {
                dgvSachDangMuon.DataSource = phieuMuonBUS.LayDanhSachSachDangMuon();

                if (dgvSachDangMuon.Columns.Count > 0)
                {
                    dgvSachDangMuon.Columns["MaPhieu"].HeaderText = "Mã phiếu";
                    dgvSachDangMuon.Columns["MaDG"].HeaderText = "Mã độc giả";
                    dgvSachDangMuon.Columns["TenDG"].HeaderText = "Họ tên độc giả";
                    dgvSachDangMuon.Columns["MaSach"].HeaderText = "Mã sách";
                    dgvSachDangMuon.Columns["TenSach"].HeaderText = "Tên sách";
                    dgvSachDangMuon.Columns["NgayMuon"].HeaderText = "Ngày mượn";
                    dgvSachDangMuon.Columns["NgayPhaiTra"].HeaderText = "Ngày phải trả";
                    dgvSachDangMuon.Columns["NgayTra"].HeaderText = "Ngày trả";
                    dgvSachDangMuon.Columns["TienPhatKyNay"].HeaderText = "Tiền phạt kỳ này";
                    dgvSachDangMuon.Columns["TinhTrang"].HeaderText = "Tình trạng";
                }

                dgvSachDangMuon.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải danh sách sách đang mượn.\nChi tiết: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
