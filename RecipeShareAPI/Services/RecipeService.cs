using Microsoft.Extensions.Logging;
using RecipeShareAPI.Models;
using RecipeShareAPI.Models.DTO;
using RecipeShareAPI.Repositories;

namespace RecipeShareAPI.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly IRecipeRepository _repository;
        private readonly ILogger<RecipeService> _logger;

        public RecipeService(IRecipeRepository repository, ILogger<RecipeService> logger)
        {
            _repository = repository;
            _logger = logger;
            _logger.LogInformation("RecipeService initialized");
        }

        public async Task<IEnumerable<RecipeDto>> GetAllRecipesAsync()
        {
            _logger.LogInformation("Getting all recipes");
            try
            {
                var recipes = await _repository.GetAllAsync();
                _logger.LogInformation("Retrieved {Count} recipes", recipes.Count());
                return recipes.Select(r => new RecipeDto(r));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all recipes");
                throw;
            }
        }

        public async Task<RecipeDto?> GetRecipeByIdAsync(int id)
        {
            _logger.LogInformation("Getting recipe by ID: {Id}", id);
            try
            {
                var recipe = await _repository.GetByIdAsync(id);
                if (recipe != null)
                {
                    _logger.LogInformation("Found recipe: {Title}", recipe.Title);
                    return new RecipeDto(recipe);
                }
                else
                {
                    _logger.LogWarning("Recipe with ID {Id} not found", id);
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting recipe by ID {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<RecipeDto>> GetRecipesByDietaryTagAsync(string tag)
        {
            _logger.LogInformation("Getting recipes by dietary tag: {Tag}", tag);
            try
            {
                var recipes = await _repository.GetByDietaryTagAsync(tag);
                _logger.LogInformation("Found {Count} recipes with tag '{Tag}'", recipes.Count(), tag);
                return recipes.Select(r => new RecipeDto(r));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting recipes by tag '{Tag}'", tag);
                throw;
            }
        }

        public async Task<IEnumerable<RecipeDto>> SearchRecipesAsync(string searchTerm)
        {
            _logger.LogInformation("Searching recipes with term: {SearchTerm}", searchTerm);
            try
            {
                var recipes = await _repository.SearchAsync(searchTerm);
                _logger.LogInformation("Search returned {Count} results for '{SearchTerm}'", recipes.Count(), searchTerm);
                return recipes.Select(r => new RecipeDto(r));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching recipes with term '{SearchTerm}'", searchTerm);
                throw;
            }
        }

        public async Task<RecipeDto> CreateRecipeAsync(CreateRecipeDto createDto)
        {
            _logger.LogInformation("Creating new recipe: {Title}", createDto?.Title);
            try
            {
                ValidateCreateRecipeDto(createDto);
                _logger.LogInformation("Recipe validation passed");

                var recipe = new Recipe
                {
                    Title = createDto.Title,
                    Ingredients = string.Join("|", createDto.Ingredients),
                    Steps = string.Join("|", createDto.Steps),
                    CookingTime = createDto.CookingTime,
                    DietaryTags = string.Join(",", createDto.DietaryTags)
                };

                var created = await _repository.CreateAsync(recipe);
                _logger.LogInformation("Recipe created successfully with ID: {Id}", created.ID);
                return new RecipeDto(created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating recipe '{Title}'", createDto?.Title);
                throw;
            }
        }

        public async Task<RecipeDto?> UpdateRecipeAsync(int id, UpdateRecipeDto updateDto)
        {
            _logger.LogInformation("Updating recipe ID: {Id}, Title: {Title}", id, updateDto?.Title);
            try
            {
                ValidateUpdateRecipeDto(updateDto);
                _logger.LogInformation("Update validation passed");

                var recipe = new Recipe
                {
                    ID = id,
                    Title = updateDto.Title,
                    Ingredients = string.Join("|", updateDto.Ingredients),
                    Steps = string.Join("|", updateDto.Steps),
                    CookingTime = updateDto.CookingTime,
                    DietaryTags = string.Join(",", updateDto.DietaryTags)
                };

                var updated = await _repository.UpdateAsync(recipe);
                if (updated != null)
                {
                    _logger.LogInformation("Recipe ID {Id} updated successfully", id);
                    return new RecipeDto(updated);
                }
                else
                {
                    _logger.LogWarning("Recipe ID {Id} not found for update", id);
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating recipe ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> DeleteRecipeAsync(int id)
        {
            _logger.LogInformation("Deleting recipe ID: {Id}", id);
            try
            {
                bool result = await _repository.DeleteAsync(id);
                if (result)
                {
                    _logger.LogInformation("Recipe ID {Id} deleted successfully", id);
                }
                else
                {
                    _logger.LogWarning("Recipe ID {Id} not found for deletion", id);
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting recipe ID {Id}", id);
                throw;
            }
        }

        private void ValidateCreateRecipeDto(CreateRecipeDto createDto)
        {
            _logger.LogInformation("Validating create recipe data");
            if (createDto == null)
                throw new ArgumentNullException(nameof(createDto), "Recipe cannot be empty");

            if (string.IsNullOrWhiteSpace(createDto.Title))
                throw new ArgumentException("Title is required");

            if (createDto.CookingTime <= 0)
                throw new ArgumentException("Cooking time must be greater than zero");

            if (createDto.CookingTime > 1440)
                throw new ArgumentException("Cooking time cannot exceed 24 hours", nameof(createDto.CookingTime));

            if (createDto.Ingredients == null || !createDto.Ingredients.Any())
                throw new ArgumentException("At least one ingredient is required");

            if (createDto.Ingredients.Any(i => string.IsNullOrWhiteSpace(i)))
                throw new ArgumentException("Ingredients cannot have empty values");

            if (createDto.Steps == null || !createDto.Steps.Any())
                throw new ArgumentException("At least one step is required");

            if (createDto.Steps.Any(s => string.IsNullOrWhiteSpace(s)))
                throw new ArgumentException("Steps are required they cannot be empty");

            if (createDto.DietaryTags != null && createDto.DietaryTags.Any(t => string.IsNullOrWhiteSpace(t)))
                throw new ArgumentException("Dietary tags are required they cannot be empty");
        }

        private void ValidateUpdateRecipeDto(UpdateRecipeDto updateDto)
        {
            _logger.LogInformation("Validating update recipe data");

            if (updateDto == null)
                throw new ArgumentNullException(nameof(updateDto), "Recipe data cannot be null");

            if (string.IsNullOrWhiteSpace(updateDto.Title))
                throw new ArgumentException("Title is required");

            if (updateDto.CookingTime <= 0)
                throw new ArgumentException("Cooking time must be greater than zero");

            if (updateDto.CookingTime > 1440)
                throw new ArgumentException("Cooking time cannot exceed 24 hours");

            if (updateDto.Ingredients == null || !updateDto.Ingredients.Any())
                throw new ArgumentException("At least one ingredient is required");

            if (updateDto.Ingredients.Any(i => string.IsNullOrWhiteSpace(i)))
                throw new ArgumentException("Ingredients cannot have empty values");

            if (updateDto.Steps == null || !updateDto.Steps.Any())
                throw new ArgumentException("At least one step is required");

            if (updateDto.Steps.Any(s => string.IsNullOrWhiteSpace(s)))
                throw new ArgumentException("Steps are required they cannot be empty");

            if (updateDto.DietaryTags != null && updateDto.DietaryTags.Any(t => string.IsNullOrWhiteSpace(t)))
                throw new ArgumentException("Dietary tags are required they cannot be empty");
        }
    }
}
