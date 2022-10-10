using FluentResults.Extensions.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace FluentResults.Samples.WebHost.Controllers;

public class CustomAspNetCoreResultEndpointProfile : DefaultAspNetCoreResultEndpointProfile
{
    public override ActionResult TransformFailedResultToActionResult(FailedResultToActionResultTransformationContext context)
    {
        var result = context.Result;

        if (result.HasError<UnauthorizedError>())
            return new UnauthorizedResult();

        if (result.HasError<NotFoundError>())
            return new NotFoundResult();

        return new BadRequestObjectResult(context.GetErrors());
    }
}