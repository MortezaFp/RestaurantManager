CREATE TABLE [dbo].[Adjustments]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Name] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Amount] [int] NOT NULL,
[IsMandatory] [bit] NOT NULL,
[IsActive] [bit] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Adjustments] ADD CONSTRAINT [PK_Adjustments] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
