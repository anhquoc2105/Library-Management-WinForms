using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using QuanLyThuVien.BUS;

namespace QuanLyThuVien.GUI
{
    public class FormMuonSach : Form
    {
        private readonly DocGiaBUS docGiaBUS = new DocGiaBUS();
        private readonly SachBUS sachBUS = new SachBUS();
        private readonly PhieuMuonBUS phieuMuonBUS = new PhieuMuonBUS();
        private Label lblTieuDe;
        private Label lblMoTa;
        private Label lblDocGia;
        private Label lblTimSach;
        private ComboBox cboDocGia;
        private TextBox txtTimSach;
        private DataGridView dgvSachCon;
        private Button btnLapPhieu;
        private Button btnTaiLai;
        private Button btnDong;
        private DataTable danhSachSachConGoc;
        private readonly HashSet<int> maSachDangChon = new HashSet<int>();

        public FormMuonSach()
        {
            TaoGiaoDien();
            Load += FormMuonSach_Load;
        }

        private void TaoGiaoDien()
        {
            Text = "Cho mượn sách";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1380, 800);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.FromArgb(244, 247, 251);
            Font = new Font("Segoe UI", 11F);

            lblTieuDe = new Label();
            lblTieuDe.Text = "Cho mượn sách";
            lblTieuDe.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTieuDe.ForeColor = Color.FromArgb(28, 77, 125);
            lblTieuDe.AutoSize = true;
            lblTieuDe.Location = new Point(34, 24);

            lblMoTa = new Label();
            lblMoTa.Text = "Chọn độc giả và sách còn trong kho để lập giao dịch mượn.";
            lblMoTa.ForeColor = Color.FromArgb(102, 117, 132);
            lblMoTa.Font = new Font("Segoe UI", 10.5F);
            lblMoTa.AutoSize = true;
            lblMoTa.Location = new Point(38, 64);

            lblDocGia = new Label();
            lblDocGia.Text = "Độc giả";
            lblDocGia.AutoSize = true;
            lblDocGia.Location = new Point(40, 110);

            cboDocGia = new ComboBox();
            cboDocGia.DropDownStyle = ComboBoxStyle.DropDownList;
            cboDocGia.Location = new Point(40, 136);
            cboDocGia.Size = new Size(400, 30);

            lblTimSach = new Label();
            lblTimSach.Text = "T\u00ecm s\u00e1ch";
            lblTimSach.AutoSize = true;
            lblTimSach.Location = new Point(470, 110);

            txtTimSach = new TextBox();
            txtTimSach.Location = new Point(470, 136);
            txtTimSach.Size = new Size(360, 30);
            txtTimSach.TextChanged += txtTimSach_TextChanged;

            btnLapPhieu = TaoButton("Lập phiếu mượn", 880, 128, 180, Color.FromArgb(28, 77, 125), Color.White);
            btnTaiLai = TaoButton("Tải lại", 1080, 128, 120, Color.FromArgb(230, 235, 241), Color.FromArgb(50, 60, 70));
            btnDong = TaoButton("Đóng", 1214, 128, 120, Color.FromArgb(230, 235, 241), Color.FromArgb(50, 60, 70));

            btnLapPhieu.Click += btnLapPhieu_Click;
            btnTaiLai.Click += btnTaiLai_Click;
            btnDong.Click += (sender, e) => Close();

