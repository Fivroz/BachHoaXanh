CREATE DATABASE BACHHOAXANH;
GO

USE BACHHOAXANH;
GO

CREATE TABLE VaiTroNhanVien (
  MaVaiTro SMALLINT PRIMARY KEY IDENTITY(1,1),
  TenVaiTro VARCHAR(10) NOT NULL CHECK (TenVaiTro IN ('NVKD','NVIT','NVGH','NVK'))
);

CREATE TABLE HangKhachHang (
  MaHangKhachHang CHAR(4) PRIMARY KEY,
  TenHangKhachHang NVARCHAR(30) NOT NULL CHECK (TenHangKhachHang IN (N'Vàng',N'Bạc',N'Đồng')),
  ChietKhau FLOAT NOT NULL CHECK (ChietKhau IN (1, 0.5, 0)),
  DiemQuyDinh MONEY NOT NULL
);

CREATE TABLE PhuongThucThanhToan (
  MaPhuongThuc SMALLINT PRIMARY KEY IDENTITY(1,1),
  TenPhuongThuc NVARCHAR(30) NOT NULL CHECK (TenPhuongThuc IN (N'Tiền mặt', N'Chuyển khoản', N'Ví điện tử'))
);

CREATE TABLE PhiGiaoHang (
  MaPhi SMALLINT PRIMARY KEY IDENTITY(1,1),
  GiaPhi MONEY NOT NULL,
  CanNang INT NOT NULL
);

CREATE TABLE ThuongHieu (
  MaThuongHieu SMALLINT PRIMARY KEY IDENTITY(1,1),
  HinhThuongHieu VARCHAR(255) NOT NULL,
  TenThuongHieu NVARCHAR(100) NOT NULL
);

CREATE TABLE LoaiDanhMuc (
  MaLoaiDanhMuc SMALLINT PRIMARY KEY IDENTITY(1,1),
  TenLoaiDanhMuc NVARCHAR(50) NOT NULL
);

CREATE TABLE DanhMuc (
  MaDanhMuc SMALLINT PRIMARY KEY IDENTITY(1,1),
  TenDanhMuc NVARCHAR(30) NOT NULL,
  MaLoaiDanhMuc SMALLINT NOT NULL,
  FOREIGN KEY (MaLoaiDanhMuc) REFERENCES LoaiDanhMuc(MaLoaiDanhMuc)
);

CREATE TABLE KhachHang (
  MaKhachHang INT PRIMARY KEY IDENTITY(1,1),
  HoTen NVARCHAR(50) NOT NULL,
  GioiTinh BIT NOT NULL, --1: nam, 0: nữ
  SDT CHAR(10) NOT NULL UNIQUE,
  DiaChi NVARCHAR(50) NOT NULL,
  MatKhau VARCHAR(30) NOT NULL,
  TrangThai BIT DEFAULT 0,
  HangKhachHang CHAR(4) FOREIGN KEY (HangKhachHang) REFERENCES HangKhachHang(MaHangKhachHang) DEFAULT 'TV',
  DiemTichLuy FLOAT DEFAULT 0
);

