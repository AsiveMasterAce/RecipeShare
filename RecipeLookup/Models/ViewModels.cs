using System.ComponentModel.DataAnnotations;

namespace RecipeLookup.Models
{
    public class ViewModels
    {
        public class RecipeIndexViewModel
        {
            public List<RecipeDto> Recipes { get; set; } = new List<RecipeDto>();
            public string SearchQuery { get; set; } = string.Empty;
            public string SelectedDietaryTag { get; set; } = string.Empty;
            public List<string> AvailableDietaryTags { get; set; } = new List<string>();
        }

        public class CreateRecipeViewModel
        {
            [Required(ErrorMessage = "Title is required")]
            public string Title { get; set; } = string.Empty;

            [Required(ErrorMessage = "At least one ingredient is required")]
            public string IngredientsText { get; set; } = string.Empty;
            public string StepsText { get; set; } = string.Empty;
            [Required(ErrorMessage = "Cooking time is required")]
            [Range(1, 1440, ErrorMessage = "Cooking time must be between 1 and 1440 minutes (24 hours)")]
            public int CookingTime { get; set; }
            [Display(Name = "Dietary Tags (comma separated, optional)")]
            public string DietaryTagsText { get; set; } = string.Empty;
        }

        public class EditRecipeViewModel
        {
            public int Id { get; set; }

            [Required(ErrorMessage = "Title is required")]
            public string Title { get; set; } = string.Empty;

            [Required(ErrorMessage = "At least one ingredient is required")]
            public string IngredientsText { get; set; } = string.Empty;

            public string StepsText { get; set; } = string.Empty;

            [Required(ErrorMessage = "Cooking time is required")]
            [Range(1, 1440, ErrorMessage = "Cooking time must be between 1 and 1440 minutes (24 hours)")]
            public int CookingTime { get; set; }

            [Display(Name = "Dietary Tags (comma separated, optional)")]
            public string DietaryTagsText { get; set; } = string.Empty;
        }
    }
}
