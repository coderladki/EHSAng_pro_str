CREATE TABLE [dbo].[Reminder]
(
	[Id] Bigint NOT NULL PRIMARY KEY identity(1,1),
	Date date null,
	Time time null,
	EventName nvarchar(250),
	EventDetail nvarchar(500)
)
