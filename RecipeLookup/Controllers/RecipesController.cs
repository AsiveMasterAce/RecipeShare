using Microsoft.AspNetCore.Mvc;
using RecipeLookup.Models;
using RecipeLookup.Services;
using static RecipeLookup.Models.ViewModels;

namespace RecipeLookup.Controllers
{
    public class RecipesController : Controller
    {
        private readonly IRecipeClientService _recipeService;

        public RecipesController(IRecipeClientService recipeService)
        {
            _recipeService = recipeService;
        }
        public async Task<IActionResult> Index(string searchQuery = "", string dietaryTag = "")
        {
            var viewModel = new RecipeIndexViewModel();

   
            var recipes = await _recipeService.GetAllRecipesAsync();


            if (!string.IsNullOrEmpty(searchQuery))
            {
                recipes = await _recipeService.SearchRecipesAsync(searchQuery);
            }

            if (!string.IsNullOrEmpty(dietaryTag))
            {
                recipes = await _recipeService.GetRecipesByDietaryTagAsync(dietaryTag);
            }


            var allRecipes = await _recipeService.GetAllRecipesAsync();

            viewModel.AvailableDietaryTags = allRecipes
                .SelectMany(r => r.DietaryTags)
                .Distinct()
                .OrderBy(t => t)
                .ToList();


            viewModel.Recipes = recipes.ToList();
            viewModel.SearchQuery = searchQuery;
            viewModel.SelectedDietaryTag = dietaryTag;

            return View(viewModel);
        }
        public async Task<IActionResult> Details(int id)
        {
            var recipe = await _recipeService.GetRecipeByIdAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }
            return View(recipe);
        }

        public IActionResult Create()
        {
           var viewM = new CreateRecipeViewModel();
            return View(viewM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRecipeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var createDto = new CreateRecipeDto
                {
                    Title = viewModel.Title,
                    Ingredients = ParseTextToList(viewModel.IngredientsText),
                    Steps = ParseTextToList(viewModel.StepsText),
                    CookingTime = viewModel.CookingTime,
                    DietaryTags = ParseTextToList(viewModel.DietaryTagsText)
                };

                var createdRecipe = await _recipeService.CreateRecipeAsync(createDto);
                if (createdRecipe != null)
                {
                    TempData["Success"] = "Recipe created successfully!";
                    return RedirectToAction(nameof(Details), new { id = createdRecipe.Id });
                }
                else
                {
                    TempData["Error"] = "Failed to create recipe. Please check your input and try again.";
                }
            }

            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var recipe = await _recipeService.GetRecipeByIdAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }

            var viewModel = new EditRecipeViewModel
            {
                Id = recipe.Id,
                Title = recipe.Title,
                //IngredientsText = string.Join("| ", recipe.Ingredients),
                IngredientsText = string.Join(Environment.NewLine, recipe.Ingredients),
                StepsText = string.Join(Environment.NewLine, recipe.Steps),
                CookingTime = recipe.CookingTime,
                //DietaryTagsText = string.Join(", ", recipe.DietaryTags)
                DietaryTagsText = string.Join(Environment.NewLine, recipe.DietaryTags)
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditRecipeViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var updateDto = new UpdateRecipeDto
                {
                    Title = viewModel.Title,
                    Ingredients = ParseTextToList(viewModel.IngredientsText),
                    Steps = ParseTextToList(viewModel.StepsText),
                    CookingTime = viewModel.CookingTime,
                    DietaryTags = ParseTextToList(viewModel.DietaryTagsText)
                };

                var updatedRecipe = await _recipeService.UpdateRecipeAsync(id, updateDto);
                if (updatedRecipe != null)
                {
                    TempData["Success"] = "Recipe updated successfully!";
                    return RedirectToAction(nameof(Details), new { id = updatedRecipe.Id });
                }
                else
                {
                    TempData["Error"] = "Failed to update recipe. Please check your input and try again.";
                }
            }

            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deleted = await _recipeService.DeleteRecipeAsync(id);
            if (deleted)
            {
                TempData["Success"] = "Recipe deleted successfully!";
            }
            else
            {
                TempData["Error"] = "Failed to delete recipe.";
            }
            return RedirectToAction(nameof(Index));
        }

        private List<string> ParseTextToList(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return new List<string>();

            return text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                      .Select(s => s.Trim())
                      .Where(s => !string.IsNullOrWhiteSpace(s))
                      .ToList();
        }
    }

}
