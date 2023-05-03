namespace RestaurantManager.Models
{
    public partial class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int FoodId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }

        public virtual Food Food { get; set; } = null!;
        public virtual Order Order { get; set; } = null!;
    }
}