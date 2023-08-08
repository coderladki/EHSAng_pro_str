CREATE TABLE [dbo].[Schedule]
(
	[Id] INT NOT NULL PRIMARY KEY,

	FirstConsulting nvarchar(500),

	ShortIntro nvarchar(500),

	AgreeMeetingTime time
)