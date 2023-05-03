namespace RestaurantManager.Models
{
    public class RestaurantStatsViewModel
    {
        public double TotalEarnings { get; set; }
        public IEnumerable<FoodEarnings> FoodEarnings { get; set; }
        public IEnumerable<FoodQuantities> FoodQuantities { get; set; }
        public IEnumerable<CategoryEarnings> CategoryEarnings { get; set; }
    }

    public class FoodEarnings
    {
        public string Food { get; set; }
        public double Earnings { get; set; }
    }

    public class FoodQuantities
    {
        public string Food { get; set; }
        public int Quantity { get; set; }
    }

    public class CategoryEarnings
    {
        public string Category { get; set; }
        public double Earnings { get; set; }
    }
}