using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManager.Models
{
    public partial class Food
    {
        public Food()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public double Price { get; set; }
        public string? PicturePath { get; set; }
        public bool IsActive { get; set; }
        public bool InStock { get; set; }

        [Required]
        [NotMapped]
        public IFormFile? PictureFile { get; set; }

        public virtual FoodCategory? Category { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}