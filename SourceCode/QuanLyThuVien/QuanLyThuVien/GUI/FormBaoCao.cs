using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanLyThuVien.BUS;

namespace QuanLyThuVien.GUI
{
    public class FormBaoCao : Form
    {
        private readonly BaoCaoBUS baoCaoBUS = new BaoCaoBUS();
        private Label lblTieuDe;
        private Label lblMoTa;
        private Button btnMuonTheoTheLoai;
        private Button btnTraTre;
        private Button btnDong;
        private Panel pnlEmptyState;
        private Label lblEmptyTitle;
        private Label lblEmptySubtitle;
        private Panel pnlReport;
        private Label lblTenBaoCao;
        private Label lblThoiGian;
        private Label lblTongKet;
        private DataGridView dgvBaoCao;

        public FormBaoCao()
        {
            TaoGiaoDien();
        }

        private void TaoGiaoDien()
        {
            Text = "Báo cáo";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1040, 660);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.FromArgb(243, 246, 250);
            Font = new Font("Segoe UI", 11F);

            lblTieuDe = new Label();
            lblTieuDe.Text = "Báo cáo thống kê";
            lblTieuDe.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTieuDe.ForeColor = Color.FromArgb(28, 77, 125);
            lblTieuDe.AutoSize = true;
            lblTieuDe.Location = new Point(34, 24);

            lblMoTa = new Label();
            lblMoTa.Text = "Chọn loại báo cáo để xem tổng hợp dữ liệu mượn sách và các trường hợp trả trễ.";
            lblMoTa.ForeColor = Color.FromArgb(102, 117, 132);
            lblMoTa.Font = new Font("Segoe UI", 10.5F);
            lblMoTa.AutoSize = true;
            lblMoTa.Location = new Point(38, 64);

            btnMuonTheoTheLoai = TaoButton("Mượn theo thể loại", 40, 112, 180, Color.FromArgb(28, 77, 125), Color.White);
            btnTraTre = TaoButton("Sách trả trễ", 232, 112, 160, Color.FromArgb(28, 77, 125), Color.White);
            btnDong = TaoButton("Đóng", 900, 112, 100, Color.FromArgb(230, 235, 241), Color.FromArgb(50, 60, 70));

            btnMuonTheoTheLoai.Click += (sender, e) => HienThiBaoCaoMuonTheoTheLoai();
            btnTraTre.Click += (sender, e) => HienThiBaoCaoTraTre();
            btnDong.Click += (sender, e) => Close();

            TaoEmptyState();
            TaoReportPanel();

            Controls.Add(lblTieuDe);
            Controls.Add(lblMoTa);
            Controls.Add(btnMuonTheoTheLoai);
            Controls.Add(btnTraTre);
            Controls.Add(btnDong);
            Controls.Add(pnlEmptyState);
            Controls.Add(pnlReport);
        }

        private void TaoEmptyState()
        {
            pnlEmptyState = new Panel();
            pnlEmptyState.Location = new Point(24, 176);
            pnlEmptyState.Size = new Size(980, 440);
            pnlEmptyState.BackColor = Color.White;
            pnlEmptyState.BorderStyle = BorderStyle.FixedSingle;

            lblEmptyTitle = new Label();
            lblEmptyTitle.Text = "Chưa chọn báo cáo";
            lblEmptyTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblEmptyTitle.ForeColor = Color.FromArgb(28, 77, 125);
            lblEmptyTitle.AutoSize = true;
            lblEmptyTitle.Location = new Point(360, 150);

            lblEmptySubtitle = new Label();
            lblEmptySubtitle.Text = "Bấm \"Mượn theo thể loại\" hoặc \"Sách trả trễ\" để hiển thị dữ liệu.";
            lblEmptySubtitle.Font = new Font("Segoe UI", 11F);
            lblEmptySubtitle.ForeColor = Color.FromArgb(102, 117, 132);
            lblEmptySubtitle.AutoSize = true;
            lblEmptySubtitle.Location = new Point(230, 195);

            pnlEmptyState.Controls.Add(lblEmptyTitle);
            pnlEmptyState.Controls.Add(lblEmptySubtitle);
        }

        private void TaoReportPanel()
        {
            pnlReport = new Panel();
            pnlReport.Location = new Point(24, 176);
            pnlReport.Size = new Size(980, 440);
            pnlReport.BackColor = Color.White;
            pnlReport.BorderStyle = BorderStyle.FixedSingle;
            pnlReport.Visible = false;

            lblTenBaoCao = new Label();
            lblTenBaoCao.Text = "Báo cáo thống kê";
            lblTenBaoCao.TextAlign = ContentAlignment.MiddleLeft;
            lblTenBaoCao.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            lblTenBaoCao.ForeColor = Color.White;
            lblTenBaoCao.BackColor = Color.FromArgb(28, 77, 125);
            lblTenBaoCao.Location = new Point(24, 22);
            lblTenBaoCao.Size = new Size(930, 38);
            lblTenBaoCao.Padding = new Padding(16, 0, 0, 0);

            lblThoiGian = new Label();
            lblThoiGian.Text = "Tháng: --";
            lblThoiGian.TextAlign = ContentAlignment.MiddleCenter;
            lblThoiGian.Font = new Font("Segoe UI", 12F, FontStyle.Italic);
            lblThoiGian.ForeColor = Color.FromArgb(62, 76, 91);
            lblThoiGian.Location = new Point(24, 76);
            lblThoiGian.Size = new Size(930, 28);

            dgvBaoCao = new DataGridView();
            dgvBaoCao.Location = new Point(24, 118);
            dgvBaoCao.Size = new Size(930, 248);
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
            dgvBaoCao.ColumnHeadersHeight = 40;
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
            lblTongKet.Location = new Point(24, 378);
            lblTongKet.Size = new Size(930, 34);
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
            button.Size = new Size(width, 40);
            button.Location = new Point(x, y);
            button.BackColor = backColor;
            button.ForeColor = foreColor;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            return button;
        }

