use master
go
if exists(select * from sysdatabases where name = 'QLCAOOC')
drop database QLCAOOC
go
Create database QLCAOOC
go
use QLCAOOC
go
Create table [DIENTICH] (
	[MaDienTich] varchar(10) NOT NULL,
	[TenDienTich] Nvarchar(20) NOT NULL
Primary Key  ([MaDienTich])
) 
go
Create table [TANG] (
	[MaTang] varchar(10) NOT NULL,
	[TenTang] Nvarchar(20) NOT NULL,
Primary Key  ([MaTang])
) 
go
Create table [GIA] (
	[MaDienTich] varchar(10) NOT NULL,
	[MaTang] varchar(10) NOT NULL,
	[Gia] Integer NOT NULL,
Primary Key  ([MaDienTich], [MaTang])
) 
go

Create table [CANHO] (
	[MaCanHo] varchar(10) NOT NULL,
	[TinhTrang] Nvarchar(20) NOT NULL,
	[HinhAnh] varchar(30) NOT NULL,
	[GhiChu] Nvarchar(20) NULL,
	[MaDienTich] varchar(10) NOT NULL,
	[MaTang] varchar(10) NOT NULL,
Primary Key  ([MaCanHo])
) 
go


Create table [HOPDONG] (
	[MaHopDong] integer NOT NULL,
	[NgayBatDauThue] Date NOT NULL,
	[NgayHetHan] Date NOT NULL,
	[MaNK] integer NOT NULL,
	[MaCanHo] varchar(10) NOT NULL,
	[GiaPhong] Integer NOT NULL,
	[ThoiGianThue] Integer NOT NULL,
	[DaThanhToan] Integer NOT NULL,
Primary Key  ([MaHopDong])
) 
go

Create table [PHIEUTHUTIEN] (
	[MaPhieuThu] integer NOT NULL,
	[MaHopDong] integer NOT NULL,
	[DotThu] Integer NOT NULL,
	[NgayThu] Date NOT NULL,
	[SoThangThu] Integer NOT NULL,
	[SoTien] Integer NOT NULL,
Primary Key  ([MaPhieuThu])
) 
go

Create table [NHANKHAU] (
	[MaNK] integer NOT NULL,
	[HoTen] Nvarchar(20) NOT NULL,
	[CMND] Integer NOT NULL,
	[DienThoai] varchar(20) NOT NULL,
	[DiaChi] Nvarchar(30) NOT NULL,
	[HinhAnh] varchar(30) NOT NULL,
	[MaHopDong] integer NULL,
	[TenDN] varchar(20) NULL,
	[MatKhau] varchar(20) NULL,
Primary Key  ([MaNK])
) 

go

Create table [ADMIN] (
	[HoTen] Nvarchar(20) NULL,
	[TenDN] varchar(20) NOT NULL,
	[MatKhau] varchar(20) NOT NULL,
Primary Key  ([TenDN])
) 
go
Alter table [GIA] add  foreign key([MaDienTich]) references [DIENTICH] ([MaDienTich]) 
go
Alter table [GIA] add  foreign key([MaTang]) references [TANG] ([MaTang]) 
go
Alter table [CANHO] add  foreign key([MaDienTich], [MaTang]) references [GIA] ([MaDienTich], [MaTang])
go
Alter table [HOPDONG] add  foreign key([MaCanHo]) references [CANHO] ([MaCanHo]) 
go
Alter table [HOPDONG] add  foreign key([MaNK]) references [NHANKHAU] ([MaNK]) 
go
Alter table [PHIEUTHUTIEN] add  foreign key([MaHopDong]) references [HOPDONG] ([MaHopDong])
go
Alter table [NHANKHAU] add  foreign key([MaHopDong]) references [HOPDONG] ([MaHopDong])


INSERT INTO TANG
VALUES ('1', N'Tầng 1'), ('2', N'Tầng 2'), ('3', N'Tầng 3')


INSERT INTO DIENTICH
VALUES ('A', N'150 mét vuông'), ('B', N'100 mét vuông'), ('C', N'50 mét vuông')

INSERT INTO GIA
VALUES ('A', '1', 5000000)
INSERT INTO GIA
VALUES ('A', '2', 6000000)
INSERT INTO GIA
VALUES ('A', '3', 7000000)
INSERT INTO GIA
VALUES ('B', '1', 3000000)
INSERT INTO GIA
VALUES ('B', '2', 4000000)
INSERT INTO GIA
VALUES ('B', '3', 5000000)
INSERT INTO GIA
VALUES ('C', '1', 2000000)
INSERT INTO GIA
VALUES ('C', '2', 3000000)
INSERT INTO GIA
VALUES ('C', '3', 4000000)

INSERT INTO CANHO
VALUES ('A1', N'Còn trống', 'A1-3.jpg', '', 'A', '1')
INSERT INTO CANHO
VALUES ('A2', N'Còn trống', 'A2-3.jpg', '', 'A', '2')
INSERT INTO CANHO
VALUES ('A3', N'Còn trống', 'A3-3.jpg', '', 'A', '3')
INSERT INTO CANHO
VALUES ('B1', N'Còn trống', 'B1-3.jpg', '', 'B', '1')
INSERT INTO CANHO
VALUES ('B2', N'Còn trống', 'B2-3.jpg', '', 'B', '2')
INSERT INTO CANHO
VALUES ('B3', N'Còn trống', 'B3-3.jpg', '', 'B', '3')
INSERT INTO CANHO
VALUES ('C1', N'Còn trống', 'C1-3.jpg', '', 'C', '1')
INSERT INTO CANHO
VALUES ('C2', N'Còn trống', 'C2-3.jpg', '', 'C', '2')
INSERT INTO CANHO
VALUES ('C3', N'Còn trống', 'C3-3.jpg', '', 'C', '3')


INSERT INTO ADMIN
VALUES (N'ADMIN', 'admin', '123')



	INSERT INTO NHANKHAU
	VALUES (1, N'Nguyễn Kiên', '2141', '082814', 'HCM', 'hk1.jpg', null, 'kh1', 'kh1')

	
	INSERT INTO HOPDONG
	VALUES (1, '5/9/2018', '11/9/2018', 1, 'A2', 6000000, 6, 6)
	UPDATE CANHO
	SET TinhTrang = N'Đã thuê'
	where MaCanHo = 'A2'
	INSERT INTO PHIEUTHUTIEN
	VALUES (1, 1, 1, '5/9/2018', 6, 36000000)