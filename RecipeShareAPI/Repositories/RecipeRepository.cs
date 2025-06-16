using Microsoft.EntityFrameworkCore;
using RecipeShareAPI.Data;
using RecipeShareAPI.Models;

namespace RecipeShareAPI.Repositories
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly ApplicationDBContext _context;
        public RecipeRepository(ApplicationDBContext context) 
        { 
            _context = context;
        }
        public async Task<IEnumerable<Recipe>> GetAllAsync()
        {
            return await _context.Recipes
                .OrderByDescending(r => r.CreatedAt)
                .Where(r => (bool)!r.Deleted)
                .ToListAsync();
        }

        public async Task<Recipe?> GetByIdAsync(int id)
        {
            return await _context.Recipes
                .FirstOrDefaultAsync(r => r.ID == id);
        }

        public async Task<IEnumerable<Recipe>> GetByDietaryTagAsync(string tag)
        {
            return await _context.Recipes
                .Where(r => r.DietaryTags.Contains(tag)&& !r.Deleted)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Recipe>> SearchAsync(string searchTerm)
        {
            return await _context.Recipes
                .Where(r => r.Title.Contains(searchTerm) ||
                           r.Ingredients.Contains(searchTerm) ||
                           r.DietaryTags.Contains(searchTerm))
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<Recipe> CreateAsync(Recipe recipe)
        {
            recipe.CreatedAt = DateTime.UtcNow;
            recipe.UpdatedAt = DateTime.UtcNow;

            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();
            return recipe;
        }

        public async Task<Recipe?> UpdateAsync(Recipe recipe)
        {
            var existingRecipe = await _context.Recipes.FindAsync(recipe.ID);
            if (existingRecipe == null) return null;

            existingRecipe.Title = recipe.Title;
            existingRecipe.Ingredients = recipe.Ingredients;
            existingRecipe.Steps = recipe.Steps;
            existingRecipe.CookingTime = recipe.CookingTime;
            existingRecipe.DietaryTags = recipe.DietaryTags;
            existingRecipe.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingRecipe;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null) return false;

             recipe.Deleted = true;
             _context.Recipes.Update(recipe);
             await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Recipes.AnyAsync(r=>r.ID==id);
        }
    }
}
