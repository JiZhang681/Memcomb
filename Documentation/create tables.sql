/*
CREATE TABLE [dbo].[Users](
	[User_ID] [int] NOT NULL IDENTITY PRIMARY KEY,
	[First_Name] [varchar](50) NOT NULL,
	[Last_Name] [varchar](50) NOT NULL,
	[Email_ID] [varchar](50) NOT NULL,
	[Password] [varchar](50) NOT NULL,
	[Date_Of_Birth] [date] NULL,
	[Phone_Number] [varchar](10) NULL,
	[Profile_Picture] [varchar](100) NULL,
	[Biography] [varchar](300) NULL,
	[Is_Verified] [bit] NOT NULL DEFAULT 0,
	[Activation_Code] [varchar](16) NULL,
	[Is_Admin] [bit] NOT NULL DEFAULT 0
	)
*/

/*
CREATE TABLE [dbo].[Following](
	[User_Following] [int] NOT NULL,
	[User_Followed] [int] NOT NULL,
	CONSTRAINT PK_Following PRIMARY KEY NONCLUSTERED ([User_Following],[User_Followed]),
	CONSTRAINT FK_Following_Users FOREIGN KEY ([User_Following])
	REFERENCES dbo.Users([User_ID])
	ON DELETE CASCADE
	ON UPDATE CASCADE
)
*/

/*
CREATE TABLE [dbo].[Memories](
	[Memory_ID] [int] NOT NULL IDENTITY PRIMARY KEY,
	[User_ID] [int] NOT NULL,
	[Date_Created] [date],
	[Memory_Title] [varchar](50) NOT NULL,
	[Memory_Description] [varchar](300) NOT NULL,
	CONSTRAINT FK_Users FOREIGN KEY ([User_ID])
	REFERENCES dbo.Users([User_ID])
	ON DELETE CASCADE
	ON UPDATE CASCADE
)
*/

/*
CREATE TABLE [dbo].[Fragments](
	[Fragment_ID] [int] NOT NULL IDENTITY PRIMARY KEY,
	[Memory_ID] [int] NOT NULL,
	[Date_Posted] [datetime],
	[Fragment_Date] [datetime] NOT NULL,
	[Fragment_Location] [varchar](50) NOT NULL,
	[Memory_Description] [text],
	[Fragment_Data] [varbinary] NOT NULL,
	[Is_Highlight] [bit] NOT NULL DEFAULT 0,
	CONSTRAINT FK_Memories FOREIGN KEY ([Memory_ID])
	REFERENCES dbo.Memories([Memory_ID])
	ON DELETE CASCADE
	ON UPDATE CASCADE
)
*/

/*
CREATE TABLE [dbo].[Comments](
	[Comment_ID] [int] NOT NULL IDENTITY PRIMARY KEY,
	[Fragment_ID] [int] NOT NULL,
	[User_ID] [int] NOT NULL,
	[Datetime_Posted] [datetime],
	[Comment] [text] NOT NULL,
	CONSTRAINT FK_Fragments FOREIGN KEY ([Fragment_ID])
	REFERENCES dbo.Fragments([Fragment_ID])
	ON DELETE CASCADE
	ON UPDATE CASCADE
)
*/