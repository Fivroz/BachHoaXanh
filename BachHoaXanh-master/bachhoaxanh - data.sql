Use BACHHOAXANH
Go
CREATE PROC THEM_VaiTroNhanVien
@TenVaiTro VARCHAR(10)
AS
BEGIN
INSERT INTO VaiTroNhanVien(TenVaiTro) VALUES (@TenVaiTro)
END
GO
EXEC THEM_VaiTroNhanVien 'NVIT'
EXEC THEM_VaiTroNhanVien 'NVKD'
EXEC THEM_VaiTroNhanVien 'NVGH'
EXEC THEM_VaiTroNhanVien 'NVK'

GO
CREATE PROC THEM_HangKhachHang
(@MaHang Char(4),
@TenHangKhachHang NVARCHAR(30),
@ChietKhau FLOAT,
@DiemQuyDinh Money)
AS
BEGIN
INSERT INTO HangKhachHang(MaHangKhachHang,TenHangKhachHang,ChietKhau,DiemQuyDinh) VALUES (@MaHang,@TenHangKhachHang,@ChietKhau,@DiemQuyDinh)
END
GO
EXEC THEM_HangKhachHang 'VIP',N'Vàng',1,2000000
EXEC THEM_HangKhachHang 'TT',N'Bạc',0.5,1000000
EXEC THEM_HangKhachHang 'TV',N'Đồng',0,0.0

GO
CREATE PROC THEM_PhuongThucThanhToan
@TenPhuongThuc NVARCHAR(30)
AS
BEGIN
INSERT INTO PhuongThucThanhToan(TenPhuongThuc) VALUES (@TenPhuongThuc)
END
GO
EXEC THEM_PhuongThucThanhToan N'Tiền mặt'
EXEC THEM_PhuongThucThanhToan N'Chuyển khoản'
EXEC THEM_PhuongThucThanhToan N'Ví điện tử'

GO
insert into PhiGiaoHang values(15000,5000)
insert into PhiGiaoHang values ( 20000,10000)
insert into PhiGiaoHang values  (25000,15000)
insert into PhiGiaoHang values ( 30000,20000)
insert into PhiGiaoHang values ( 35000,30000)
insert into PhiGiaoHang values  (40000,40000)


GO
CREATE PROC THEM_LoaiDanhMuc
@TenLoaiDanhMuc NVARCHAR(50)
AS
BEGIN
INSERT INTO LoaiDanhMuc (TenLoaiDanhMuc) VALUES (@TenLoaiDanhMuc)
END
GO
EXEC THEM_LoaiDanhMuc N'Thịt, cá, trứng, hải sản'
EXEC THEM_LoaiDanhMuc N'Rau, củ, nấm, trái cây'
EXEC THEM_LoaiDanhMuc N'Dầu ăn, nước chấm, gia vị'
EXEC THEM_LoaiDanhMuc N'Kem, Thực phẩm đông mát'
EXEC THEM_LoaiDanhMuc N'Gạo , Bột, Đồ khô'
EXEC THEM_LoaiDanhMuc N'Mì, miến, cháo, phở'
EXEC THEM_LoaiDanhMuc N'Sữa các loại'
EXEC THEM_LoaiDanhMuc N'Bánh kẹo các loại'
EXEC THEM_LoaiDanhMuc N'Chăm sóc cá nhân'
EXEC THEM_LoaiDanhMuc N'Mẹ và bé'
EXEC THEM_LoaiDanhMuc N'Vệ sinh nhà cửa'
EXEC THEM_LoaiDanhMuc N'Đồ dùng gia đình'
EXEC THEM_LoaiDanhMuc N'Chăm sóc thú cưng'
EXEC THEM_LoaiDanhMuc N'Bia, nước giải khát'


