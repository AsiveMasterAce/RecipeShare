using Microsoft.AspNetCore.Http.HttpResults;
using RecipeShareAPI.Data;
using RecipeShareAPI.Models;
using RecipeShareAPI.Models.DTO;
using RecipeShareAPI.Repositories;

namespace RecipeShareAPI.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly IRecipeRepository _repository;
        public RecipeService(IRecipeRepository repository)
        {
            _repository = repository;
            Console.WriteLine("[RecipeService] Service initialized");
        }

        public async Task<IEnumerable<RecipeDto>> GetAllRecipesAsync()
        {
            Console.WriteLine("[RecipeService] Getting all recipes");
            try
            {
                var recipes = await _repository.GetAllAsync();
                Console.WriteLine($"[RecipeService] Retrieved {recipes.Count()} recipes");
                return recipes.Select(r => new RecipeDto(r));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[RecipeService] Error getting all recipes: {ex.Message}");
                throw;
            }
        }

        public async Task<RecipeDto?> GetRecipeByIdAsync(int id)
        {
            Console.WriteLine($"[RecipeService] Getting recipe by ID: {id}");
            try
            {
                var recipe = await _repository.GetByIdAsync(id);
                if (recipe != null)
                {
                    Console.WriteLine($"[RecipeService] Found recipe: {recipe.Title}");
                    return new RecipeDto(recipe);
                }
                else
                {
                    Console.WriteLine($"[RecipeService] Recipe with ID {id} not found");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[RecipeService] Error getting recipe by ID {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<RecipeDto>> GetRecipesByDietaryTagAsync(string tag)
        {
            Console.WriteLine($"[RecipeService] Getting recipes by dietary tag: {tag}");
            try
            {
                var recipes = await _repository.GetByDietaryTagAsync(tag);
                Console.WriteLine($"[RecipeService] Found {recipes.Count()} recipes with tag '{tag}'");
                return recipes.Select(r => new RecipeDto(r));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[RecipeService] Error getting recipes by tag '{tag}': {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<RecipeDto>> SearchRecipesAsync(string searchTerm)
        {
            Console.WriteLine($"[RecipeService] Searching recipes with term: {searchTerm}");
            try
            {
                var recipes = await _repository.SearchAsync(searchTerm);
                Console.WriteLine($"[RecipeService] Search returned {recipes.Count()} results for '{searchTerm}'");
                return recipes.Select(r => new RecipeDto(r));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[RecipeService] Error searching recipes with term '{searchTerm}': {ex.Message}");
                throw;
            }
        }

        public async Task<RecipeDto> CreateRecipeAsync(CreateRecipeDto createDto)
        {
            Console.WriteLine($"[RecipeService] Creating new recipe: {createDto?.Title}");
            try
            {
                ValidateCreateRecipeDto(createDto);
                Console.WriteLine("[RecipeService] Recipe validation passed");

                var recipe = new Recipe
                {
                    Title = createDto.Title,
                    Ingredients = string.Join("|", createDto.Ingredients),
                    Steps = string.Join("|", createDto.Steps),
                    CookingTime = createDto.CookingTime,
                    DietaryTags = string.Join(",", createDto.DietaryTags)
                };

                var created = await _repository.CreateAsync(recipe);
                Console.WriteLine($"[RecipeService] Recipe created successfully with ID: {created.ID}");
                return new RecipeDto(created);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[RecipeService] Error creating recipe '{createDto?.Title}': {ex.Message}");
                throw;
            }
        }

        public async Task<RecipeDto?> UpdateRecipeAsync(int id, UpdateRecipeDto updateDto)
        {
            Console.WriteLine($"[RecipeService] Updating recipe ID: {id}, Title: {updateDto?.Title}");
            try
            {
                ValidateUpdateRecipeDto(updateDto);
                Console.WriteLine("[RecipeService] Update validation passed");

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
                    Console.WriteLine($"[RecipeService] Recipe ID {id} updated successfully");
                    return new RecipeDto(updated);
                }
                else
                {
                    Console.WriteLine($"[RecipeService] Recipe ID {id} not found for update");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[RecipeService] Error updating recipe ID {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteRecipeAsync(int id)
        {
            Console.WriteLine($"[RecipeService] Deleting recipe ID: {id}");
            try
            {
                bool result = await _repository.DeleteAsync(id);
                if (result)
                {
                    Console.WriteLine($"[RecipeService] Recipe ID {id} deleted successfully");
                }
                else
                {
                    Console.WriteLine($"[RecipeService] Recipe ID {id} not found for deletion");
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[RecipeService] Error deleting recipe ID {id}: {ex.Message}");
                throw;
            }
        }

        private void ValidateCreateRecipeDto(CreateRecipeDto createDto)
        {
            Console.WriteLine("[RecipeService] Validating create recipe data");
            if (createDto == null)
            {
                Console.WriteLine("[RecipeService] Validation failed: Recipe cannot be empty");
                throw new ArgumentNullException(nameof(createDto), "Recipe cannot be empty");
            }
            if (string.IsNullOrWhiteSpace(createDto.Title))
            {
                Console.WriteLine("[RecipeService] Validation failed: Title is required");
                throw new ArgumentException("Title is required");
            }

            if (createDto.CookingTime <= 0)
            {
                Console.WriteLine("[RecipeService] Validation failed: Cooking time must be greater than zero");
                throw new ArgumentException("Cooking time must be greater than zero");
            }

            if (createDto.CookingTime > 1440)
            {
                Console.WriteLine("[RecipeService] Validation failed: Cooking time cannot exceed 24 hours");
                throw new ArgumentException("Cooking time cannot exceed 24 hours", nameof(createDto.CookingTime));
            }

            if (createDto.Ingredients == null || !createDto.Ingredients.Any())
            {
                Console.WriteLine("[RecipeService] Validation failed: At least one ingredient is required");
                throw new ArgumentException("At least one ingredient is required");
            }

            if (createDto.Ingredients.Any(i => string.IsNullOrWhiteSpace(i)))
            {
                Console.WriteLine("[RecipeService] Validation failed: Ingredients cannot have empty values");
                throw new ArgumentException("Ingredients cannot have empty values");
            }

            if (createDto.Steps == null || !createDto.Steps.Any())
            {
                Console.WriteLine("[RecipeService] Validation failed: At least one step is required");
                throw new ArgumentException("At least one step is required");
            }

            if (createDto.Steps.Any(s => string.IsNullOrWhiteSpace(s)))
            {
                Console.WriteLine("[RecipeService] Validation failed: Steps are required they cannot be empty");
                throw new ArgumentException("Steps are required they cannot be empty");
            }

            if (createDto.DietaryTags != null && createDto.DietaryTags.Any(t => string.IsNullOrWhiteSpace(t)))
            {
                Console.WriteLine("[RecipeService] Validation failed: Dietary tags are required they cannot be empty");
                throw new ArgumentException("Dietary tags are required they cannot be empty");
            }
        }

        private void ValidateUpdateRecipeDto(UpdateRecipeDto updateDto)
        {
            Console.WriteLine("[RecipeService] Validating update recipe data");
            if (updateDto == null)
            {
                Console.WriteLine("[RecipeService] Validation failed: Recipe data cannot be null");
                throw new ArgumentNullException(nameof(updateDto), "Recipe data cannot be null");
            }

            if (string.IsNullOrWhiteSpace(updateDto.Title))
            {
                Console.WriteLine("[RecipeService] Validation failed: Title is required");
                throw new ArgumentException("Title is required");
            }

            if (updateDto.CookingTime <= 0)
            {
                Console.WriteLine("[RecipeService] Validation failed: Cooking time must be greater than zero");
                throw new ArgumentException("Cooking time must be greater than zero");
            }

            if (updateDto.CookingTime > 1440)
            {
                Console.WriteLine("[RecipeService] Validation failed: Cooking time cannot exceed 24 hours");
                throw new ArgumentException("Cooking time cannot exceed 24 hours");
            }

            if (updateDto.Ingredients == null || !updateDto.Ingredients.Any())
            {
                Console.WriteLine("[RecipeService] Validation failed: At least one ingredient is required");
                throw new ArgumentException("At least one ingredient is required");
            }

            if (updateDto.Ingredients.Any(i => string.IsNullOrWhiteSpace(i)))
            {
                Console.WriteLine("[RecipeService] Validation failed: Ingredients cannot have empty values");
                throw new ArgumentException("Ingredients cannot have empty values");
            }

            if (updateDto.Steps == null || !updateDto.Steps.Any())
            {
                Console.WriteLine("[RecipeService] Validation failed: At least one step is required");
                throw new ArgumentException("At least one step is required");
            }

            if (updateDto.Steps.Any(s => string.IsNullOrWhiteSpace(s)))
            {
                Console.WriteLine("[RecipeService] Validation failed: Steps are required they cannot be empty");
                throw new ArgumentException("Steps are required they cannot be empty");
            }

            if (updateDto.DietaryTags != null && updateDto.DietaryTags.Any(t => string.IsNullOrWhiteSpace(t)))
            {
                Console.WriteLine("[RecipeService] Validation failed: Dietary tags are required they cannot be empty");
                throw new ArgumentException("Dietary tags are required they cannot be empty");
            }
        }
    }
}