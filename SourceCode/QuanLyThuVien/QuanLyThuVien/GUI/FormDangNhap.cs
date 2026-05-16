using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyThuVien.BUS;
using QuanLyThuVien.DTO;
using QuanLyThuVien.UTILS;

namespace QuanLyThuVien.GUI
{
    public class FormDangNhap : Form
    {
        private readonly TaiKhoanBUS taiKhoanBUS = new TaiKhoanBUS();
        private Panel pnlHeader;
        private Panel pnlBody;
        private Label lblTieuDe;
        private Label lblMoTa;
        private Label lblTenDangNhap;
        private Label lblMatKhau;
        private TextBox txtTenDangNhap;
        private TextBox txtMatKhau;
        private Button btnDangNhap;
        private Button btnThoat;

        public FormDangNhap()
        {
            TaoGiaoDien();
        }

        private void TaoGiaoDien()
        {
            Text = "Đăng nhập hệ thống";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(520, 360);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.FromArgb(244, 247, 251);
            Font = new Font("Segoe UI", 11F);

            pnlHeader = new Panel();
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 110;
            pnlHeader.BackColor = Color.FromArgb(28, 77, 125);

            lblTieuDe = new Label();
            lblTieuDe.Text = "Quản Lý Thư Viện";
            lblTieuDe.ForeColor = Color.White;
            lblTieuDe.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblTieuDe.AutoSize = true;
            lblTieuDe.Location = new Point(28, 22);

            lblMoTa = new Label();
            lblMoTa.Text = "Đăng nhập để sử dụng các chức năng của hệ thống";
            lblMoTa.ForeColor = Color.FromArgb(222, 233, 243);
            lblMoTa.Font = new Font("Segoe UI", 10.5F);
            lblMoTa.AutoSize = true;
            lblMoTa.Location = new Point(32, 72);

            pnlHeader.Controls.Add(lblTieuDe);
            pnlHeader.Controls.Add(lblMoTa);

            pnlBody = new Panel();
            pnlBody.Location = new Point(30, 135);
            pnlBody.Size = new Size(460, 190);
            pnlBody.BackColor = Color.White;
            pnlBody.BorderStyle = BorderStyle.FixedSingle;

            lblTenDangNhap = new Label();
            lblTenDangNhap.Text = "Tên đăng nhập";
            lblTenDangNhap.AutoSize = true;
            lblTenDangNhap.Location = new Point(28, 24);

            txtTenDangNhap = new TextBox();
            txtTenDangNhap.Name = "txtTenDangNhap";
            txtTenDangNhap.Location = new Point(32, 48);
            txtTenDangNhap.Size = new Size(390, 30);

            lblMatKhau = new Label();
            lblMatKhau.Text = "Mật khẩu";
            lblMatKhau.AutoSize = true;
            lblMatKhau.Location = new Point(28, 88);

            txtMatKhau = new TextBox();
            txtMatKhau.Name = "txtMatKhau";
            txtMatKhau.Location = new Point(32, 112);
            txtMatKhau.Size = new Size(390, 30);
            txtMatKhau.UseSystemPasswordChar = true;

            btnDangNhap = new Button();
            btnDangNhap.Name = "btnDangNhap";
            btnDangNhap.Text = "Đăng nhập";
            btnDangNhap.Location = new Point(210, 148);
            btnDangNhap.Size = new Size(112, 38);
            btnDangNhap.BackColor = Color.FromArgb(28, 77, 125);
            btnDangNhap.ForeColor = Color.White;
            btnDangNhap.FlatStyle = FlatStyle.Flat;
            btnDangNhap.FlatAppearance.BorderSize = 0;
            btnDangNhap.Click += btnDangNhap_Click;

            btnThoat = new Button();
            btnThoat.Name = "btnThoat";
            btnThoat.Text = "Thoát";
            btnThoat.Location = new Point(322, 148);
            btnThoat.Size = new Size(112, 38);
            btnThoat.BackColor = Color.FromArgb(230, 235, 241);
            btnThoat.ForeColor = Color.FromArgb(50, 60, 70);
            btnThoat.FlatStyle = FlatStyle.Flat;
            btnThoat.FlatAppearance.BorderSize = 0;
            btnThoat.Click += btnThoat_Click;

            pnlBody.Controls.Add(lblTenDangNhap);
            pnlBody.Controls.Add(txtTenDangNhap);
            pnlBody.Controls.Add(lblMatKhau);
            pnlBody.Controls.Add(txtMatKhau);
            pnlBody.Controls.Add(btnDangNhap);
            pnlBody.Controls.Add(btnThoat);

            Controls.Add(pnlHeader);
            Controls.Add(pnlBody);

            AcceptButton = btnDangNhap;
            CancelButton = btnThoat;
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string tenDangNhap = txtTenDangNhap.Text.Trim();
            string matKhau = txtMatKhau.Text.Trim();
            string thongBao;

            TaiKhoanDTO taiKhoan = taiKhoanBUS.DangNhap(tenDangNhap, matKhau, out thongBao);

            if (taiKhoan == null)
            {
                MessageBox.Show(thongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                if (string.IsNullOrWhiteSpace(tenDangNhap))
                {
                    txtTenDangNhap.Focus();
                }
                else
                {
                    txtMatKhau.Focus();
                }

                return;
            }

            string vaiTroHienThi = string.Equals(taiKhoan.VaiTro, "ThuThu", StringComparison.OrdinalIgnoreCase)
                ? "Thủ thư"
                : string.Equals(taiKhoan.VaiTro, "DocGia", StringComparison.OrdinalIgnoreCase)
                    ? "Độc giả"
                    : taiKhoan.VaiTro;

            MessageBox.Show(
                thongBao + "\nVai trò: " + vaiTroHienThi,
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            SessionManager.MaTaiKhoan = taiKhoan.MaTaiKhoan;
            SessionManager.TenDangNhap = taiKhoan.TenDangNhap;
            SessionManager.VaiTro = taiKhoan.VaiTro;
            SessionManager.MaDG = taiKhoan.MaDG;
            SessionManager.MaTT = taiKhoan.MaTT;

            Hide();
            FormMain formMain = new FormMain(taiKhoan.TenDangNhap, taiKhoan.VaiTro);
            formMain.FormClosed += (s, args) => Close();
            formMain.Show();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
