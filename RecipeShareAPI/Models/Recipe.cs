using System.ComponentModel.DataAnnotations;

namespace RecipeShareAPI.Models
{
    public class Recipe
    {
        [Key]
        public int ID { get; set; }
        public string? Title { get; set; }
        public string? Ingredients { get; set; }
        public string? Steps { get; set; }
        public int? CookingTime { get; set; }
        public string? DietaryTags { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool Deleted { get; set; } = false;
    }
}