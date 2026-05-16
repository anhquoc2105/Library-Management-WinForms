using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyThuVien.BUS;

namespace QuanLyThuVien.GUI
{
    public class FormSuaQuyDinh : Form
    {
        private readonly ThamSoBUS thamSoBUS = new ThamSoBUS();
        private readonly TheLoaiBUS theLoaiBUS = new TheLoaiBUS();

        private Label lblTieuDe;
        private Label lblMoTa;
        private Label lblGiaTriThe;
        private Label lblSoTuoiToiThieu;
        private Label lblSoTuoiToiDa;
        private Label lblThoiGianXB;
        private Label lblSoSachMuonToiDa;
        private Label lblSoNgayMuonToiDa;
        private TextBox txtGiaTriThe;
        private TextBox txtSoTuoiToiThieu;
        private TextBox txtSoTuoiToiDa;
        private TextBox txtThoiGianXB;
        private TextBox txtSoSachMuonToiDa;
        private TextBox txtSoNgayMuonToiDa;
        private Button btnLuuThamSo;

        private Label lblTheLoai;
        private TextBox txtTenTheLoai;
        private DataGridView dgvTheLoai;
        private Button btnThemTheLoai;
        private Button btnSuaTheLoai;
        private Button btnXoaTheLoai;
        private Button btnDong;

        private int maThamSoHienTai;

        public FormSuaQuyDinh()
        {
            TaoGiaoDien();
            Load += FormSuaQuyDinh_Load;
        }

        private void TaoGiaoDien()
        {
            Text = "Thay đổi quy định";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1120, 700);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.FromArgb(244, 247, 251);
            Font = new Font("Segoe UI", 11F);

            lblTieuDe = new Label();
            lblTieuDe.Text = "Thay đổi quy định";
            lblTieuDe.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTieuDe.ForeColor = Color.FromArgb(28, 77, 125);
            lblTieuDe.AutoSize = true;
            lblTieuDe.Location = new Point(34, 22);

            lblMoTa = new Label();
            lblMoTa.Text = "Thủ thư có thể thay đổi quy định về thẻ, thể loại, khoảng năm xuất bản và số lượng mượn tối đa.";
            lblMoTa.ForeColor = Color.FromArgb(102, 117, 132);
            lblMoTa.AutoSize = true;
            lblMoTa.Location = new Point(38, 58);

            lblGiaTriThe = TaoLabel("Thời hạn thẻ (tháng)", 40, 110);
            lblSoTuoiToiThieu = TaoLabel("Tuổi tối thiểu", 40, 165);
            lblSoTuoiToiDa = TaoLabel("Tuổi tối đa", 40, 220);
            lblThoiGianXB = TaoLabel("Khoảng năm XB", 380, 110);
            lblSoSachMuonToiDa = TaoLabel("Sách mượn tối đa", 380, 165);
            lblSoNgayMuonToiDa = TaoLabel("Ngày mượn tối đa", 380, 220);

            txtGiaTriThe = TaoTextBox(220, 106, 120);
            txtSoTuoiToiThieu = TaoTextBox(220, 161, 120);
            txtSoTuoiToiDa = TaoTextBox(220, 216, 120);
            txtThoiGianXB = TaoTextBox(560, 106, 120);
            txtSoSachMuonToiDa = TaoTextBox(560, 161, 120);
            txtSoNgayMuonToiDa = TaoTextBox(560, 216, 120);

            btnLuuThamSo = TaoButton("Lưu tham số", 860, 208, 160, Color.FromArgb(28, 77, 125), Color.White);
            btnLuuThamSo.Click += btnLuuThamSo_Click;

            lblTheLoai = TaoLabel("Tên thể loại", 40, 320);
            txtTenTheLoai = TaoTextBox(180, 316, 260);

            btnThemTheLoai = TaoButton("Thêm", 470, 312, 100, Color.FromArgb(28, 77, 125), Color.White);
            btnSuaTheLoai = TaoButton("Sửa", 580, 312, 100, Color.FromArgb(230, 235, 241), Color.FromArgb(50, 60, 70));
            btnXoaTheLoai = TaoButton("Xóa", 690, 312, 100, Color.FromArgb(230, 235, 241), Color.FromArgb(50, 60, 70));
            btnDong = TaoButton("Đóng", 920, 620, 100, Color.FromArgb(230, 235, 241), Color.FromArgb(50, 60, 70));

