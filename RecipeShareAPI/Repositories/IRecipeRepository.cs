using RecipeShareAPI.Models;

namespace RecipeShareAPI.Repositories
{
    public interface IRecipeRepository
    {
        Task<IEnumerable<Recipe>> GetAllAsync();
        Task<Recipe?> GetByIdAsync(int id);
        Task<IEnumerable<Recipe>> GetByDietaryTagAsync(string tag);
        Task<IEnumerable<Recipe>> SearchAsync(string searchTerm);
        Task<Recipe> CreateAsync(Recipe recipe);
        Task<Recipe?> UpdateAsync(Recipe recipe);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
