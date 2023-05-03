CREATE TABLE [dbo].[Map_Orders_Adjustments]
(
[OrderId] [int] NOT NULL,
[AdjustmentId] [int] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Map_Orders_Adjustments] ADD CONSTRAINT [PK_Map_Orders_Adjustments] PRIMARY KEY CLUSTERED  ([OrderId], [AdjustmentId]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Map_Orders_Adjustments] ADD CONSTRAINT [FK_Map_Orders_Adjustments_Adjustments] FOREIGN KEY ([AdjustmentId]) REFERENCES [dbo].[Adjustments] ([Id])
GO
ALTER TABLE [dbo].[Map_Orders_Adjustments] ADD CONSTRAINT [FK_Map_Orders_Adjustments_Orders] FOREIGN KEY ([OrderId]) REFERENCES [dbo].[Orders] ([Id]) ON DELETE CASCADE
GO
