CREATE TABLE [dbo].[FoodCategories]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Name] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[FoodCategories] ADD CONSTRAINT [PK_FoodCategories] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
