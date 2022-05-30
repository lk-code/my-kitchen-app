namespace MyKitchenApp.Models.CookingRecipes
{
    public class Recipe
    {
        public string Title { get; set; }
        public List<Ingredient> Ingredients { get; set; }

        public Recipe(string title)
        {
            this.Title = title;
            this.Ingredients = new List<Ingredient>();
        }
    }
}