GO
CREATE PROC THEM_DanhMuc
@TenDanhMuc NVARCHAR(30),
@MaLoaiDanhMuc SMALLINT
AS
BEGIN
INSERT INTO DanhMuc (TenDanhMuc,MaLoaiDanhMuc) VALUES (@TenDanhMuc,@MaLoaiDanhMuc)
END
Go
exec THEM_DanhMuc N'Thịt các loại',1
exec THEM_DanhMuc N'Cá, hải sản',1
exec THEM_DanhMuc N'Trứng gà, vịt, cút',1
exec THEM_DanhMuc N'Thực phẩm làm sẵn',1
exec THEM_DanhMuc N'Rau, củ, nấm các loại',2
exec THEM_DanhMuc N'Rau, củ sơ chế',2
exec THEM_DanhMuc N'Trái cây',2
exec THEM_DanhMuc N'Đồ chay các loại',2
exec THEM_DanhMuc N'Dầu ăn',3
exec THEM_DanhMuc N'Đường',3
exec THEM_DanhMuc N'Hạt nêm, bột ngọt, bột canh',3
exec THEM_DanhMuc N'Muối',3
exec THEM_DanhMuc N'Nước mắm',3
exec THEM_DanhMuc N'Nước tương',3
exec THEM_DanhMuc N'Tương ớt - đen, mayonnaise',3
exec THEM_DanhMuc N'Dầu hào, giấm, bơ',3
exec THEM_DanhMuc N'Tiêu, sa tế, ớt bột',3
exec THEM_DanhMuc N'Kem cây, kem hộp',4
exec THEM_DanhMuc N'Bánh đông, bánh tươi',4
exec THEM_DanhMuc N'Xúc xích, lạp xưởng, chả lụa',4
exec THEM_DanhMuc N'Chả giò, cá - bò viên',4
exec THEM_DanhMuc N'Há cảo, sủi cảo',4
exec THEM_DanhMuc N'Thịt, cá đông lạnh',4
exec THEM_DanhMuc N'Sữa chua, pho mai',4
exec THEM_DanhMuc N'Gạo các loại',5
exec THEM_DanhMuc N'Bột các loại',5
exec THEM_DanhMuc N'Đồ chay các loại',5
exec THEM_DanhMuc N'Đồ hộp các loại',5
exec THEM_DanhMuc N'Mì ăn liền',6
exec THEM_DanhMuc N'Hủ tiếu, miến',6
exec THEM_DanhMuc N'Phở, bún ăn liền',6
exec THEM_DanhMuc N'Cháo gói, cháo tươi',6
exec THEM_DanhMuc N'Bánh gạo Hàn Quốc',6
exec THEM_DanhMuc N'Mì, nui, bún khô',6
exec THEM_DanhMuc N'Sữa tươi',7
exec THEM_DanhMuc N'Sữa chua uống',7
exec THEM_DanhMuc N'Sữa hạt, sữa đậu',7
exec THEM_DanhMuc N'Sữa cacao, lúa mạch',7
exec THEM_DanhMuc N'Sữa đặc',7
exec THEM_DanhMuc N'Ngũ cốc',7
exec THEM_DanhMuc N'Sữa bột',7
exec THEM_DanhMuc N'Socola',8
exec THEM_DanhMuc N'Kẹo các loại',8
exec THEM_DanhMuc N'Bánh quy, bánh gạo',8
exec THEM_DanhMuc N'Bánh snack',8
exec THEM_DanhMuc N'Bánh mềm',8
exec THEM_DanhMuc N'Bánh xốp',8
exec THEM_DanhMuc N'Trái cây sấy',8
exec THEM_DanhMuc N'Dầu gội, dầu xã',9
exec THEM_DanhMuc N'Sữa tắm',9
exec THEM_DanhMuc N'Nước rửa tay',9
exec THEM_DanhMuc N'Khăn giấy vệ sinh',9
exec THEM_DanhMuc N'Tả cho bé',10
exec THEM_DanhMuc N'Sữa cho bé',10
exec THEM_DanhMuc N'Tắm gội cho bé',10
exec THEM_DanhMuc N'Giặt xã cho bé',10
exec THEM_DanhMuc N'Nước giặt, bột giặt',11
exec THEM_DanhMuc N'Nước xả',11
exec THEM_DanhMuc N'Nước rửa chén',11
exec THEM_DanhMuc N'Nước lau kính',11
exec THEM_DanhMuc N'Nước lau sàn',11
exec THEM_DanhMuc N'Túi đựng rác',12
exec THEM_DanhMuc N'Bọc thực phẩm',12
exec THEM_DanhMuc N'Tô, chén, dĩa',12
exec THEM_DanhMuc N'Đũa, muỗng',12
exec THEM_DanhMuc N'Nồi, niêu, xoong, chảo',12
exec THEM_DanhMuc N'Đồ dùng bếp',12
exec THEM_DanhMuc N'Thức ăn cho cho',13
exec THEM_DanhMuc N'Thức ăn cho mèo',13
exec THEM_DanhMuc N'Dầu gội chó mèo',13
exec THEM_DanhMuc N'Bia',14
exec THEM_DanhMuc N'Rượu',14
exec THEM_DanhMuc N'Nướt ngọt',14
exec THEM_DanhMuc N'Nước tăng lực',14
exec THEM_DanhMuc N'Nước khoáng',14
exec THEM_DanhMuc N'Nước trái cây',14
GO
INSERT INTO ThuongHieu VALUES
('vinamilk.jpg',N'VinaMilk'),
('thtruemilk.jpg',N'TH True Milk'),
('dutchlady.jpg',N'Dutch Lady'),
('nutimilk.jpg',N'Nuti Milk'),
('kun.jpg',N'Kun'),
('kitkat.jpg',N'Kit Kat'),
('nabati.jpg',N'Nabati'),
('cosy.jpg',N'Cosy'),
('oneone.jpg',N'One One'),
('vuagao.jpg',N'Vua Gạo'),
('neptune.jpg',N'Nep Tune'),
('lavie.jpg',N'Lavie'),
('aquafina.jpg',N'Aquafina'),
('3nfoods.jpg',N'3NFOODS'),
('333.jpg',N'Bia 333'),
('aan.jpg',N'AAN'),
('afc.jpg',N'AFC'),
('biaviet.jpg',N'Bia Việt'),
('blessyou.jpg',N'Bless You'),
('celano.jpg',N'Kem Celano'),
('clear.jpg',N'Clear'),
('cozy.jpg',N'Cozy'),
('cp.jpg',N'CP'),
('dalatmilk.jpg',N'Đà Lạt Mik'),
('dalattruemilk.jpg',N'Đà Lạt True Milk'),
('dasani.jpg',N'Dasani'),
('dove.jpg',N'Dove'),
('fujiwa.jpg',N'Fujiwa'),
('g7.jpg',N'G7'),
('gkitchen.jpg',N'GkitChen'),
('goute.jpg',N'Goute'),
('happyegg.jpg',N'HappyEgg'),
('headandshouder.jpg',N'HeadAndShouder'),
('heniken.jpg',N'Heniken'),
('highlandscoffe.jpg',N'HighlandsCoffee'),
('inexmilk.jpg',N'InexMilk'),
('ionlife.jpg',N'IonLife'),
('legoumet.jpg',N'Legoumet'),
('lenger.jpg',N'Lenger'),
('meadowfresh.jpg',N'Medowfresh'),
('merino.jpg',N'Merino'),
('nescafe.jpg',N'NesCafe'),
('nguyenho.jpg',N'Nguyên Hồ'),
('nhattam.jpg',N'Nhật Tâm'),
('nuvimilk.jpg',N'Nuvimilk'),
('oreo.jpg',N'Oreo'),
('premier.jpg',N'Premier'),
('puri.jpg',N'Puri'),
('qlegg.jpg',N'Qlegg'),
('romano.jpg',N'Romano'),
('saigon.jpg',N'Saigon'),
('sgfood.jpg',N'Sgfood'),
('songphuong.jpg',N'SongPhuong'),
('tempo.jpg',N'Tempo'),
('tiger.jpg',N'Tiger'),
('trungnguyen.jpg',N'Trung Nguyên'),
('vissan.jpg','Vissan'),
('vpmilk.jpg',N'VpMilk'),
('walls.jpg',N'Walls'),
('xmen.jpg',N'Xmen')

