using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyThuVien.BUS;

namespace QuanLyThuVien.GUI
{
    public class FormBaoCao : Form
    {
        private const string BaoCaoSachMuon = "SachMuon";
        private const string BaoCaoSachTra = "SachTra";

        private readonly BaoCaoBUS baoCaoBUS = new BaoCaoBUS();

        private Label lblTieuDe;
        private Label lblMoTa;
        private Label lblNgayBaoCao;
        private DateTimePicker dtpNgayBaoCao;
        private Button btnSachMuon;
        private Button btnSachTra;
        private Button btnDong;
        private Panel pnlEmptyState;
        private Label lblEmptyTitle;
        private Label lblEmptySubtitle;
        private Panel pnlReport;
        private Label lblTenBaoCao;
        private Label lblThoiGian;
        private Label lblTongKet;
        private DataGridView dgvBaoCao;
        private string loaiBaoCaoDangXem;

        public FormBaoCao()
        {
            TaoGiaoDien();
            Load += FormBaoCao_Load;
        }

        private void TaoGiaoDien()
        {
            Text = "Báo cáo";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1120, 720);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.FromArgb(243, 246, 250);
            Font = new Font("Segoe UI", 11F);

            lblTieuDe = new Label();
            lblTieuDe.Text = "Báo cáo thống kê";
            lblTieuDe.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTieuDe.ForeColor = Color.FromArgb(28, 77, 125);
            lblTieuDe.AutoSize = true;
            lblTieuDe.Location = new Point(38, 28);

            lblMoTa = new Label();
            lblMoTa.Text = "Chọn ngày và loại báo cáo để xem danh sách sách mượn hoặc sách trả.";
            lblMoTa.ForeColor = Color.FromArgb(102, 117, 132);
            lblMoTa.Font = new Font("Segoe UI", 10.5F);
            lblMoTa.AutoSize = true;
            lblMoTa.Location = new Point(42, 68);

            lblNgayBaoCao = new Label();
            lblNgayBaoCao.Text = "Ngày báo cáo";
            lblNgayBaoCao.AutoSize = true;
            lblNgayBaoCao.Location = new Point(44, 118);

            dtpNgayBaoCao = new DateTimePicker();
            dtpNgayBaoCao.Format = DateTimePickerFormat.Short;
            dtpNgayBaoCao.Location = new Point(154, 114);
            dtpNgayBaoCao.Size = new Size(150, 30);
            dtpNgayBaoCao.Value = DateTime.Today;
            dtpNgayBaoCao.ValueChanged += dtpNgayBaoCao_ValueChanged;

            btnSachMuon = TaoButton("Sách mượn", 338, 108, 150, Color.FromArgb(28, 77, 125), Color.White);
            btnSachTra = TaoButton("Sách trả", 502, 108, 140, Color.FromArgb(28, 77, 125), Color.White);
            btnDong = TaoButton("Đóng", 956, 108, 100, Color.FromArgb(230, 235, 241), Color.FromArgb(50, 60, 70));

            btnSachMuon.Click += (sender, e) => HienThiBaoCaoSachMuon();
            btnSachTra.Click += (sender, e) => HienThiBaoCaoSachTra();
            btnDong.Click += (sender, e) => Close();

            TaoEmptyState();
            TaoReportPanel();

            Controls.Add(lblTieuDe);
            Controls.Add(lblMoTa);
            Controls.Add(lblNgayBaoCao);
            Controls.Add(dtpNgayBaoCao);
            Controls.Add(btnSachMuon);
            Controls.Add(btnSachTra);
            Controls.Add(btnDong);
            Controls.Add(pnlEmptyState);
            Controls.Add(pnlReport);
        }

        private void TaoEmptyState()
        {
            pnlEmptyState = new Panel();
            pnlEmptyState.Location = new Point(24, 184);
            pnlEmptyState.Size = new Size(1072, 480);
            pnlEmptyState.BackColor = Color.White;
            pnlEmptyState.BorderStyle = BorderStyle.FixedSingle;

            lblEmptyTitle = new Label();
            lblEmptyTitle.Text = "Chưa chọn báo cáo";
            lblEmptyTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblEmptyTitle.ForeColor = Color.FromArgb(28, 77, 125);
            lblEmptyTitle.AutoSize = true;
            lblEmptyTitle.Location = new Point(400, 170);

            lblEmptySubtitle = new Label();
            lblEmptySubtitle.Text = "Bấm Sách mượn hoặc Sách trả để hiển thị dữ liệu theo ngày đã chọn.";
            lblEmptySubtitle.Font = new Font("Segoe UI", 11F);
            lblEmptySubtitle.ForeColor = Color.FromArgb(102, 117, 132);
            lblEmptySubtitle.AutoSize = true;
            lblEmptySubtitle.Location = new Point(282, 215);

            pnlEmptyState.Controls.Add(lblEmptyTitle);
            pnlEmptyState.Controls.Add(lblEmptySubtitle);
        }

