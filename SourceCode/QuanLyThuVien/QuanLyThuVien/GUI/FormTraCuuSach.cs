using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyThuVien.BUS;

namespace QuanLyThuVien.GUI
{
    public class FormTraCuuSach : Form
    {
        private readonly SachBUS sachBUS = new SachBUS();
        private Label lblTieuDe;
        private Label lblMoTa;
        private Label lblMaSach;
        private Label lblTenSach;
        private Label lblTheLoai;
        private Label lblTacGia;
        private Label lblNhaXB;
        private Label lblNamXB;
        private TextBox txtMaSach;
        private TextBox txtTenSach;
        private TextBox txtTheLoai;
        private TextBox txtTacGia;
        private TextBox txtNhaXB;
        private TextBox txtNamXB;
        private Button btnTimKiem;
        private Button btnTaiLai;
        private Button btnDong;
        private DataGridView dgvSach;

        public FormTraCuuSach()
        {
            TaoGiaoDien();
            Load += FormTraCuuSach_Load;
        }

        private void TaoGiaoDien()
        {
            Text = "Tra cứu sách";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1120, 660);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.FromArgb(244, 247, 251);
            Font = new Font("Segoe UI", 11F);

            lblTieuDe = new Label();
            lblTieuDe.Text = "Tra cứu sách";
            lblTieuDe.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTieuDe.ForeColor = Color.FromArgb(28, 77, 125);
            lblTieuDe.AutoSize = true;
            lblTieuDe.Location = new Point(34, 24);

            lblMoTa = new Label();
            lblMoTa.Text = "Tìm kiếm sách theo mã, tên, thể loại, tác giả, nhà xuất bản và năm xuất bản.";
            lblMoTa.ForeColor = Color.FromArgb(102, 117, 132);
            lblMoTa.AutoSize = true;
            lblMoTa.Location = new Point(38, 60);

            lblMaSach = TaoLabel("Mã sách", 40, 112);
            lblTenSach = TaoLabel("Tên sách", 280, 112);
            lblTheLoai = TaoLabel("Thể loại", 520, 112);
            lblTacGia = TaoLabel("Tác giả", 760, 112);
            lblNhaXB = TaoLabel("Nhà xuất bản", 40, 176);
            lblNamXB = TaoLabel("Năm XB", 280, 176);

            txtMaSach = TaoTextBox(40, 138, 190);
            txtTenSach = TaoTextBox(280, 138, 190);
            txtTheLoai = TaoTextBox(520, 138, 190);
            txtTacGia = TaoTextBox(760, 138, 190);
            txtNhaXB = TaoTextBox(40, 202, 190);
            txtNamXB = TaoTextBox(280, 202, 190);

            btnTimKiem = TaoButton("Tìm kiếm", 736, 198, Color.FromArgb(28, 77, 125), Color.White);
            btnTaiLai = TaoButton("Tải lại", 852, 198, Color.FromArgb(230, 235, 241), Color.FromArgb(50, 60, 70));
            btnDong = TaoButton("Đóng", 968, 198, Color.FromArgb(230, 235, 241), Color.FromArgb(50, 60, 70));

            btnTimKiem.Click += btnTimKiem_Click;
            btnTaiLai.Click += btnTaiLai_Click;
            btnDong.Click += (sender, e) => Close();

            dgvSach = new DataGridView();
            dgvSach.Location = new Point(24, 270);
            dgvSach.Size = new Size(1072, 350);
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
            dgvSach.ColumnHeadersHeight = 42;
            dgvSach.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvSach.DefaultCellStyle.SelectionBackColor = Color.FromArgb(220, 232, 246);
            dgvSach.DefaultCellStyle.SelectionForeColor = Color.FromArgb(36, 52, 71);
            dgvSach.DefaultCellStyle.Padding = new Padding(2, 4, 2, 4);
            dgvSach.RowTemplate.Height = 36;

