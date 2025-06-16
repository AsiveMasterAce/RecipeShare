using RecipeShareAPI.Models;

namespace RecipeShareAPI.Data
{
    public static class DBSeeder
    {
        public static void SeedData(ApplicationDBContext _context)
        {
            if (_context.Recipes.Any()) return;

            var recipes = new List<Recipe>
            {
                    new Recipe
                    {
                        Title = "Boerewors Rolls",
                        Ingredients = "500g boerewors sausage|4 hot dog rolls|1 onion|2 tbsp oil|2 tbsp chutney|1 tbsp tomato sauce|Salt|Pepper",
                        Steps = "Grill or pan-fry boerewors until cooked through|Slice onions and fry in oil until golden|Warm hot dog rolls|Place boerewors in rolls, top with fried onions|Add chutney and tomato sauce|Serve hot",
                        CookingTime = 30,
                        DietaryTags = "Street Food,Grilled"
                    },
                    new Recipe
                    {
                        Title = "Bunny Chow",
                        Ingredients = "500g beef or lamb curry|1 loaf white bread (unsliced)|1 onion|2 tomatoes|2 cloves garlic|1 tsp ginger|Curry powder|Oil|Salt|Fresh coriander",
                        Steps = "Cook curry with onion, garlic, ginger, tomatoes, and spices|Simmer until meat is tender|Cut bread loaf in quarters, hollow out center|Spoon hot curry into bread cavity|Garnish with coriander|Serve with remaining bread on the side",
                        CookingTime = 50,
                        DietaryTags = "Spicy,Street Food"
                    },
                    new Recipe
                    {
                        Title = "Pap and Chakalaka",
                        Ingredients = "2 cups maize meal|4 cups water|1 tbsp salt|1 onion|1 bell pepper|2 carrots|1 can baked beans|1 tsp curry powder|Oil|Salt|Pepper",
                        Steps = "Bring water and salt to boil, add maize meal gradually|Stir constantly until thick, cover and simmer|For chakalaka, fry onion, pepper, and carrots|Add curry powder, baked beans, salt and pepper|Simmer for 10 minutes|Serve pap with warm chakalaka on top",
                        CookingTime = 40,
                        DietaryTags = "Vegan,Gluten-Free"
                    },
                    new Recipe
                    {
                        Title = "Samp and Beans (Umngqusho)",
                        Ingredients = "2 cups samp|1 cup sugar beans|1 onion|2 tbsp oil|Salt|Pepper|Optional: beef stock or chili flakes",
                        Steps = "Soak samp and beans overnight|Boil together until soft (about 2 hours)|Fry onion in oil and add to mixture|Season with salt, pepper, and optional flavors|Simmer to blend flavors|Serve hot as main or side dish",
                        CookingTime = 120,
                        DietaryTags = "Vegetarian,Hearty"
                    },
                    new Recipe
                    {
                        Title = "Vetkoek with Mince",
                        Ingredients = "3 cups flour|1 packet instant yeast|1 tbsp sugar|1 tsp salt|1 ¼ cup warm water|Oil for frying|500g minced beef|1 onion|1 tsp curry powder|Salt|Pepper",
                        Steps = "Mix flour, yeast, sugar, salt and water to form dough|Let rise until doubled in size|Shape into balls and deep-fry until golden|Fry onion and cook mince with spices|Cut open vetkoek and fill with mince mixture|Serve warm",
                        CookingTime = 90,
                        DietaryTags = "Comfort Food,Fried"
                    }
            };


            _context.Recipes.AddRange(recipes);
            _context.SaveChanges();
        }
    }
}
