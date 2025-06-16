using Microsoft.AspNetCore.Mvc;
using RecipeShareAPI.Models.DTO;
using RecipeShareAPI.Services;

namespace RecipeShareAPI.Controllers
{
    [ApiController]
      [Route("api/[controller]/[action]")]

    public class RecipeController : Controller
    {
        private readonly IRecipeService _recipeService;
        public RecipeController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecipeDto>>> GetRecipes()
        {
            var recipes = await _recipeService.GetAllRecipesAsync();
            return Ok(recipes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RecipeDto>> GetRecipe(int id)
        {
            var recipe = await _recipeService.GetRecipeByIdAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }
            return Ok(recipe);
        }


        [HttpGet("dietary/{tag}")]
        public async Task<ActionResult<IEnumerable<RecipeDto>>> GetRecipesByDietaryTag(string tag)
        {
            var recipes = await _recipeService.GetRecipesByDietaryTagAsync(tag);
            return Ok(recipes);
        }
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<RecipeDto>>> SearchRecipes([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                return BadRequest("Search term cannot be empty.");
            }

            var recipes = await _recipeService.SearchRecipesAsync(q);
            return Ok(recipes);
        }


        [HttpPost]
        public async Task<ActionResult<RecipeDto>> CreateRecipe(CreateRecipeDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var recipe = await _recipeService.CreateRecipeAsync(createDto);
            return CreatedAtAction(nameof(GetRecipe), new { id = recipe.Id }, recipe);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<RecipeDto>> UpdateRecipe(int id, UpdateRecipeDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var recipe = await _recipeService.UpdateRecipeAsync(id, updateDto);
            if (recipe == null)
            {
                return NotFound($"Recipe with ID {id} not found.");
            }

            return Ok(recipe);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            var deleted = await _recipeService.DeleteRecipeAsync(id);
            if (!deleted)
            {
                return NotFound($"Recipe with ID {id} not found.");
            }

            return NoContent();
        }
    }
}
