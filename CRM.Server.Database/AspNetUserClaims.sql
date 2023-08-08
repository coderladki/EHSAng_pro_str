
CREATE TABLE [dbo].[AspNetUserClaims](

       [Id] [int] IDENTITY(1,1) NOT NULL,

       [UserId] [nvarchar](450) NOT NULL,

       [ClaimType] [nvarchar](max) NULL,

       [ClaimValue] [nvarchar](max) NULL,

CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED

(

       [Id] ASC

),
CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])

REFERENCES [dbo].[AspNetUsers] ([Id])

ON DELETE CASCADE


) ON [PRIMARY]