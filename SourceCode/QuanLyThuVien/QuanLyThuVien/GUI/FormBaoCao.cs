using System.Drawing;
using System.Windows.Forms;
using QuanLyThuVien.BUS;

namespace QuanLyThuVien.GUI
{
    public class FormBaoCao : Form
    {
        private readonly BaoCaoBUS baoCaoBUS = new BaoCaoBUS();
        private Label lblTieuDe;
        private Label lblMoTa;
        private DataGridView dgvBaoCao;
        private Button btnMuonTheoTheLoai;
        private Button btnTraTre;
        private Button btnDong;

        public FormBaoCao()
        {
            TaoGiaoDien();
        }

        private void TaoGiaoDien()
        {
            Text = "Báo cáo";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1020, 600);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.FromArgb(244, 247, 251);
            Font = new Font("Segoe UI", 11F);

            lblTieuDe = new Label();
            lblTieuDe.Text = "Báo cáo thống kê";
            lblTieuDe.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTieuDe.ForeColor = Color.FromArgb(28, 77, 125);
            lblTieuDe.AutoSize = true;
            lblTieuDe.Location = new Point(34, 24);

            lblMoTa = new Label();
            lblMoTa.Text = "Theo dõi tình hình mượn sách theo thể loại và kiểm soát các trường hợp trả trễ.";
            lblMoTa.ForeColor = Color.FromArgb(102, 117, 132);
            lblMoTa.Font = new Font("Segoe UI", 10.5F);
            lblMoTa.AutoSize = true;
            lblMoTa.Location = new Point(38, 64);

            btnMuonTheoTheLoai = TaoButton("Mượn theo thể loại", 40, 110, Color.FromArgb(28, 77, 125), Color.White);
            btnTraTre = TaoButton("Sách trả trễ", 220, 110, Color.FromArgb(28, 77, 125), Color.White);
            btnDong = TaoButton("Đóng", 900, 110, Color.FromArgb(230, 235, 241), Color.FromArgb(50, 60, 70));

            btnMuonTheoTheLoai.Click += (sender, e) =>
            {
                dgvBaoCao.DataSource = baoCaoBUS.LayBaoCaoMuonTheoTheLoai();
                DinhDangBaoCaoMuonTheoTheLoai();
            };
            btnTraTre.Click += (sender, e) =>
            {
                dgvBaoCao.DataSource = baoCaoBUS.LayBaoCaoTraTre();
                DinhDangBaoCaoTraTre();
            };
            btnDong.Click += (sender, e) => Close();

            dgvBaoCao = new DataGridView();
            dgvBaoCao.Location = new Point(24, 180);
            dgvBaoCao.Size = new Size(960, 380);
            dgvBaoCao.ReadOnly = true;
            dgvBaoCao.AllowUserToAddRows = false;
            dgvBaoCao.AllowUserToDeleteRows = false;
            dgvBaoCao.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBaoCao.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBaoCao.BackgroundColor = Color.White;
            dgvBaoCao.BorderStyle = BorderStyle.None;
            dgvBaoCao.RowHeadersVisible = false;
            dgvBaoCao.EnableHeadersVisualStyles = false;
            dgvBaoCao.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(237, 242, 247);
            dgvBaoCao.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(36, 52, 71);
            dgvBaoCao.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dgvBaoCao.ColumnHeadersHeight = 42;
            dgvBaoCao.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvBaoCao.DefaultCellStyle.SelectionBackColor = Color.FromArgb(220, 232, 246);
            dgvBaoCao.DefaultCellStyle.SelectionForeColor = Color.FromArgb(36, 52, 71);
            dgvBaoCao.DefaultCellStyle.Padding = new Padding(2, 4, 2, 4);
            dgvBaoCao.RowTemplate.Height = 36;

            Controls.Add(lblTieuDe);
            Controls.Add(lblMoTa);
            Controls.Add(btnMuonTheoTheLoai);
            Controls.Add(btnTraTre);
            Controls.Add(btnDong);
            Controls.Add(dgvBaoCao);
        }

        private Button TaoButton(string text, int x, int y, Color backColor, Color foreColor)
        {
            Button button = new Button();
            button.Text = text;
            button.Size = new Size(170, 40);
            button.Location = new Point(x, y);
            button.BackColor = backColor;
            button.ForeColor = foreColor;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            return button;
        }

        private void DinhDangBaoCaoMuonTheoTheLoai()
        {
            if (dgvBaoCao.Columns.Count == 0)
            {
                return;
            }

            if (dgvBaoCao.Columns["TenTheLoai"] != null) dgvBaoCao.Columns["TenTheLoai"].HeaderText = "Thể loại";
            if (dgvBaoCao.Columns["SoLuotMuon"] != null) dgvBaoCao.Columns["SoLuotMuon"].HeaderText = "Số lượt mượn";
            if (dgvBaoCao.Columns["TiLe"] != null) dgvBaoCao.Columns["TiLe"].HeaderText = "Tỷ lệ (%)";

            dgvBaoCao.ClearSelection();
        }

        private void DinhDangBaoCaoTraTre()
        {
            if (dgvBaoCao.Columns.Count == 0)
            {
                return;
            }

            if (dgvBaoCao.Columns["TenSach"] != null) dgvBaoCao.Columns["TenSach"].HeaderText = "Tên sách";
            if (dgvBaoCao.Columns["NgayMuon"] != null) dgvBaoCao.Columns["NgayMuon"].HeaderText = "Ngày mượn";
            if (dgvBaoCao.Columns["SoNgayTraTre"] != null) dgvBaoCao.Columns["SoNgayTraTre"].HeaderText = "Số ngày trả trễ";

            dgvBaoCao.ClearSelection();
        }
    }
}