            dgvSachCon = new DataGridView();
            dgvSachCon.Location = new Point(24, 210);
            dgvSachCon.Size = new Size(1332, 548);
            dgvSachCon.AllowUserToAddRows = false;
            dgvSachCon.AllowUserToDeleteRows = false;
            dgvSachCon.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSachCon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSachCon.MultiSelect = true;
            dgvSachCon.BackgroundColor = Color.White;
            dgvSachCon.BorderStyle = BorderStyle.None;
            dgvSachCon.RowHeadersVisible = false;
            dgvSachCon.EnableHeadersVisualStyles = false;
            dgvSachCon.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(237, 242, 247);
            dgvSachCon.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(36, 52, 71);
            dgvSachCon.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dgvSachCon.ColumnHeadersHeight = 46;
            dgvSachCon.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvSachCon.DefaultCellStyle.SelectionBackColor = Color.FromArgb(220, 232, 246);
            dgvSachCon.DefaultCellStyle.SelectionForeColor = Color.FromArgb(36, 52, 71);
            dgvSachCon.DefaultCellStyle.Padding = new Padding(2, 4, 2, 4);
            dgvSachCon.RowTemplate.Height = 40;
            dgvSachCon.CellContentClick += dgvSachCon_CellContentClick;

            Controls.Add(lblTieuDe);
            Controls.Add(lblMoTa);
            Controls.Add(lblDocGia);
            Controls.Add(cboDocGia);
            Controls.Add(lblTimSach);
            Controls.Add(txtTimSach);
            Controls.Add(btnLapPhieu);
            Controls.Add(btnTaiLai);
            Controls.Add(btnDong);
            Controls.Add(dgvSachCon);
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

        private void FormMuonSach_Load(object sender, EventArgs e)
        {
            TaiDuLieu();
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            TaiDuLieu();
        }

        private void TaiDuLieu()
        {
            DataTable docGiaTable = docGiaBUS.LayDanhSachDocGiaChoCombo();
            cboDocGia.DataSource = docGiaTable;
            cboDocGia.DisplayMember = "TenDG";
            cboDocGia.ValueMember = "MaDG";

            maSachDangChon.Clear();
            danhSachSachConGoc = sachBUS.LayDanhSachSachCon();
            if (!danhSachSachConGoc.Columns.Contains("MaSachHienThi"))
            {
                danhSachSachConGoc.Columns.Add("MaSachHienThi", typeof(string));
            }

            foreach (DataRow row in danhSachSachConGoc.Rows)
            {
                row["MaSachHienThi"] = Convert.ToInt32(row["MaSach"]).ToString("D5");
            }

            danhSachSachConGoc.Columns["MaSachHienThi"].SetOrdinal(0);
            ApDungLocSach();
        }

        private void txtTimSach_TextChanged(object sender, EventArgs e)
        {
            ApDungLocSach();
        }

        private void ApDungLocSach()
        {
            CapNhatSachDangChonTuGrid();

            if (danhSachSachConGoc == null)
            {
                return;
            }

            string tuKhoa = ChuanHoaTimKiem(txtTimSach == null ? string.Empty : txtTimSach.Text);
            DataTable ketQua = danhSachSachConGoc.Clone();

            foreach (DataRow row in danhSachSachConGoc.Rows)
            {
                if (string.IsNullOrWhiteSpace(tuKhoa)
                    || ChuaTuKhoa(row, "MaSachHienThi", tuKhoa)
                    || ChuaTuKhoa(row, "TenSach", tuKhoa)
                    || ChuaTuKhoa(row, "TenTheLoai", tuKhoa)
                    || ChuaTuKhoa(row, "TenTG", tuKhoa))
                {
                    ketQua.ImportRow(row);
                }
            }

            dgvSachCon.DataSource = ketQua;
            DinhDangCot();
            KhoiPhucSachDangChon();
        }

        private bool ChuaTuKhoa(DataRow row, string tenCot, string tuKhoa)
        {
            if (!row.Table.Columns.Contains(tenCot))
            {
                return false;
            }

            return ChuanHoaTimKiem(Convert.ToString(row[tenCot])).Contains(tuKhoa);
        }

