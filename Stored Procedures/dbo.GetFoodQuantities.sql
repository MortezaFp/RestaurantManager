SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROCEDURE [dbo].[GetFoodQuantities]
AS
BEGIN
    SELECT Foods.Name AS Food,
        SUM(OrderItems.Quantity) AS Quantity
    FROM OrderItems
        INNER JOIN Foods ON OrderItems.FoodId = Foods.Id
    GROUP BY Foods.Name;
END;
GO
