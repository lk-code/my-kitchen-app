using MyKitchenApp.Models.Shopping;

namespace MyKitchenApp.Interfaces
{
    public interface IShoppingService
    {
        Task<List<Product>> GetProductsAsync();
    }
}
