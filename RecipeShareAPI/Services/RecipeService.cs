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
        }
        public async Task<IEnumerable<RecipeDto>> GetAllRecipesAsync()
        {
            var recipes = await _repository.GetAllAsync();
            return recipes.Select(r => new RecipeDto(r));
        }

        public async Task<RecipeDto?> GetRecipeByIdAsync(int id)
        {
            var recipe = await _repository.GetByIdAsync(id);
            return recipe != null ? new RecipeDto(recipe) : null;
        }

        public async Task<IEnumerable<RecipeDto>> GetRecipesByDietaryTagAsync(string tag)
        {
            var recipes = await _repository.GetByDietaryTagAsync(tag);
            return recipes.Select(r => new RecipeDto(r));
        }

        public async Task<IEnumerable<RecipeDto>> SearchRecipesAsync(string searchTerm)
        {
            var recipes = await _repository.SearchAsync(searchTerm);
            return recipes.Select(r => new RecipeDto(r));
        }

        public async Task<RecipeDto> CreateRecipeAsync(CreateRecipeDto createDto)
        {

            ValidateCreateRecipeDto(createDto);

            var recipe = new Recipe
            {
                Title = createDto.Title,
                Ingredients = string.Join("|", createDto.Ingredients),
                Steps = string.Join("|", createDto.Steps),
                CookingTime = createDto.CookingTime,
                DietaryTags = string.Join(",", createDto.DietaryTags)
            };

            var created = await _repository.CreateAsync(recipe);
            return new RecipeDto(created);
        }

        public async Task<RecipeDto?> UpdateRecipeAsync(int id, UpdateRecipeDto updateDto)
        {

            ValidateUpdateRecipeDto(updateDto);
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
            return updated != null ? new RecipeDto(updated) : null;
        }

        public async Task<bool> DeleteRecipeAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        private void ValidateCreateRecipeDto(CreateRecipeDto createDto)
        {
            if (createDto == null)
            {
                throw new ArgumentNullException(nameof(createDto), "Recipe cannot be empty");
            }
            if (string.IsNullOrWhiteSpace(createDto.Title))
            {
                throw new ArgumentException("Title is required");
            }
 
            if (createDto.CookingTime <= 0)
            {
                throw new ArgumentException("Cooking time must be greater than zero");
            }

            if (createDto.CookingTime > 1440) 
            {
                throw new ArgumentException("Cooking time cannot exceed 24 hours", nameof(createDto.CookingTime));
            }

            if (createDto.Ingredients == null || !createDto.Ingredients.Any())
            {
                throw new ArgumentException("At least one ingredient is required");
            }

            if (createDto.Ingredients.Any(i => string.IsNullOrWhiteSpace(i)))
            {
                throw new ArgumentException("Ingredients cannot have empty values");
            }

            if (createDto.Steps == null || !createDto.Steps.Any())
            {
                throw new ArgumentException("At least one step is required");
            }

            if (createDto.Steps.Any(s => string.IsNullOrWhiteSpace(s)))
            {
                throw new ArgumentException("Steps are required they cannot be empty");
            }

            if (createDto.DietaryTags != null && createDto.DietaryTags.Any(t => string.IsNullOrWhiteSpace(t)))
            {
                throw new ArgumentException("Dietary tags are required they cannot be empty");
            }
        }
        private void ValidateUpdateRecipeDto(UpdateRecipeDto updateDto)
        {
            if (updateDto == null)
            {
                throw new ArgumentNullException(nameof(updateDto), "Recipe data cannot be null");
            }

            if (string.IsNullOrWhiteSpace(updateDto.Title))
            {
                throw new ArgumentException("Title is required");
            }

            if (updateDto.CookingTime <= 0)
            {
                throw new ArgumentException("Cooking time must be greater than zero");
            }

            if (updateDto.CookingTime > 1440) 
            {
                throw new ArgumentException("Cooking time cannot exceed 24 hours");
            }

            if (updateDto.Ingredients == null || !updateDto.Ingredients.Any())
            {
                throw new ArgumentException("At least one ingredient is required");
            }

            if (updateDto.Ingredients.Any(i => string.IsNullOrWhiteSpace(i)))
            {
                throw new ArgumentException("Ingredients cannot have empty values");
            }

            if (updateDto.Steps == null || !updateDto.Steps.Any())
            {
                throw new ArgumentException("At least one step is required");
            }

            if (updateDto.Steps.Any(s => string.IsNullOrWhiteSpace(s)))
            {
                throw new ArgumentException("Steps are required they cannot be empty");
            }

            if (updateDto.DietaryTags != null && updateDto.DietaryTags.Any(t => string.IsNullOrWhiteSpace(t)))
            {
                throw new ArgumentException("Dietary tags are required they cannot be empty");
            }
        }
    }
}
