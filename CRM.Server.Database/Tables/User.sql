CREATE TABLE [dbo].[User]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	FirstName nvarchar(100) not null,
	LastName nvarchar(100) not null,
	UserName nvarchar(100) not null UNIQUE,
	Email nvarchar(100) not null,
	Salt uniqueidentifier not null,
	PasswordHash binary(64) not null,
	Status nvarchar(20) not null,
	Gender SMALLINT not null,
	[CreatedDateTimeUtc] datetime not null,
	[UpdatedDateTimeUtc] datetime not null,
)
