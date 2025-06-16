using RecipeLookup.Models;

namespace RecipeLookup.Services
{
    public interface IRecipeClientService
    {
        Task<IEnumerable<RecipeDto>> GetAllRecipesAsync();
        Task<RecipeDto?> GetRecipeByIdAsync(int id);
        Task<IEnumerable<RecipeDto>> GetRecipesByDietaryTagAsync(string tag);
        Task<IEnumerable<RecipeDto>> SearchRecipesAsync(string searchTerm);
        Task<RecipeDto> CreateRecipeAsync(CreateRecipeDto recipe);
        Task<RecipeDto?> UpdateRecipeAsync(int id, UpdateRecipeDto recipe);
        Task<bool> DeleteRecipeAsync(int id);
    }
}
