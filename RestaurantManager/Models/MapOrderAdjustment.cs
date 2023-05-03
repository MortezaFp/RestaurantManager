namespace RestaurantManager.Models
{
    public partial class MapOrderAdjustment
    {
        public int OrderId { get; set; }
        public int AdjustmentId { get; set; }

        public virtual Adjustment Adjustment { get; set; }
        public virtual Order Order { get; set; }
    }
}