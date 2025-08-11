using WebApplicationFarmerShop.Models;

namespace WebApplicationFarmerShop.Services
{
    public class BuyService: IBuyService
    {
        private readonly IFarmRepository _repository;   
        public BuyService(IFarmRepository repository)
        {
            _repository = repository;
        }
        public async Task<Result> BuyAsync(string productId, int quantity)
        {
            // Simulate a buy operation
            await Task.Delay(1000);
            Console.WriteLine($"Buying product with ID: {productId}");
            int total = await _repository.GetTotalByProductId(productId);
            int resultQuantity = total - quantity;
            if (resultQuantity < 0 )
            {
                return Result.Fail($"Product {productId} is out of stock.");
            }
            else
            {
                var result = await _repository.UpdateProduct(productId, resultQuantity);
                if (result.Success)
                {
                    return Result.Ok($"Product {productId} purchased successfully.");
                }
                else
                { 
                    return Result.Fail(result.Message);
                }
            }
            
        }

    }
}
