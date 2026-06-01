using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyThuVien.BUS;

namespace QuanLyThuVien.GUI
{
    public class FormThamSo : Form
    {
        private readonly ThamSoBUS thamSoBUS = new ThamSoBUS();
        private Label lblTieuDe;
        private Label lblMoTa;
        private DataGridView dgvThamSo;
        private Button btnTaiLai;
        private Button btnDong;

        public FormThamSo()
        {
            TaoGiaoDien();
            Load += FormThamSo_Load;
        }

        private void TaoGiaoDien()
        {
            Text = "Tham số hệ thống";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(900, 500);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.FromArgb(244, 247, 251);
            Font = new Font("Segoe UI", 11F);

            lblTieuDe = new Label();
            lblTieuDe.Text = "Tham số hệ thống";
            lblTieuDe.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTieuDe.ForeColor = Color.FromArgb(28, 77, 125);
            lblTieuDe.AutoSize = true;
            lblTieuDe.Location = new Point(34, 24);

            lblMoTa = new Label();
            lblMoTa.Text = "Theo dõi giới hạn tuổi, thời hạn thẻ, số sách mượn tối đa và mức tiền phạt.";
            lblMoTa.ForeColor = Color.FromArgb(102, 117, 132);
            lblMoTa.AutoSize = true;
            lblMoTa.Location = new Point(38, 60);

            btnTaiLai = TaoButton("Tải lại", 670, 104, Color.FromArgb(230, 235, 241), Color.FromArgb(50, 60, 70));
            btnDong = TaoButton("Đóng", 786, 104, Color.FromArgb(230, 235, 241), Color.FromArgb(50, 60, 70));
            btnTaiLai.Click += btnTaiLai_Click;
            btnDong.Click += (sender, e) => Close();

            dgvThamSo = new DataGridView();
            dgvThamSo.Location = new Point(24, 170);
            dgvThamSo.Size = new Size(840, 280);
            dgvThamSo.ReadOnly = true;
            dgvThamSo.AllowUserToAddRows = false;
            dgvThamSo.AllowUserToDeleteRows = false;
            dgvThamSo.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvThamSo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvThamSo.MultiSelect = false;
            dgvThamSo.BackgroundColor = Color.White;
            dgvThamSo.BorderStyle = BorderStyle.None;
            dgvThamSo.RowHeadersVisible = false;
            dgvThamSo.EnableHeadersVisualStyles = false;
            dgvThamSo.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(237, 242, 247);
            dgvThamSo.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(36, 52, 71);
            dgvThamSo.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dgvThamSo.ColumnHeadersHeight = 42;
            dgvThamSo.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvThamSo.DefaultCellStyle.SelectionBackColor = Color.FromArgb(220, 232, 246);
            dgvThamSo.DefaultCellStyle.SelectionForeColor = Color.FromArgb(36, 52, 71);
            dgvThamSo.DefaultCellStyle.Padding = new Padding(2, 4, 2, 4);
            dgvThamSo.RowTemplate.Height = 36;

            Controls.Add(lblTieuDe);
            Controls.Add(lblMoTa);
            Controls.Add(btnTaiLai);
            Controls.Add(btnDong);
            Controls.Add(dgvThamSo);
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

        private void FormThamSo_Load(object sender, EventArgs e)
        {
            TaiDanhSachThamSo();
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            TaiDanhSachThamSo();
        }

        private void TaiDanhSachThamSo()
        {
            try
            {
                dgvThamSo.DataSource = thamSoBUS.LayDanhSachThamSo();

                if (dgvThamSo.Columns.Count > 0)
                {
                    dgvThamSo.Columns["MaThamSo"].HeaderText = "Mã tham số";
                    dgvThamSo.Columns["GiaTriThe"].HeaderText = "Giá trị thẻ";
                    dgvThamSo.Columns["SoTuoiDG"].HeaderText = "Tuổi tối thiểu";
                    dgvThamSo.Columns["SoTuoiDGToiDa"].HeaderText = "Tuổi tối đa";
                    dgvThamSo.Columns["ThoiGianXB"].HeaderText = "Khoảng năm XB";
                    dgvThamSo.Columns["SoSachMuonToiDa"].HeaderText = "Sách mượn tối đa";
                    dgvThamSo.Columns["SoNgayMuonToiDa"].HeaderText = "Ngày mượn tối đa";
                    dgvThamSo.Columns["TienPhat"].HeaderText = "Tiền phạt/ngày";
                    dgvThamSo.Columns["TienPhat"].DefaultCellStyle.Format = "N0";
                }

                dgvThamSo.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải danh sách tham số.\nChi tiết: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