            btnThemTheLoai.Click += btnThemTheLoai_Click;
            btnSuaTheLoai.Click += btnSuaTheLoai_Click;
            btnXoaTheLoai.Click += btnXoaTheLoai_Click;
            btnDong.Click += (sender, e) => Close();

            dgvTheLoai = new DataGridView();
            dgvTheLoai.Location = new Point(40, 380);
            dgvTheLoai.Size = new Size(980, 220);
            dgvTheLoai.ReadOnly = true;
            dgvTheLoai.AllowUserToAddRows = false;
            dgvTheLoai.AllowUserToDeleteRows = false;
            dgvTheLoai.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTheLoai.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTheLoai.MultiSelect = false;
            dgvTheLoai.BackgroundColor = Color.White;
            dgvTheLoai.BorderStyle = BorderStyle.None;
            dgvTheLoai.RowHeadersVisible = false;
            dgvTheLoai.EnableHeadersVisualStyles = false;
            dgvTheLoai.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(237, 242, 247);
            dgvTheLoai.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(36, 52, 71);
            dgvTheLoai.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dgvTheLoai.ColumnHeadersHeight = 42;
            dgvTheLoai.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvTheLoai.DefaultCellStyle.SelectionBackColor = Color.FromArgb(220, 232, 246);
            dgvTheLoai.DefaultCellStyle.SelectionForeColor = Color.FromArgb(36, 52, 71);
            dgvTheLoai.DefaultCellStyle.Padding = new Padding(2, 4, 2, 4);
            dgvTheLoai.RowTemplate.Height = 36;
            dgvTheLoai.CellClick += dgvTheLoai_CellClick;

            Controls.Add(lblTieuDe);
            Controls.Add(lblMoTa);
            Controls.Add(lblGiaTriThe);
            Controls.Add(lblSoTuoiToiThieu);
            Controls.Add(lblSoTuoiToiDa);
            Controls.Add(lblThoiGianXB);
            Controls.Add(lblSoSachMuonToiDa);
            Controls.Add(lblSoNgayMuonToiDa);
            Controls.Add(txtGiaTriThe);
            Controls.Add(txtSoTuoiToiThieu);
            Controls.Add(txtSoTuoiToiDa);
            Controls.Add(txtThoiGianXB);
            Controls.Add(txtSoSachMuonToiDa);
            Controls.Add(txtSoNgayMuonToiDa);
            Controls.Add(btnLuuThamSo);
            Controls.Add(lblTheLoai);
            Controls.Add(txtTenTheLoai);
            Controls.Add(btnThemTheLoai);
            Controls.Add(btnSuaTheLoai);
            Controls.Add(btnXoaTheLoai);
            Controls.Add(btnDong);
            Controls.Add(dgvTheLoai);
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
            textBox.Size = new Size(width, 28);
            return textBox;
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

        private void FormSuaQuyDinh_Load(object sender, EventArgs e)
        {
            TaiThamSo();
            TaiTheLoai();
        }

        private void TaiThamSo()
        {
            DataTable dt = thamSoBUS.LayDanhSachThamSo();
            if (dt.Rows.Count == 0)
            {
                return;
            }

            DataRow row = dt.Rows[0];
            maThamSoHienTai = Convert.ToInt32(row["MaThamSo"]);
            txtGiaTriThe.Text = row["GiaTriThe"].ToString();
            txtSoTuoiToiThieu.Text = row["SoTuoiDG"].ToString();
            txtSoTuoiToiDa.Text = row["SoTuoiDGToiDa"].ToString();
            txtThoiGianXB.Text = row["ThoiGianXB"].ToString();
            txtSoSachMuonToiDa.Text = row["SoSachMuonToiDa"].ToString();
            txtSoNgayMuonToiDa.Text = row["SoNgayMuonToiDa"].ToString();
        }