        private void TaoReportPanel()
        {
            pnlReport = new Panel();
            pnlReport.Location = new Point(24, 184);
            pnlReport.Size = new Size(1072, 480);
            pnlReport.BackColor = Color.White;
            pnlReport.BorderStyle = BorderStyle.FixedSingle;
            pnlReport.Visible = false;

            lblTenBaoCao = new Label();
            lblTenBaoCao.Text = "Báo cáo";
            lblTenBaoCao.TextAlign = ContentAlignment.MiddleLeft;
            lblTenBaoCao.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            lblTenBaoCao.ForeColor = Color.White;
            lblTenBaoCao.BackColor = Color.FromArgb(28, 77, 125);
            lblTenBaoCao.Location = new Point(24, 22);
            lblTenBaoCao.Size = new Size(1022, 40);
            lblTenBaoCao.Padding = new Padding(16, 0, 0, 0);

            lblThoiGian = new Label();
            lblThoiGian.Text = "Ngày: --";
            lblThoiGian.TextAlign = ContentAlignment.MiddleCenter;
            lblThoiGian.Font = new Font("Segoe UI", 12F, FontStyle.Italic);
            lblThoiGian.ForeColor = Color.FromArgb(62, 76, 91);
            lblThoiGian.Location = new Point(24, 78);
            lblThoiGian.Size = new Size(1022, 28);

            dgvBaoCao = new DataGridView();
            dgvBaoCao.Location = new Point(24, 122);
            dgvBaoCao.Size = new Size(1022, 300);
            dgvBaoCao.ReadOnly = true;
            dgvBaoCao.AllowUserToAddRows = false;
            dgvBaoCao.AllowUserToDeleteRows = false;
            dgvBaoCao.AllowUserToResizeRows = false;
            dgvBaoCao.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBaoCao.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBaoCao.BackgroundColor = Color.White;
            dgvBaoCao.BorderStyle = BorderStyle.FixedSingle;
            dgvBaoCao.RowHeadersVisible = false;
            dgvBaoCao.EnableHeadersVisualStyles = false;
            dgvBaoCao.GridColor = Color.FromArgb(160, 173, 189);
            dgvBaoCao.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgvBaoCao.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvBaoCao.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(28, 77, 125);
            dgvBaoCao.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvBaoCao.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dgvBaoCao.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvBaoCao.ColumnHeadersHeight = 42;
            dgvBaoCao.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvBaoCao.DefaultCellStyle.Font = new Font("Segoe UI", 10.5F);
            dgvBaoCao.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvBaoCao.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 239, 249);
            dgvBaoCao.DefaultCellStyle.SelectionForeColor = Color.FromArgb(36, 52, 71);
            dgvBaoCao.RowTemplate.Height = 36;

            lblTongKet = new Label();
            lblTongKet.Text = string.Empty;
            lblTongKet.TextAlign = ContentAlignment.MiddleRight;
            lblTongKet.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTongKet.ForeColor = Color.FromArgb(48, 63, 79);
            lblTongKet.BackColor = Color.FromArgb(248, 250, 252);
            lblTongKet.BorderStyle = BorderStyle.FixedSingle;
            lblTongKet.Location = new Point(24, 434);
            lblTongKet.Size = new Size(1022, 34);
            lblTongKet.Padding = new Padding(0, 0, 16, 0);

