CREATE TABLE [dbo].[Ticket]
(
	Id BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	TicketId nvarchar(100) NOT NULL UNIQUE,
	IssueType nvarchar(100) not null,
	Priority nvarchar(100) not null,
	IssueDate date not null,
	IssueDescription nvarchar(max),
	ContactPersonName nvarchar(100),
	CreatedBy nvarchar(100),
	UpdatedBy nvarchar(100),
	CreatedOn datetime2,
	UpdatedOn datetime2
	)