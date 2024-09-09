
﻿

create database DataDoAnCoSo
set dateformat dmy
use DataDoAnCoSo
go

/*
dataFirst: từ table kh cần code
	- Quản lý Tài khoản
	- Quản lý Thuộc tính
	- Quản lý 
	- Quản lý Danh mục sản phẩm 
	- Quản lý khách hàng
	- Quản lý Vị trí
	- Quản lý chi tiết đơn hàng
	- Quản lý đơn hàng
	- QUản lý page
	- Quản lý sản phẩm
	- Quản lý shipper
	- Quản lý trạng thái
*/

-- Create table Vai Trò
Create table Roles
(
		RoleId			int					identity(1,1),
		RoleName		nvarchar(100)		null,
		DesRole			nvarchar(50)		null

		Constraint PK_Roles			primary key (RoleId)
)
go

select * from Roles

-- Create table Tài khoản
create table Accounts
(
	AccountId				int								Identity(1,1),
	UserNameAcc				nvarchar(50)					null,
	Phone					varchar(50)						null,
	Email					varchar(50)						null,
	LastLogin				datetime						null,
	RoleId					int								null,
	PasswordAcc				nvarchar(50)					null,
	Cate					nvarchar(10)					null,
	Active					bit								null,
	Province				nvarchar(50)					null

	constraint PK_Account			primary key (AccountId),
	constraint FK_Account_Roles		foreign key (RoleId) references  Roles(RoleId) 

)
go

select * from Accounts

ALTER TABLE Accounts
ALTER COLUMN Province nvarchar(50) null;


-- Create table Danh mục sản phẩm
Create table Category
(
	  CatId			int					identity(1,1),
	  CatName		nvarchar(50)		null,
	  Ordering		int					null,
	  ParentID		int					null,
	  Levels		int					null,
	  Published		bit					null


	  constraint PK_Category primary key (CatId)
)
GO

ALTER TABLE Category
ALTER COLUMN ParentID int null;

ALTER TABLE Category
ALTER COLUMN Levels int null;


select * from Category


SET IDENTITY_INSERT Category ON;
SET IDENTITY_INSERT Category Off;



insert into Category (CatId, CatName, Ordering, ParentID, Levels, Published) values 
(1, N'Phụ Kiện Dàn Đầu', 1, 1, 2, 1 ),
(2, N'Phụ Kiện Dàn Áo', 2, 1, 2, 1 ),
(3, N'Phụ Kiện Dàn Chân', 3, 1, 2, 1 ),
(4, N'Phụ Kiện Dàn Máy', 4, 1, 2, 1 ),
(5, N'Phụ Kiện Khác', 5, 1, 2, 1 )




(1, N'Phụ Kiện Dàn Đầu', 1, NULL, 0, 1 ),
(2, N'Đầu Đèn - Đồng Hồ - Phụ Kiện', 1, 1, 1, 1 ),
(3, N'Tay Phanh - Tay Côn', 1, 1, 1, 1 ),

(4, N'Phụ Kiện Dàn Áo', 1, NULL, 0, 1 ),
(5, N'Dàn Nhựa', 2, 1, 1, 1 ),
(6, N'Dè Trước - Dè Con - Mỏ cày - Carte', 2, 1, 1, 1 ),

(7, N'Phụ Kiện Dàn Chân', 3, Null, 0, 1 ),
(8, N'Mâm Xe', 3, 1, 1, 1 ),
(9, N'Phuộc Xe', 3, 1, 1, 1 ),

(10, N'Phụ Kiện Dàn Máy', 4, NULL, 0, 1 ),
(11, N'Bộ nồi tay ga', 4, 1, 1, 1 ),
(12, N'Pô & Cổ Pô - Phụ kiện', 4, 1, 1, 1 ),

(13, N'Phụ Kiện Khác', 5, NULL, 0, 1 ),
(14, N'Ốc - Tán - Phụ Kiện', 5, 1, 1, 1 )


DELETE FROM Category;

TRUNCATE TABLE Category;

select * from Category 

--DELETE: Xóa từng hàng một, giữ lại cấu trúc bảng.
--TRUNCATE: Xóa nhanh tất cả dữ liệu, giữ lại cấu trúc bảng và đặt lại giá trị IDENTITY.
--DROP: Xóa toàn bộ bảng cùng với cấu trúc và dữ liệu.


