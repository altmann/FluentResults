using Microsoft.AspNetCore.Mvc;

namespace FluentResults.Extensions.AspNetCore
{
    public interface IAspNetCoreResultEndpointProfile
    {
        ActionResult TransformFailedResultToActionResult(FailedResultToActionResultTransformationContext context);

        ActionResult TransformOkNoValueResultToActionResult(OkResultToActionResultTransformationContext<Result> context);

        ActionResult TransformOkValueResultToActionResult<T>(OkResultToActionResultTransformationContext<Result<T>> context);
    }
}