CREATE TABLE NhanVien (
  MaNhanVien INT PRIMARY KEY IDENTITY(1,1),
  HoTen NVARCHAR(50) NOT NULL,
  GioiTinh BIT NOT NULL,
  SDT CHAR(10) NOT NULL UNIQUE,
  DiaChi NVARCHAR(50) NOT NULL,
  MatKhau VARCHAR(30) NOT NULL,
  VaiTro SMALLINT NOT NULL FOREIGN KEY (VaiTro) REFERENCES VaiTroNhanVien(MaVaiTro),
  TrangThai BIT DEFAULT 0
);
CREATE TABLE SanPham (
  MaSanPham INT PRIMARY KEY IDENTITY(1,1),
  TenSanPham NVARCHAR(255) NOT NULL,
  HinhSanPham VARCHAR(255) NOT NULL,
  GiaNiemYet MONEY NOT NULL,
  GiamGia FLOAT CHECK (GiamGia >= 0 AND GiamGia < 1),
  KhoiLuong SMALLINT,
  DungTich SMALLINT,
  ThanhPhan NVARCHAR(255) NOT NULL,
  CachDung NVARCHAR(255) NOT NULL,
  BaoQuan NVARCHAR(255) NOT NULL,
  LuuY NVARCHAR(255),
  NoiSanXuat NVARCHAR(255) NOT NULL,
  MoTa NVARCHAR(255) NOT NULL,
  SoLuongTon SMALLINT NOT NULL,
  TrangThai NVARCHAR(20) CHECK (TrangThai IN (N'Đang bán',N'Hủy bán',N'Hết hàng')),
  MaDanhMuc SMALLINT NOT NULL,
  MaThuongHieu SMALLINT,
  FOREIGN KEY (MaDanhMuc) REFERENCES DanhMuc,
  FOREIGN KEY (MaThuongHieu) REFERENCES ThuongHieu(MaThuongHieu)
);
CREATE TABLE DanhGiaSanPham (
  MaDanhGia INT PRIMARY KEY IDENTITY(1,1),
  ThoiGian DATETIME NOT NULL,
  LuotThich SMALLINT,
  MaKhachHang INT NOT NULL,
  FOREIGN KEY (MaKhachHang) REFERENCES KhachHang(MaKhachHang)
 );
