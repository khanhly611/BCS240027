using System.ComponentModel.DataAnnotations;

namespace MID_BCS240027.Models
{
    public class DishCategory_BCS240027
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên loại không được để trống")]
        public string Name { get; set; }

        public string? Description { get; set; }

        public ICollection<Dish_BCS240027>? Dishes { get; set; }
    }
}