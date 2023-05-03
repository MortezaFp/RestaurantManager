SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROCEDURE [dbo].[GetFoodEarnings]
AS
BEGIN
    SELECT Foods.Name AS Food,
        SUM(OrderItems.Quantity * OrderItems.Price) AS Earnings
    FROM OrderItems
        INNER JOIN Foods ON OrderItems.FoodId = Foods.Id
    GROUP BY Foods.Name;
END;
GO
