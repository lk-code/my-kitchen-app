using MyKitchenApp.Interfaces;
using MyKitchenApp.Models.CookingRecipes;

namespace MyKitchenApp.Services.CookingRecipes
{
    public class CookingRecipesService : ICookingRecipesService
    {
        #region properties

        #endregion

        #region constructor

        #endregion

        #region logic

        public async Task<List<Recipe>> GetRecipesAsync()
        {
            await Task.CompletedTask;

            List<Recipe> recipes = new List<Recipe>();

            Recipe recipe = new Recipe("Nudel-Schinkel Auflauf");
            //recipe.Ingredients.Add(new Ingredient());

            recipes.Add(recipe);

            return recipes;
        }

        #endregion
    }
}