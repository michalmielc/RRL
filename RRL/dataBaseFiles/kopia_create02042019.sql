USE [master]
GO
/****** Object:  Database [rrl]    Script Date: 02.04.2019 17:11:09 ******/
CREATE DATABASE [rrl]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'rrl', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\rrl.mdf' , SIZE = 4096000KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'rrl_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\rrl_log.ldf' , SIZE = 204800KB , MAXSIZE = 512000KB , FILEGROWTH = 3%)
GO
ALTER DATABASE [rrl] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [rrl].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [rrl] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [rrl] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [rrl] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [rrl] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [rrl] SET ARITHABORT OFF 
GO
ALTER DATABASE [rrl] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [rrl] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [rrl] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [rrl] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [rrl] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [rrl] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [rrl] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [rrl] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [rrl] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [rrl] SET  DISABLE_BROKER 
GO
ALTER DATABASE [rrl] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [rrl] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [rrl] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [rrl] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [rrl] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [rrl] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [rrl] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [rrl] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [rrl] SET  MULTI_USER 
GO
ALTER DATABASE [rrl] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [rrl] SET DB_CHAINING OFF 
GO
ALTER DATABASE [rrl] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [rrl] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [rrl] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [rrl] SET QUERY_STORE = OFF
GO
USE [rrl]
GO
/****** Object:  UserDefinedFunction [dbo].[checkcubbieifempty]    Script Date: 02.04.2019 17:11:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE FUNCTION [dbo].[checkcubbieifempty](@Id int)


RETURNS bit
AS
BEGIN

DECLARE @return as bit;


IF  EXISTS (SELECT 1 CubbieId FROM [ViewSTORAGEPLACES] WHERE CubbyId=@Id) 
BEGIN
	set @return = 1
END
 
ELSE 
	set @return = 0


	RETURN @return

 
END







GO
/****** Object:  UserDefinedFunction [dbo].[checkusergroupname]    Script Date: 02.04.2019 17:11:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE FUNCTION [dbo].[checkusergroupname](@name varchar(100))


RETURNS bit
AS
BEGIN

DECLARE @return as bit;


IF  EXISTS (SELECT 1 UserGroupName FROM [ViewUSERGROUPS] WHERE UserGroupName=@name) 
BEGIN
	set @return = 1
END
 
ELSE 
	set @return = 0


	RETURN @return

 
END








GO
/****** Object:  UserDefinedFunction [dbo].[checkuserpassword]    Script Date: 02.04.2019 17:11:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE FUNCTION [dbo].[checkuserpassword](@haslo varchar(100))


RETURNS bit
AS
BEGIN

DECLARE @return as bit;


IF  EXISTS (SELECT 1 Password FROM [ViewUSERS] WHERE Password=@haslo) 
BEGIN
	set @return = 1
END
 
ELSE 
	set @return = 0


	RETURN @return

 
END







GO
/****** Object:  UserDefinedFunction [dbo].[checkuserpassword_and_status]    Script Date: 02.04.2019 17:11:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





CREATE FUNCTION [dbo].[checkuserpassword_and_status](@haslo varchar(100))


RETURNS bit
AS
BEGIN

DECLARE @return as bit;

SET @return = dbo.checkuserpassword(@haslo)

IF @return =1

BEGIN
	SET @return = (SELECT Active FROM [ViewUSERS] WHERE Password=@haslo)

END

ELSE 
	set @return = 0


	RETURN @return

 
END

GO
/****** Object:  UserDefinedFunction [dbo].[checkuserpassword_at_edit]    Script Date: 02.04.2019 17:11:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






CREATE FUNCTION [dbo].[checkuserpassword_at_edit](@haslo varchar(100), @id int)


RETURNS bit
AS
BEGIN

DECLARE @return as bit;
DECLARE @iden as int;

SET @return = dbo.checkuserpassword(@haslo)

IF @return =1

BEGIN


SET @iden = (SELECT UserId  FROM [ViewUSERS] WHERE Password=@haslo)

	IF @iden = @id 
		BEGIN
			SET @return = 0
		END
	
END

ELSE 
	SET @return = 0


	RETURN @return

 
END


GO
/****** Object:  UserDefinedFunction [dbo].[getitempicpath]    Script Date: 02.04.2019 17:11:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO







CREATE FUNCTION [dbo].[getitempicpath](@ItemId int)


RETURNS varchar(150)
AS
BEGIN

DECLARE @return as varchar(150);

	SET @return = (SELECT PicPath FROM [ViewITEMS] WHERE @ItemId=ItemId)


	RETURN @return

 
END



GO
/****** Object:  UserDefinedFunction [dbo].[getmaxamounttofill]    Script Date: 02.04.2019 17:11:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO







CREATE FUNCTION [dbo].[getmaxamounttofill](@ItemId int)


RETURNS INT
AS
BEGIN

DECLARE @return as INT;

	SET @return = (select SUM(MaxInventory-CurrentInventory) as ilość from dbo.ViewSTORAGEPLACES
  where ItemId=@ItemId)
 
   IF @return is null  
   BEGIN
   set @return =0
   END

	RETURN @return

 
END



GO
/****** Object:  UserDefinedFunction [dbo].[getnextidBalks]    Script Date: 02.04.2019 17:11:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO








CREATE FUNCTION [dbo].[getnextidBalks]()


RETURNS INT
AS
BEGIN

DECLARE @return as INT;


   IF NOT EXISTS (SELECT 1 FROM BALKS)
      BEGIN 
       set  @return=1
      END 
    ELSE
      BEGIN
        SET @return = (SELECT max(BalkId) FROM [BALKS] )+1
      END

  

	RETURN @return

 
END



GO
/****** Object:  UserDefinedFunction [dbo].[getnextidCubbies]    Script Date: 02.04.2019 17:11:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO







CREATE FUNCTION [dbo].[getnextidCubbies]()


RETURNS INT
AS
BEGIN

DECLARE @return as INT;


   IF NOT EXISTS (SELECT 1 FROM CUBBIES)
      BEGIN 
       set  @return=1
      END 
    ELSE
      BEGIN
        SET @return = (SELECT max(CubbyId) FROM [CUBBIES] )+1
      END

  

	RETURN @return

 
END



GO
/****** Object:  UserDefinedFunction [dbo].[getuserdepartment]    Script Date: 02.04.2019 17:11:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO







CREATE FUNCTION [dbo].[getuserdepartment](@haslo varchar(100))


RETURNS varchar(100)
AS
BEGIN

DECLARE @return as varchar(100);

	SET @return = (SELECT Department FROM [ViewUSERS] WHERE Password=@haslo)


	RETURN @return

 
END



GO
/****** Object:  UserDefinedFunction [dbo].[getuserid]    Script Date: 02.04.2019 17:11:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






CREATE FUNCTION [dbo].[getuserid](@haslo varchar(100))


RETURNS INT
AS
BEGIN

DECLARE @return as INT;

	SET @return = (SELECT UserId FROM [ViewUSERS] WHERE Password=@haslo)


	RETURN @return

 
END


GO
/****** Object:  UserDefinedFunction [dbo].[getusername]    Script Date: 02.04.2019 17:11:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






CREATE FUNCTION [dbo].[getusername](@haslo varchar(100))


RETURNS varchar(100)
AS
BEGIN

DECLARE @return as varchar(100);

	SET @return = (SELECT UserName FROM [ViewUsers] WHERE Password=@haslo)


	RETURN @return

 
END


GO
/****** Object:  UserDefinedFunction [dbo].[getuserrights_on_name]    Script Date: 02.04.2019 17:11:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO








CREATE FUNCTION [dbo].[getuserrights_on_name](@nazwa varchar(100))


RETURNS INT
AS
BEGIN

DECLARE @return as INT;

	SET @return = (SELECT UserGroupId  FROM dbo.ViewUSERGROUPS WHERE  UserGroupName like @nazwa)
	
	RETURN @return

 
END




GO
/****** Object:  UserDefinedFunction [dbo].[getuserrights_on_password]    Script Date: 02.04.2019 17:11:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO







CREATE FUNCTION [dbo].[getuserrights_on_password](@haslo varchar(100))


RETURNS INT
AS
BEGIN

DECLARE @return as INT;

	SET @return = (SELECT Rights FROM [ViewUSERS] WHERE Password=@haslo)


	RETURN @return

 
END



GO
/****** Object:  Table [dbo].[ITEMS]    Script Date: 02.04.2019 17:11:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ITEMS](
	[ItemId] [int] IDENTITY(1,1) NOT NULL,
	[ItemName1] [varchar](150) NOT NULL,
	[ItemName2] [varchar](100) NULL,
	[ItemName3] [varchar](100) NULL,
	[Barcode] [varchar](100) NULL,
	[PicPath] [varchar](150) NULL,
	[Active] [bit] NOT NULL,
	[Price] [smallmoney] NOT NULL,
	[MinInventory] [int] NOT NULL,
	[Supplier] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[ItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ITEMS_AVAILABILITY]    Script Date: 02.04.2019 17:11:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ITEMS_AVAILABILITY](
	[ItemId] [int] NOT NULL,
	[Amount] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[ViewITEMS_AMOUNTS_adv]    Script Date: 02.04.2019 17:11:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO







CREATE VIEW [dbo].[ViewITEMS_AMOUNTS_adv] AS

SELECT ITEMS.ItemId,Items.ItemName1,Items.ItemName2, Items.ItemName3, Items.Barcode,Items.PicPath,ITEMS.Active,Cast(ITEMS.Price as numeric(10,2)) as 'Price',ITEMS.MinInventory ,ITEMS_AVAILABILITY.Amount,ITEMS.Supplier
FROM ITEMS
INNER JOIN ITEMS_AVAILABILITY ON ITEMS.ItemId= ITEMS_AVAILABILITY.ItemId
Where ITEMS.Active = 1








GO
/****** Object:  Table [dbo].[STORAGEPLACES_TEMP]    Script Date: 02.04.2019 17:11:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STORAGEPLACES_TEMP](
	[KeyTemp] [int] IDENTITY(1,1) NOT NULL,
	[StorageIdTemp] [int] NOT NULL,
	[CubbyID] [int] NOT NULL,
	[CubbyName] [varchar](100) NULL,
	[ItemIdTemp] [int] NOT NULL,
	[AmountTemp] [int] NOT NULL,
	[StorageName] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[KeyTemp] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[ViewSTORAGEPLACES_TEMP]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO











CREATE VIEW [dbo].[ViewSTORAGEPLACES_TEMP]
AS
SELECT        CubbyID, CubbyName, StorageName, AmountTemp
FROM            dbo.STORAGEPLACES_TEMP













GO
/****** Object:  Table [dbo].[COSTCENTERSOPTIONS]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[COSTCENTERSOPTIONS](
	[OptionId] [int] IDENTITY(1,1) NOT NULL,
	[ActiveFill] [bit] NULL,
	[ActiveWithdraw] [bit] NULL,
	[ActiveEdit] [bit] NULL
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[ViewCOSTCENTERSOPTIONS]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE VIEW [dbo].[ViewCOSTCENTERSOPTIONS]
AS
SELECT       dbo.[COSTCENTERSOPTIONS]. *
FROM            dbo.COSTCENTERSOPTIONS








GO
/****** Object:  Table [dbo].[SUPPLIERS]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SUPPLIERS](
	[SupplierId] [int] IDENTITY(1,1) NOT NULL,
	[SupplierName] [varchar](150) NOT NULL,
	[SupplierEmail] [varchar](100) NULL
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[ViewSUPPLIERS]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[ViewSUPPLIERS]
AS
SELECT       dbo.[SUPPLIERS]. *
FROM            dbo.SUPPLIERS






GO
/****** Object:  Table [dbo].[USERGROUPS]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[USERGROUPS](
	[UserGroupId] [int] IDENTITY(1,1) NOT NULL,
	[UserGroupName] [varchar](100) NOT NULL,
	[OptionAdministrator] [bit] NOT NULL,
	[OptionReceive] [bit] NOT NULL,
	[OptionReceipt] [bit] NOT NULL,
	[OptionEditUser] [bit] NOT NULL,
	[OptionEditItem] [bit] NOT NULL,
	[OptionEditStorageplace] [bit] NOT NULL,
	[OptionReports] [bit] NOT NULL,
	[OptionGroupUser] [bit] NOT NULL,
	[OptionSupplier] [bit] NOT NULL,
	[OptionCostcenter] [bit] NOT NULL,
	[OptionStocktaking] [bit] NOT NULL,
 CONSTRAINT [PK_USERGROUP] PRIMARY KEY CLUSTERED 
(
	[UserGroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UserGroupName] UNIQUE NONCLUSTERED 
(
	[UserGroupName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[ViewUSERGROUPS]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[ViewUSERGROUPS]
AS
SELECT        dbo.USERGROUPS.*
FROM            dbo.USERGROUPS



GO
/****** Object:  Table [dbo].[USERS]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[USERS](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](100) NULL,
	[Department] [varchar](100) NULL,
	[Rights] [int] NOT NULL,
	[Password] [varchar](100) NOT NULL,
	[Active] [bit] NULL,
 CONSTRAINT [PK_USER] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Password] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Password] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Password] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Password] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Password] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[ViewUSERS]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[ViewUSERS]
AS
SELECT        dbo.[USERS].*
FROM            dbo.[USERS]


GO
/****** Object:  View [dbo].[ViewUSERSwithUsergroups]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[ViewUSERSwithUsergroups] AS

SELECT UserID,UserName,USERGROUPS.UserGroupName,Password, Department,Active,USERGROUPS.UserGroupId
FROM USERS
INNER JOIN USERGROUPS ON USERS.Rights = USERGROUPS.UserGroupId


GO
/****** Object:  Table [dbo].[LOGS_TABLE]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LOGS_TABLE](
	[IdLog] [int] IDENTITY(1,1) NOT NULL,
	[LogName] [varchar](100) NULL,
	[LogDepartment] [varchar](100) NULL,
	[LogDate] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdLog] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[ViewLOGS_TABLE]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ViewLOGS_TABLE]
AS
SELECT    *
FROM             dbo.[LOGS_TABLE]




GO
/****** Object:  Table [dbo].[COSTCENTERS]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[COSTCENTERS](
	[CostId] [int] IDENTITY(1,1) NOT NULL,
	[CostName] [varchar](150) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[ViewCOSTCENTERS]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE VIEW [dbo].[ViewCOSTCENTERS]
AS
SELECT       dbo.[COSTCENTERS]. *
FROM            dbo.COSTCENTERS







GO
/****** Object:  Table [dbo].[WAREHOUSEOPERATIONS]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WAREHOUSEOPERATIONS](
	[IdLog] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[Department] [varchar](100) NULL,
	[Date] [varchar](50) NULL,
	[Time] [varchar](50) NULL,
	[ItemNo] [varchar](100) NULL,
	[ItemName] [varchar](130) NULL,
	[Amount] [int] NULL,
	[Storage] [varchar](100) NULL,
	[CostCenter] [varchar](100) NULL,
	[Operation] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdLog] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[ViewWAREHOUSEOPERATIONS_all]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






CREATE VIEW [dbo].[ViewWAREHOUSEOPERATIONS_all]
AS
SELECT        *
FROM            dbo.WAREHOUSEOPERATIONS









GO
/****** Object:  View [dbo].[ViewITEMS]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





CREATE VIEW [dbo].[ViewITEMS]
AS
SELECT       dbo.[ITEMS]. *
FROM            dbo.ITEMS








GO
/****** Object:  Table [dbo].[CUBBIES]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CUBBIES](
	[CubbyId] [int] IDENTITY(100,1) NOT NULL,
	[CubbyName] [varchar](100) NULL,
	[PositionX] [int] NOT NULL,
	[PositionY] [int] NOT NULL,
	[Height] [int] NOT NULL,
	[Width] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CubbyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[ViewCUBBIES]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






CREATE VIEW [dbo].[ViewCUBBIES]
AS
SELECT       dbo.[CUBBIES]. *
FROM            dbo.CUBBIES









GO
/****** Object:  Table [dbo].[BALKS]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BALKS](
	[BalkId] [int] IDENTITY(100,1) NOT NULL,
	[PositionX] [int] NOT NULL,
	[PositionY] [int] NOT NULL,
	[Height] [int] NOT NULL,
	[Width] [int] NOT NULL,
	[RackId] [int] NOT NULL,
	[RackName] [varchar](80) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[BalkId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[ViewBALKS]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO







CREATE VIEW [dbo].[ViewBALKS]
AS
SELECT       dbo.[BALKS]. *
FROM            dbo.BALKS









GO
/****** Object:  View [dbo].[ViewITEMS_AMOUNTS]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE VIEW [dbo].[ViewITEMS_AMOUNTS] AS

SELECT ITEMS.ItemId,ItemName1,ItemName2, ItemName3, Barcode,ITEMS_AVAILABILITY.Amount
FROM ITEMS
INNER JOIN ITEMS_AVAILABILITY ON ITEMS.ItemId= ITEMS_AVAILABILITY.ItemId
Where ITEMS.Active = 1




GO
/****** Object:  View [dbo].[ViewITEMS_AMOUNTS_for_cubbies]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE VIEW [dbo].[ViewITEMS_AMOUNTS_for_cubbies] AS


SELECT ITEMS.ItemId,ItemName1,ItemName2, ItemName3, Barcode,ITEMS_AVAILABILITY.Amount, ItemName1+' | '+ItemName2+' | '+ ItemName3 as new
FROM ITEMS
INNER JOIN ITEMS_AVAILABILITY ON ITEMS.ItemId= ITEMS_AVAILABILITY.ItemId
Where ITEMS.Active = 1





GO
/****** Object:  Table [dbo].[STORAGEPLACES]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STORAGEPLACES](
	[StorageId] [int] IDENTITY(100,1) NOT NULL,
	[CubbyId] [int] NOT NULL,
	[CubbyName] [varchar](100) NULL,
	[StorageName] [varchar](100) NOT NULL,
	[ItemId] [int] NOT NULL,
	[ItemName] [varchar](300) NULL,
	[CurrentInventory] [int] NOT NULL,
	[MaxInventory] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[StorageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[ViewSTORAGEPLACES]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO







CREATE VIEW [dbo].[ViewSTORAGEPLACES]
AS
SELECT       *
FROM            dbo.STORAGEPLACES










GO
ALTER TABLE [dbo].[USERGROUPS] ADD  CONSTRAINT [DF_USERGROUP_OptionAdministrator]  DEFAULT ((0)) FOR [OptionAdministrator]
GO
ALTER TABLE [dbo].[USERGROUPS] ADD  CONSTRAINT [DF_USERGROUP_OptionReceive]  DEFAULT ((0)) FOR [OptionReceive]
GO
ALTER TABLE [dbo].[ITEMS_AVAILABILITY]  WITH CHECK ADD FOREIGN KEY([ItemId])
REFERENCES [dbo].[ITEMS] ([ItemId])
GO
ALTER TABLE [dbo].[USERS]  WITH CHECK ADD  CONSTRAINT [FK_USERGR_USER] FOREIGN KEY([Rights])
REFERENCES [dbo].[USERGROUPS] ([UserGroupId])
GO
ALTER TABLE [dbo].[USERS] CHECK CONSTRAINT [FK_USERGR_USER]
GO
ALTER TABLE [dbo].[ITEMS_AVAILABILITY]  WITH CHECK ADD CHECK  (([Amount]>=(0)))
GO
ALTER TABLE [dbo].[STORAGEPLACES]  WITH CHECK ADD CHECK  (([CurrentInventory]>=(0)))
GO
ALTER TABLE [dbo].[STORAGEPLACES]  WITH CHECK ADD CHECK  (([MaxInventory]>=(0)))
GO
ALTER TABLE [dbo].[STORAGEPLACES]  WITH CHECK ADD CHECK  (([CurrentInventory]<=[MaxInventory]))
GO
/****** Object:  StoredProcedure [dbo].[addbalk]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[addbalk]


AS
INSERT into dbo.BALKS
VALUES  (20,20,30,200,'','nowa')
    
GO
/****** Object:  StoredProcedure [dbo].[addbalk_by_creator]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[addbalk_by_creator]



@PositionX  int,
@PositionY int,
@Height  int,
@Width int,
@RackId int,
@RackName varchar(80)

AS
INSERT into dbo.BALKS
VALUES  (@PositionX,@PositionY ,@Height,@Width,'' ,@RackName)
    
GO
/****** Object:  StoredProcedure [dbo].[addcostcenter]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[addcostcenter]



@CostName varchar(150)


AS
INSERT into COSTCENTERS
VALUES  
   (@CostName); 




GO
/****** Object:  StoredProcedure [dbo].[addcubby]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[addcubby]


AS
INSERT into dbo.CUBBIES
VALUES  ('nowa',20,20,30,100)
    
GO
/****** Object:  StoredProcedure [dbo].[addcubby_by_creator]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[addcubby_by_creator]



@PositionX  int,
@PositionY int,
@Height  int,
@Width int,
@Name varchar(50)



AS
INSERT into dbo.CUBBIES
VALUES  (@Name, @PositionX, @PositionY , @Height, @Width)
    
GO
/****** Object:  StoredProcedure [dbo].[additem]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[additem]



@Name1 varchar(150),
@Name2  varchar(100),
@Name3 varchar(100),
@Barcode varchar(100),
@Picpath varchar(150),
@Active bit,
@Price float,
@MinInv int,
@Supplier varchar(100)

AS
INSERT into ITEMS
VALUES  
   (@Name1 ,@Name2  ,@Name3 ,@Barcode,@Picpath ,@Active ,@Price ,@MinInv,@Supplier ); 



GO
/****** Object:  StoredProcedure [dbo].[addlog]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[addlog] 


@Name varchar(100),
@Department varchar(100),
@Date varchar(100)


AS
INSERT into LOGS_TABLE 
VALUES  
   (@Name,@Department,@Date); 



GO
/****** Object:  StoredProcedure [dbo].[addstorageplace]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[addstorageplace]



@CubbyId int,
@CubbyName varchar(150),
@StorageName  varchar(100),
@ItemId int,
@ItemName varchar(300),
@MaxInventory int

AS
INSERT into STORAGEPLACES
VALUES  
   (
@CubbyId,
@CubbyName,
@StorageName,
@ItemId,
@ItemName,0,
@MaxInventory); 




GO
/****** Object:  StoredProcedure [dbo].[addsupplier]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[addsupplier]



@Name varchar(150),
@Email  varchar(100)

AS
INSERT into SUPPLIERS
VALUES  
   (@Name ,@Email); 



GO
/****** Object:  StoredProcedure [dbo].[adduser]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[adduser] 


@UserName varchar(100),
@Department varchar(100),
@Rights int,
@Password  varchar(100),
@Active bit


AS
INSERT into USERS 
VALUES  
   (@UserName,@Department,@Rights,@Password,@Active); 


GO
/****** Object:  StoredProcedure [dbo].[addusersgroup]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[addusersgroup]
     @UserGroupName varchar(100), 
     @OptionAdministrator bit,
     @OptionReceive bit,
     @OptionReceipt bit,
     @OptionEditUser bit,
     @OptionEditItem bit,
     @OptionEditStorageplace bit,
     @OptionReports bit,
     @OptionGroupUser bit,
	  @OptionSupplier bit,
     @OptionCostcenter bit,
     @OptionStocktaking bit
AS


INSERT into dbo.USERGROUPS
VALUES  
   ( @UserGroupName,
   @OptionAdministrator,
    @OptionReceive, 
	@OptionReceipt,
	 @OptionEditUser,  
	 @OptionEditItem,
	 @OptionEditStorageplace,
     @OptionReports,
     @OptionGroupUser,
	 @OptionSupplier,
     @OptionCostcenter,
     @OptionStocktaking 
	 )



GO
/****** Object:  StoredProcedure [dbo].[addwarehouseoperation]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[addwarehouseoperation] 


@Name varchar(100),
@Department varchar(100),
@Date varchar(50),
@Time varchar(50),
@ItemNo varchar(100),
@ItemName varchar(130),
@Amount int,
@Storage varchar(100),
@CostCenter varchar(100),
@Operation varchar(100)

AS
INSERT into WAREHOUSEOPERATIONS 
VALUES  
   (@Name,@Department,@Date,@Time,@ItemNo,@ItemName,@Amount,@Storage,@CostCenter,@Operation ); 




GO
/****** Object:  StoredProcedure [dbo].[changeitemamount]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[changeitemamount] 

@ItemID int,
@Amount int,
@Fill bit



AS

DECLARE @Amount_curr as int;

SET @Amount_curr = (select Amount from ITEMS_AVAILABILITY where @ItemID=ItemId)

IF @Fill = 1
SET @Amount_curr = @Amount_curr + @Amount

IF @Fill = 0
SET @Amount_curr = @Amount_curr - @Amount 


UPDATE ITEMS_AVAILABILITY
SET  Amount=@Amount_curr
 
  WHERE ItemId =@ItemID



GO
/****** Object:  StoredProcedure [dbo].[changeitemamount_storageplace]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[changeitemamount_storageplace] 

@ItemID int,
@Amount int,
@Fill bit



AS

--WYCZYSZCZENIE TABLICY TYMCZASOWEJ
TRUNCATE TABLE STORAGEPLACES_TEMP

-- DEKLARACJA ZMIENNYCH POMOCNICZYCH ILOŚĆ
DECLARE @Amount_seeked as int;
DECLARE @StorageId as int;
DECLARE @CubbyId as int;
DECLARE @Counter as int;
DECLARE @CubbyName as varchar(100);
DECLARE @StorageName as varchar(100);
SET @Counter = @Amount

WHILE @Counter>0
BEGIN

--ZNAJDYWANIE PIERWSZEJ ILOŚCI, NR PRZEGRODY, NR CUBBY W ZALEZNOŚCI OD POBRANIE/UZUPEŁNIENIE

IF @Fill = 1

	BEGIN
		SET @Amount_seeked = (select top 1 (MaxInventory- CurrentInventory) from ViewSTORAGEPLACES where ItemId=@ItemID and  (MaxInventory- CurrentInventory)>0)
		SET @StorageId= (select top 1 StorageId from ViewSTORAGEPLACES where ItemId=@ItemID and (MaxInventory- CurrentInventory)>0)
		SET @CubbyId= (select top 1 CubbyId from ViewSTORAGEPLACES where ItemId=@ItemID and (MaxInventory- CurrentInventory)>0)
	    SET @CubbyName= (select top 1 CubbyName from ViewSTORAGEPLACES where ItemId=@ItemID and (MaxInventory- CurrentInventory)>0)
	    SET @StorageName= (select top 1 StorageName from ViewSTORAGEPLACES where ItemId=@ItemID and (MaxInventory- CurrentInventory)>0)
	

	   IF @Amount_seeked>=@Counter
			BEGIN
				INSERT INTO STORAGEPLACES_TEMP
				VALUES(@StorageId,@CubbyId,@CubbyName,@ItemID,@Counter,@StorageName)

				UPDATE STORAGEPLACES
				SET CurrentInventory=CurrentInventory+ @Counter
				WHERE StorageId= @StorageId and ItemID = @ItemID

				SET @Counter=0
			END
		IF @Amount_seeked<@Counter
			BEGIN
				INSERT INTO STORAGEPLACES_TEMP
				VALUES(@StorageId,@CubbyId,@CubbyName,@ItemID,@Amount_seeked,@StorageName)
				SET @Counter = @Counter - @Amount_seeked

				UPDATE STORAGEPLACES
				SET CurrentInventory=CurrentInventory+ @Amount_seeked
				WHERE StorageId= @StorageId and ItemID = @ItemID

			END
	END


IF @Fill = 0
	BEGIN

		SET @Amount_seeked = (select top 1 (CurrentInventory) from ViewSTORAGEPLACES where ItemId=@ItemID and  CurrentInventory>0)
		SET @StorageId= (select top 1 StorageId from ViewSTORAGEPLACES where ItemId=@ItemID and CurrentInventory>0)
		SET @CubbyId= (select top 1 CubbyId from ViewSTORAGEPLACES where ItemId=@ItemID and CurrentInventory>0)
		SET @CubbyName= (select top 1 CubbyName from ViewSTORAGEPLACES where ItemId=@ItemID and CurrentInventory>0)
	   SET @StorageName= (select top 1 StorageName from ViewSTORAGEPLACES  where ItemId=@ItemID and CurrentInventory>0)
	

		
	   IF @Amount_seeked>=@Counter
			BEGIN
				INSERT INTO STORAGEPLACES_TEMP
				VALUES(@StorageId,@CubbyId,@CubbyName,@ItemID,@Counter,@StorageName)
			

				UPDATE STORAGEPLACES
				SET CurrentInventory=CurrentInventory-@Counter
				WHERE StorageId= @StorageId and ItemID = @ItemID

				SET @Counter=0
			END
		  IF @Amount_seeked<@Counter
			BEGIN
				INSERT INTO STORAGEPLACES_TEMP
				VALUES(@StorageId,@CubbyId,@CubbyName,@ItemID,@Amount_seeked,@StorageName)
				SET @Counter = @Counter - @Amount_seeked

				UPDATE STORAGEPLACES
				SET CurrentInventory=CurrentInventory-@Amount_seeked
				WHERE StorageId= @StorageId and ItemID = @ItemID

			END

	END

	



	

END



GO
/****** Object:  StoredProcedure [dbo].[changeitemamount_storageplacetozero]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[changeitemamount_storageplacetozero] 

@ItemID int



AS


				UPDATE STORAGEPLACES
				SET CurrentInventory=0
				WHERE ItemID = @ItemID

	

	



GO
/****** Object:  StoredProcedure [dbo].[changeitemamounttozero]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[changeitemamounttozero] 

@ItemID int



AS


UPDATE ITEMS_AVAILABILITY
SET  Amount=0
WHERE ItemId =@ItemID



GO
/****** Object:  StoredProcedure [dbo].[delallstorageplace]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[delallstorageplace]


AS


DELETE FROM dbo.STORAGEPLACES_TEMP
DBCC CHECKIDENT ([dbo.STORAGEPLACES_TEMP], RESEED, 0)
DELETE FROM dbo.STORAGEPLACES
DBCC CHECKIDENT ([dbo.STORAGEPLACES], RESEED, 0)
DELETE FROM dbo.CUBBIES
DBCC CHECKIDENT ([dbo.CUBBIES], RESEED, 0)
DELETE FROM dbo.BALKS
DBCC CHECKIDENT ([dbo.BALKS], RESEED, 0)



GO
/****** Object:  StoredProcedure [dbo].[delbalk]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[delbalk]
@BalkId int

AS




DELETE FROM BALKS
WHERE BalkId=@BalkId; 

GO
/****** Object:  StoredProcedure [dbo].[delcostcenter]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[delcostcenter]
@CostId int

AS

DELETE FROM COSTCENTERS
WHERE CostId=@CostId; 



GO
/****** Object:  StoredProcedure [dbo].[delcubby]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[delcubby]
@CubbyId int

AS




DELETE FROM CUBBIES
WHERE CubbyId=@CubbyId; 

GO
/****** Object:  StoredProcedure [dbo].[delitem]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[delitem]
@ItemId int

AS

DELETE FROM ITEMS_AVAILABILITY
WHERE ItemID=@ItemId; 

DELETE FROM ITEMS
WHERE ItemID=@ItemId; 

DELETE FROM STORAGEPLACES
WHERE ItemID=@ItemId;


GO
/****** Object:  StoredProcedure [dbo].[delstorageplace]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[delstorageplace]
@StorageId int

AS

DECLARE @ilosc AS int
DECLARE @nrart AS int

SET @ilosc = (Select CurrentInventory  from STORAGEPLACES Where StorageId = @StorageId)
SET @nrart = (Select ItemId from STORAGEPLACES Where StorageId= @StorageId)

Update dbo.ITEMS_AVAILABILITY
SET Amount = Amount - @ilosc
Where ItemId = @nrart

DELETE FROM STORAGEPLACES
WHERE StorageId =@StorageId; 

GO
/****** Object:  StoredProcedure [dbo].[delsupplier]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[delsupplier]
@SupplierId int

AS

DELETE FROM SUPPLIERS
WHERE SupplierId=@SupplierId; 


GO
/****** Object:  StoredProcedure [dbo].[deluser]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[deluser]
@UserId int

AS

DELETE FROM USERS
WHERE UserID=@UserId; 
GO
/****** Object:  StoredProcedure [dbo].[delusergroup]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[delusergroup] @id int
AS
delete FROM USERGROUPS WHERE 
  UserGroupId= @id


GO
/****** Object:  StoredProcedure [dbo].[editbalk]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[editbalk] 

@BalkId int,
@PositionX  int,
@PositionY int,
@Height  int,
@Width int,
@RackId int,
@RackName varchar(80)

AS
UPDATE BALKS
SET 
PositionX = @PositionX, PositionY  = @PositionY , Height = @Height, Width= @Width, RackId = @RackId, RackName = @RackName
  WHERE BalkId = @BalkId

  

GO
/****** Object:  StoredProcedure [dbo].[editbalkname]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[editbalkname] 

@BalkId int,
@RackName varchar(80)


AS
UPDATE BALKS
SET RackName =@RackName
 
  WHERE BalkId =@BalkId



GO
/****** Object:  StoredProcedure [dbo].[editbalksize]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[editbalksize] 

@BalkId int,
@Height int,
@Width int



AS
UPDATE BALKS
SET Height =@Height , Width =@Width  
  WHERE BalkId =@BalkId




GO
/****** Object:  StoredProcedure [dbo].[editcostcenter]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[editcostcenter] 

@CostId int,
@CostName varchar(150)


AS
UPDATE COSTCENTERS

SET  CostName= @CostName 
 
  WHERE CostId =@CostId




GO
/****** Object:  StoredProcedure [dbo].[editcostcentersoptions]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[editcostcentersoptions] 

@ActiveFill bit,
@ActiveWithdraw bit,
@ActiveEdit bit



AS
UPDATE COSTCENTERSOPTIONS 
SET  ActiveFill= @ActiveFill ,  ActiveWithdraw = @ActiveWithdraw  , ActiveEdit = @ActiveEdit 
 
  WHERE OptionId = 1




GO
/****** Object:  StoredProcedure [dbo].[editcubby]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[editcubby] 

@CubbyId int,
@CubbyName varchar(100),
@PositionX  int,
@PositionY int,
@Height  int,
@Width int

AS
UPDATE CUBBIES
SET 
CubbyName = @CubbyName,PositionX = @PositionX, PositionY  = @PositionY , Height = @Height, Width= @Width 
  WHERE CubbyId =@CubbyId

  

GO
/****** Object:  StoredProcedure [dbo].[editcubbyname]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[editcubbyname] 

@CubbyId int,
@CubbyName varchar(100)


AS
UPDATE CUBBIES 
SET CubbyName =@CubbyName
 
  WHERE CubbyId =@CubbyId
 
 
 UPDATE STORAGEPLACES
 SET CubbyName =@CubbyName
 
  WHERE CubbyId =@CubbyId



GO
/****** Object:  StoredProcedure [dbo].[editcubbysize]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[editcubbysize] 

@CubbyId int,
@Height int,
@Width int



AS
UPDATE CUBBIES 
SET Height =@Height , Width =@Width  
  WHERE CubbyId =@CubbyId




GO
/****** Object:  StoredProcedure [dbo].[edititem]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[edititem] 

@ItemID int,
@Name1 varchar(150),
@Name2  varchar(100),
@Name3 varchar(100),
@Barcode varchar(100),
@Picpath varchar(150),
@Active bit,
@Price float,
@MinInv int,
@Supplier varchar(100)


AS
UPDATE ITEMS 
SET ItemName1 =@Name1,ItemName2 = @Name2,ItemName3= @Name3, Barcode= @Barcode, PicPath= @PicPath, Active=@Active,Price = @Price, MinInventory = @MinInv, Supplier = @Supplier
 
  WHERE ItemId =@ItemID



GO
/****** Object:  StoredProcedure [dbo].[editstorageplace]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[editstorageplace]


@StorageId  int,
@StorageName  varchar(100),
@CurrentInventory int,
@DiffInventory int,
@MaxInventory int,
@ItemId int

AS

UPDATE STORAGEPLACES
SET 
StorageName =@StorageName, CurrentInventory = @CurrentInventory,MaxInventory =@MaxInventory
  WHERE StorageId  =@StorageId 

  
  UPDATE ITEMS_AVAILABILITY
SET 
Amount = Amount + @DiffInventory
  WHERE ItemId  =@ItemId
GO
/****** Object:  StoredProcedure [dbo].[editsupplier]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[editsupplier] 

@SupplierId int,
@Name varchar(150),
@Email  varchar(100)




AS
UPDATE SUPPLIERS 

SET SupplierName= @Name, SupplierEmail= @Email
 
  WHERE SupplierId =@SupplierId



GO
/****** Object:  StoredProcedure [dbo].[edituser]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[edituser] 

@UserId int,
@UserName varchar(100),
@Department varchar(100),
@Rights int,
@Password  varchar(100),
@Active bit


AS
UPDATE USERS 
SET UserName =@UserName,Department = @Department,Rights = @Rights, Password = @Password, Active= @Active
 
  WHERE UserId =@UserId

GO
/****** Object:  StoredProcedure [dbo].[editusersgroup]    Script Date: 02.04.2019 17:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[editusersgroup] 
	 @UserGroupId int,
     @UserGroupName varchar(100), 
     @OptionAdministrator bit,
     @OptionReceive bit,
     @OptionReceipt bit,
     @OptionEditUser bit,
     @OptionEditItem bit,
     @OptionEditStorageplace bit,
     @OptionReports bit,
     @OptionGroupUser bit,
     @OptionSupplier bit,
     @OptionCostcenter bit,
     @OptionStocktaking bit
AS
UPDATE dbo.USERGROUPS
SET  UserGroupName = @UserGroupName,OptionAdministrator=@OptionAdministrator,
    OptionReceive = @OptionReceive, OptionReceipt = @OptionReceipt,
	 OptionEditUser = @OptionEditUser, OptionEditItem = @OptionEditItem,
	 OptionEditStorageplace =@OptionEditStorageplace, OptionReports=@OptionReports,
     OptionGroupUser =@OptionGroupUser, OptionSupplier= @OptionSupplier,
     OptionCostcenter = @OptionCostcenter, OptionStocktaking = @OptionStocktaking
WHERE   UserGroupId= @UserGroupId;

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "USERGROUP"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 135
               Right = 252
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewUSERGROUPS'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewUSERGROUPS'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "USER"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 135
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 2
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewUSERS'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewUSERS'
GO
USE [master]
GO
ALTER DATABASE [rrl] SET  READ_WRITE 
GO
