USE [master]
GO
/****** Object:  Database [StationaryApp]    Script Date: 12/7/2024 4:38:26 PM ******/
CREATE DATABASE [StationaryApp]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'StationaryApp', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\StationaryApp.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'StationaryApp_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\StationaryApp_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [StationaryApp] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [StationaryApp].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [StationaryApp] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [StationaryApp] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [StationaryApp] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [StationaryApp] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [StationaryApp] SET ARITHABORT OFF 
GO
ALTER DATABASE [StationaryApp] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [StationaryApp] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [StationaryApp] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [StationaryApp] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [StationaryApp] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [StationaryApp] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [StationaryApp] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [StationaryApp] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [StationaryApp] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [StationaryApp] SET  DISABLE_BROKER 
GO
ALTER DATABASE [StationaryApp] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [StationaryApp] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [StationaryApp] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [StationaryApp] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [StationaryApp] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [StationaryApp] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [StationaryApp] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [StationaryApp] SET RECOVERY FULL 
GO
ALTER DATABASE [StationaryApp] SET  MULTI_USER 
GO
ALTER DATABASE [StationaryApp] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [StationaryApp] SET DB_CHAINING OFF 
GO
ALTER DATABASE [StationaryApp] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [StationaryApp] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [StationaryApp] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'StationaryApp', N'ON'
GO
ALTER DATABASE [StationaryApp] SET QUERY_STORE = OFF
GO
USE [StationaryApp]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
USE [StationaryApp]
GO
/****** Object:  Table [dbo].[addcategory]    Script Date: 12/7/2024 4:38:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[addcategory](
	[cat_id] [int] IDENTITY(1,1) NOT NULL,
	[cat_name] [varchar](50) NULL,
	[cat_desc] [varchar](50) NULL,
 CONSTRAINT [PK_addcategory] PRIMARY KEY CLUSTERED 
(
	[cat_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[product]    Script Date: 12/7/2024 4:38:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[product](
	[pro_id] [int] IDENTITY(1,1) NOT NULL,
	[pro_name] [varchar](50) NULL,
	[pro_desc] [varchar](50) NULL,
	[pro_price] [varchar](50) NULL,
	[pro_img] [varchar](50) NULL,
	[procatid_fk] [int] NULL,
	[availability] [varchar](255) NULL,
 CONSTRAINT [PK_product] PRIMARY KEY CLUSTERED 
(
	[pro_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[pro_details]    Script Date: 12/7/2024 4:38:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   VIEW [dbo].[pro_details] AS
SELECT TOP (1000)
      p.pro_id,
      p.pro_name,
      p.pro_desc,
      p.pro_price,
      p.pro_img,
      p.availability,  -- Add the new column here
      c.cat_name,
      c.cat_desc
FROM dbo.product AS p
JOIN dbo.addcategory AS c
    ON p.procatid_fk = c.cat_id;
GO
/****** Object:  Table [dbo].[whorder]    Script Date: 12/7/2024 4:38:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[whorder](
	[worder_id] [int] IDENTITY(1,1) NOT NULL,
	[wpro_idfk] [int] NULL,
	[catename] [varchar](50) NULL,
	[wproqty] [int] NULL,
	[wtotalprice] [varchar](50) NULL,
	[wstatus] [varchar](50) NULL,
	[whsaleremail] [varchar](50) NULL,
	[OrderDate] [datetime] NOT NULL,
 CONSTRAINT [PK_whorder] PRIMARY KEY CLUSTERED 
(
	[worder_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[whsalerorderlist]    Script Date: 12/7/2024 4:38:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  CREATE VIEW [dbo].[whsalerorderlist] AS
SELECT 
    w.[worder_id],
    w.[catename],
    w.[wproqty],
    w.[wtotalprice],
    w.[wstatus],
    w.[whsaleremail],
    p.[pro_name],
    p.[pro_desc],
    p.[pro_price],
    p.[pro_img]
FROM 
    [StationaryApp].[dbo].[whorder] w
JOIN 
    [StationaryApp].[dbo].[product] p
    ON w.[wpro_idfk] = p.[pro_id];
GO
/****** Object:  Table [dbo].[retailerorder]    Script Date: 12/7/2024 4:38:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[retailerorder](
	[rorder_id] [int] IDENTITY(1,1) NOT NULL,
	[rpro_idfk] [int] NULL,
	[catename] [varchar](50) NULL,
	[rproqty] [int] NULL,
	[rtotalprice] [varchar](255) NULL,
	[rstatus] [varchar](50) NULL,
	[retaileremail] [varchar](50) NULL,
	[OrderDate] [datetime] NULL,
 CONSTRAINT [PK_retailerorder] PRIMARY KEY CLUSTERED 
(
	[rorder_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[retailerorderlist]    Script Date: 12/7/2024 4:38:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  CREATE VIEW [dbo].[retailerorderlist] AS
SELECT 
    r.rorder_id,
    r.catename,
    r.rproqty,
    r.rtotalprice,
    r.rstatus,
    r.retaileremail,
    p.pro_name,
    p.pro_desc,
    p.pro_price,
    p.pro_img
FROM 
    retailerorder r
JOIN 
    product p ON r.rpro_idfk = p.pro_id;
GO
/****** Object:  Table [dbo].[AdminNotifications]    Script Date: 12/7/2024 4:38:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AdminNotifications](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
	[WholesalerEmail] [nvarchar](255) NOT NULL,
	[Timestamp] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notifications]    Script Date: 12/7/2024 4:38:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notifications](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[WholesalerEmail] [varchar](255) NOT NULL,
	[Message] [varchar](max) NOT NULL,
	[Timestamp] [datetime] NOT NULL,
	[IsRead] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[regretailer]    Script Date: 12/7/2024 4:38:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[regretailer](
	[user_id] [int] IDENTITY(1,1) NOT NULL,
	[user_name] [nvarchar](255) NOT NULL,
	[user_pass] [nvarchar](255) NOT NULL,
	[user_status] [nvarchar](50) NOT NULL,
	[user_email] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[regwholesaler]    Script Date: 12/7/2024 4:38:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[regwholesaler](
	[user_id] [int] IDENTITY(1,1) NOT NULL,
	[user_name] [varchar](100) NULL,
	[user_pass] [varchar](100) NULL,
	[user_status] [varchar](50) NULL,
	[user_email] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RetailerNotifications]    Script Date: 12/7/2024 4:38:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RetailerNotifications](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RetailerEmail] [nvarchar](255) NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
	[Timestamp] [datetime] NOT NULL,
	[IsRead] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[addcategory] ON 

INSERT [dbo].[addcategory] ([cat_id], [cat_name], [cat_desc]) VALUES (2, N'Paint Markers', N'Metallic Oil BAased')
INSERT [dbo].[addcategory] ([cat_id], [cat_name], [cat_desc]) VALUES (3, N'Pens', N'Ball Point Pens')
INSERT [dbo].[addcategory] ([cat_id], [cat_name], [cat_desc]) VALUES (7, N'Pencils', N'A pencil is a versatile tool for writing, drawing.')
SET IDENTITY_INSERT [dbo].[addcategory] OFF
GO
SET IDENTITY_INSERT [dbo].[Notifications] ON 

INSERT [dbo].[Notifications] ([Id], [WholesalerEmail], [Message], [Timestamp], [IsRead]) VALUES (16, N'tariq45@gmail.com', N'Your request for this Faber-Castell  has been Approved by the admin.', CAST(N'2024-12-06T17:19:56.567' AS DateTime), 0)
INSERT [dbo].[Notifications] ([Id], [WholesalerEmail], [Message], [Timestamp], [IsRead]) VALUES (17, N'tariq45@gmail.com', N'Your request for this realmi c5 has been Rejected by the admin.', CAST(N'2024-12-07T15:24:21.143' AS DateTime), 0)
SET IDENTITY_INSERT [dbo].[Notifications] OFF
GO
SET IDENTITY_INSERT [dbo].[product] ON 

INSERT [dbo].[product] ([pro_id], [pro_name], [pro_desc], [pro_price], [pro_img], [procatid_fk], [availability]) VALUES (2, N'Ballpoint Pen GT ', N'Parker Jotter Brushed Metal Ballpoint Pen', N'250', N'/pen.jpg', 3, N'available')
INSERT [dbo].[product] ([pro_id], [pro_name], [pro_desc], [pro_price], [pro_img], [procatid_fk], [availability]) VALUES (3, N'Alberto Twin Marker', N'Twin Marker Chisel 7mm, Round 1 mm', N'20000', N'/marker.jpg', 2, N'available')
INSERT [dbo].[product] ([pro_id], [pro_name], [pro_desc], [pro_price], [pro_img], [procatid_fk], [availability]) VALUES (7, N'Prismacolor', N'Design, color, and create', N'3000', N'/prisma.jfif', 7, N'Out of Stock')
INSERT [dbo].[product] ([pro_id], [pro_name], [pro_desc], [pro_price], [pro_img], [procatid_fk], [availability]) VALUES (8, N'Faber-Castell ', N'best color ', N'4000', N'Faber-Castell.jpg', 7, N'available')
SET IDENTITY_INSERT [dbo].[product] OFF
GO
SET IDENTITY_INSERT [dbo].[regretailer] ON 

INSERT [dbo].[regretailer] ([user_id], [user_name], [user_pass], [user_status], [user_email]) VALUES (1, N'Qurat123', N'Qurat123@', N'Active', N'quratulainbutt377@gmail.com')
SET IDENTITY_INSERT [dbo].[regretailer] OFF
GO
SET IDENTITY_INSERT [dbo].[regwholesaler] ON 

INSERT [dbo].[regwholesaler] ([user_id], [user_name], [user_pass], [user_status], [user_email]) VALUES (8, N'tariq123', N'tariq@5aa', N'Active', N'tariq45@gmail.com')
INSERT [dbo].[regwholesaler] ([user_id], [user_name], [user_pass], [user_status], [user_email]) VALUES (9, N'yasir567', N'yasir2@kkk', N'Deactive', N'yasir34@gmail.com')
INSERT [dbo].[regwholesaler] ([user_id], [user_name], [user_pass], [user_status], [user_email]) VALUES (10, N'hamza444', N'hamza@5hh', N'Deactive', N'hamza66@gmail.com')
INSERT [dbo].[regwholesaler] ([user_id], [user_name], [user_pass], [user_status], [user_email]) VALUES (11, N'shami888', N'shami4@mm', N'Deactive', N'shami45@gmail.com')
INSERT [dbo].[regwholesaler] ([user_id], [user_name], [user_pass], [user_status], [user_email]) VALUES (12, N'shami888', N'shami7@yy', N'Deactive', N'shami99@gmail.com')
INSERT [dbo].[regwholesaler] ([user_id], [user_name], [user_pass], [user_status], [user_email]) VALUES (13, N'yusra123', N'yusra3#tt', N'Deactive', N'yusra45@gmail.com')
SET IDENTITY_INSERT [dbo].[regwholesaler] OFF
GO
SET IDENTITY_INSERT [dbo].[RetailerNotifications] ON 

INSERT [dbo].[RetailerNotifications] ([Id], [RetailerEmail], [Message], [Timestamp], [IsRead]) VALUES (1, N'quratulainbutt377@gmail.com', N'Your request for this redmi has been Rejected by the manager.', CAST(N'2024-12-07T15:27:51.117' AS DateTime), 0)
INSERT [dbo].[RetailerNotifications] ([Id], [RetailerEmail], [Message], [Timestamp], [IsRead]) VALUES (2, N'quratulainbutt377@gmail.com', N'Your request for this redmi has been Delivered by the manager.', CAST(N'2024-12-07T15:28:04.040' AS DateTime), 0)
INSERT [dbo].[RetailerNotifications] ([Id], [RetailerEmail], [Message], [Timestamp], [IsRead]) VALUES (3, N'quratulainbutt377@gmail.com', N'Your request for this redmi has been Delivered by the manager.', CAST(N'2024-12-07T15:28:34.850' AS DateTime), 0)
SET IDENTITY_INSERT [dbo].[RetailerNotifications] OFF
GO
SET IDENTITY_INSERT [dbo].[whorder] ON 

INSERT [dbo].[whorder] ([worder_id], [wpro_idfk], [catename], [wproqty], [wtotalprice], [wstatus], [whsaleremail], [OrderDate]) VALUES (26, 8, N'Pencils', 26, N'104000.00', N'Approved', N'tariq45@gmail.com', CAST(N'2024-12-06T17:17:14.590' AS DateTime))
INSERT [dbo].[whorder] ([worder_id], [wpro_idfk], [catename], [wproqty], [wtotalprice], [wstatus], [whsaleremail], [OrderDate]) VALUES (27, 8, N'Pencils', 16, N'64000.00', N'Pending', N'tariq45@gmail.com', CAST(N'2024-12-06T17:21:14.223' AS DateTime))
SET IDENTITY_INSERT [dbo].[whorder] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__regretai__B0FBA212F83646AC]    Script Date: 12/7/2024 4:38:31 PM ******/
ALTER TABLE [dbo].[regretailer] ADD UNIQUE NONCLUSTERED 
(
	[user_email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AdminNotifications] ADD  DEFAULT (getdate()) FOR [Timestamp]
GO
ALTER TABLE [dbo].[Notifications] ADD  DEFAULT (getdate()) FOR [Timestamp]
GO
ALTER TABLE [dbo].[Notifications] ADD  DEFAULT ((0)) FOR [IsRead]
GO
ALTER TABLE [dbo].[product] ADD  CONSTRAINT [DF_Product_Availability]  DEFAULT ('available') FOR [availability]
GO
ALTER TABLE [dbo].[regretailer] ADD  DEFAULT ('Deactive') FOR [user_status]
GO
ALTER TABLE [dbo].[regwholesaler] ADD  DEFAULT ('Deactive') FOR [user_status]
GO
ALTER TABLE [dbo].[RetailerNotifications] ADD  DEFAULT (getdate()) FOR [Timestamp]
GO
ALTER TABLE [dbo].[RetailerNotifications] ADD  DEFAULT ((0)) FOR [IsRead]
GO
ALTER TABLE [dbo].[whorder] ADD  DEFAULT (getdate()) FOR [OrderDate]
GO
ALTER TABLE [dbo].[product]  WITH CHECK ADD  CONSTRAINT [FK_product_addcategory] FOREIGN KEY([procatid_fk])
REFERENCES [dbo].[addcategory] ([cat_id])
GO
ALTER TABLE [dbo].[product] CHECK CONSTRAINT [FK_product_addcategory]
GO
ALTER TABLE [dbo].[retailerorder]  WITH CHECK ADD  CONSTRAINT [FK_retailerorder_product] FOREIGN KEY([rpro_idfk])
REFERENCES [dbo].[product] ([pro_id])
GO
ALTER TABLE [dbo].[retailerorder] CHECK CONSTRAINT [FK_retailerorder_product]
GO
ALTER TABLE [dbo].[whorder]  WITH CHECK ADD  CONSTRAINT [FK_whorder_product] FOREIGN KEY([wpro_idfk])
REFERENCES [dbo].[product] ([pro_id])
GO
ALTER TABLE [dbo].[whorder] CHECK CONSTRAINT [FK_whorder_product]
GO
USE [master]
GO
ALTER DATABASE [StationaryApp] SET  READ_WRITE 
GO