            Controls.Add(lblTieuDe);
            Controls.Add(lblMoTa);
            Controls.Add(lblMaSach);
            Controls.Add(lblTenSach);
            Controls.Add(lblTheLoai);
            Controls.Add(lblTacGia);
            Controls.Add(lblNhaXB);
            Controls.Add(lblNamXB);
            Controls.Add(txtMaSach);
            Controls.Add(txtTenSach);
            Controls.Add(txtTheLoai);
            Controls.Add(txtTacGia);
            Controls.Add(txtNhaXB);
            Controls.Add(txtNamXB);
            Controls.Add(btnTimKiem);
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
            textBox.Size = new Size(width, 28);
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

        private void FormTraCuuSach_Load(object sender, EventArgs e)
        {
            TaiDanhSachSach();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            DataTable dataTable = sachBUS.TimKiemSach(
                txtMaSach.Text.Trim(),
                txtTenSach.Text.Trim(),
                txtTheLoai.Text.Trim(),
                txtTacGia.Text.Trim(),
                txtNhaXB.Text.Trim(),
                txtNamXB.Text.Trim());

            HienThiDanhSachSach(dataTable);
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            txtMaSach.Clear();
            txtTenSach.Clear();
            txtTheLoai.Clear();
            txtTacGia.Clear();
            txtNhaXB.Clear();
            txtNamXB.Clear();
            TaiDanhSachSach();
        }

        private void TaiDanhSachSach()
        {
            HienThiDanhSachSach(sachBUS.LayDanhSachSach());
        }

        private void HienThiDanhSachSach(DataTable sourceTable)
        {
            dgvSach.DataSource = TaoBangHienThi(sourceTable);
            DinhDangCot();
        }

        private DataTable TaoBangHienThi(DataTable sourceTable)
        {
            DataTable table = new DataTable();
            table.Columns.Add("MaSach", typeof(string));
            table.Columns.Add("TenSach", typeof(string));
            table.Columns.Add("TenTheLoai", typeof(string));
            table.Columns.Add("TenTG", typeof(string));
            table.Columns.Add("NhaXB", typeof(string));
            table.Columns.Add("NamXB", typeof(int));
            table.Columns.Add("SoLuongTon", typeof(int));

            for (int i = 0; i < sourceTable.Rows.Count; i++)
            {
                DataRow sourceRow = sourceTable.Rows[i];
                table.Rows.Add(
                    Convert.ToInt32(sourceRow["MaSach"]).ToString("D5"),
                    sourceRow["TenSach"],
                    sourceRow["TenTheLoai"],
                    sourceRow["TenTG"],
                    sourceRow["NhaXB"],
                    sourceRow["NamXB"],
                    sourceRow["SoLuongTon"]);
            }

            return table;
        }

        private void DinhDangCot()
        {
            if (dgvSach.Columns.Count == 0)
            {
                return;
            }

            dgvSach.Columns["MaSach"].HeaderText = "Mã sách";
            dgvSach.Columns["TenSach"].HeaderText = "Tên sách";
            dgvSach.Columns["TenTheLoai"].HeaderText = "Thể loại";
            dgvSach.Columns["TenTG"].HeaderText = "Tác giả";
            dgvSach.Columns["NhaXB"].HeaderText = "Nhà XB";
            dgvSach.Columns["NamXB"].HeaderText = "Năm XB";
            dgvSach.Columns["SoLuongTon"].HeaderText = "Số lượng";

            dgvSach.Columns["MaSach"].FillWeight = 80;
            dgvSach.Columns["TenSach"].FillWeight = 165;
            dgvSach.Columns["TenTheLoai"].FillWeight = 100;
            dgvSach.Columns["TenTG"].FillWeight = 140;
            dgvSach.Columns["NhaXB"].FillWeight = 130;
            dgvSach.Columns["NamXB"].FillWeight = 80;
            dgvSach.Columns["SoLuongTon"].FillWeight = 90;

            dgvSach.ClearSelection();
        }
    }
}