        private void TaiTheLoai()
        {
            dgvTheLoai.DataSource = theLoaiBUS.LayDanhSachTheLoai();
            if (dgvTheLoai.Columns.Count > 0)
            {
                dgvTheLoai.Columns["MaTheLoai"].HeaderText = "Mã thể loại";
                dgvTheLoai.Columns["TenTheLoai"].HeaderText = "Tên thể loại";
            }

            dgvTheLoai.ClearSelection();
        }

        private void btnLuuThamSo_Click(object sender, EventArgs e)
        {
            int giaTriThe;
            int soTuoiToiThieu;
            int soTuoiToiDa;
            int thoiGianXB;
            int soSachMuonToiDa;
            int soNgayMuonToiDa;

            if (!int.TryParse(txtGiaTriThe.Text.Trim(), out giaTriThe) ||
                !int.TryParse(txtSoTuoiToiThieu.Text.Trim(), out soTuoiToiThieu) ||
                !int.TryParse(txtSoTuoiToiDa.Text.Trim(), out soTuoiToiDa) ||
                !int.TryParse(txtThoiGianXB.Text.Trim(), out thoiGianXB) ||
                !int.TryParse(txtSoSachMuonToiDa.Text.Trim(), out soSachMuonToiDa) ||
                !int.TryParse(txtSoNgayMuonToiDa.Text.Trim(), out soNgayMuonToiDa))
            {
                MessageBox.Show("Vui lòng nhập đúng định dạng cho các tham số.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string thongBao;
            bool thanhCong = thamSoBUS.CapNhatThamSo(
                maThamSoHienTai,
                giaTriThe,
                soTuoiToiThieu,
                soTuoiToiDa,
                thoiGianXB,
                soSachMuonToiDa,
                soNgayMuonToiDa,
                out thongBao);

            MessageBox.Show(
                thongBao,
                thanhCong ? "Thông báo" : "Lỗi",
                MessageBoxButtons.OK,
                thanhCong ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
        }

        private void btnThemTheLoai_Click(object sender, EventArgs e)
        {
            string thongBao;
            bool thanhCong = theLoaiBUS.ThemTheLoai(txtTenTheLoai.Text.Trim(), out thongBao);
            MessageBox.Show(
                thongBao,
                thanhCong ? "Thông báo" : "Lỗi",
                MessageBoxButtons.OK,
                thanhCong ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            if (thanhCong)
            {
                txtTenTheLoai.Clear();
                TaiTheLoai();
            }
        }

        private void btnSuaTheLoai_Click(object sender, EventArgs e)
        {
            if (dgvTheLoai.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn thể loại cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maTheLoai = Convert.ToInt32(dgvTheLoai.CurrentRow.Cells["MaTheLoai"].Value);
            string thongBao;
            bool thanhCong = theLoaiBUS.CapNhatTheLoai(maTheLoai, txtTenTheLoai.Text.Trim(), out thongBao);

            MessageBox.Show(
                thongBao,
                thanhCong ? "Thông báo" : "Lỗi",
                MessageBoxButtons.OK,
                thanhCong ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            if (thanhCong)
            {
                txtTenTheLoai.Clear();
                TaiTheLoai();
            }
        }

        private void btnXoaTheLoai_Click(object sender, EventArgs e)
        {
            if (dgvTheLoai.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn thể loại cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maTheLoai = Convert.ToInt32(dgvTheLoai.CurrentRow.Cells["MaTheLoai"].Value);
            string thongBao;
            bool thanhCong = theLoaiBUS.XoaTheLoai(maTheLoai, out thongBao);

            MessageBox.Show(
                thongBao,
                thanhCong ? "Thông báo" : "Lỗi",
                MessageBoxButtons.OK,
                thanhCong ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            if (thanhCong)
            {
                txtTenTheLoai.Clear();
                TaiTheLoai();
            }
        }

        private void dgvTheLoai_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            txtTenTheLoai.Text = dgvTheLoai.Rows[e.RowIndex].Cells["TenTheLoai"].Value.ToString();
        }
    }
}
