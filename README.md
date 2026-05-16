# Quản Lý Thư Viện

Ứng dụng desktop quản lý thư viện viết bằng C# WinForms và SQL Server, hỗ trợ đăng nhập theo vai trò, quản lý sách, độc giả, mượn trả sách, thu tiền phạt và xem báo cáo.

## Mục tiêu dự án

Dự án mô phỏng các nghiệp vụ cơ bản trong thư viện:

- Tiếp nhận sách mới
- Lập thẻ độc giả
- Cho mượn sách
- Nhận trả sách
- Thu tiền phạt
- Tra cứu sách
- Xem thông tin cá nhân
- Báo cáo và thay đổi quy định

## Công nghệ sử dụng

- C# .NET Framework 4.7.2
- Windows Forms
- SQL Server
- ADO.NET (`System.Data.SqlClient`)

## Cấu trúc thư mục

- `SourceCode/QuanLyThuVien/QuanLyThuVien.slnx`: solution chính
- `SourceCode/QuanLyThuVien/QuanLyThuVien/GUI`: giao diện người dùng
- `SourceCode/QuanLyThuVien/QuanLyThuVien/BUS`: tầng nghiệp vụ
- `SourceCode/QuanLyThuVien/QuanLyThuVien/DAL`: tầng truy cập dữ liệu
- `SourceCode/QuanLyThuVien/QuanLyThuVien/DTO`: các lớp dữ liệu
- `SourceCode/QuanLyThuVien/QuanLyThuVien/UTILS`: tiện ích dùng chung
- `Database/01_QuanLyThuVien.sql`: script tạo database, bảng, trigger và dữ liệu mẫu

## Chức năng chính

### 1. Đăng nhập

Ứng dụng khởi động từ màn hình đăng nhập `FormDangNhap`.

- Phân quyền theo vai trò `Thủ thư` và `Độc giả`
- Sau khi đăng nhập, giao diện menu chính thay đổi theo quyền truy cập

### 2. Quản lý sách

- Tiếp nhận sách mới
- Xem danh sách sách trong thư viện
- Theo dõi thể loại, tác giả, nhà xuất bản, trị giá, số lượng còn và tình trạng

### 3. Quản lý độc giả

- Lập thẻ độc giả
- Kiểm soát tuổi độc giả theo tham số hệ thống
- Tự sinh tài khoản đăng nhập cho độc giả

### 4. Mượn sách

- Chọn độc giả và sách còn trong kho
- Kiểm tra hạn thẻ, nợ phạt và số sách mượn tối đa
- Tự cập nhật phiếu mượn và tình trạng sách

### 5. Trả sách

- Chọn độc giả
- Hiển thị danh sách sách độc giả đang mượn
- Có thể chọn một hoặc nhiều cuốn để trả
- Tự tính tiền phạt trễ hạn và cập nhật công nợ

### 6. Thu tiền phạt

- Chọn độc giả đang còn nợ
- Nhập số tiền thu
- Tính số tiền còn lại sau khi thu

### 7. Tra cứu và báo cáo

- Tìm kiếm sách theo nhiều tiêu chí
- Xem báo cáo liên quan đến hoạt động thư viện

## Cơ sở dữ liệu

Database mặc định là `QuanLyThuVien`.

Chuỗi kết nối đang được cấu hình trong `SourceCode/QuanLyThuVien/QuanLyThuVien/App.config`:

```xml
<add name="QuanLyThuVienConnection"
     connectionString="Data Source=.;Initial Catalog=QuanLyThuVien;Integrated Security=True"
     providerName="System.Data.SqlClient" />
```

Nếu máy bạn dùng SQL Server instance khác, hãy sửa lại `Data Source`.

## Cách chạy dự án

### 1. Tạo database

Mở SQL Server Management Studio rồi chạy file:

```text
Database/01_QuanLyThuVien.sql
```

Script này sẽ:

- Tạo database `QuanLyThuVien`
- Tạo bảng, ràng buộc và trigger
- Chèn dữ liệu mẫu ban đầu

### 2. Mở source

Mở file:

```text
SourceCode/QuanLyThuVien/QuanLyThuVien.slnx
```

hoặc mở project:

```text
SourceCode/QuanLyThuVien/QuanLyThuVien/QuanLyThuVien.csproj
```

### 3. Chạy ứng dụng

- Build project bằng Visual Studio
- Chạy ứng dụng
- Màn hình đầu tiên là `FormDangNhap`

## Tài khoản mẫu

Theo dữ liệu mẫu trong script SQL:

- Độc giả 1:
  - Tên đăng nhập: `docgia01`
  - Mật khẩu: `123456`
- Độc giả 2:
  - Tên đăng nhập: `docgia02`
  - Mật khẩu: `123456`
- Thủ thư:
  - Tên đăng nhập: `thuthu01`
  - Mật khẩu: `admin123`

## Một số quy tắc nghiệp vụ đang áp dụng

- Độc giả phải còn hạn thẻ mới được mượn sách
- Độc giả còn nợ tiền phạt thì không được mượn thêm
- Độc giả không được vượt quá số sách mượn tối đa
- Sách trả trễ sẽ phát sinh tiền phạt theo tham số hệ thống
- Nhiều nghiệp vụ được kiểm soát bằng trigger trong SQL Server

## Ghi chú

- Ứng dụng dùng dữ liệu mẫu để phục vụ học tập và thử nghiệm
- Nếu thay đổi cấu trúc database, cần kiểm tra lại các trigger và tầng `DAL`
- Khi đang mở file `.exe`, đôi lúc build có thể bị khóa file đầu ra; chỉ cần đóng ứng dụng rồi build lại

## Tác giả

README này mô tả dự án dựa trên mã nguồn hiện có trong repository.
