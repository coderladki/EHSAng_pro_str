CREATE TABLE [dbo].[Product]
(
	PId BIGINT NOT NULL PRIMARY KEY identity(1,1),
	PName nvarchar(100) not null ,
	PType nvarchar(100)  null,
	PVersion NVARCHAR(10) not NULL,
	PPrice Decimal(18,2) NOT NULL
)