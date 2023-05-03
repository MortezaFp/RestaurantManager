namespace RestaurantManager.Areas.Manage.Models
{
    public class UserRoleViewModel
    {
        public string? UserId { get; set; }
        public string? Email { get; set; }
        public IEnumerable<string>? Roles { get; set; }
    }
}