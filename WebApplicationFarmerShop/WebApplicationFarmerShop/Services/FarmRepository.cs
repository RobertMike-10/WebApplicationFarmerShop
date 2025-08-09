using WebApplicationFarmerShop.Models;

namespace WebApplicationFarmerShop.Services
{
    public class FarmRepository: IFarmRepository
    {
        public static readonly Dictionary<string, int> Products = new()
        {
            { "corn001", 100000 }
        };

        public async Task<int> GetTotalByProductId(string productId)
        {          
            await Task.Delay(100);
            return Products.TryGetValue(productId, out var total) ? total : 0;
        }

        public async Task<Result> UpdateProduct(string productId, int quantity)
        {
            await Task.Delay(100);
            if (Products.TryGetValue(productId, out var total))
            {
                Products[productId] = quantity;
                return Result.Ok($"Updated product");
            }
            else
                return Result.Fail($"There is no such product on the farm");
        }

    }
}
