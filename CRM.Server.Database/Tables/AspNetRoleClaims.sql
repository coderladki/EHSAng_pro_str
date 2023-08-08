
/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 10-09-2021 13:01:45 ******/


CREATE TABLE [dbo].[AspNetRoleClaims](

       [Id] [int] IDENTITY(1,1) NOT NULL,

       [RoleId] INT NOT NULL,

       [ClaimType] [nvarchar](max) NULL,

       [ClaimValue] [nvarchar](max) NULL,

CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED

(

       [Id] ASC

),

CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])

REFERENCES [dbo].[AspNetRoles] ([Id])

ON DELETE CASCADE
)