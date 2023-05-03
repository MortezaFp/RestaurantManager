namespace RestaurantManager.Models
{
    public partial class Order
    {
        public Order()
        {
            MapOrdersAdjustments = new HashSet<MapOrderAdjustment>();
            OrderItems = new HashSet<OrderItem>();
        }

        public int Id { get; set; }
        public double SubTotalPrice { get; set; }
        public double TotalPrice { get; set; }
        public bool IsTakeAway { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerMobileNumber { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<MapOrderAdjustment> MapOrdersAdjustments { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}