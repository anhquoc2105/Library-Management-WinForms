using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyThuVien.GUI
{
    public class FormMain : Form
    {
        private Label lblTieuDe;
        private Label lblXinChao;
        private FlowLayoutPanel pnlMenu;
        private Button btnQuanLySach;
        private Button btnQuanLyDocGia;
        private Button btnQuanLyThuThu;
        private Button btnThamSo;
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

            Text = "Menu chính - Quản lý thư viện";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1140, 720);
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

            pnlMenu = new FlowLayoutPanel();
            pnlMenu.Location = new Point(70, 130);
            pnlMenu.Size = new Size(1000, 520);
            pnlMenu.BackColor = Color.White;
            pnlMenu.Padding = new Padding(24);
            pnlMenu.WrapContents = true;
            pnlMenu.AutoScroll = true;
            pnlMenu.BorderStyle = BorderStyle.FixedSingle;

            btnQuanLySach = TaoButton("Tiếp nhận sách");
            btnQuanLyDocGia = TaoButton("Lập thẻ độc giả");
            btnQuanLyThuThu = TaoButton("Quản lý thủ thư");
            btnMuonSach = TaoButton("Cho mượn sách");
            btnTraSach = TaoButton("Nhận trả sách");
            btnThamSo = TaoButton("Tham số hiện tại");
            btnTraCuuSach = TaoButton("Tra cứu sách");
            btnBaoCao = TaoButton("Báo cáo");
            btnThongTinCaNhan = TaoButton("Thông tin cá nhân");
            btnThuTienPhat = TaoButton("Thu tiền phạt");
            btnSuaQuyDinh = TaoButton("Thay đổi quy định");
            btnDangXuat = TaoButton("Đăng xuất");

            btnQuanLySach.Click += (sender, e) => new FormQuanLySach().ShowDialog();
            btnQuanLyDocGia.Click += (sender, e) => new FormQuanLyDocGia().ShowDialog();
            btnQuanLyThuThu.Click += (sender, e) => new FormQuanLyThuThu().ShowDialog();
            btnMuonSach.Click += (sender, e) => new FormMuonSach().ShowDialog();
            btnTraSach.Click += (sender, e) => new FormTraSach().ShowDialog();
            btnThamSo.Click += (sender, e) => new FormThamSo().ShowDialog();
            btnTraCuuSach.Click += (sender, e) => new FormTraCuuSach().ShowDialog();
            btnBaoCao.Click += (sender, e) => new FormBaoCao().ShowDialog();
            btnThongTinCaNhan.Click += (sender, e) => new FormThongTinCaNhan().ShowDialog();
            btnThuTienPhat.Click += (sender, e) => new FormThuTienPhat().ShowDialog();
            btnSuaQuyDinh.Click += (sender, e) => new FormSuaQuyDinh().ShowDialog();
            btnDangXuat.Click += btnDangXuat_Click;

            pnlMenu.Controls.Add(btnQuanLySach);
            pnlMenu.Controls.Add(btnQuanLyDocGia);
            pnlMenu.Controls.Add(btnQuanLyThuThu);
            pnlMenu.Controls.Add(btnMuonSach);
            pnlMenu.Controls.Add(btnTraSach);
            pnlMenu.Controls.Add(btnThamSo);
            pnlMenu.Controls.Add(btnTraCuuSach);
            pnlMenu.Controls.Add(btnBaoCao);
            pnlMenu.Controls.Add(btnThongTinCaNhan);
            pnlMenu.Controls.Add(btnThuTienPhat);
            pnlMenu.Controls.Add(btnSuaQuyDinh);
            pnlMenu.Controls.Add(btnDangXuat);

            Controls.Add(lblTieuDe);
            Controls.Add(lblXinChao);
            Controls.Add(pnlMenu);

            PhanQuyen(vaiTro);
        }

        private Button TaoButton(string text)
        {
            Button button = new Button();
            button.Text = text;
            button.Size = new Size(280, 98);
            button.Margin = new Padding(14);
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = Color.FromArgb(235, 242, 248);
            button.ForeColor = Color.FromArgb(28, 77, 125);
            button.Font = new Font("Segoe UI", 13.5F, FontStyle.Bold);
            button.Cursor = Cursors.Hand;
            return button;
        }

        private void PhanQuyen(string vaiTro)
        {
            bool laDocGia = string.Equals(vaiTro, "DocGia", StringComparison.OrdinalIgnoreCase);

            btnQuanLySach.Visible = !laDocGia;
            btnQuanLyDocGia.Visible = !laDocGia;
            btnQuanLyThuThu.Visible = !laDocGia;
            btnMuonSach.Visible = !laDocGia;
            btnTraSach.Visible = !laDocGia;
            btnThamSo.Visible = !laDocGia;
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
