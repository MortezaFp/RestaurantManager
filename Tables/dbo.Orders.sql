CREATE TABLE [dbo].[Orders]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[SubTotalPrice] [float] NOT NULL,
[TotalPrice] [float] NOT NULL,
[IsTakeAway] [bit] NOT NULL,
[CustomerName] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CustomerAddress] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CustomerMobileNumber] [varchar] (11) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CreatedAt] [datetime] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Orders] ADD CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
