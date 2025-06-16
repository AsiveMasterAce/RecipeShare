namespace RecipeLookup.Models
{
    public class Dtos
    {
    }
    public class RecipeDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<string> Ingredients { get; set; }
        public List<string> Steps { get; set; }
        public int CookingTime { get; set; }
        public List<string> DietaryTags { get; set; }
    }

    public class CreateRecipeDto
    {
        public string Title { get; set; }
        public List<string> Ingredients { get; set; }
        public List<string> Steps { get; set; }
        public int CookingTime { get; set; }
        public List<string> DietaryTags { get; set; }
    }

    public class UpdateRecipeDto
    {
        public string Title { get; set; }
        public List<string> Ingredients { get; set; }
        public List<string> Steps { get; set; }
        public int CookingTime { get; set; }
        public List<string> DietaryTags { get; set; }
    }
}