-- Create table Khách hàng
Create table Customer
(
	CusId				int				identity(1,1),
	CusName				varchar(50)		null,
	CusPassword			varchar(255)	null,
	CusEmail			varchar(50)		null,
	Address				nvarchar(250)	null,
	Phone				int				null,
	LocationId			int				null,
	CreateDate			datetime		null,
	LastLogin			datetime		null,
	Avatar				nvarchar(250)	null,
	Active				bit				null

	constraint PK_Customer	primary key (CusId)

)
GO

select * from Customer

-- Create table Địa phương
create table Locations
(
	LocationId			int				identity(1,1),
	Name				nvarchar(100)	null,

	constraint PK_Location		primary key(LocationId)
)

select * from Locations

ALTER TABLE Locations
ALTER COLUMN Name int null;



--Create table Trạng thái đơn hàng
Create table TransactStatus
(
		StatusId		int					identity(1,1),
		Status			nvarchar(100)		null,
		DesStatus		nvarchar(max)		null

		constraint PK_Status		primary key (StatusId)
)
go

select * from TransactStatus


-- Create table Đơn hàng - Chi Tiết thanh Toán
create table Orders
(
	OrderId						int								identity(1,1),
	CusId						int								null,
	OderDate					datetime						null,				-- Ngày đặt hàng
	ShipDate					datetime						null,				-- Ngày Giao hàng cho khách hàng
	StatusId					int								null,			-- Trạng thái đơn hàng
	Paid						bit								null,			-- Trạng thái thanh toán hay chưa
	PaymentDate					datetime						null,			-- Ngày giờ thanh toán
	PaymentId					int								null,			-- Mã thanh toán
	PaymentType					nvarchar(50)					null,			-- Loại thanh toán
	Note						nvarchar(max)					null,
	Quantity					int								null,
	CustomerName				nvarchar(100)					null,
	CustomerPhone				varchar(11)						null,
	CustomerEmail				nvarchar(100)					null,
	CustomerAddress				nvarchar(250)					null

	Constraint PK_Orders					primary key (OrderId),
	Constraint FK_Orders_Customer			foreign key (CusId) references Customer(CusId),
	Constraint FK_Orders_TransactStatus		foreign key (StatusId) references  TransactStatus(StatusId)
)
go

select * from Orders

ALTER TABLE Orders
ALTER COLUMN OderDate date null;

ALTER TABLE Orders
ALTER COLUMN ShipDate date null;


ALTER TABLE Orders
ALTER COLUMN Note nvarchar(max) null;


Create table Products
(
	  ProId				int					 identity(1,1),
	  ProName			nvarchar(50)		 null,
	  ProImage			nvarchar(100)		 null,
	  ProPrice			decimal(18,2)		 null,
	  ShortDes			nvarchar(50)		 null,
	  CatId				int					 null,
	  DateCreated		datetime			 null,
	  DateModified		datetime			 null,
	  Active			bit					 null,
	  MetaDesc			nvarchar(100)		 null,
	  MeetaKey			nvarchar(100)		 null,
	  Quantity			int					 null,
	  UnitlnStock		int					 null

		constraint PK_Products				primary key (ProId, CatId)
		
)
GO

SET IDENTITY_INSERT Products ON; 
SET IDENTITY_INSERT Products OFF;
SET IDENTITY_INSERT AnotherTable OFF;

select * from Products

update Products set UnitlnStock = '2' where ProId = 1

select * from Category

Delete from Products

ALTER TABLE Products 


drop constraint PK_Products	

ALTER COLUMN ProName nvarchar(100) null;

ALTER TABLE Products
ALTER COLUMN UnitlnStock int null;


