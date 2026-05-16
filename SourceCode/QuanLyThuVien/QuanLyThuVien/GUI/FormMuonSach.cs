using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyThuVien.BUS;

namespace QuanLyThuVien.GUI
{
    public class FormMuonSach : Form
    {
        private readonly DocGiaBUS docGiaBUS = new DocGiaBUS();
        private readonly SachBUS sachBUS = new SachBUS();
        private readonly PhieuMuonBUS phieuMuonBUS = new PhieuMuonBUS();
        private Label lblTieuDe;
        private Label lblMoTa;
        private Label lblDocGia;
        private ComboBox cboDocGia;
        private DataGridView dgvSachCon;
        private Button btnLapPhieu;
        private Button btnTaiLai;
        private Button btnDong;

        public FormMuonSach()
        {
            TaoGiaoDien();
            Load += FormMuonSach_Load;
        }

        private void TaoGiaoDien()
        {
            Text = "Cho mượn sách";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1000, 600);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.FromArgb(244, 247, 251);
            Font = new Font("Segoe UI", 11F);

            lblTieuDe = new Label();
            lblTieuDe.Text = "Cho mượn sách";
            lblTieuDe.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTieuDe.ForeColor = Color.FromArgb(28, 77, 125);
            lblTieuDe.AutoSize = true;
            lblTieuDe.Location = new Point(34, 24);

            lblMoTa = new Label();
            lblMoTa.Text = "Chọn độc giả và sách còn trong kho để lập giao dịch mượn.";
            lblMoTa.ForeColor = Color.FromArgb(102, 117, 132);
            lblMoTa.Font = new Font("Segoe UI", 10.5F);
            lblMoTa.AutoSize = true;
            lblMoTa.Location = new Point(38, 64);

            lblDocGia = new Label();
            lblDocGia.Text = "Độc giả";
            lblDocGia.AutoSize = true;
            lblDocGia.Location = new Point(40, 110);

            cboDocGia = new ComboBox();
            cboDocGia.DropDownStyle = ComboBoxStyle.DropDownList;
            cboDocGia.Location = new Point(40, 136);
            cboDocGia.Size = new Size(340, 30);

            btnLapPhieu = TaoButton("Lập phiếu mượn", 590, 128, 160, Color.FromArgb(28, 77, 125), Color.White);
            btnTaiLai = TaoButton("Tải lại", 770, 128, 108, Color.FromArgb(230, 235, 241), Color.FromArgb(50, 60, 70));
            btnDong = TaoButton("Đóng", 892, 128, 108, Color.FromArgb(230, 235, 241), Color.FromArgb(50, 60, 70));

            btnLapPhieu.Click += btnLapPhieu_Click;
            btnTaiLai.Click += btnTaiLai_Click;
            btnDong.Click += (sender, e) => Close();

            dgvSachCon = new DataGridView();
            dgvSachCon.Location = new Point(24, 200);
            dgvSachCon.Size = new Size(940, 360);
            dgvSachCon.ReadOnly = true;
            dgvSachCon.AllowUserToAddRows = false;
            dgvSachCon.AllowUserToDeleteRows = false;
            dgvSachCon.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSachCon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSachCon.MultiSelect = false;
            dgvSachCon.BackgroundColor = Color.White;
            dgvSachCon.BorderStyle = BorderStyle.None;
            dgvSachCon.RowHeadersVisible = false;
            dgvSachCon.EnableHeadersVisualStyles = false;
            dgvSachCon.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(237, 242, 247);
            dgvSachCon.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(36, 52, 71);
            dgvSachCon.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dgvSachCon.ColumnHeadersHeight = 42;
            dgvSachCon.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvSachCon.DefaultCellStyle.SelectionBackColor = Color.FromArgb(220, 232, 246);
            dgvSachCon.DefaultCellStyle.SelectionForeColor = Color.FromArgb(36, 52, 71);
            dgvSachCon.DefaultCellStyle.Padding = new Padding(2, 4, 2, 4);
            dgvSachCon.RowTemplate.Height = 36;

            Controls.Add(lblTieuDe);
            Controls.Add(lblMoTa);
            Controls.Add(lblDocGia);
            Controls.Add(cboDocGia);
            Controls.Add(btnLapPhieu);
            Controls.Add(btnTaiLai);
            Controls.Add(btnDong);
            Controls.Add(dgvSachCon);
        }

        private Button TaoButton(string text, int x, int y, int width, Color backColor, Color foreColor)
        {
            Button button = new Button();
            button.Text = text;
            button.Size = new Size(width, 40);
            button.Location = new Point(x, y);
            button.BackColor = backColor;
            button.ForeColor = foreColor;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            return button;
        }

        private void FormMuonSach_Load(object sender, EventArgs e)
        {
            TaiDuLieu();
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            TaiDuLieu();
        }

        private void TaiDuLieu()
        {
            DataTable docGiaTable = docGiaBUS.LayDanhSachDocGiaChoCombo();
            cboDocGia.DataSource = docGiaTable;
            cboDocGia.DisplayMember = "TenDG";
            cboDocGia.ValueMember = "MaDG";

            dgvSachCon.DataSource = sachBUS.LayDanhSachSachCon();
            DinhDangCot();
        }

        private void btnLapPhieu_Click(object sender, EventArgs e)
        {
            if (cboDocGia.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn độc giả.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvSachCon.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn sách cần mượn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maDG = Convert.ToInt32(cboDocGia.SelectedValue);
            int maSach = Convert.ToInt32(dgvSachCon.CurrentRow.Cells["MaSach"].Value);
            string thongBao;

            bool thanhCong = phieuMuonBUS.LapPhieuMuon(maDG, maSach, out thongBao);
            MessageBox.Show(
                thongBao,
                thanhCong ? "Thông báo" : "Lỗi",
                MessageBoxButtons.OK,
                thanhCong ? MessageBoxIcon.Information : MessageBoxIcon.Error);

            if (thanhCong)
            {
                TaiDuLieu();
            }
        }

        private void DinhDangCot()
        {
            if (dgvSachCon.Columns.Count == 0)
            {
                return;
            }

            if (dgvSachCon.Columns["MaSach"] != null) dgvSachCon.Columns["MaSach"].HeaderText = "Mã sách";
            if (dgvSachCon.Columns["TenSach"] != null) dgvSachCon.Columns["TenSach"].HeaderText = "Tên sách";
            if (dgvSachCon.Columns["TenTheLoai"] != null) dgvSachCon.Columns["TenTheLoai"].HeaderText = "Thể loại";
            if (dgvSachCon.Columns["TenTG"] != null) dgvSachCon.Columns["TenTG"].HeaderText = "Tác giả";
            if (dgvSachCon.Columns["NamXB"] != null) dgvSachCon.Columns["NamXB"].HeaderText = "Năm XB";
            if (dgvSachCon.Columns["NhaXB"] != null) dgvSachCon.Columns["NhaXB"].HeaderText = "Nhà XB";
            if (dgvSachCon.Columns["TriGia"] != null) dgvSachCon.Columns["TriGia"].HeaderText = "Trị giá";
            if (dgvSachCon.Columns["SoLuongTon"] != null) dgvSachCon.Columns["SoLuongTon"].HeaderText = "Số lượng còn";
            if (dgvSachCon.Columns["TinhTrang"] != null) dgvSachCon.Columns["TinhTrang"].HeaderText = "Tình trạng";
            if (dgvSachCon.Columns["NgayNhap"] != null) dgvSachCon.Columns["NgayNhap"].HeaderText = "Ngày nhập";

            dgvSachCon.ClearSelection();
        }
    }
}
