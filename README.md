# Quản lý thư viện

Ứng dụng quản lý thư viện trên desktop được xây dựng bằng **C# WinForms** và **SQL Server**, phục vụ các nghiệp vụ cơ bản trong thư viện như quản lý sách, quản lý độc giả, mượn sách, trả sách, thu tiền phạt và xem báo cáo.

---
## Giới thiệu

Project được tổ chức theo mô hình 3 lớp:

- `GUI`: giao diện người dùng
- `BUS`: xử lý nghiệp vụ
- `DAL`: truy cập dữ liệu

Hệ thống hỗ trợ 2 vai trò chính:

- `Thủ thư`: thao tác quản lý và vận hành thư viện
- `Độc giả`: xem thông tin cá nhân và lịch sử mượn sách

## Chức năng chính

### Đối với thủ thư

- Đăng nhập hệ thống
- Tiếp nhận sách mới
- Lập thẻ độc giả
- Cho mượn sách
- Nhận trả sách
- Thu tiền phạt
- Tra cứu sách
- Xem báo cáo thống kê
- Thay đổi quy định hệ thống

### Đối với độc giả

- Đăng nhập hệ thống
- Xem thông tin cá nhân
- Xem lịch sử mượn sách
- Tra cứu sách

## Công nghệ sử dụng

- C# (.NET Framework 4.7.2)
- Windows Forms
- SQL Server
- ADO.NET (`System.Data.SqlClient`)

## Cấu trúc thư mục

```text
quanlythuvien/
|-- Database/
|   `-- 01_QuanLyThuVien.sql
|-- SourceCode/
|   `-- QuanLyThuVien/
|       |-- QuanLyThuVien.slnx
|       `-- QuanLyThuVien/
|           |-- GUI/
|           |-- BUS/
|           |-- DAL/
|           |-- DTO/
|           `-- UTILS/
|-- .gitignore
`-- README.md
```

## Cơ sở dữ liệu

Database mặc định là:

```text
QuanLyThuVien
```

Chuỗi kết nối hiện tại nằm trong file:

```text
SourceCode/QuanLyThuVien/QuanLyThuVien/App.config
```

Giá trị mặc định:

```xml
<add name="QuanLyThuVienConnection"
     connectionString="Data Source=.;Initial Catalog=QuanLyThuVien;Integrated Security=True"
     providerName="System.Data.SqlClient" />
```

Nếu máy bạn dùng SQL Server instance khác, hãy sửa lại `Data Source`.

## Cách chạy dự án

### 1. Tạo database

Mở **SQL Server Management Studio** và chạy file:

```text
Database/01_QuanLyThuVien.sql
```

Script này sẽ:

- Tạo database `QuanLyThuVien`
- Tạo bảng, khóa, ràng buộc và trigger
- Chèn dữ liệu mẫu ban đầu

### 2. Mở project

Bạn có thể mở một trong hai file sau bằng Visual Studio:

```text
SourceCode/QuanLyThuVien/QuanLyThuVien.slnx
```

hoặc:

```text
SourceCode/QuanLyThuVien/QuanLyThuVien/QuanLyThuVien.csproj
```

### 3. Chạy ứng dụng

- Build project
- Chạy ứng dụng
- Màn hình khởi động là `FormDangNhap`

## Tài khoản mẫu

Dữ liệu mẫu trong script SQL có sẵn các tài khoản sau:

### Độc giả

- `docgia01` / `123456`
- `docgia02` / `123456`

### Thủ thư

- `thuthu01` / `admin123`

## Một số quy tắc nghiệp vụ

- Độc giả phải còn hạn thẻ mới được mượn sách
- Độc giả đang còn nợ tiền phạt không được mượn thêm
- Độc giả không được vượt quá số sách mượn tối đa
- Sách trả trễ sẽ phát sinh tiền phạt
- Một số ràng buộc nghiệp vụ được kiểm soát bằng trigger ở SQL Server

## Ghi chú

- Project hiện phù hợp cho mục đích học tập, demo hoặc làm đồ án môn học
- Nếu đang mở file `.exe`, việc build lại có thể bị khóa file đầu ra; chỉ cần đóng ứng dụng rồi build lại
- Nếu bạn thay đổi schema database, nên kiểm tra lại các trigger và phần `DAL`
---
## Tác giả

**Họ và Tên:** Thái Công Anh Quốc\
**Gmail:** anhquoc212005@gmail.com
