using System.ComponentModel.DataAnnotations;

namespace MID_BCS240027.Models
{
    public class DishImage_BCS240027
    {
        public int Id { get; set; }

        [Required]
        public string ImageUrl { get; set; } = "";

        public bool IsThumbnail { get; set; }

        public int DishId { get; set; }

        public Dish_BCS240027? Dish { get; set; }
    }
}