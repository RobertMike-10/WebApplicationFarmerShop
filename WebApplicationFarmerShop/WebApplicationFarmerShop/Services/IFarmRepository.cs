using WebApplicationFarmerShop.Models;

namespace WebApplicationFarmerShop.Services
{
    public interface IFarmRepository
    {
        public Task<int> GetTotalByProductId(string productId);
        public Task<Result> UpdateProduct(string productId, int quantity);
    }
}
