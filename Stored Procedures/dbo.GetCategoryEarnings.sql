SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROCEDURE [dbo].[GetCategoryEarnings]
AS
BEGIN
    SELECT FoodCategories.Name AS Category,
        SUM(OrderItems.Quantity * OrderItems.Price) AS Earnings
    FROM OrderItems
        INNER JOIN Foods ON OrderItems.FoodId = Foods.Id
        INNER JOIN FoodCategories ON Foods.CategoryId = FoodCategories.Id
    GROUP BY FoodCategories.Name;
END;
GO
