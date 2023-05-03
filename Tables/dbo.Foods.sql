CREATE TABLE [dbo].[Foods]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[CategoryId] [int] NULL,
[Name] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Price] [float] NOT NULL,
[PicturePath] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[IsActive] [bit] NOT NULL,
[InStock] [bit] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Foods] ADD CONSTRAINT [PK_Foods] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Foods] ADD CONSTRAINT [FK_Foods_FoodCategories] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[FoodCategories] ([Id])
GO
