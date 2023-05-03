namespace RestaurantManager.Models
{
    public partial class Adjustment
    {
        public Adjustment()
        {
            MapOrdersAdjustments = new HashSet<MapOrderAdjustment>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public bool IsMandatory { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<MapOrderAdjustment> MapOrdersAdjustments { get; set; }
    }
}