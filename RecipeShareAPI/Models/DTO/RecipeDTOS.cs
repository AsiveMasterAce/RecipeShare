using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;

namespace RecipeShareAPI.Models.DTO
{
    public class RecipeDTOS
    {
    }
    public class RecipeDto
    {
        public int Id { get; set; }
        public string? Title { get; set; } 
        public List<string> Ingredients { get; set; } = new();
        public List<string> Steps { get; set; } = new();
        public int? CookingTime { get; set; }
        public List<string> DietaryTags { get; set; } = new();
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public RecipeDto(Recipe recipe)
        {
            Id = recipe.ID;
            Title = recipe.Title;
            Ingredients = recipe.Ingredients?.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList() ?? new();
            Steps = recipe.Steps?.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList() ?? new();
            CookingTime = recipe.CookingTime;
            DietaryTags = recipe.DietaryTags?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() ?? new();
            CreatedAt = recipe.CreatedAt;
            UpdatedAt = recipe.UpdatedAt;
        }
    }
    public class CreateRecipeDto
    {
        [Required]
        public string? Title { get; set; } 
        [Required]
        public List<string> Ingredients { get; set; } = new();

        [Required]
        public List<string> Steps { get; set; } = new();

        [Range(1, 1440)]
        public int? CookingTime { get; set; }

        public List<string> DietaryTags { get; set; } = new();
    }
    public class UpdateRecipeDto
    {
        [Required]
        public string? Title { get; set; }

        [Required]
        public List<string> Ingredients { get; set; } = new();

        [Required]
        public List<string> Steps { get; set; } = new();
        [Range(1, 1440)]
        public int CookingTime { get; set; }

        public List<string> DietaryTags { get; set; } = new();
    }
}
