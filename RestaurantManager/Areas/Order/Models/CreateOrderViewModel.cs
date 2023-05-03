using System.ComponentModel.DataAnnotations;

namespace RestaurantManager.Models
{
    public class CreateOrderViewModel
    {
        public double SubTotalPrice { get; set; }
        public double TotalPrice { get; set; }
        public bool IsTakeAway { get; set; }

        [Required]
        public string CustomerName { get; set; } = null!;

        public string? CustomerAddress { get; set; }
        public string? CustomerMobileNumber { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }

        public virtual ICollection<Adjustment> Adjustments { get; set; }
        public IEnumerable<Food> Foods { get; set; }
    }
}