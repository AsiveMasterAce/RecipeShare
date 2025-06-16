using Moq;
using RecipeShareAPI.Models;
using RecipeShareAPI.Models.DTO;
using RecipeShareAPI.Repositories;
using RecipeShareAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeApi.Tests
{
    public class RecipeServiceTests
    {
        private readonly Mock<IRecipeRepository> _mockRepo;
        private readonly RecipeService _service;

        public RecipeServiceTests()
        {
            _mockRepo = new Mock<IRecipeRepository>();
            _service = new RecipeService(_mockRepo.Object);
        }
        #region TestsShouldPass
        [Fact]
        public async Task GetAllRecipes_ReturnsAllRecipes()
        {
            var recipes = new List<Recipe>
            {
                new Recipe { ID = 1, Title = "Boerewors Rolls" },
                new Recipe { ID = 2, Title = "Bunny Chow" }
            };
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(recipes);


            var result = await _service.GetAllRecipesAsync();


            Assert.Equal(2, result.Count());
            Assert.Contains(result, r => r.Title == "Boerewors Rolls");
        }

        [Fact]
        public async Task GetRecipeById_ValidId_ReturnsRecipe()
        {

            var recipe = new Recipe { ID = 1, Title = "Boerewors Rolls" };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(recipe);


            var result = await _service.GetRecipeByIdAsync(1);


            Assert.NotNull(result);
            Assert.Equal("Boerewors Rolls", result.Title);
        }

        [Fact]
        public async Task GetRecipeById_InvalidId_ReturnsNull()
        {

            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Recipe?)null);


            var result = await _service.GetRecipeByIdAsync(99);


            Assert.Null(result);
        }

        [Fact]
        public async Task CreateRecipe_ValidDto_ReturnsCreatedRecipe()
        {

            var createDto = new CreateRecipeDto
            {
                Title = "Test Recipe",
                Ingredients = new List<string> { "Sugar", "Flour" },
                Steps = new List<string> { "Mix", "Bake" },
                CookingTime = 45,
                DietaryTags = new List<string> { "Sweet" }
            };

            var createdRecipe = new Recipe
            {
                ID = 10,
                Title = "Test Recipe",
                Ingredients = "Sugar|Flour",
                Steps = "Mix|Bake",
                CookingTime = 45,
                DietaryTags = "Sweet"
            };

            _mockRepo.Setup(r => r.CreateAsync(It.IsAny<Recipe>())).ReturnsAsync(createdRecipe);


            var result = await _service.CreateRecipeAsync(createDto);


            Assert.NotNull(result);
            Assert.Equal("Test Recipe", result.Title);
        }

        [Fact]
        public async Task DeleteRecipe_ValidId_ReturnsTrue()
        {

            _mockRepo.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);


            var result = await _service.DeleteRecipeAsync(1);


            Assert.True(result);
        }

        [Fact]
        public async Task UpdateRecipe_ValidId_ReturnsUpdatedRecipe()
        {
            
            var updateDto = new UpdateRecipeDto
            {
                Title = "Updated Recipe",
                Ingredients = new List<string> { "Eggs", "Milk" },
                Steps = new List<string> { "Beat", "Cook" },
                CookingTime = 15,
                DietaryTags = new List<string> { "Breakfast" }
            };

            var updatedRecipe = new Recipe
            {
                ID = 1,
                Title = "Updated Recipe",
                Ingredients = "Eggs|Milk",
                Steps = "Beat|Cook",
                CookingTime = 15,
                DietaryTags = "Breakfast"
            };

            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Recipe>())).ReturnsAsync(updatedRecipe);

            
            var result = await _service.UpdateRecipeAsync(1, updateDto);

            
            Assert.NotNull(result);
            Assert.Equal("Updated Recipe", result.Title);
            Assert.Equal(15, result.CookingTime);
        }

        [Fact]
        public async Task GetRecipesByDietaryTag_ExistingTag_ReturnsMatchingRecipes()
        {
            
            var recipes = new List<Recipe>
                {
                    new Recipe { ID = 1, Title = "Pap and Chakalaka", DietaryTags = "Vegan,Gluten-Free" },
                    new Recipe { ID = 2, Title = "Samp and Beans", DietaryTags = "Vegetarian,Hearty" }
                };

            _mockRepo.Setup(r => r.GetByDietaryTagAsync("Vegan")).ReturnsAsync(recipes.Where(r => r.DietaryTags.Contains("Vegan")).ToList());

       
            var result = await _service.GetRecipesByDietaryTagAsync("Vegan");

           
            Assert.Single(result);
            Assert.Contains(result, r => r.Title == "Pap and Chakalaka");
        }

        [Fact]
        public async Task SearchRecipes_ValidTerm_ReturnsMatchingRecipes()
        {

            var searchTerm = "Vetkoek";
            var recipes = new List<Recipe>
            {
                new Recipe { ID = 5, Title = "Vetkoek with Mince" }
            };

            _mockRepo.Setup(r => r.SearchAsync(searchTerm)).ReturnsAsync(recipes);

         
            var result = await _service.SearchRecipesAsync(searchTerm);

            Assert.Single(result);
            Assert.Equal("Vetkoek with Mince", result.First().Title);
        }
        #endregion

        #region TestsShouldFail
        [Fact]
        public async Task CreateRecipeAsync_MissingTitle_ThrowsException()
        {
            
            var createDto = new CreateRecipeDto
            {
                Title = null,
                Ingredients = new List<string> { "X" },
                Steps = new List<string> { "Y" },
                CookingTime = 10,
                DietaryTags = new List<string> { "Tag" }
            };

     
            await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateRecipeAsync(createDto));
        }
        [Fact]
        public async Task SearchRecipes_ReturnsEmptyList()
        {
         
            var searchTerm = "An Empty Recipe";
            _mockRepo.Setup(r => r.SearchAsync(searchTerm)).ReturnsAsync(new List<Recipe>());

         
            var result = await _service.SearchRecipesAsync(searchTerm);

         
            Assert.Empty(result); 
        }

        [Fact]
        public async Task GetRecipeById_IDNotInDB_ReturnsNull()
        {
          
            _mockRepo.Setup(r => r.GetByIdAsync(999))
                     .ReturnsAsync((Recipe?)null);

            
            var result = await _service.GetRecipeByIdAsync(999);

            
            Assert.Null(result);
        }
        [Fact]
        public async Task UpdateRecipe_InvalidId_ReturnsNull()
        {
            
            var updateDto = new UpdateRecipeDto
            {
                Title = "Not Exist",
                Ingredients = new List<string> { "X" },
                Steps = new List<string> { "Nothing" },
                CookingTime = 0,
                DietaryTags = new List<string> { "None" }
            };

            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Recipe>())).ReturnsAsync((Recipe?)null);

        
            var result = await _service.UpdateRecipeAsync(999, updateDto);

  
            Assert.Null(result); 
        }

        [Fact]
        public async Task DeleteRecipe_InvalidId_ReturnsFalse()
        {
            
            _mockRepo.Setup(r => r.DeleteAsync(999)).ReturnsAsync(false);

          
            var result = await _service.DeleteRecipeAsync(999);

            
            Assert.False(result); 
        }
        #endregion
    }
}
