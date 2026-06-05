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
        private readonly LoaiDocGiaBUS loaiDocGiaBUS = new LoaiDocGiaBUS();

        private Label lblTieuDe;
        private Label lblMoTa;
        private Label lblGiaTriThe;
        private Label lblSoTuoiToiThieu;
        private Label lblSoTuoiToiDa;
        private Label lblThoiGianXB;
        private Label lblSoSachMuonToiDa;
        private Label lblSoNgayMuonToiDa;
        private Label lblTienPhat;
        private TextBox txtGiaTriThe;
        private TextBox txtSoTuoiToiThieu;
        private TextBox txtSoTuoiToiDa;
        private TextBox txtThoiGianXB;
        private TextBox txtSoSachMuonToiDa;
        private TextBox txtSoNgayMuonToiDa;
        private TextBox txtTienPhat;
        private Button btnLuuThamSo;

        private Label lblTheLoai;
        private TextBox txtTenTheLoai;
        private DataGridView dgvTheLoai;
        private Button btnThemTheLoai;
        private Button btnSuaTheLoai;
        private Button btnXoaTheLoai;

        private Label lblLoaiDocGia;
        private TextBox txtTenLoaiDocGia;
        private DataGridView dgvLoaiDocGia;
        private Button btnThemLoaiDocGia;
        private Button btnSuaLoaiDocGia;
        private Button btnXoaLoaiDocGia;
        private Button btnDong;

        private int maThamSoHienTai;
        private int? maTheLoaiDangChon;
        private int? maLoaiDocGiaDangChon;
        private bool isLoading;

        public FormSuaQuyDinh()
        {
            TaoGiaoDien();
            Load += FormSuaQuyDinh_Load;
        }

        private void TaoGiaoDien()
        {
            Text = "Thay đổi quy định";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1450, 820);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.FromArgb(244, 247, 251);
            Font = new Font("Segoe UI", 11F);

            lblTieuDe = new Label();
            lblTieuDe.Text = "Thay đổi quy định";
            lblTieuDe.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTieuDe.ForeColor = Color.FromArgb(28, 77, 125);
            lblTieuDe.AutoSize = true;
            lblTieuDe.Location = new Point(44, 28);

            lblMoTa = new Label();
            lblMoTa.Text = "Thủ thư có thể thay đổi quy định về thẻ, thể loại, khoảng năm xuất bản và số lượng mượn tối đa.";
            lblMoTa.ForeColor = Color.FromArgb(102, 117, 132);
            lblMoTa.AutoSize = true;
            lblMoTa.Font = new Font("Segoe UI", 10.5F);
            lblMoTa.Location = new Point(48, 72);

            lblGiaTriThe = TaoLabel("Thời hạn thẻ (tháng)", 40, 110);
            lblSoTuoiToiThieu = TaoLabel("Tuổi tối thiểu", 40, 165);
            lblSoTuoiToiDa = TaoLabel("Tuổi tối đa", 40, 220);
            lblThoiGianXB = TaoLabel("Khoảng năm XB", 380, 110);
            lblSoSachMuonToiDa = TaoLabel("Sách mượn tối đa", 380, 165);
            lblSoNgayMuonToiDa = TaoLabel("Ngày mượn tối đa", 380, 220);
            lblTienPhat = TaoLabel("Tiền phạt/ngày", 720, 110);

            txtGiaTriThe = TaoTextBox(220, 106, 120);
            txtSoTuoiToiThieu = TaoTextBox(220, 161, 120);
            txtSoTuoiToiDa = TaoTextBox(220, 216, 120);
            txtThoiGianXB = TaoTextBox(560, 106, 120);
            txtSoSachMuonToiDa = TaoTextBox(560, 161, 120);
            txtSoNgayMuonToiDa = TaoTextBox(560, 216, 120);
            txtTienPhat = TaoTextBox(860, 106, 120);

            btnLuuThamSo = TaoButton("Lưu tham số", 860, 208, 160, Color.FromArgb(28, 77, 125), Color.White);
            btnLuuThamSo.Click += btnLuuThamSo_Click;

            lblTheLoai = TaoLabel("Thể loại sách", 40, 306);
            txtTenTheLoai = TaoTextBox(40, 334, 260);

            btnThemTheLoai = TaoButton("Thêm", 320, 330, 90, Color.FromArgb(28, 77, 125), Color.White);
            btnSuaTheLoai = TaoButton("Sửa", 420, 330, 90, Color.FromArgb(230, 235, 241), Color.FromArgb(50, 60, 70));
            btnXoaTheLoai = TaoButton("Xóa", 520, 330, 90, Color.FromArgb(230, 235, 241), Color.FromArgb(50, 60, 70));

            lblLoaiDocGia = TaoLabel("Loại độc giả", 640, 306);
            txtTenLoaiDocGia = TaoTextBox(640, 334, 260);

            btnThemLoaiDocGia = TaoButton("Thêm", 920, 330, 90, Color.FromArgb(28, 77, 125), Color.White);
            btnSuaLoaiDocGia = TaoButton("Sửa", 1020, 330, 90, Color.FromArgb(230, 235, 241), Color.FromArgb(50, 60, 70));
            btnXoaLoaiDocGia = TaoButton("Xóa", 920, 382, 90, Color.FromArgb(230, 235, 241), Color.FromArgb(50, 60, 70));
            btnDong = TaoButton("Đóng", 920, 620, 100, Color.FromArgb(230, 235, 241), Color.FromArgb(50, 60, 70));

            btnThemTheLoai.Click += btnThemTheLoai_Click;
            btnSuaTheLoai.Click += btnSuaTheLoai_Click;
            btnXoaTheLoai.Click += btnXoaTheLoai_Click;
            btnThemLoaiDocGia.Click += btnThemLoaiDocGia_Click;
            btnSuaLoaiDocGia.Click += btnSuaLoaiDocGia_Click;
            btnXoaLoaiDocGia.Click += btnXoaLoaiDocGia_Click;
            btnDong.Click += (sender, e) => Close();

            dgvTheLoai = new DataGridView();
            dgvTheLoai.Location = new Point(40, 386);
            dgvTheLoai.Size = new Size(570, 214);
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
            dgvTheLoai.SelectionChanged += dgvTheLoai_SelectionChanged;

            dgvLoaiDocGia = TaoDataGridView(640, 434, 470, 166);
            dgvLoaiDocGia.CellClick += dgvLoaiDocGia_CellClick;
            dgvLoaiDocGia.SelectionChanged += dgvLoaiDocGia_SelectionChanged;

            ApDungBoCucRong();

            Controls.Add(lblTieuDe);
            Controls.Add(lblMoTa);
            Controls.Add(lblGiaTriThe);
            Controls.Add(lblSoTuoiToiThieu);
            Controls.Add(lblSoTuoiToiDa);
            Controls.Add(lblThoiGianXB);
            Controls.Add(lblSoSachMuonToiDa);
            Controls.Add(lblSoNgayMuonToiDa);
            Controls.Add(lblTienPhat);
            Controls.Add(txtGiaTriThe);
            Controls.Add(txtSoTuoiToiThieu);
            Controls.Add(txtSoTuoiToiDa);
            Controls.Add(txtThoiGianXB);
            Controls.Add(txtSoSachMuonToiDa);
            Controls.Add(txtSoNgayMuonToiDa);
            Controls.Add(txtTienPhat);
            Controls.Add(btnLuuThamSo);
            Controls.Add(lblTheLoai);
            Controls.Add(txtTenTheLoai);
            Controls.Add(btnThemTheLoai);
            Controls.Add(btnSuaTheLoai);
            Controls.Add(btnXoaTheLoai);
            Controls.Add(lblLoaiDocGia);
            Controls.Add(txtTenLoaiDocGia);
            Controls.Add(btnThemLoaiDocGia);
            Controls.Add(btnSuaLoaiDocGia);
            Controls.Add(btnXoaLoaiDocGia);
            Controls.Add(btnDong);
            Controls.Add(dgvTheLoai);
            Controls.Add(dgvLoaiDocGia);
        }

        private void ApDungBoCucRong()
        {
            lblGiaTriThe.Location = new Point(50, 126);
            lblSoTuoiToiThieu.Location = new Point(50, 190);
            lblSoTuoiToiDa.Location = new Point(50, 254);
            lblThoiGianXB.Location = new Point(440, 126);
            lblSoSachMuonToiDa.Location = new Point(440, 190);
            lblSoNgayMuonToiDa.Location = new Point(440, 254);
            lblTienPhat.Location = new Point(830, 126);

            txtGiaTriThe.Location = new Point(250, 122);
            txtSoTuoiToiThieu.Location = new Point(250, 186);
            txtSoTuoiToiDa.Location = new Point(250, 250);
            txtThoiGianXB.Location = new Point(650, 122);
            txtSoSachMuonToiDa.Location = new Point(650, 186);
            txtSoNgayMuonToiDa.Location = new Point(650, 250);
            txtTienPhat.Location = new Point(1010, 122);

            txtGiaTriThe.Size = new Size(150, 30);
            txtSoTuoiToiThieu.Size = new Size(150, 30);
            txtSoTuoiToiDa.Size = new Size(150, 30);
            txtThoiGianXB.Size = new Size(150, 30);
            txtSoSachMuonToiDa.Size = new Size(150, 30);
            txtSoNgayMuonToiDa.Size = new Size(150, 30);
            txtTienPhat.Size = new Size(170, 30);

            btnLuuThamSo.Location = new Point(1220, 184);
            btnLuuThamSo.Size = new Size(150, 42);

            lblTheLoai.Location = new Point(50, 340);
            txtTenTheLoai.Location = new Point(50, 370);
            txtTenTheLoai.Size = new Size(360, 30);
            btnThemTheLoai.Location = new Point(430, 366);
            btnSuaTheLoai.Location = new Point(542, 366);
            btnXoaTheLoai.Location = new Point(654, 366);
            btnThemTheLoai.Size = new Size(100, 42);
            btnSuaTheLoai.Size = new Size(100, 42);
            btnXoaTheLoai.Size = new Size(100, 42);

            lblLoaiDocGia.Location = new Point(785, 340);
            txtTenLoaiDocGia.Location = new Point(785, 370);
            txtTenLoaiDocGia.Size = new Size(330, 30);
            btnThemLoaiDocGia.Location = new Point(1135, 366);
            btnSuaLoaiDocGia.Location = new Point(1235, 366);
            btnXoaLoaiDocGia.Location = new Point(1335, 366);
            btnThemLoaiDocGia.Size = new Size(90, 42);
            btnSuaLoaiDocGia.Size = new Size(90, 42);
            btnXoaLoaiDocGia.Size = new Size(90, 42);

            dgvTheLoai.Location = new Point(50, 430);
            dgvTheLoai.Size = new Size(704, 290);
            dgvTheLoai.ColumnHeadersHeight = 46;
            dgvTheLoai.RowTemplate.Height = 40;

            dgvLoaiDocGia.Location = new Point(785, 430);
            dgvLoaiDocGia.Size = new Size(640, 290);
            dgvLoaiDocGia.ColumnHeadersHeight = 46;
            dgvLoaiDocGia.RowTemplate.Height = 40;

            btnDong.Location = new Point(1280, 740);
            btnDong.Size = new Size(110, 42);
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

        private DataGridView TaoDataGridView(int x, int y, int width, int height)
        {
            DataGridView dataGridView = new DataGridView();
            dataGridView.Location = new Point(x, y);
            dataGridView.Size = new Size(width, height);
            dataGridView.ReadOnly = true;
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.MultiSelect = false;
            dataGridView.BackgroundColor = Color.White;
            dataGridView.BorderStyle = BorderStyle.None;
            dataGridView.RowHeadersVisible = false;
            dataGridView.EnableHeadersVisualStyles = false;
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(237, 242, 247);
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(36, 52, 71);
            dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dataGridView.ColumnHeadersHeight = 42;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(220, 232, 246);
            dataGridView.DefaultCellStyle.SelectionForeColor = Color.FromArgb(36, 52, 71);
            dataGridView.DefaultCellStyle.Padding = new Padding(2, 4, 2, 4);
            dataGridView.RowTemplate.Height = 36;
            return dataGridView;
        }

        private void FormSuaQuyDinh_Load(object sender, EventArgs e)
        {
            TaiThamSo();
            TaiTheLoai();
            TaiLoaiDocGia();
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
            txtTienPhat.Text = Convert.ToDecimal(row["TienPhat"]).ToString("0.##");
        }

        private void TaiTheLoai()
        {
            isLoading = true;
            dgvTheLoai.DataSource = theLoaiBUS.LayDanhSachTheLoai();
            if (dgvTheLoai.Columns.Count > 0)
            {
                dgvTheLoai.Columns["MaTheLoai"].HeaderText = "Mã thể loại";
                dgvTheLoai.Columns["TenTheLoai"].HeaderText = "Tên thể loại";
            }

            dgvTheLoai.ClearSelection();
            maTheLoaiDangChon = null;
            txtTenTheLoai.Clear();
            isLoading = false;
        }

        private void TaiLoaiDocGia()
        {
            isLoading = true;
            dgvLoaiDocGia.DataSource = loaiDocGiaBUS.LayDanhSachLoaiDocGia();
            if (dgvLoaiDocGia.Columns.Count > 0)
            {
                dgvLoaiDocGia.Columns["MaLoaiDG"].HeaderText = "Mã loại";
                dgvLoaiDocGia.Columns["TenLoaiDG"].HeaderText = "Tên loại độc giả";
            }

            dgvLoaiDocGia.ClearSelection();
            maLoaiDocGiaDangChon = null;
            txtTenLoaiDocGia.Clear();
            isLoading = false;
        }

        private void btnLuuThamSo_Click(object sender, EventArgs e)
        {
            int giaTriThe;
            int soTuoiToiThieu;
            int soTuoiToiDa;
            int thoiGianXB;
            int soSachMuonToiDa;
            int soNgayMuonToiDa;
            decimal tienPhat;

            if (!int.TryParse(txtGiaTriThe.Text.Trim(), out giaTriThe) ||
                !int.TryParse(txtSoTuoiToiThieu.Text.Trim(), out soTuoiToiThieu) ||
                !int.TryParse(txtSoTuoiToiDa.Text.Trim(), out soTuoiToiDa) ||
                !int.TryParse(txtThoiGianXB.Text.Trim(), out thoiGianXB) ||
                !int.TryParse(txtSoSachMuonToiDa.Text.Trim(), out soSachMuonToiDa) ||
                !int.TryParse(txtSoNgayMuonToiDa.Text.Trim(), out soNgayMuonToiDa) ||
                !decimal.TryParse(txtTienPhat.Text.Trim(), out tienPhat))
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
                tienPhat,
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
                maTheLoaiDangChon = null;
                TaiTheLoai();
            }
        }

        private void btnSuaTheLoai_Click(object sender, EventArgs e)
        {
            if (!maTheLoaiDangChon.HasValue)
            {
                MessageBox.Show("Vui lòng chọn thể loại cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string thongBao;
            bool thanhCong = theLoaiBUS.CapNhatTheLoai(maTheLoaiDangChon.Value, txtTenTheLoai.Text.Trim(), out thongBao);

            MessageBox.Show(
                thongBao,
                thanhCong ? "Thông báo" : "Lỗi",
                MessageBoxButtons.OK,
                thanhCong ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            if (thanhCong)
            {
                txtTenTheLoai.Clear();
                maTheLoaiDangChon = null;
                TaiTheLoai();
            }
        }

        private void btnXoaTheLoai_Click(object sender, EventArgs e)
        {
            if (!maTheLoaiDangChon.HasValue)
            {
                MessageBox.Show("Vui lòng chọn thể loại cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string thongBao;
            bool thanhCong = theLoaiBUS.XoaTheLoai(maTheLoaiDangChon.Value, out thongBao);

            MessageBox.Show(
                thongBao,
                thanhCong ? "Thông báo" : "Lỗi",
                MessageBoxButtons.OK,
                thanhCong ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            if (thanhCong)
            {
                txtTenTheLoai.Clear();
                maTheLoaiDangChon = null;
                TaiTheLoai();
            }
        }

        private void btnThemLoaiDocGia_Click(object sender, EventArgs e)
        {
            string thongBao;
            bool thanhCong = loaiDocGiaBUS.ThemLoaiDocGia(txtTenLoaiDocGia.Text.Trim(), out thongBao);
            MessageBox.Show(
                thongBao,
                thanhCong ? "Thông báo" : "Lỗi",
                MessageBoxButtons.OK,
                thanhCong ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            if (thanhCong)
            {
                TaiLoaiDocGia();
            }
        }

        private void btnSuaLoaiDocGia_Click(object sender, EventArgs e)
        {
            if (!maLoaiDocGiaDangChon.HasValue)
            {
                MessageBox.Show("Vui lòng chọn loại độc giả cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string thongBao;
            bool thanhCong = loaiDocGiaBUS.CapNhatLoaiDocGia(maLoaiDocGiaDangChon.Value, txtTenLoaiDocGia.Text.Trim(), out thongBao);
            MessageBox.Show(
                thongBao,
                thanhCong ? "Thông báo" : "Lỗi",
                MessageBoxButtons.OK,
                thanhCong ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            if (thanhCong)
            {
                TaiLoaiDocGia();
            }
        }

        private void btnXoaLoaiDocGia_Click(object sender, EventArgs e)
        {
            if (!maLoaiDocGiaDangChon.HasValue)
            {
                MessageBox.Show("Vui lòng chọn loại độc giả cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string thongBao;
            bool thanhCong = loaiDocGiaBUS.XoaLoaiDocGia(maLoaiDocGiaDangChon.Value, out thongBao);
            MessageBox.Show(
                thongBao,
                thanhCong ? "Thông báo" : "Lỗi",
                MessageBoxButtons.OK,
                thanhCong ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            if (thanhCong)
            {
                TaiLoaiDocGia();
            }
        }

        private void dgvTheLoai_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            CapNhatTheLoaiDangChon(dgvTheLoai.Rows[e.RowIndex]);
        }

        private void dgvTheLoai_SelectionChanged(object sender, EventArgs e)
        {
            if (isLoading)
            {
                return;
            }

            if (dgvTheLoai.CurrentRow == null || dgvTheLoai.CurrentRow.IsNewRow)
            {
                return;
            }

            CapNhatTheLoaiDangChon(dgvTheLoai.CurrentRow);
        }

        private void CapNhatTheLoaiDangChon(DataGridViewRow row)
        {
            if (row == null || row.Cells["MaTheLoai"].Value == null)
            {
                maTheLoaiDangChon = null;
                txtTenTheLoai.Clear();
                return;
            }

            maTheLoaiDangChon = Convert.ToInt32(row.Cells["MaTheLoai"].Value);
            txtTenTheLoai.Text = row.Cells["TenTheLoai"].Value == null
                ? string.Empty
                : row.Cells["TenTheLoai"].Value.ToString();
        }

        private void dgvLoaiDocGia_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            CapNhatLoaiDocGiaDangChon(dgvLoaiDocGia.Rows[e.RowIndex]);
        }

        private void dgvLoaiDocGia_SelectionChanged(object sender, EventArgs e)
        {
            if (isLoading)
            {
                return;
            }

            if (dgvLoaiDocGia.CurrentRow == null || dgvLoaiDocGia.CurrentRow.IsNewRow)
            {
                return;
            }

            CapNhatLoaiDocGiaDangChon(dgvLoaiDocGia.CurrentRow);
        }

        private void CapNhatLoaiDocGiaDangChon(DataGridViewRow row)
        {
            if (row == null || row.Cells["MaLoaiDG"].Value == null)
            {
                maLoaiDocGiaDangChon = null;
                txtTenLoaiDocGia.Clear();
                return;
            }

            maLoaiDocGiaDangChon = Convert.ToInt32(row.Cells["MaLoaiDG"].Value);
            txtTenLoaiDocGia.Text = row.Cells["TenLoaiDG"].Value == null
                ? string.Empty
                : row.Cells["TenLoaiDG"].Value.ToString();
        }
    }
}
