namespace FluentResults.Extensions.AspNetCore
{
    public class AspNetCoreResultSettings
    {
        public IAspNetCoreResultEndpointProfile DefaultProfile { get; set; } = new DefaultAspNetCoreResultEndpointProfile();
    }
}