        private string ChuanHoaTimKiem(string giaTri)
        {
            if (string.IsNullOrWhiteSpace(giaTri))
            {
                return string.Empty;
            }

            string normalized = giaTri.Normalize(NormalizationForm.FormD);
            StringBuilder builder = new StringBuilder();

            foreach (char c in normalized)
            {
                UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(c);
                if (category != UnicodeCategory.NonSpacingMark)
                {
                    builder.Append(c);
                }
            }

            return builder.ToString()
                .Normalize(NormalizationForm.FormC)
                .Replace('đ', 'd')
                .Replace('Đ', 'D')
                .ToLowerInvariant();
        }

        private void CapNhatSachDangChonTuGrid()
        {
            if (dgvSachCon == null || dgvSachCon.Columns["Chon"] == null || dgvSachCon.Columns["MaSach"] == null)
            {
                return;
            }

            foreach (DataGridViewRow row in dgvSachCon.Rows)
            {
                if (row.IsNewRow)
                {
                    continue;
                }

                int maSach = Convert.ToInt32(row.Cells["MaSach"].Value);
                object cellValue = row.Cells["Chon"].Value;
                bool daChon = cellValue != null && Convert.ToBoolean(cellValue);

                if (daChon)
                {
                    maSachDangChon.Add(maSach);
                }
                else
                {
                    maSachDangChon.Remove(maSach);
                }
            }
        }

        private void KhoiPhucSachDangChon()
        {
            if (dgvSachCon.Columns["Chon"] == null || dgvSachCon.Columns["MaSach"] == null)
            {
                return;
            }

            foreach (DataGridViewRow row in dgvSachCon.Rows)
            {
                if (!row.IsNewRow)
                {
                    int maSach = Convert.ToInt32(row.Cells["MaSach"].Value);
                    row.Cells["Chon"].Value = maSachDangChon.Contains(maSach);
                }
            }
        }