INSERT INTO Products (ProId, ProName, ProImage, ProPrice, ShortDes, CatId, DateCreated, DateModified, Active, MetaDesc, MeetaKey, Quantity, UnitlnStock)
VALUES 
-- Loại 1
(1, N'Chigee AIO-5 Lite - CAM HÀNH TRÌNH VÀ Dàn ĐƯỜNG', N'img1-01', 9550000, N'Dàn Đầu', 1, '27/08/2024', '', 1, N'Dàn Đầu', N'Đầu Xe', 10, 0),
(2, N'Evotech - Bảo vệ choá Triumph Speed 400', N'img1-01', 2599000, N'Dàn Đầu', 1, '28/08/2024', '28/08/2024', 1, N'Dàn Đầu', N'Đầu Xe', 10, 0),
(3, N'Koso USA - Đồng hồ điện tử RX3 Honda Monkey 125', N'img3-01', 14500000, N'Dàn Đầu', 1, '28/08/2024', '28/08/2024', 1, N'Dàn Đầu', N'Đầu Xe', 10, 0),
(4, N'RG Racing - Bảo vệ chóa đèn cho BMW R1200GS R1250GS', N'img4-01', 3150000, N'Dàn Đầu', 1, '28/08/2024', '28/08/2024', 1, N'Dàn Đầu', N'Đầu Xe', 10, 0),
(5, N'Brembo - Cùm côn dây billet', N'img5-01', 6499000, N'Dàn Đầu', 1, '28/08/2024', '28/08/2024', 1, N'Dàn Đầu', N'Đầu Xe', 10, 0),
(6, N'Brembo - Cùm côn thắng dầu billet Corsa Corta RR 16', N'img6-01', 17600000, N'Dàn Đầu', 1, '28/08/2024', '28/08/2024', 1, N'Dàn Đầu', N'Đầu Xe', 10, 0),
-- Loại 2
(7, N'CNC Racing - Cánh gió carbon Ducati Multistrada V4', N'img7-02', 17280000, N'Dàn Áo', 2, '28/08/2024', '28/08/2024', 1, N'Dàn Áo', N'Thân Xe', 10, 0),
(8, N'Oya - Che két Triumph Trident 660', N'img8-02', 2300000, N'Dàn Áo', 2, '28/08/2024', '28/08/2024', 1, N'Dàn Áo', N'Thân Xe', 10, 0),
(9, N'Puig - Cánh gió đầu đèn Yamaha MT09 2021+', N'img9-02', 4500000, N'Dàn Áo', 2, '28/08/2024', '28/08/2024', 1, N'Dàn Áo', N'Thân Xe', 10, 0),
(10, N'Puig - Cánh gió GP Yamaha R1 2020+', N'img10-02', 7250000, N'Dàn Áo', 2, '28/08/2024', '28/08/2024', 1, N'Dàn Áo', N'Thân Xe', 10, 0),
(11, N'Oya - Dè con + carte BMW S1000R 2021 S1000RR 2019+', N'img11-02', 6300000, N'Dàn Áo', 2, '28/08/2024', '28/08/2024', 1, N'Dàn Áo', N'Thân Xe', 10, 0),
(12, N'Oya - Dè sau bánh Triumph Trident 660', N'img12-02', 2550000, N'Dàn Áo', 2, '28/08/2024', '28/08/2024', 1, N'Dàn Áo', N'Thân Xe', 10, 0),
-- Loại 3
(13, N'GaleSpeed - Cặp mâm CNC Kawasaki Z400 Ninja 400', N'img13-03', 40000000, N'Dàn Chân', 3, '28/08/2024', '28/08/2024', 1, N'Dàn Chân', N'Đuôi Xe', 10, 0),
(14, N'GaleSpeed - Cặp mâm CNC Moto 3 vừa Exicter 150', N'img14-03', 45000000, N'Dàn Chân', 3, '28/08/2024', '28/08/2024', 1, N'Dàn Chân', N'Đuôi Xe', 10, 0),
(15, N'MOS - mâm CNC cho Vespa Sprint Premavera 125-150', N'img15-03', 27700000, N'Dàn Chân', 3, '28/08/2024', '28/08/2024', 1, N'Dàn Chân', N'Đuôi Xe', 10, 0),
(16, N'OZ - Mâm nhôm cho BMW S1000R 21+S1000RR 2019', N'img16-03', 78000000, N'Dàn Chân', 3, '28/08/2024', '28/08/2024', 1, N'Dàn Chân', N'Đuôi Xe', 10, 0),
(17, N'Ohlins - Phuộc sau TTX BMW S1000RR 2019+  S1000R', N'img17-03', 38000000, N'Dàn Chân', 3, '28/08/2024', '28/08/2024', 1, N'Dàn Chân', N'Đuôi Xe', 10, 0),
(18, N'Ohlins - Phuộc sau Triumph Trident 660 ( TR 135)', N'img18-03', 20000000, N'Dàn Chân', 3, '28/08/2024', '28/08/2024', 1, N'Dàn Chân', N'Đuôi Xe', 10, 0),
-- Loại 4
(19, N'Malossi - Bi nồi 19gr Honda SH300i Ø 23X18 GR.19', N'img19-04', 3000000, N'Dàn Máy', 4, '28/08/2024', '28/08/2024', 1, N'Dàn Máy', N'Máy Xe', 10, 0),
(20, N'Malossi - Dây curoa Aprilia SRGT 200', N'img20-04', 1799000, N'Dàn Máy', 4, '28/08/2024', '28/08/2024', 1, N'Dàn Máy', N'Máy Xe', 10, 0),
(21, N'Malossi - Đế quạt gió SRGT200 Vespa 150', N'img21-04', 1399000, N'Dàn Máy', 4, '28/08/2024', '28/08/2024', 1, N'Dàn Máy', N'Máy Xe', 10, 0),
(22, N'Malossi - Nồi trước BMW C400X GT', N'img22-04', 5199000, N'Dàn Máy', 4, '28/08/2024', '28/08/2024', 1, N'Dàn Máy', N'Máy Xe', 10, 0),
(23, N'Arrow - pô slip on Yamaha Xmax 300', N'img23-04', 12300000, N'Dàn Máy', 4, '28/08/2024', '28/08/2024', 1, N'Dàn Máy', N'Máy Xe', 10, 0),
(24, N'Giannelli - Pô full system cho Vespa 300', N'img24-04', 10000000, N'Dàn Máy', 4, '28/08/2024', '28/08/2024', 1, N'Dàn Máy', N'Máy Xe', 10, 0),
-- Loại 5
(25, N'CNC Racing - Ốc lỗ gác chân Hyper 950-821-939-Scrambler', N'img25-05', 1979000, N'Phụ Kiện Ngoài', 5, '28/08/2024', '28/08/2024', 1, N'Phụ Kiện Ngoài', N'Phụ Kiện Đẹp', 10, 0),
(26, N'Ốc titanium GR5 10li bắt heo đầu trụ', N'img26-05', 1979000, N'Phụ Kiện Ngoài', 5, '28/08/2024', '28/08/2024', 1, N'Phụ Kiện Ngoài', N'Phụ Kiện Đẹp', 10, 0),
(27, N'Ốc titanium GR5 10li nhìu size', N'img27-05', 42000, N'Phụ Kiện Ngoài', 5, '28/08/2024', '28/08/2024', 1, N'Phụ Kiện Ngoài', N'Phụ Kiện Đẹp', 10, 0),
(28, N'Puig - Bộ ốc kính chắn gió', N'img28-05', 330000, N'Phụ Kiện Ngoài', 5, '28/08/2024', '28/08/2024', 1, N'Phụ Kiện Ngoài', N'Phụ Kiện Đẹp', 10, 0)




alter table Products
add UnitlnStock int null


-- Create table Ordertail - Giỏ hàng
Create table OrderDetail
(		  	
		  OrderDetailId				int								identity(1,1),
		  OrderId					int								null,
		  ProId						int								null,
		  ProName					nvarchar(50)					null,
		  ProImage					nvarchar(100)					null,
		  OrderDate					datetime						null,
		  PaymentType				nvarchar(50)					null,
		  StatusOrderDetail			nvarchar(50)					null,
		  Price						float							null,
		  Quantity					int								null,
		  Total						float							null
		  

		  constraint PK_OrderDetail					Primary key (OrderDetailId)
)
GO


-- Create table người giao hàng

Create table Shipper
(
		ShipperId		int					identity(1,1),
		ShipperName		nvarchar(50)		null,
		Phone			nchar(10)			null,
		Company			nvarchar(50)		null,
		ShipDate		datetime			null


		constraint Pk_Shipper primary key (ShipperId)
)
go


select * from Accounts
