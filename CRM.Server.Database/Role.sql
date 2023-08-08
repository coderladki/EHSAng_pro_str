CREATE TABLE [dbo].[Role]
(
	[Id] tinyint NOT NULL PRIMARY KEY IDENTITY(1,1),
	[Name] NVARCHAR(100) NOT NULL,
	[Description] NVARCHAR(300) 
)
