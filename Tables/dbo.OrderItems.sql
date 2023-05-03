CREATE TABLE [dbo].[OrderItems]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[OrderId] [int] NOT NULL,
[FoodId] [int] NOT NULL,
[Quantity] [int] NOT NULL,
[Price] [float] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[OrderItems] ADD CONSTRAINT [PK_OrderItems] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[OrderItems] ADD CONSTRAINT [FK_OrderItems_Food] FOREIGN KEY ([FoodId]) REFERENCES [dbo].[Foods] ([Id])
GO
ALTER TABLE [dbo].[OrderItems] ADD CONSTRAINT [FK_OrderItems_Orders] FOREIGN KEY ([OrderId]) REFERENCES [dbo].[Orders] ([Id]) ON DELETE CASCADE
GO
