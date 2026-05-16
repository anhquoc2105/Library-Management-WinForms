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
        private ComboBox cboDocGia;
        private TextBox txtTongNo;
        private TextBox txtSoTienThu;
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
            Text = "Thu tiền phạt";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(760, 360);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.FromArgb(244, 247, 251);
            Font = new Font("Segoe UI", 11F);

            lblTieuDe = new Label();
            lblTieuDe.Text = "Thu tiền phạt";
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
            lblTongNo = TaoLabel("Tổng nợ", 40, 175);
            lblSoTienThu = TaoLabel("Số tiền thu", 40, 230);

            cboDocGia = new ComboBox();
            cboDocGia.DropDownStyle = ComboBoxStyle.DropDownList;
            cboDocGia.Location = new Point(210, 116);
            cboDocGia.Size = new Size(500, 28);
            cboDocGia.SelectedIndexChanged += cboDocGia_SelectedIndexChanged;

            txtTongNo = new TextBox();
            txtTongNo.Location = new Point(210, 171);
            txtTongNo.Size = new Size(500, 28);
            txtTongNo.ReadOnly = true;

            txtSoTienThu = new TextBox();
            txtSoTienThu.Location = new Point(210, 226);
            txtSoTienThu.Size = new Size(500, 28);

            btnLapPhieuThu = TaoButton("Lập phiếu thu", 382, 285, Color.FromArgb(28, 77, 125), Color.White);
            btnTaiLai = TaoButton("Tải lại", 498, 285, Color.FromArgb(230, 235, 241), Color.FromArgb(50, 60, 70));
            btnDong = TaoButton("Đóng", 614, 285, Color.FromArgb(230, 235, 241), Color.FromArgb(50, 60, 70));

            btnLapPhieuThu.Click += btnLapPhieuThu_Click;
            btnTaiLai.Click += btnTaiLai_Click;
            btnDong.Click += (sender, e) => Close();

            Controls.Add(lblTieuDe);
            Controls.Add(lblMoTa);
            Controls.Add(lblDocGia);
            Controls.Add(lblTongNo);
            Controls.Add(lblSoTienThu);
            Controls.Add(cboDocGia);
            Controls.Add(txtTongNo);
            Controls.Add(txtSoTienThu);
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
        }

        private void cboDocGia_SelectedIndexChanged(object sender, EventArgs e)
        {
            CapNhatTongNo();
        }

        private void CapNhatTongNo()
        {
            DataRowView row = cboDocGia.SelectedItem as DataRowView;
            txtTongNo.Text = row == null ? string.Empty : Convert.ToDecimal(row["TongNo"]).ToString("N0");
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
