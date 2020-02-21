CREATE TABLE [dbo].[Users] (
  [User_ID] INT UNIQUE NOT NULL,
  [First_Name] VARCHAR(50) NOT NULL,
  [Last_Name] VARCHAR(50) NOT NULL,
  [Email_ID] VARCHAR(50) UNIQUE NOT NULL,
  [Password] VARCHAR(50) NOT NULL,
  [Date_Of_Birth] DATE NULL,
  [Phone_Number] INT NULL,
  [Profile_Picture] VARCHAR(100) NULL,
  [Biography] VARCHAR(300) NULL,
  [Is_Verified] BIT NOT NULL DEFAULT 0,
  [Activation_Code] VARCHAR(16) UNIQUE NULL,
  [Is_Admin] BIT NOT NULL DEFAULT 0,
  PRIMARY KEY ([User_ID])
);