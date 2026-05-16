using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using QuanLyThuVien.BUS;

namespace QuanLyThuVien.GUI
{
    public class FormTraSach : Form
    {
        private readonly PhieuMuonBUS phieuMuonBUS = new PhieuMuonBUS();
        private readonly DocGiaBUS docGiaBUS = new DocGiaBUS();
        private DataTable danhSachSachDangMuon;
        private Label lblTieuDe;
        private Label lblMoTa;
        private Panel pnlPhieuTraSach;
        private Label lblTenPhieu;
        private ComboBox cboDocGia;
        private TextBox txtNgayTra;
        private TextBox txtTienPhatKyNay;
        private TextBox txtTongNo;
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
            Text = "Phiếu trả sách";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1120, 700);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.FromArgb(244, 247, 251);
            Font = new Font("Segoe UI", 11F);

            lblTieuDe = new Label();
            lblTieuDe.Text = "Phiếu trả sách";
            lblTieuDe.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTieuDe.ForeColor = Color.FromArgb(28, 77, 125);
            lblTieuDe.AutoSize = true;
            lblTieuDe.Location = new Point(34, 24);

            lblMoTa = new Label();
            lblMoTa.Text = "Chọn độc giả, đánh dấu các sách cần trả rồi thực hiện nhận trả sách.";
            lblMoTa.ForeColor = Color.FromArgb(102, 117, 132);
            lblMoTa.Font = new Font("Segoe UI", 10.5F);
            lblMoTa.AutoSize = true;
            lblMoTa.Location = new Point(38, 64);

            pnlPhieuTraSach = new Panel();
            pnlPhieuTraSach.Location = new Point(24, 106);
            pnlPhieuTraSach.Size = new Size(1060, 170);
            pnlPhieuTraSach.BackColor = Color.White;
            pnlPhieuTraSach.BorderStyle = BorderStyle.FixedSingle;

            lblTenPhieu = new Label();
            lblTenPhieu.Text = "Thông tin phiếu trả";
            lblTenPhieu.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTenPhieu.ForeColor = Color.White;
            lblTenPhieu.BackColor = Color.FromArgb(28, 77, 125);
            lblTenPhieu.TextAlign = ContentAlignment.MiddleLeft;
            lblTenPhieu.Location = new Point(0, 0);
            lblTenPhieu.Size = new Size(1058, 42);
            lblTenPhieu.Padding = new Padding(18, 0, 0, 0);

            cboDocGia = new ComboBox();
            cboDocGia.DropDownStyle = ComboBoxStyle.DropDownList;
            cboDocGia.SelectedIndexChanged += cboDocGia_SelectedIndexChanged;

            txtNgayTra = TaoTextBoxThongTin();
            txtTienPhatKyNay = TaoTextBoxThongTin();
            txtTongNo = TaoTextBoxThongTin();
            txtNgayTra.TextAlign = HorizontalAlignment.Center;

