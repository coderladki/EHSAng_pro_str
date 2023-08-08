
CREATE TABLE [dbo].[CustomerImplementation](
	[OrderAudit] [bit] NULL,
	[BQ] [bit] NULL,
	[Session1] [bit] NULL,
	[Session2] [bit] NULL,
	[Session3] [bit] NULL,
	[Session4] [bit] NULL,
	[Session5] [bit] NULL,
	[LeadId] [int] NOT NULL,
	[ClientId] [int] NOT NULL,
	[OrderId] [int] NULL,
	[ConfirmOrderAudit] [bit] NULL,
	[Done] [bit] NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[CustomerImplementation] ADD  DEFAULT ((0)) FOR [Done]
GO
