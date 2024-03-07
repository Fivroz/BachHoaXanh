Use BACHHOAXANH
Go
CREATE TRIGGER KIEMTRA_HOADON
ON DonDatHang
AFTER INSERT, UPDATE
AS
BEGIN
	DECLARE @MAKH INT, @DIEM MONEY, @TONGDIEM MONEY, @NGAY DATE, @Check Nvarchar(20)
	SELECT @DIEM = TongTien, @MAKH = MaKhachHang ,@check = TrangThaiDonHang FROM inserted

	If(@Check = N'Hủy đơn hàng')
	Begin
		UPDATE KhachHang
		SET DiemTichLuy = DiemTichLuy - @DIEM
		WHERE MaKhachHang = @MAKH
	end
	Else
	begin

	UPDATE KhachHang
	SET DiemTichLuy = DiemTichLuy + @DIEM
	WHERE MaKhachHang = @MAKH

	SELECT @TONGDIEM = DiemTichLuy FROM KhachHang WHERE MaKhachHang = @MAKH

	DECLARE @DIEMTV MONEY, @DIEMTT MONEY, @DIEMVIP MONEY;
	SELECT @DIEMTV = DiemQuyDinh FROM HangKhachHang WHERE TenHangKhachHang = N'Đồng'
	SELECT @DIEMTT = DiemQuyDinh FROM HangKhachHang WHERE TenHangKhachHang = N'Bạc'
	SELECT @DIEMVIP = DiemQuyDinh FROM HangKhachHang WHERE TenHangKhachHang = N'Vàng'

	IF (@TONGDIEM >= @DIEMVIP)
		BEGIN
		UPDATE KhachHang
		SET HangKhachHang = (select MaHangKhachHang from HangKhachHang where TenHangKhachHang=N'Vàng')
		WHERE MaKhachHang = @MAKH
		END
	ELSE 
	IF (@TONGDIEM < @DIEMVIP AND @TONGDIEM>=@DIEMTT)
		BEGIN
		UPDATE KHACHHANG
		SET HangKhachHang =  (select MaHangKhachHang from HangKhachHang where TenHangKhachHang=N'Bạc')
		WHERE MaKhachHang = @MAKH
		END
	end
END

Go
CREATE TRIGGER CHECK_KH
ON KHACHHANG
AFTER INSERT, UPDATE
AS
BEGIN
	DECLARE @TONGDIEM INT,@MAKH INT
	SELECT @TONGDIEM = DiemTichLuy,@MAKH = MaKhachHang FROM inserted 

	DECLARE @DIEMTV MONEY, @DIEMTT MONEY, @DIEMVIP MONEY;
	SELECT @DIEMTV = DiemQuyDinh FROM HangKhachHang WHERE TenHangKhachHang = N'Đồng'
	SELECT @DIEMTT = DiemQuyDinh FROM HangKhachHang WHERE TenHangKhachHang = N'Bạc'
	SELECT @DIEMVIP = DiemQuyDinh FROM HangKhachHang WHERE TenHangKhachHang = N'Vàng'

	IF (@TONGDIEM >= @DIEMVIP)
		BEGIN
		UPDATE KhachHang
		SET HangKhachHang = (select MaHangKhachHang from HangKhachHang where TenHangKhachHang=N'Vàng')
		WHERE MaKhachHang = @MAKH
		END
	ELSE 
	begin
	IF (@TONGDIEM < @DIEMVIP AND @TONGDIEM>=@DIEMTT)
		BEGIN
		UPDATE KHACHHANG
		SET HangKhachHang =  (select MaHangKhachHang from HangKhachHang where TenHangKhachHang=N'Bạc')
		WHERE MaKhachHang = @MAKH
		END
		

	IF (@TONGDIEM < @DIEMTT)
		BEGIN
		UPDATE KHACHHANG
		SET HangKhachHang =  (select MaHangKhachHang from HangKhachHang where TenHangKhachHang=N'Đồng')
		WHERE MaKhachHang = @MAKH
		END
	end
END

GO
CREATE TRIGGER KIEMTRASL_HD
ON ChiTietDatHang
AFTER INSERT, UPDATE
AS
BEGIN
	DECLARE @MAHD INT, @SOLUONG INT, @MASP INT;
		
	SELECT @MAHD = DonDatHang.MaDonHang,  @SOLUONG = inserted.SoLuong, @MASP = inserted.MaSanPham 
	FROM DonDatHang JOIN inserted ON inserted.MaDonHang = DonDatHang.MaDonHang

	IF @SOLUONG <= (SELECT SoLuongTon FROM SanPham WHERE MaSanPham=@MASP)
	BEGIN 
		UPDATE SanPham
		SET SoLuongTon = SoLuongTon - @SOLUONG
		WHERE MaSanPham = @MASP
			
		IF  (select SoLuongTon from SanPham where MaSanPham = @MASP) = 0
		Begin
			Update SanPham
			set TrangThai = N'Hết hàng'
			where MaSanPham = @MASP
		End
	END
	ELSE
		BEGIN
		ROLLBACK TRANSACTION 
		RAISERROR(N'Số lượng sản phẩm không đủ!',16,1)
		END
END

Go
CREATE TRIGGER UpdateSLT
ON SanPham
AFTER INSERT, UPDATE
AS
BEGIN

	declare @MASP INT, @SLT INT
	SELECT @MASP = MaSanPham, @SLT=SoLuongTon FROM inserted 
	  
	IF(@SLT = 0)
	BEGIN
	UPDATE SANPHAM SET TrangThai = N'Hết hàng'
	WHERE MaSanPham = @MASP
	END

END;

Go
CREATE TRIGGER HoanLaiSoLuongTon
ON DonDatHang
AFTER INSERT, UPDATE
AS
BEGIN
	IF EXISTS (SELECT * FROM inserted WHERE TrangThaiDonHang = N'Hủy Đơn hàng')
	BEGIN
		-- Cập nhật lại số lượng tồn cho sản phẩm từ chi tiết đơn hàng
		UPDATE SanPham
		SET SoLuongTon = SoLuongTon + cth.SoLuong
		FROM SanPham sp
		INNER JOIN ChiTietDatHang cth ON sp.MaSanPham = cth.MaSanPham
		INNER JOIN inserted i ON cth.MaDonHang = i.MaDonHang
		WHERE i.TrangThaiDonHang = N'Hủy Đơn hàng'
	END
END;
GO
CREATE TRIGGER KiemTraNhapKho
ON CTPhieuNhapKho
AFTER INSERT,UPDATE
AS
BEGIN
	DECLARE @TT NVARCHAR(20)
	SELECT @TT= TRANGTHAI FROM inserted JOIN PhieuNhapKho ON inserted.MaPhieuNhap = PhieuNhapKho.MaPhieuNhap
	IF(@TT = N'Đã nhập hàng')
	BEGIN
		UPDATE sp
		SET sp.SoLuongTon = sp.SoLuongTon + ctpnk.SoLuong
		FROM SanPham sp JOIN CTPhieuNhapKho ctpnk ON sp.MaSanPham = ctpnk.MaSanPham 
										  JOIN inserted i ON ctpnk.MaPhieuNhap = i.MaPhieuNhap
	END
END
