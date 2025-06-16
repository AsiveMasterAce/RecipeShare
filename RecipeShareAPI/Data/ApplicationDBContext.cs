using Microsoft.EntityFrameworkCore;
using RecipeShareAPI.Models;

namespace RecipeShareAPI.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
                        : base(options) { }



        public DbSet<Recipe>Recipes { get; set; }
    }
}
