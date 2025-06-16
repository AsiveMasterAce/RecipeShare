using Microsoft.AspNetCore.Http.HttpResults;
using RecipeLookup.Models;
using System.Text;
using System.Text.Json;

namespace RecipeLookup.Services
{
    public class RecipeClientService: IRecipeClientService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;
        public RecipeClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<IEnumerable<RecipeDto>> GetAllRecipesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Recipe/GetRecipes");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<RecipeDto>>(json, _jsonOptions) ?? new List<RecipeDto>();
                }
                return new List<RecipeDto>();
            }
            catch
            {
                return new List<RecipeDto>();
            }
        }

        public async Task<RecipeDto?> GetRecipeByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Recipe/GetRecipe/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<RecipeDto>(json, _jsonOptions);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<RecipeDto>> GetRecipesByDietaryTagAsync(string tag)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Recipe/GetRecipesByDietaryTag/dietary/{tag}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<RecipeDto>>(json, _jsonOptions) ?? new List<RecipeDto>();
                }
                return new List<RecipeDto>();
            }
            catch
            {
                return new List<RecipeDto>();
            }
        }

        public async Task<IEnumerable<RecipeDto>> SearchRecipesAsync(string searchTerm)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Recipe/SearchRecipes/search?q={Uri.EscapeDataString(searchTerm)}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<RecipeDto>>(json, _jsonOptions) ?? new List<RecipeDto>();
                }
                return new List<RecipeDto>();
            }
            catch
            {
                return new List<RecipeDto>();
            }
        }

        public async Task<RecipeDto> CreateRecipeAsync(CreateRecipeDto dto)
        {
            try
            {
                var json = JsonSerializer.Serialize(dto, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/Recipe/CreateRecipe", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<RecipeDto>(responseJson, _jsonOptions);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<RecipeDto?> UpdateRecipeAsync(int id, UpdateRecipeDto dto)
        {
            try
            {
                var json = JsonSerializer.Serialize(dto, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"api/Recipe/UpdateRecipe/{id}", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<RecipeDto>(responseJson, _jsonOptions);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> DeleteRecipeAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Recipe/DeleteRecipe/{id}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
