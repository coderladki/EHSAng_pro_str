CREATE TABLE [dbo].[ClientOrganization]
(
	Id BIGINT NOT NULL PRIMARY KEY,
	ClientId nvarchar(100) not null UNIQUE,
	Name nvarchar(100) not null,
	Email nvarchar(100) not null,
	ContactPerson nvarchar(100),
	CreatedBy nvarchar(100),
	CreatedDate datetime2,
	UpdatedBy nvarchar(100),
	UpdateDate datetime2
)
