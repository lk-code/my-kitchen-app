using MyKitchenApp.Models.CookingRecipes;

namespace MyKitchenApp.Interfaces
{
    public interface ICookingRecipesService
    {
        Task<List<Recipe>> GetRecipesAsync();
    }
}