CREATE TABLE ChiTietDanhGia (
  MaDanhGia INT NOT NULL,
  MaSanPham INT NOT NULL,
  HinhDanhGia VARCHAR(255),
  SaoDanhGia SMALLINT NOT NULL,
  BinhLuan NVARCHAR(300) NOT NULL,
  PRIMARY KEY (MaDanhGia, MaSanPham),
  FOREIGN KEY (MaSanPham) REFERENCES SanPham(MaSanPham),
  FOREIGN KEY (MaDanhGia) REFERENCES DanhGiaSanPham(MaDanhGia)
);
CREATE TABLE Voucher (
  MaVoucher INT PRIMARY KEY IDENTITY(1,1),
  MaCode VARCHAR(10) UNIQUE NOT NULL,
  GiaTri MONEY NOT NULL,
  GiaTriType VARCHAR(10) NOT NULL,
  NgayHetHan DATETIME NOT NULL,
  TrangThai NVARCHAR(20) NOT NULL CHECK (TrangThai IN (N'Hết hạn',N'Đang mở',N'Đã khóa')),
  MoTa NVARCHAR(255) NOT NULL
);
CREATE TABLE Voucher_KhachHang(
	MaKhachHang int,
	MaVoucher int,
	SoLuong int,
	Primary key(MaKhachHang, MaVoucher),
	FOREIGN KEY (MaKhachHang) REFERENCES KhachHang(MaKhachHang),
	FOREIGN KEY (MaVoucher) REFERENCES Voucher(MaVoucher)
)
CREATE TABLE DonDatHang (
  MaDonHang INT PRIMARY KEY IDENTITY(1,1),
  NgayDat DATETIME NOT NULL,
  MaKhachHang INT NOT NULL,
  MaNhanVien INT,
  PhiGiaoHang SMALLINT NOT NULL,
  TongTien MONEY NOT NULL,
  PhuongThucThanhToan SMALLINT NOT NULL,
  TrangThaiDonHang NVARCHAR(20) NOT NULL CHECK (TrangThaiDonHang in(N'Đã xác nhận',N'Đang giao hàng',N'Đã giao hàng',N'Hủy Đơn hàng')),
  DiaChiNhanHang NVARCHAR(100) NOT NULL,
  SDTNhanHang CHAR(10) NOT NULL,
  Voucher int,
  FOREIGN KEY (Voucher) REFERENCES Voucher(MaVoucher),
  FOREIGN KEY (MaKhachHang) REFERENCES KhachHang(MaKhachHang),
  FOREIGN KEY (MaNhanVien) REFERENCES NhanVien(MaNhanVien),
  FOREIGN KEY (PhiGiaoHang) REFERENCES PhiGiaoHang(MaPhi),
  FOREIGN KEY (PhuongThucThanhToan) REFERENCES PhuongThucThanhToan(MaPhuongThuc)
);
CREATE TABLE ChiTietDatHang (
  MaDonHang INT NOT NULL,
  MaSanPham INT NOT NULL,
  SoLuong INT NOT NULL CHECK (SoLuong <= 50),
  ThanhTien MONEY NOT NULL,
  PRIMARY KEY (MaDonHang, MaSanPham),
  FOREIGN KEY (MaDonHang) REFERENCES DonDatHang(MaDonHang),
  FOREIGN KEY (MaSanPham) REFERENCES SanPham(MaSanPham)
);
CREATE TABLE NhaCungCap(
  MaNhaCC INT PRIMARY KEY IDENTITY(1,1),
  TenNhaCC NVARCHAR(255) NOT NULL,
  DiaChi NVARCHAR(255) NOT NULL,
  SDT CHAR(10) NOT NULL
);
CREATE TABLE PhieuNhapKho(
  MaPhieuNhap INT PRIMARY KEY IDENTITY(1,1),
  NhanVienTao Int NOT NULL,
  NhanVienNhap Int,
  MaNhaCC int,
  NgayTao DateTime NOT NULL,
  NgayNhap DATETIME,
  TrangThai NVARCHAR(20) NOT NULL CHECK (TrangThai in(N'Đã xác nhận',N'Đang nhập hàng',N'Đã nhập hàng',N'Hủy nhập hàng')),
  FOREIGN KEY(NHANVIENTAO) REFERENCES NHANVIEN(MANHANVIEN),
  FOREIGN KEY(NHANVIENNHAP) REFERENCES NHANVIEN(MANHANVIEN),
  FOREIGN KEY(MaNhaCC) REFERENCES NhaCungCap(MaNhaCC),
);
CREATE TABLE CTPhieuNhapKho(
  MaPhieuNhap INT, 
  MaSanPham INT,
  GiaNhap MONEY NOT NULL,
  SoLuong INT NOT NULL,
  PRIMARY KEY(MaPhieuNhap, MaSanPham),
  FOREIGN KEY (MaPhieuNhap) REFERENCES PhieuNhapKho(MaPhieuNhap),
  FOREIGN KEY (MaSanPham) REFERENCES SanPham(MaSanPham)
);
CREATE TABLE NhaCungCapSanPham (
  MaNhaCC INT, 
  MaSanPham INT,
  FOREIGN KEY (MaNhaCC) REFERENCES NhaCungCap(MaNhaCC),
  FOREIGN KEY (MaSanPham) REFERENCES SanPham(MaSanPham),
  PRIMARY KEY (MaNhaCC, MaSanPham)
);
CREATE TABLE ThongKeTheoNam (
  Nam SMALLINT NOT NULL PRIMARY KEY,
  TongSoLuongNam INT NOT NULL,
  TongDoanhThuNam MONEY NOT NULL
);

CREATE TABLE ThongKeTheoThang (
  Thang SMALLINT NOT NULL PRIMARY KEY,
  TongSoLuongThang SMALLINT NOT NULL,
  TongDoanhThuThang MONEY NOT NULL,
  Nam SMALLINT NOT NULL,
  FOREIGN KEY (Nam) REFERENCES ThongKeTheoNam(Nam)
); 

CREATE TABLE ChiTietThongKeThang (
  Thang SMALLINT NOT NULL,
  MaSanPham INT NOT NULL,
  SoLuong INT NOT NULL,
  DoanhThu MONEY NOT NULL,
  TyLeDoanhThu FLOAT NOT NULL,
  PRIMARY KEY (Thang, MaSanPham),
  FOREIGN KEY (MaSanPham) REFERENCES SanPham(MaSanPham),
  FOREIGN KEY (Thang) REFERENCES ThongKeTheoThang(Thang)
);





