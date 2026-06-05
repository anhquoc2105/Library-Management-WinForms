using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyThuVien.GUI
{
    public class FormMain : Form
    {
        private Label lblTieuDe;
        private Label lblXinChao;
        private Panel pnlContainer;
        private Label lblMoTa;
        private TableLayoutPanel tblMenu;
        private Button btnQuanLySach;
        private Button btnQuanLyDocGia;
        private Button btnMuonSach;
        private Button btnTraSach;
        private Button btnTraCuuSach;
        private Button btnBaoCao;
        private Button btnThongTinCaNhan;
        private Button btnThuTienPhat;
        private Button btnSuaQuyDinh;
        private Button btnDangXuat;

        public FormMain(string tenDangNhap, string vaiTro)
        {
            TaoGiaoDien(tenDangNhap, vaiTro);
        }

        private void TaoGiaoDien(string tenDangNhap, string vaiTro)
        {
            string vaiTroHienThi = string.Equals(vaiTro, "ThuThu", StringComparison.OrdinalIgnoreCase)
                ? "Thủ thư"
                : string.Equals(vaiTro, "DocGia", StringComparison.OrdinalIgnoreCase)
                    ? "Độc giả"
                    : vaiTro;
            bool laDocGia = string.Equals(vaiTro, "DocGia", StringComparison.OrdinalIgnoreCase);
            int soHangMenu = laDocGia ? 4 : 3;
            int chieuCaoBang = soHangMenu * 110;
            int chieuCaoKhung = chieuCaoBang + 70;

            Text = "Menu chính - Quản lý thư viện";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1140, laDocGia ? 720 : 610);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.FromArgb(243, 246, 250);
            Font = new Font("Segoe UI", 12F);

            lblTieuDe = new Label();
            lblTieuDe.Text = "HỆ THỐNG QUẢN LÝ THƯ VIỆN";
            lblTieuDe.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblTieuDe.ForeColor = Color.FromArgb(28, 77, 125);
            lblTieuDe.AutoSize = true;
            lblTieuDe.Location = new Point(310, 28);

            lblXinChao = new Label();
            lblXinChao.Text = "Xin chào: " + tenDangNhap + " | Vai trò: " + vaiTroHienThi;
            lblXinChao.Font = new Font("Segoe UI", 13F);
            lblXinChao.ForeColor = Color.FromArgb(90, 105, 120);
            lblXinChao.AutoSize = true;
            lblXinChao.Location = new Point(410, 72);

            pnlContainer = new Panel();
            pnlContainer.Location = new Point(70, 130);
            pnlContainer.Size = new Size(1000, chieuCaoKhung);
            pnlContainer.BackColor = Color.White;
            pnlContainer.BorderStyle = BorderStyle.FixedSingle;

            btnQuanLySach = TaoButton("Tiếp nhận sách");
            btnQuanLyDocGia = TaoButton("Quản lý thẻ độc giả");
            btnMuonSach = TaoButton("Cho mượn sách");
            btnTraSach = TaoButton("Nhận trả sách");
            btnTraCuuSach = TaoButton("Tra cứu sách");
            btnBaoCao = TaoButton("Báo cáo");
            btnThongTinCaNhan = TaoButton("Thông tin cá nhân");
            btnThuTienPhat = TaoButton("Thu tiền phạt");
            btnSuaQuyDinh = TaoButton("Thay đổi quy định");
            btnDangXuat = TaoButton("Đăng xuất");

            btnQuanLySach.Click += (sender, e) => new FormQuanLySach().ShowDialog();
            btnQuanLyDocGia.Click += (sender, e) => new FormQuanLyDocGia().ShowDialog();
            btnMuonSach.Click += (sender, e) => new FormMuonSach().ShowDialog();
            btnTraSach.Click += (sender, e) => new FormTraSach().ShowDialog();
            btnTraCuuSach.Click += (sender, e) => new FormTraCuuSach().ShowDialog();
            btnBaoCao.Click += (sender, e) => new FormBaoCao().ShowDialog();
            btnThongTinCaNhan.Click += (sender, e) => new FormThongTinCaNhan().ShowDialog();
            btnThuTienPhat.Click += (sender, e) => new FormThuTienPhat().ShowDialog();
            btnSuaQuyDinh.Click += (sender, e) => new FormSuaQuyDinh().ShowDialog();
            btnDangXuat.Click += btnDangXuat_Click;

            if (laDocGia)
            {
                TaoGiaoDienDocGia(tenDangNhap);
            }
            else
            {
                TaoGiaoDienThuThu(soHangMenu, chieuCaoBang);
            }

            Controls.Add(lblTieuDe);
            Controls.Add(lblXinChao);
            Controls.Add(pnlContainer);
            PhanQuyen(vaiTro);
        }

        private void TaoGiaoDienThuThu(int soHangMenu, int chieuCaoBang)
        {
            tblMenu = new TableLayoutPanel();
            tblMenu.Location = new Point(36, 28);
            tblMenu.Size = new Size(928, chieuCaoBang);
            tblMenu.ColumnCount = 3;
            tblMenu.RowCount = soHangMenu;
            tblMenu.BackColor = Color.White;

            tblMenu.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            tblMenu.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            tblMenu.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));

            for (int i = 0; i < soHangMenu; i++)
            {
                tblMenu.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
            }

            ThemNutVaoBang(btnQuanLySach, 0, 0);
            ThemNutVaoBang(btnQuanLyDocGia, 1, 0);
            ThemNutVaoBang(btnMuonSach, 2, 0);

            ThemNutVaoBang(btnTraSach, 0, 1);
            ThemNutVaoBang(btnTraCuuSach, 1, 1);
            ThemNutVaoBang(btnBaoCao, 2, 1);

            ThemNutVaoBang(btnThuTienPhat, 0, 2);
            ThemNutVaoBang(btnSuaQuyDinh, 1, 2);
            ThemNutVaoBang(btnDangXuat, 2, 2);

            pnlContainer.Controls.Add(tblMenu);
        }

        private void TaoGiaoDienDocGia(string tenDangNhap)
        {
            pnlContainer.BackColor = Color.FromArgb(247, 250, 253);

            Panel pnlHero = new Panel();
            pnlHero.Location = new Point(28, 24);
            pnlHero.Size = new Size(944, 140);
            pnlHero.BackColor = Color.FromArgb(28, 77, 125);

            Label lblChaoMung = new Label();
            lblChaoMung.Text = "Xin chào, " + tenDangNhap;
            lblChaoMung.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblChaoMung.ForeColor = Color.White;
            lblChaoMung.AutoSize = true;
            lblChaoMung.Location = new Point(34, 26);

            lblMoTa = new Label();
            lblMoTa.Text = "Khu vực dành cho độc giả: tra cứu sách, xem thông tin cá nhân và quản lý phiên đăng nhập.";
            lblMoTa.Font = new Font("Segoe UI", 11.5F);
            lblMoTa.ForeColor = Color.FromArgb(222, 233, 243);
            lblMoTa.AutoSize = true;
            lblMoTa.Location = new Point(38, 76);

            pnlHero.Controls.Add(lblChaoMung);
            pnlHero.Controls.Add(lblMoTa);

            TableLayoutPanel tblDocGia = new TableLayoutPanel();
            tblDocGia.Location = new Point(28, 188);
            tblDocGia.Size = new Size(944, 278);
            tblDocGia.ColumnCount = 3;
            tblDocGia.RowCount = 2;
            tblDocGia.BackColor = Color.Transparent;
            tblDocGia.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tblDocGia.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 0F));
            tblDocGia.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tblDocGia.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tblDocGia.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

            Control cardTraCuuSach = TaoTheTinhNang(
                "Tra cứu sách",
                "Tìm nhanh sách theo mã, tên, thể loại hoặc tác giả đang có trong thư viện.",
                Color.FromArgb(28, 77, 125),
                btnTraCuuSach_Click);

            Control cardThongTinCaNhan = TaoTheTinhNang(
                "Thông tin cá nhân",
                "Xem hồ sơ độc giả, ngày lập thẻ và lịch sử mượn sách của bạn.",
                Color.FromArgb(42, 102, 148),
                btnThongTinCaNhan_Click);

            Control cardDangXuat = TaoTheTinhNang(
                "Đăng xuất",
                "Kết thúc phiên làm việc hiện tại và quay lại màn hình đăng nhập.",
                Color.FromArgb(90, 105, 120),
                btnDangXuat_Click);

            tblDocGia.Controls.Add(cardTraCuuSach, 0, 0);
            tblDocGia.Controls.Add(cardThongTinCaNhan, 2, 0);
            tblDocGia.Controls.Add(cardDangXuat, 0, 1);
            tblDocGia.SetColumnSpan(cardDangXuat, 3);
            cardDangXuat.Margin = new Padding(236, 12, 236, 12);

            pnlContainer.Controls.Add(pnlHero);
            pnlContainer.Controls.Add(tblDocGia);
        }

        private Button TaoButton(string text)
        {
            Button button = new Button();
            button.Text = text;
            button.Dock = DockStyle.Fill;
            button.Margin = new Padding(12);
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.TabStop = false;
            button.BackColor = Color.FromArgb(235, 242, 248);
            button.ForeColor = Color.FromArgb(28, 77, 125);
            button.Font = new Font("Segoe UI", 13.5F, FontStyle.Bold);
            button.Cursor = Cursors.Hand;
            return button;
        }

        private Control TaoTheTinhNang(string title, string description, Color accentColor, EventHandler onClick)
        {
            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.Margin = new Padding(12);
            panel.BackColor = Color.White;
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.Cursor = Cursors.Hand;

            Panel accent = new Panel();
            accent.Dock = DockStyle.Top;
            accent.Height = 10;
            accent.BackColor = accentColor;

            Label lblTitle = new Label();
            lblTitle.Text = title;
            lblTitle.Font = new Font("Segoe UI", 17F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(28, 77, 125);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(24, 26);
            lblTitle.Cursor = Cursors.Hand;

            Label lblDescription = new Label();
            lblDescription.Text = description;
            lblDescription.Font = new Font("Segoe UI", 11F);
            lblDescription.ForeColor = Color.FromArgb(90, 105, 120);
            lblDescription.Size = new Size(380, 76);
            lblDescription.Location = new Point(24, 66);
            lblDescription.Cursor = Cursors.Hand;

            Label lblAction = new Label();
            lblAction.Text = "Mo chuc nang";
            lblAction.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            lblAction.ForeColor = accentColor;
            lblAction.AutoSize = true;
            lblAction.Location = new Point(24, 150);
            lblAction.Cursor = Cursors.Hand;

            panel.Click += onClick;
            accent.Click += onClick;
            lblTitle.Click += onClick;
            lblDescription.Click += onClick;
            lblAction.Click += onClick;

            panel.Controls.Add(accent);
            panel.Controls.Add(lblTitle);
            panel.Controls.Add(lblDescription);
            panel.Controls.Add(lblAction);
            return panel;
        }

        private void ThemNutVaoBang(Control control, int col, int row)
        {
            tblMenu.Controls.Add(control, col, row);
        }

        private void PhanQuyen(string vaiTro)
        {
            bool laDocGia = string.Equals(vaiTro, "DocGia", StringComparison.OrdinalIgnoreCase);

            btnQuanLySach.Visible = !laDocGia;
            btnQuanLyDocGia.Visible = !laDocGia;
            btnMuonSach.Visible = !laDocGia;
            btnTraSach.Visible = !laDocGia;
            btnBaoCao.Visible = !laDocGia;
            btnThuTienPhat.Visible = !laDocGia;
            btnSuaQuyDinh.Visible = !laDocGia;

            btnTraCuuSach.Visible = true;
            btnThongTinCaNhan.Visible = laDocGia;
        }

        private void btnTraCuuSach_Click(object sender, EventArgs e)
        {
            new FormTraCuuSach().ShowDialog();
        }

        private void btnThongTinCaNhan_Click(object sender, EventArgs e)
        {
            new FormThongTinCaNhan().ShowDialog();
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            QuanLyThuVien.UTILS.SessionManager.Clear();
            Hide();
            FormDangNhap formDangNhap = new FormDangNhap();
            formDangNhap.Show();
        }
    }
}
