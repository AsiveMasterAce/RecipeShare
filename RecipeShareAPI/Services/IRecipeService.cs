using RecipeShareAPI.Models.DTO;

namespace RecipeShareAPI.Services
{
    public interface IRecipeService
    {
        Task<IEnumerable<RecipeDto>> GetAllRecipesAsync();
        Task<RecipeDto?> GetRecipeByIdAsync(int id);
        Task<IEnumerable<RecipeDto>> GetRecipesByDietaryTagAsync(string tag);
        Task<IEnumerable<RecipeDto>> SearchRecipesAsync(string searchTerm);
        Task<RecipeDto> CreateRecipeAsync(CreateRecipeDto createDto);
        Task<RecipeDto?> UpdateRecipeAsync(int id, UpdateRecipeDto updateDto);
        Task<bool> DeleteRecipeAsync(int id);
    }
}