        private void HienThiBaoCaoMuonTheoTheLoai()
        {
            DataTable sourceTable = baoCaoBUS.LayBaoCaoMuonTheoTheLoai();
            dgvBaoCao.DataSource = TaoBangBaoCaoMuonTheoTheLoai(sourceTable);

            string thangHienThi = sourceTable.Rows.Count == 0 ? "--" : sourceTable.Rows[0]["Thang"].ToString();
            int tongSoLuotMuon = sourceTable.AsEnumerable().Sum(row => row.Field<int>("SoLuotMuon"));

            pnlEmptyState.Visible = false;
            pnlReport.Visible = true;

            lblTenBaoCao.Text = "Báo cáo thống kê tình hình mượn sách theo thể loại";
            lblThoiGian.Visible = true;
            lblThoiGian.Text = "Tháng: " + thangHienThi;
            lblTongKet.Visible = true;
            lblTongKet.Text = "Tổng số lượt mượn: " + tongSoLuotMuon;

            DinhDangBaoCaoMuonTheoTheLoai();
        }

        private void HienThiBaoCaoTraTre()
        {
            DataTable sourceTable = baoCaoBUS.LayBaoCaoTraTre();
            dgvBaoCao.DataSource = TaoBangBaoCaoTraTre(sourceTable);

            string ngayHienThi = sourceTable.Rows.Count == 0
                ? "--"
                : Convert.ToDateTime(sourceTable.Rows[0]["NgayMuon"]).ToString("dd/MM/yyyy");

            pnlEmptyState.Visible = false;
            pnlReport.Visible = true;

            lblTenBaoCao.Text = "Báo cáo thống kê sách trả trễ";
            lblThoiGian.Visible = true;
            lblThoiGian.Text = "Ngày: " + ngayHienThi;
            lblTongKet.Visible = false;

            DinhDangBaoCaoTraTre();
        }

        private DataTable TaoBangBaoCaoMuonTheoTheLoai(DataTable sourceTable)
        {
            DataTable table = new DataTable();
            table.Columns.Add("STT", typeof(int));
            table.Columns.Add("TenTheLoai", typeof(string));
            table.Columns.Add("SoLuotMuon", typeof(int));
            table.Columns.Add("TiLe", typeof(string));

            for (int i = 0; i < sourceTable.Rows.Count; i++)
            {
                DataRow sourceRow = sourceTable.Rows[i];
                table.Rows.Add(
                    i + 1,
                    sourceRow["TenTheLoai"],
                    sourceRow["SoLuotMuon"],
                    Convert.ToDecimal(sourceRow["TiLe"]).ToString("0.00"));
            }

            return table;
        }

        private DataTable TaoBangBaoCaoTraTre(DataTable sourceTable)
        {
            DataTable table = new DataTable();
            table.Columns.Add("STT", typeof(int));
            table.Columns.Add("TenSach", typeof(string));
            table.Columns.Add("NgayMuon", typeof(string));
            table.Columns.Add("SoNgayTraTre", typeof(int));

            for (int i = 0; i < sourceTable.Rows.Count; i++)
            {
                DataRow sourceRow = sourceTable.Rows[i];
                table.Rows.Add(
                    i + 1,
                    sourceRow["TenSach"],
                    Convert.ToDateTime(sourceRow["NgayMuon"]).ToString("dd/MM/yyyy"),
                    sourceRow["SoNgayTraTre"]);
            }

            return table;
        }

        private void DinhDangBaoCaoMuonTheoTheLoai()
        {
            if (dgvBaoCao.Columns.Count == 0)
            {
                return;
            }

            dgvBaoCao.Columns["STT"].HeaderText = "STT";
            dgvBaoCao.Columns["TenTheLoai"].HeaderText = "Tên thể loại";
            dgvBaoCao.Columns["SoLuotMuon"].HeaderText = "Số lượt mượn";
            dgvBaoCao.Columns["TiLe"].HeaderText = "Tỉ lệ";

            dgvBaoCao.Columns["STT"].FillWeight = 55;
            dgvBaoCao.Columns["TenTheLoai"].FillWeight = 200;
            dgvBaoCao.Columns["SoLuotMuon"].FillWeight = 140;
            dgvBaoCao.Columns["TiLe"].FillWeight = 120;

            dgvBaoCao.ClearSelection();
        }

        private void DinhDangBaoCaoTraTre()
        {
            if (dgvBaoCao.Columns.Count == 0)
            {
                return;
            }

            dgvBaoCao.Columns["STT"].HeaderText = "STT";
            dgvBaoCao.Columns["TenSach"].HeaderText = "Tên sách";
            dgvBaoCao.Columns["NgayMuon"].HeaderText = "Ngày mượn";
            dgvBaoCao.Columns["SoNgayTraTre"].HeaderText = "Số ngày trả trễ";

            dgvBaoCao.Columns["STT"].FillWeight = 55;
            dgvBaoCao.Columns["TenSach"].FillWeight = 250;
            dgvBaoCao.Columns["NgayMuon"].FillWeight = 130;
            dgvBaoCao.Columns["SoNgayTraTre"].FillWeight = 140;

            dgvBaoCao.ClearSelection();
        }
    }
}
