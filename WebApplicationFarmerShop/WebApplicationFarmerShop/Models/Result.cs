namespace WebApplicationFarmerShop.Models
{
    public class Result
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Result(bool success, string message)
        {
            Success = success;
            Message = message;
        }
        public Result() : this(false, string.Empty) { }
        public static Result Ok(string message = "") => new Result(true, message);
        public static Result Fail(string message) => new Result(false, message);
    }
}