            TableLayoutPanel tblThongTin = new TableLayoutPanel();
            tblThongTin.Location = new Point(28, 58);
            tblThongTin.Size = new Size(1000, 108);
            tblThongTin.ColumnCount = 2;
            tblThongTin.RowCount = 2;
            tblThongTin.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tblThongTin.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tblThongTin.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
            tblThongTin.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));

            tblThongTin.Controls.Add(TaoOThongTin("Họ tên độc giả", cboDocGia), 0, 0);
            tblThongTin.Controls.Add(TaoOThongTin("Ngày trả", txtNgayTra), 1, 0);
            tblThongTin.Controls.Add(TaoOThongTin("Tiền phạt kỳ này", txtTienPhatKyNay), 0, 1);
            tblThongTin.Controls.Add(TaoOThongTin("Tổng nợ sau trả", txtTongNo), 1, 1);

            pnlPhieuTraSach.Controls.Add(lblTenPhieu);
            pnlPhieuTraSach.Controls.Add(tblThongTin);

            btnTraSach = TaoButton("Trả sách", 736, 294, Color.FromArgb(28, 77, 125), Color.White);
            btnTaiLai = TaoButton("Tải lại", 852, 294, Color.FromArgb(230, 235, 241), Color.FromArgb(50, 60, 70));
            btnDong = TaoButton("Đóng", 968, 294, Color.FromArgb(230, 235, 241), Color.FromArgb(50, 60, 70));

            btnTraSach.Click += btnTraSach_Click;
            btnTaiLai.Click += btnTaiLai_Click;
            btnDong.Click += (sender, e) => Close();

            dgvSachDangMuon = new DataGridView();
            dgvSachDangMuon.Location = new Point(24, 360);
            dgvSachDangMuon.Size = new Size(1060, 310);
            dgvSachDangMuon.ReadOnly = false;
            dgvSachDangMuon.AllowUserToAddRows = false;
            dgvSachDangMuon.AllowUserToDeleteRows = false;
            dgvSachDangMuon.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSachDangMuon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSachDangMuon.MultiSelect = true;
            dgvSachDangMuon.BackgroundColor = Color.White;
            dgvSachDangMuon.BorderStyle = BorderStyle.FixedSingle;
            dgvSachDangMuon.RowHeadersVisible = false;
            dgvSachDangMuon.EnableHeadersVisualStyles = false;
            dgvSachDangMuon.GridColor = Color.FromArgb(216, 223, 230);
            dgvSachDangMuon.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgvSachDangMuon.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvSachDangMuon.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(28, 77, 125);
            dgvSachDangMuon.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvSachDangMuon.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dgvSachDangMuon.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSachDangMuon.ColumnHeadersHeight = 42;
            dgvSachDangMuon.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvSachDangMuon.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 239, 249);
            dgvSachDangMuon.DefaultCellStyle.SelectionForeColor = Color.FromArgb(36, 52, 71);
            dgvSachDangMuon.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSachDangMuon.RowTemplate.Height = 36;
            dgvSachDangMuon.CellContentClick += dgvSachDangMuon_CellContentClick;
            dgvSachDangMuon.CellValueChanged += dgvSachDangMuon_CellValueChanged;
            dgvSachDangMuon.CurrentCellDirtyStateChanged += dgvSachDangMuon_CurrentCellDirtyStateChanged;

            Controls.Add(lblTieuDe);
            Controls.Add(lblMoTa);
            Controls.Add(pnlPhieuTraSach);
            Controls.Add(btnTraSach);
            Controls.Add(btnTaiLai);
            Controls.Add(btnDong);
            Controls.Add(dgvSachDangMuon);
        }

        private Control TaoOThongTin(string text, Control control)
        {
            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.Margin = new Padding(0, 0, 18, 0);
            panel.Padding = new Padding(0);
            panel.BackColor = Color.Transparent;

            TableLayoutPanel layout = new TableLayoutPanel();
            layout.Dock = DockStyle.Fill;
            layout.ColumnCount = 1;
            layout.RowCount = 2;
            layout.Margin = new Padding(0);
            layout.Padding = new Padding(0);
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));

            Label label = new Label();
            label.Text = text;
            label.Dock = DockStyle.Fill;
            label.ForeColor = Color.FromArgb(62, 76, 91);
            label.Font = new Font("Segoe UI", 10.5F);
            label.TextAlign = ContentAlignment.BottomLeft;

            control.Dock = DockStyle.Fill;
            control.Margin = new Padding(0);

            layout.Controls.Add(label, 0, 0);
            layout.Controls.Add(control, 0, 1);
            panel.Controls.Add(layout);
            return panel;
        }

        private TextBox TaoTextBoxThongTin()
        {
            TextBox textBox = new TextBox();
            textBox.Multiline = true;
            textBox.Height = 28;
            textBox.ReadOnly = true;
            textBox.BackColor = Color.White;
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.Margin = new Padding(0);
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

        private void FormTraSach_Load(object sender, EventArgs e)
        {
            TaiDuLieu();
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            TaiDuLieu();
        }

        private void cboDocGia_SelectedIndexChanged(object sender, EventArgs e)
        {
            LocDanhSachSachTheoDocGia();
        }

        private void dgvSachDangMuon_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvSachDangMuon.IsCurrentCellDirty)
            {
                dgvSachDangMuon.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dgvSachDangMuon_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvSachDangMuon.Columns[e.ColumnIndex].Name == "ChonTra")
            {
                CapNhatThongTinPhieu();
            }
        }

        private void dgvSachDangMuon_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvSachDangMuon.Columns[e.ColumnIndex].Name == "ChonTra")
            {
                CapNhatThongTinPhieu();
            }
        }

        private void btnTraSach_Click(object sender, EventArgs e)
        {
            DataGridViewRow[] selectedRows = dgvSachDangMuon.Rows
                .Cast<DataGridViewRow>()
                .Where(row => Convert.ToBoolean(row.Cells["ChonTra"].Value))
                .ToArray();

            if (selectedRows.Length == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một sách cần trả.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int soSachTraThanhCong = 0;
            decimal tongTienPhatThucTe = 0;

            foreach (DataGridViewRow row in selectedRows)
            {
                int maPhieu = Convert.ToInt32(row.Cells["MaPhieu"].Value);
                int maSach = Convert.ToInt32(row.Cells["MaSachGoc"].Value);
                string thongBao;

                bool thanhCong = phieuMuonBUS.TraSach(maPhieu, maSach, out thongBao);
                if (!thanhCong)
                {
                    MessageBox.Show(thongBao, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    TaiDuLieu();
                    return;
                }

                decimal tienPhatDuKien = decimal.Parse(
                    row.Cells["TienPhatDuKienGoc"].Value.ToString(),
                    CultureInfo.InvariantCulture);

                tongTienPhatThucTe += tienPhatDuKien;
                soSachTraThanhCong++;
            }

            TaiDuLieu();
            MessageBox.Show(
                "Trả " + soSachTraThanhCong + " sách thành công. Tổng tiền phạt kỳ này: " + tongTienPhatThucTe.ToString("N0") + "đ",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void TaiDuLieu()
        {
            try
            {
                TaiDanhSachSachDangMuon();
                TaiDanhSachDocGia();
                LocDanhSachSachTheoDocGia();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải dữ liệu phiếu trả.\nChi tiết: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TaiDanhSachDocGia()
        {
            object selectedValue = cboDocGia.SelectedValue;
            DataTable docGiaTable = TaoDanhSachDocGiaDangMuon();

            cboDocGia.DataSource = docGiaTable;
            cboDocGia.DisplayMember = "TenDG";
            cboDocGia.ValueMember = "MaDG";

            if (selectedValue != null && docGiaTable.AsEnumerable().Any(row => row.Field<int>("MaDG") == Convert.ToInt32(selectedValue)))
            {
                cboDocGia.SelectedValue = selectedValue;
            }
        }

        private DataTable TaoDanhSachDocGiaDangMuon()
        {
            DataTable source = docGiaBUS.LayDanhSachDocGiaChoCombo();
            DataTable result = source.Clone();

            if (danhSachSachDangMuon == null || danhSachSachDangMuon.Rows.Count == 0)
            {
                return result;
            }

            var maDGDangMuon = danhSachSachDangMuon.AsEnumerable()
                .Select(row => Convert.ToInt32(row["MaDG"]))
                .Distinct()
                .ToHashSet();

            foreach (DataRow row in source.Rows)
            {
                int maDG = Convert.ToInt32(row["MaDG"]);
                if (maDGDangMuon.Contains(maDG))
                {
                    result.ImportRow(row);
                }
            }

            return result;
        }

        private void TaiDanhSachSachDangMuon()
        {
            danhSachSachDangMuon = phieuMuonBUS.LayDanhSachSachDangMuon();
        }

        private void LocDanhSachSachTheoDocGia()
        {
            DataTable table = TaoBangHienThiTheoDocGia();
            dgvSachDangMuon.DataSource = table;
            DinhDangBangSachDangMuon();
            CapNhatThongTinPhieu();
        }

        private DataTable TaoBangHienThiTheoDocGia()
        {
            DataTable table = new DataTable();
            table.Columns.Add("ChonTra", typeof(bool));
            table.Columns.Add("STT", typeof(int));
            table.Columns.Add("MaPhieu", typeof(int));
            table.Columns.Add("MaSachGoc", typeof(int));
            table.Columns.Add("TenDG", typeof(string));
            table.Columns.Add("TongNo", typeof(decimal));
            table.Columns.Add("MaSach", typeof(string));
            table.Columns.Add("NgayMuon", typeof(string));
            table.Columns.Add("SoNgayMuon", typeof(int));
            table.Columns.Add("TienPhatDuKien", typeof(string));
            table.Columns.Add("TienPhatDuKienGoc", typeof(decimal));

            if (danhSachSachDangMuon == null || cboDocGia.SelectedValue == null)
            {
                return table;
            }

            int maDG;
            if (!int.TryParse(cboDocGia.SelectedValue.ToString(), out maDG))
            {
                return table;
            }

            DataRow[] rows = danhSachSachDangMuon.Select("MaDG = " + maDG);
            for (int i = 0; i < rows.Length; i++)
            {
                DataRow sourceRow = rows[i];
                decimal tienPhatDuKien = Convert.ToDecimal(sourceRow["TienPhatDuKien"]);

                table.Rows.Add(
                    false,
                    i + 1,
                    sourceRow["MaPhieu"],
                    sourceRow["MaSach"],
                    sourceRow["TenDG"],
                    sourceRow["TongNo"],
                    Convert.ToInt32(sourceRow["MaSach"]).ToString("D5"),
                    Convert.ToDateTime(sourceRow["NgayMuon"]).ToString("dd/MM/yyyy"),
                    sourceRow["SoNgayMuon"],
                    tienPhatDuKien.ToString("N0"),
                    tienPhatDuKien);
            }

            return table;
        }

        private void DinhDangBangSachDangMuon()
        {
            if (dgvSachDangMuon.Columns.Count == 0)
            {
                return;
            }

            dgvSachDangMuon.Columns["ChonTra"].HeaderText = "Chọn";
            dgvSachDangMuon.Columns["STT"].HeaderText = "STT";
            dgvSachDangMuon.Columns["MaSach"].HeaderText = "Mã sách";
            dgvSachDangMuon.Columns["NgayMuon"].HeaderText = "Ngày mượn";
            dgvSachDangMuon.Columns["SoNgayMuon"].HeaderText = "Số ngày mượn";
            dgvSachDangMuon.Columns["TienPhatDuKien"].HeaderText = "Tiền phạt";

            dgvSachDangMuon.Columns["ChonTra"].FillWeight = 45;
            dgvSachDangMuon.Columns["STT"].FillWeight = 50;
            dgvSachDangMuon.Columns["MaSach"].FillWeight = 100;
            dgvSachDangMuon.Columns["NgayMuon"].FillWeight = 120;
            dgvSachDangMuon.Columns["SoNgayMuon"].FillWeight = 110;
            dgvSachDangMuon.Columns["TienPhatDuKien"].FillWeight = 120;

            dgvSachDangMuon.Columns["ChonTra"].ReadOnly = false;
            dgvSachDangMuon.Columns["STT"].ReadOnly = true;
            dgvSachDangMuon.Columns["MaSach"].ReadOnly = true;
            dgvSachDangMuon.Columns["NgayMuon"].ReadOnly = true;
            dgvSachDangMuon.Columns["SoNgayMuon"].ReadOnly = true;
            dgvSachDangMuon.Columns["TienPhatDuKien"].ReadOnly = true;

            dgvSachDangMuon.Columns["ChonTra"].DefaultCellStyle.NullValue = false;

            dgvSachDangMuon.Columns["MaPhieu"].Visible = false;
            dgvSachDangMuon.Columns["MaSachGoc"].Visible = false;
            dgvSachDangMuon.Columns["TenDG"].Visible = false;
            dgvSachDangMuon.Columns["TongNo"].Visible = false;
            dgvSachDangMuon.Columns["TienPhatDuKienGoc"].Visible = false;
        }

        private void CapNhatThongTinPhieu()
        {
            txtNgayTra.Text = DateTime.Today.ToString("dd/MM/yyyy");

            if (cboDocGia.SelectedValue == null || dgvSachDangMuon.Rows.Count == 0)
            {
                txtTienPhatKyNay.Text = "0";
                txtTongNo.Text = "0";
                return;
            }

            decimal tongNoHienTai = 0;
            if (dgvSachDangMuon.Rows.Count > 0)
            {
                tongNoHienTai = Convert.ToDecimal(dgvSachDangMuon.Rows[0].Cells["TongNo"].Value);
            }

            decimal tongTienPhatDuKien = dgvSachDangMuon.Rows
                .Cast<DataGridViewRow>()
                .Where(row => Convert.ToBoolean(row.Cells["ChonTra"].Value))
                .Sum(row => Convert.ToDecimal(row.Cells["TienPhatDuKienGoc"].Value));

            txtTienPhatKyNay.Text = tongTienPhatDuKien.ToString("N0");
            txtTongNo.Text = (tongNoHienTai + tongTienPhatDuKien).ToString("N0");
        }
    }
}
