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

            btnQuanLySach = TaoButton("Tiếp nhận sách");
            btnQuanLyDocGia = TaoButton("Lập thẻ độc giả");
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

            ThemNutVaoBang(btnQuanLySach, 0, 0);
            ThemNutVaoBang(btnQuanLyDocGia, 1, 0);
            ThemNutVaoBang(btnMuonSach, 2, 0);

            ThemNutVaoBang(btnTraSach, 0, 1);
            ThemNutVaoBang(btnTraCuuSach, 1, 1);
            ThemNutVaoBang(btnBaoCao, 2, 1);

            ThemNutVaoBang(btnThuTienPhat, 0, 2);
            ThemNutVaoBang(btnSuaQuyDinh, 1, 2);
            ThemNutVaoBang(btnDangXuat, 2, 2);

            if (laDocGia)
            {
                ThemNutVaoBang(btnThongTinCaNhan, 1, 3);
            }

            pnlContainer.Controls.Add(tblMenu);

            Controls.Add(lblTieuDe);
            Controls.Add(lblXinChao);
            Controls.Add(pnlContainer);
            PhanQuyen(vaiTro);
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

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            QuanLyThuVien.UTILS.SessionManager.Clear();
            Hide();
            FormDangNhap formDangNhap = new FormDangNhap();
            formDangNhap.Show();
        }
    }
}