            pnlReport.Controls.Add(lblTenBaoCao);
            pnlReport.Controls.Add(lblThoiGian);
            pnlReport.Controls.Add(dgvBaoCao);
            pnlReport.Controls.Add(lblTongKet);
        }

        private Button TaoButton(string text, int x, int y, int width, Color backColor, Color foreColor)
        {
            Button button = new Button();
            button.Text = text;
            button.Size = new Size(width, 42);
            button.Location = new Point(x, y);
            button.BackColor = backColor;
            button.ForeColor = foreColor;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            return button;
        }

        private void FormBaoCao_Load(object sender, EventArgs e)
        {
        }

        private void dtpNgayBaoCao_ValueChanged(object sender, EventArgs e)
        {
            if (loaiBaoCaoDangXem == BaoCaoSachMuon)
            {
                HienThiBaoCaoSachMuon();
            }
            else if (loaiBaoCaoDangXem == BaoCaoSachTra)
            {
                HienThiBaoCaoSachTra();
            }
        }

        private void HienThiBaoCaoSachMuon()
        {
            DateTime ngayBaoCao = dtpNgayBaoCao.Value.Date;
            loaiBaoCaoDangXem = BaoCaoSachMuon;

            DataTable sourceTable = baoCaoBUS.LayBaoCaoSachMuon(ngayBaoCao);
            dgvBaoCao.DataSource = TaoBangBaoCaoSachMuon(sourceTable);

            pnlEmptyState.Visible = false;
            pnlReport.Visible = true;
            lblTenBaoCao.Text = "Báo cáo sách mượn theo ngày";
            lblThoiGian.Text = "Ngày mượn: " + ngayBaoCao.ToString("dd/MM/yyyy");
            lblTongKet.Text = "Tổng số sách mượn: " + sourceTable.Rows.Count;

            DinhDangBaoCaoSachMuon();
        }

        private void HienThiBaoCaoSachTra()
        {
            DateTime ngayBaoCao = dtpNgayBaoCao.Value.Date;
            loaiBaoCaoDangXem = BaoCaoSachTra;

            DataTable sourceTable = baoCaoBUS.LayBaoCaoSachTra(ngayBaoCao);
            dgvBaoCao.DataSource = TaoBangBaoCaoSachTra(sourceTable);

            pnlEmptyState.Visible = false;
            pnlReport.Visible = true;
            lblTenBaoCao.Text = "Báo cáo sách trả theo ngày";
            lblThoiGian.Text = "Ngày trả: " + ngayBaoCao.ToString("dd/MM/yyyy");
            lblTongKet.Text = "Tổng số sách trả: " + sourceTable.Rows.Count;

            DinhDangBaoCaoSachTra();
        }

        private DataTable TaoBangBaoCaoSachMuon(DataTable sourceTable)
        {
            DataTable table = new DataTable();
            table.Columns.Add("TenSach", typeof(string));
            table.Columns.Add("TenDG", typeof(string));
            table.Columns.Add("NgayMuon", typeof(string));

            foreach (DataRow sourceRow in sourceTable.Rows)
            {
                table.Rows.Add(
                    sourceRow["TenSach"],
                    sourceRow["TenDG"],
                    Convert.ToDateTime(sourceRow["NgayMuon"]).ToString("dd/MM/yyyy"));
            }

            return table;
        }

        private DataTable TaoBangBaoCaoSachTra(DataTable sourceTable)
        {
            DataTable table = new DataTable();
            table.Columns.Add("TenSach", typeof(string));
            table.Columns.Add("TenDG", typeof(string));
            table.Columns.Add("NgayMuon", typeof(string));
            table.Columns.Add("NgayTra", typeof(string));
            table.Columns.Add("SoNgayTre", typeof(int));

            foreach (DataRow sourceRow in sourceTable.Rows)
            {
                table.Rows.Add(
                    sourceRow["TenSach"],
                    sourceRow["TenDG"],
                    Convert.ToDateTime(sourceRow["NgayMuon"]).ToString("dd/MM/yyyy"),
                    Convert.ToDateTime(sourceRow["NgayTra"]).ToString("dd/MM/yyyy"),
                    Convert.ToInt32(sourceRow["SoNgayTre"]));
            }

            return table;
        }

        private void DinhDangBaoCaoSachMuon()
        {
            if (dgvBaoCao.Columns.Count == 0)
            {
                return;
            }

            dgvBaoCao.Columns["TenSach"].HeaderText = "Tên sách";
            dgvBaoCao.Columns["TenDG"].HeaderText = "Độc giả";
            dgvBaoCao.Columns["NgayMuon"].HeaderText = "Ngày mượn";

            dgvBaoCao.Columns["TenSach"].FillWeight = 240;
            dgvBaoCao.Columns["TenDG"].FillWeight = 180;
            dgvBaoCao.Columns["NgayMuon"].FillWeight = 110;

            dgvBaoCao.ClearSelection();
        }

        private void DinhDangBaoCaoSachTra()
        {
            if (dgvBaoCao.Columns.Count == 0)
            {
                return;
            }

            dgvBaoCao.Columns["TenSach"].HeaderText = "Tên sách";
            dgvBaoCao.Columns["TenDG"].HeaderText = "Độc giả";
            dgvBaoCao.Columns["NgayMuon"].HeaderText = "Ngày mượn";
            dgvBaoCao.Columns["NgayTra"].HeaderText = "Ngày trả";
            dgvBaoCao.Columns["SoNgayTre"].HeaderText = "Số ngày trễ";

            dgvBaoCao.Columns["TenSach"].FillWeight = 220;
            dgvBaoCao.Columns["TenDG"].FillWeight = 160;
            dgvBaoCao.Columns["NgayMuon"].FillWeight = 115;
            dgvBaoCao.Columns["NgayTra"].FillWeight = 115;
            dgvBaoCao.Columns["SoNgayTre"].FillWeight = 110;

            dgvBaoCao.ClearSelection();
        }
    }
}
