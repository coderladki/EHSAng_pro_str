
CREATE TABLE [dbo].[AspNetRoles](

       [Id] INT NOT NULL,

       [Name] [nvarchar](256) NULL,

       [NormalizedName] [nvarchar](256) NULL,

       [ConcurrencyStamp] [nvarchar](max) NULL,

       [CreatedDateTimeUtc] datetime not null,

       [UpdatedDateTimeUtc] datetime not null,

CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED

(

       [Id] ASC

)

) ON [PRIMARY]
