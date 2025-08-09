using AspNetCoreRateLimit;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace WebApplicationFarmerShop.Middlewares
{
    public class ClientFarmRateLimitMiddleware: ClientRateLimitMiddleware
    {
        public ClientFarmRateLimitMiddleware(RequestDelegate next,
                                             IProcessingStrategy processingStrategy,
                                             IOptions<ClientRateLimitOptions> options,
                                             IClientPolicyStore clientPolicyStore,
                                             IRateLimitConfiguration configuration,
                                             ILogger<ClientFarmRateLimitMiddleware> logger)
            : base(next,processingStrategy, options, clientPolicyStore, configuration, logger)
        {
        }

        public override Task ReturnQuotaExceededResponse(HttpContext httpContext, 
                                                         RateLimitRule rule, 
                                                         string retryAfter)
        {
            string requestPath = httpContext?.Request?.Path.Value!;
            var result = JsonSerializer.Serialize("Too many request, allowed 1 per minute");
            httpContext!.Response.Headers["Retry-After"] = retryAfter;
            httpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            return httpContext.Response.WriteAsync(result);
        }
    }
}
