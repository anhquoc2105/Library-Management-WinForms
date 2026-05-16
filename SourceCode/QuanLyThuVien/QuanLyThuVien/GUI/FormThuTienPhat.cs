using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyThuVien.BUS;

namespace QuanLyThuVien.GUI
{
    public class FormThuTienPhat : Form
    {
        private readonly PhieuThuTienPhatBUS phieuThuBUS = new PhieuThuTienPhatBUS();
        private Label lblTieuDe;
        private Label lblMoTa;
        private Label lblDocGia;
        private Label lblTongNo;
        private Label lblSoTienThu;
        private Label lblConLai;
        private ComboBox cboDocGia;
        private TextBox txtTongNo;
        private TextBox txtSoTienThu;
        private TextBox txtConLai;
        private Button btnLapPhieuThu;
        private Button btnTaiLai;
        private Button btnDong;

        public FormThuTienPhat()
        {
            TaoGiaoDien();
            Load += FormThuTienPhat_Load;
        }

        private void TaoGiaoDien()
        {
            Text = "Phiếu thu tiền phạt";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(820, 420);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.FromArgb(244, 247, 251);
            Font = new Font("Segoe UI", 11F);

            lblTieuDe = new Label();
            lblTieuDe.Text = "Phiếu thu tiền phạt";
            lblTieuDe.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTieuDe.ForeColor = Color.FromArgb(28, 77, 125);
            lblTieuDe.AutoSize = true;
            lblTieuDe.Location = new Point(34, 24);

            lblMoTa = new Label();
            lblMoTa.Text = "Lập phiếu thu cho độc giả đang còn nợ, đảm bảo số tiền thu không vượt quá tổng nợ.";
            lblMoTa.ForeColor = Color.FromArgb(102, 117, 132);
            lblMoTa.AutoSize = true;
            lblMoTa.Location = new Point(38, 60);

            lblDocGia = TaoLabel("Họ tên độc giả", 40, 120);
            lblTongNo = TaoLabel("Tổng nợ", 40, 180);
            lblSoTienThu = TaoLabel("Số tiền thu", 40, 240);
            lblConLai = TaoLabel("Còn lại", 40, 300);

            cboDocGia = new ComboBox();
            cboDocGia.DropDownStyle = ComboBoxStyle.DropDownList;
            cboDocGia.Location = new Point(220, 116);
            cboDocGia.Size = new Size(540, 28);
            cboDocGia.SelectedIndexChanged += cboDocGia_SelectedIndexChanged;

            txtTongNo = new TextBox();
            txtTongNo.Location = new Point(220, 176);
            txtTongNo.Size = new Size(540, 28);
            txtTongNo.ReadOnly = true;

            txtSoTienThu = new TextBox();
            txtSoTienThu.Location = new Point(220, 236);
            txtSoTienThu.Size = new Size(540, 28);
            txtSoTienThu.TextChanged += txtSoTienThu_TextChanged;

            txtConLai = new TextBox();
            txtConLai.Location = new Point(220, 296);
            txtConLai.Size = new Size(540, 28);
            txtConLai.ReadOnly = true;

            btnLapPhieuThu = TaoButton("Lập phiếu thu", 424, 350, 120, Color.FromArgb(28, 77, 125), Color.White);
            btnTaiLai = TaoButton("Tải lại", 556, 350, 100, Color.FromArgb(230, 235, 241), Color.FromArgb(50, 60, 70));
            btnDong = TaoButton("Đóng", 668, 350, 100, Color.FromArgb(230, 235, 241), Color.FromArgb(50, 60, 70));

            btnLapPhieuThu.Click += btnLapPhieuThu_Click;
            btnTaiLai.Click += btnTaiLai_Click;
            btnDong.Click += (sender, e) => Close();

            Controls.Add(lblTieuDe);
            Controls.Add(lblMoTa);
            Controls.Add(lblDocGia);
            Controls.Add(lblTongNo);
            Controls.Add(lblSoTienThu);
            Controls.Add(lblConLai);
            Controls.Add(cboDocGia);
            Controls.Add(txtTongNo);
            Controls.Add(txtSoTienThu);
            Controls.Add(txtConLai);
            Controls.Add(btnLapPhieuThu);
            Controls.Add(btnTaiLai);
            Controls.Add(btnDong);
        }

        private Label TaoLabel(string text, int x, int y)
        {
            Label label = new Label();
            label.Text = text;
            label.AutoSize = true;
            label.Location = new Point(x, y);
            return label;
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

        private void FormThuTienPhat_Load(object sender, EventArgs e)
        {
            TaiDuLieu();
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            txtSoTienThu.Clear();
            TaiDuLieu();
        }

        private void TaiDuLieu()
        {
            DataTable dataTable = phieuThuBUS.LayDanhSachDocGiaNo();
            cboDocGia.DataSource = dataTable;
            cboDocGia.DisplayMember = "TenDG";
            cboDocGia.ValueMember = "MaDG";
            CapNhatTongNo();
            TinhConLai();
        }

        private void cboDocGia_SelectedIndexChanged(object sender, EventArgs e)
        {
            CapNhatTongNo();
            TinhConLai();
        }

        private void txtSoTienThu_TextChanged(object sender, EventArgs e)
        {
            TinhConLai();
        }

        private void CapNhatTongNo()
        {
            DataRowView row = cboDocGia.SelectedItem as DataRowView;
            txtTongNo.Text = row == null ? string.Empty : Convert.ToDecimal(row["TongNo"]).ToString("N0");
        }

        private void TinhConLai()
        {
            decimal tongNo;
            decimal soTienThu;

            if (!decimal.TryParse(txtTongNo.Text.Replace(",", ""), out tongNo))
            {
                txtConLai.Text = string.Empty;
                return;
            }

            if (!decimal.TryParse(txtSoTienThu.Text.Trim(), out soTienThu))
            {
                txtConLai.Text = tongNo.ToString("N0");
                return;
            }

            decimal conLai = tongNo - soTienThu;
            if (conLai < 0)
            {
                conLai = 0;
            }

            txtConLai.Text = conLai.ToString("N0");
        }

        private void btnLapPhieuThu_Click(object sender, EventArgs e)
        {
            if (cboDocGia.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn độc giả.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal soTienThu;
            if (!decimal.TryParse(txtSoTienThu.Text.Trim(), out soTienThu))
            {
                MessageBox.Show("Số tiền thu không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoTienThu.Focus();
                return;
            }

            int maDG = Convert.ToInt32(cboDocGia.SelectedValue);
            string thongBao;
            bool thanhCong = phieuThuBUS.LapPhieuThu(maDG, soTienThu, out thongBao);

            MessageBox.Show(
                thongBao,
                thanhCong ? "Thông báo" : "Lỗi",
                MessageBoxButtons.OK,
                thanhCong ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            if (thanhCong)
            {
                txtSoTienThu.Clear();
                TaiDuLieu();
            }
        }
    }
}