Go

INSERT INTO SanPham (TenSanPham, HinhSanPham, GiaNiemYet, GiamGia, KhoiLuong, DungTich, ThanhPhan, CachDung, BaoQuan, LuuY, NoiSanXuat, MoTa, SoLuongTon, TrangThai, MaDanhMuc, MaThuongHieu) VALUES 
(N'Sữa chua trái cây', 'sua-chua-trai-cay.jpg', 5000, 0.1, 20, 50, N'Trái cây tươi', N'Uống trực tiếp', N'Bảo quản lạnh', NULL, N'Việt Nam', N'Sữa chua trái cây ngon miệng, nuốt ực ực ngon ngon', 0, N'Hết hàng', 36, 1),
(N'Nước tăng lực Red Bull', 'redbull.jpg', 15000, 0.2, 100, 220, N'Nước tăng lực', N'Uống trực tiếp', N'Bảo quản nơi khô ráo', NULL, N'Việt Nam', N'Nước tăng lực Red Bull giúp tăng cường năng lượng và tỉnh táo', 100, N'Đang bán', 74, 1),
(N'Kẹo Chocolate KitKat', 'kitkat.jpg', 15000, 0.2, 20, NULL, N'Chocolate, bánh wafer', N'Ăn trực tiếp', N'Bảo quản nơi khô ráo, tránh ánh nắng trực tiếp', NULL, N'Nhật Bản', N'Kẹo Chocolate KitKat với hương vị thơm ngon và giòn tan', 100, N'Đang bán', 42, 2), 
(N'Sữa tươi Vinamilk', 'vinamilk.jpg', 20000, 0.1, 1000, 220, N'Sữa tươi', N'Uống trực tiếp', N'Bảo quản trong tủ lạnh từ 2-8°C', N'Chỉ sử dụng cho trẻ em trên 3 tuổi', N'Việt Nam', N'Sữa tươi Vinamilk chất lượng và bổ dưỡng', 200, N'Đang bán', 35, 1),
(N'Nước giải khát Coca-Cola', 'cocacola.jpg', 10000, 0.1, 330, 330, N'Nước uống có ga', N'Uống trực tiếp', N'Bảo quản nơi khô ráo, tránh ánh nắng trực tiếp', NULL, N'Mỹ', N'Nước giải khát Coca-Cola với hương vị ngọt ngào và sảng khoái', 300, N'Đang bán', 73, 2),
(N'Nước suối Lavie', 'lavie.jpg', 6000, 0.1, 200, 500, N'Nước khoáng', N'Uống trực tiếp', N'Bảo quản nơi khô ráo, tránh ánh nắng trực tiếp', NULL, N'Việt Nam', N'Nước suối Lavie chất lượng và sạch sẽ', 200, N'Đang bán', 75, 3),
(N'Chips Tỏi tây Lays', 'lays.jpg', 25000, 0.15, 100, NULL, N'Khoai tây, dầu thực vật, muối, tỏi tây', N'Ăn trực tiếp', N'Bảo quản nơi khô ráo, tránh ánh nắng trực tiếp', N'Chứa gluten', N'Việt Nam', N'Chips Tỏi tây Lays ngon và giòn', 150, N'Đang bán', 45, 6),
(N'Bánh sữa Hòa Phát', 'hoaphat.jpg', 20000, 0.1, 200, NULL, N'Bột mỳ, đường, sữa bột, dầu thực vật', N'Ăn trực tiếp', N'Bảo quản nơi khô ráo', NULL, N'Việt Nam', N'Bánh sữa Hòa Phát thơm ngon và bổ dưỡng', 100, N'Đang bán', 44, 7),
(N'Nước ép cam Trung Nguyên', 'trungnguyen.jpg', 30000, 0.2, 500, 1000, N'Cam tươi', N'Uống trực tiếp', N'Bảo quản trong ngăn mát tủ lạnh', N'Ngon nhất khi uống lạnh', N'Việt Nam', N'Nước ép cam Trung Nguyên tươi mát và thơm ngon', 100, N'Đang bán', 76, 8),
(N'Đùi bò nhập khẩu đông lạnh','thitbo.jpg',150000,0.1,500,null,N'Đùi bò Mỹ',N'Thịt đùi bò cơ bản đã được làm sạch, đóng khay, bạn chỉ cần rửa sạch lại và thái thành miếng vừa ăn, sau đó chế biến thành các món ăn yêu thích.',N'Thịt đùi bò nên được bảo quản trong ngăn mát tủ lạnh. Nếu đã rã đông thì không nên trữ đông lại.',
N'Sản phẩm nhận được có thể khác với hình ảnh về màu sắc và số lượng nhưng vẫn đảm bảo về mặt khối lượng và chất lượng.',
N'Mỹ',N'Các khoáng chất trong thịt bò giúp tổng hợp protein tốt hơn, chống oxy hóa, tăng cường hệ miễn dịch, tăng cường khả năng trao đổi chất insulin.',100,N'Đang bán',1,13),
(N'Cá diêu hồng làm sạch','cadieuhong.jpg',40000,0.05,450,null,N'Cá diêu hồng tươi sống',
N'Cá diêu hồng loại cá phổ biến có thịt nhiều, ít xương, thịt trắng, ngọt và lành tính, cá diêu hồng chế biến thành rất nhiều món ngon trong bữa cơm gia đình như cá diêu hồng kho, cá diêu hồng nấu canh chua, cá diêu hồng chiên giòn, cá diêu hồng sốt cà chua',N'Cá diêu hồng có thể bảo quản trong ngăn tủ lạnh để giúp cá tươi ngon hơn.',N'Sản phẩm nhận được có thể khác với hình ảnh về màu sắc và số lượng nhưng vẫn đảm bảo về mặt khối lượng và chất lượng.',N'Việt Nam',N'Dùng bổ dưỡng cho nhiều bệnh chứng suy nhược nhất là người già suy nhược, trẻ em còi cọc chậm lớn.',100,N'Đang bán',2,13)
Go

Insert into KhachHang (HoTen,GioiTinh,SDT,DiaChi,MatKhau,TrangThai)values (N'Đạt Võ',1,'0909075345',N'506/12 Bến Tre','dat123',0)
Insert into NhanVien values(N'Tôi Kinh Doanh',1,'0000000000',N'828 Sư Vạn Hạnh','admin123',2,0);
Insert into NhanVien values(N'Tôi IT',1,'1111111111',N'828 Sư Vạn Hạnh','admin123',1,0);
Insert into NhanVien values(N'Tôi Shipper',1,'2222222222',N'828 Sư Vạn Hạnh','admin123',3,0);
Insert into NhanVien values(N'Tôi Kho',1,'3333333333',N'828 Sư Vạn Hạnh','admin123',4,0);
