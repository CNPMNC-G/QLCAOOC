use master
go
if exists(select * from sysdatabases where name = 'QLCAOOC')
drop database QLCAOOC
go
Create database QLCAOOC
go
use QLCAOOC
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
	[MaHopDong] varchar(10) NOT NULL,
	[NgayBatDauThue] Date NOT NULL,
	[MaNK] varchar(10) NOT NULL,
	[MaCanHo] varchar(10) NOT NULL,
	[GiaPhong] Integer NOT NULL,
	[ThoiGianThue] Integer NOT NULL,
Primary Key  ([MaHopDong])
) 
go

Create table [PHIEUTHUTIEN] (
	[MaPhieuThu] varchar(10) NOT NULL,
	[MaHopDong] varchar(10) NOT NULL,
	[DotThu] Integer NOT NULL,
	[SoTien] Integer NOT NULL,
Primary Key  ([MaPhieuThu])
) 
go

Create table [NHANKHAU] (
	[MaNK] varchar(10) NOT NULL,
	[HoTen] Nvarchar(20) NOT NULL,
	[CMND] Integer NOT NULL,
	[DienThoai] varchar(20) NOT NULL,
	[DiaChi] Nvarchar(30) NOT NULL,
	[HinhAnh] varchar(30) NOT NULL,
	[MaHopDong] varchar(10) NULL,
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

Create table [THAMSO] (
	[MaThamSo] varchar(10) NOT NULL,
	[GiaTri] varchar(10) NULL,
	[GhiChu] Nvarchar(40) NULL,
Primary Key  ([MaThamSo])
) 
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
VALUES ('A1', N'Còn trống', 'CHA1.JPG', '', 'A', '1')
INSERT INTO CANHO
VALUES ('A2', N'Còn trống', 'CHA2.JPG', '', 'A', '2')
INSERT INTO CANHO
VALUES ('A3', N'Còn trống', 'CHA3.JPG', '', 'A', '3')
INSERT INTO CANHO
VALUES ('B1', N'Còn trống', 'CHB1.JPG', '', 'B', '1')
INSERT INTO CANHO
VALUES ('B2', N'Còn trống', 'CHB2.JPG', '', 'B', '2')
INSERT INTO CANHO
VALUES ('B3', N'Còn trống', 'CHB3.JPG', '', 'B', '3')
INSERT INTO CANHO
VALUES ('C1', N'Còn trống', 'CHC1.JPG', '', 'C', '1')
INSERT INTO CANHO
VALUES ('C2', N'Còn trống', 'CHC2.JPG', '', 'C', '2')
INSERT INTO CANHO
VALUES ('C3', N'Còn trống', 'CHC3.JPG', '', 'C', '3')