        private void dgvSachCon_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            if (dgvSachCon.Columns[e.ColumnIndex].Name == "Chon")
            {
                // Commit edit ngay để checkbox phản hồi tức thì
                dgvSachCon.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void btnLapPhieu_Click(object sender, EventArgs e)
        {
            if (cboDocGia.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn độc giả.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Thu thập danh sách sách được chọn qua checkbox
            CapNhatSachDangChonTuGrid();
            List<int> danhSachMaSach = new List<int>(maSachDangChon);

            if (danhSachMaSach.Count == 0)
            {
                MessageBox.Show("Vui lòng đánh dấu chọn ít nhất một sách cần mượn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maDG = Convert.ToInt32(cboDocGia.SelectedValue);

            // Mượn từng cuốn sách và gom kết quả
            List<string> thanhCongList = new List<string>();
            List<string> thatBaiList = new List<string>();

            foreach (int maSach in danhSachMaSach)
            {
                string thongBao;
                bool thanhCong = phieuMuonBUS.LapPhieuMuon(maDG, maSach, out thongBao);
                if (thanhCong)
                {
                    thanhCongList.Add("Sách " + maSach + ": " + thongBao);
                }
                else
                {
                    thatBaiList.Add("Sách " + maSach + ": " + thongBao);
                }
            }

            string ketQua = "";
            if (thanhCongList.Count > 0)
            {
                ketQua += "Thành công (" + thanhCongList.Count + " sách):\n" + string.Join("\n", thanhCongList);
            }

            if (thatBaiList.Count > 0)
            {
                if (ketQua.Length > 0)
                {
                    ketQua += "\n\n";
                }

                ketQua += "Thất bại (" + thatBaiList.Count + " sách):\n" + string.Join("\n", thatBaiList);
            }

            MessageBoxIcon icon = thatBaiList.Count == 0 ? MessageBoxIcon.Information : MessageBoxIcon.Warning;
            MessageBox.Show(ketQua, "Kết quả lập phiếu mượn", MessageBoxButtons.OK, icon);

            TaiDuLieu();
        }

        private void DinhDangCot()
        {
            if (dgvSachCon.Columns.Count == 0)
            {
                return;
            }

            // Thêm cột checkbox "Chọn" nếu chưa có
            if (dgvSachCon.Columns["Chon"] == null)
            {
                DataGridViewCheckBoxColumn colChon = new DataGridViewCheckBoxColumn();
                colChon.Name = "Chon";
                colChon.HeaderText = "Chọn";
                colChon.Width = 50;
                colChon.FillWeight = 30;
                colChon.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                colChon.Width = 60;
                dgvSachCon.Columns.Insert(0, colChon);
            }

            // Đặt lại ReadOnly: tất cả cột đều readonly TRỪ cột Chọn
            dgvSachCon.ReadOnly = false;
            foreach (DataGridViewColumn col in dgvSachCon.Columns)
            {
                col.ReadOnly = col.Name != "Chon";
            }

            if (dgvSachCon.Columns["MaSachHienThi"] != null) dgvSachCon.Columns["MaSachHienThi"].HeaderText = "Mã sách";
            if (dgvSachCon.Columns["MaSach"] != null) dgvSachCon.Columns["MaSach"].Visible = false;
            if (dgvSachCon.Columns["TenSach"] != null) dgvSachCon.Columns["TenSach"].HeaderText = "Tên sách";
            if (dgvSachCon.Columns["TenTheLoai"] != null) dgvSachCon.Columns["TenTheLoai"].HeaderText = "Thể loại";
            if (dgvSachCon.Columns["TenTG"] != null) dgvSachCon.Columns["TenTG"].HeaderText = "Tác giả";
            if (dgvSachCon.Columns["NamXB"] != null) dgvSachCon.Columns["NamXB"].HeaderText = "Năm XB";
            if (dgvSachCon.Columns["NhaXB"] != null) dgvSachCon.Columns["NhaXB"].HeaderText = "Nhà XB";
            if (dgvSachCon.Columns["TriGia"] != null)
            {
                dgvSachCon.Columns["TriGia"].HeaderText = "Trị giá";
                dgvSachCon.Columns["TriGia"].DefaultCellStyle.Format = "N0";
            }
            if (dgvSachCon.Columns["SoLuongTon"] != null) dgvSachCon.Columns["SoLuongTon"].HeaderText = "Số lượng";
            if (dgvSachCon.Columns["TinhTrang"] != null) dgvSachCon.Columns["TinhTrang"].HeaderText = "Tình trạng";
            if (dgvSachCon.Columns["NgayNhap"] != null) dgvSachCon.Columns["NgayNhap"].HeaderText = "Ngày nhập";

            if (dgvSachCon.Columns["MaSachHienThi"] != null) dgvSachCon.Columns["MaSachHienThi"].FillWeight = 75;
            if (dgvSachCon.Columns["TenSach"] != null) dgvSachCon.Columns["TenSach"].FillWeight = 210;
            if (dgvSachCon.Columns["TenTheLoai"] != null) dgvSachCon.Columns["TenTheLoai"].FillWeight = 80;
            if (dgvSachCon.Columns["TenTG"] != null) dgvSachCon.Columns["TenTG"].FillWeight = 105;
            if (dgvSachCon.Columns["NamXB"] != null) dgvSachCon.Columns["NamXB"].FillWeight = 80;
            if (dgvSachCon.Columns["NhaXB"] != null) dgvSachCon.Columns["NhaXB"].FillWeight = 140;
            if (dgvSachCon.Columns["NgayNhap"] != null) dgvSachCon.Columns["NgayNhap"].FillWeight = 105;
            if (dgvSachCon.Columns["TriGia"] != null) dgvSachCon.Columns["TriGia"].FillWeight = 90;
            if (dgvSachCon.Columns["SoLuongTon"] != null) dgvSachCon.Columns["SoLuongTon"].FillWeight = 95;
            if (dgvSachCon.Columns["TinhTrang"] != null) dgvSachCon.Columns["TinhTrang"].FillWeight = 100;

            dgvSachCon.ClearSelection();
        }
    }
}
