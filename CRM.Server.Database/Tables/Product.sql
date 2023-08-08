CREATE TABLE [dbo].[Product]
(
	Id BIGINT NOT NULL PRIMARY KEY identity(1,1),
	Name nvarchar(100) not null ,
	Type nvarchar(100)  null,
	Version NVARCHAR(10) not NULL,
	Price Decimal(18,2) NOT NULL
)