using Microsoft.AspNetCore.Mvc;

namespace FluentResults.Extensions.AspNetCore
{
    public class AspNetCoreResultSettings
    {
        public IAspNetCoreResultEndpointProfile DefaultProfile { get; set; } = new DefaultAspNetCoreResultEndpointProfile();
    }

    public static class AspNetCoreResult
    {
        internal static AspNetCoreResultSettings Settings { get; }

        static AspNetCoreResult()
        {
            Settings = new AspNetCoreResultSettings();
        }
    }

    internal static class ControllerExtensions
    {
        public static ActionResult ToActionResult(this ControllerBase controller, Result result, IAspNetCoreResultEndpointProfile profile)
        {
            return new ResultToActionResultTransformer().Transform(result, profile);
        }

        public static ActionResult ToActionResult(this ControllerBase controller, Result result)
        {
            return ToActionResult(controller, result, AspNetCoreResult.Settings.DefaultProfile);
        }

        public static ActionResult ToActionResult<T>(this ControllerBase controller, Result<T> result, IAspNetCoreResultEndpointProfile profile)
        {
            return new ResultToActionResultTransformer().Transform(result, profile);
        }

        public static ActionResult ToActionResult<T>(this ControllerBase controller, Result<T> result)
        {
            return ToActionResult(controller, result, AspNetCoreResult.Settings.DefaultProfile);
        }
    }

    public static class ResultExtensions
    {
        public static ActionResult ToActionResult(this Result result, IAspNetCoreResultEndpointProfile profile)
        {
            return new ResultToActionResultTransformer().Transform(result, profile);
        }

        public static ActionResult ToActionResult(this Result result)
        {
            return ToActionResult(result, AspNetCoreResult.Settings.DefaultProfile);
        }

        public static ActionResult ToActionResult<T>(this Result<T> result, IAspNetCoreResultEndpointProfile profile)
        {
            return new ResultToActionResultTransformer().Transform(result, profile);
        }

        public static ActionResult ToActionResult<T>(this Result<T> result)
        {
            return ToActionResult(result, AspNetCoreResult.Settings.DefaultProfile);
        }
    }
}
