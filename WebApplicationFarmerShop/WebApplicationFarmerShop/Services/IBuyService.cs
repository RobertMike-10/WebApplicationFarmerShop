using WebApplicationFarmerShop.Models;

namespace WebApplicationFarmerShop.Services
{
    public interface IBuyService
    {
        Task<Result> BuyAsync(string productId, int quantity);
    }
}
