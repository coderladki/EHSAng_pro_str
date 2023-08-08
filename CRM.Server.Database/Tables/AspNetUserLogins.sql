CREATE TABLE [dbo].[AspNetUserLogins](

       [LoginProvider] INT NOT NULL,

       [ProviderKey] NVARCHAR(450) NOT NULL,

       [ProviderDisplayName] [nvarchar](max) NULL,

       [UserId] INT NOT NULL,

CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED

(

       [LoginProvider] ASC,

       [ProviderKey] ASC

),
CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])

REFERENCES [dbo].[AspNetUsers] ([Id])

ON DELETE CASCADE

) ON [PRIMARY]