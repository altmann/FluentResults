using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace FluentResults.Extensions.AspNetCore
{
    public class DefaultAspNetCoreResultEndpointProfile : IAspNetCoreResultEndpointProfile
    {
        public virtual ActionResult TransformFailedResultToActionResult(FailedResultToActionResultTransformationContext context)
        {
            var result = context.Result;

            var errorDtos = result.Errors.Select(e => new ErrorDto
                                                      {
                                                          Message = e.Message
                                                      });

            return new BadRequestObjectResult(errorDtos);
        }

        public virtual ActionResult TransformOkNoValueResultToActionResult(OkResultToActionResultTransformationContext<Result> context)
        {
            return new OkResult();
        }

        public virtual ActionResult TransformOkValueResultToActionResult<T>(OkResultToActionResultTransformationContext<Result<T>> context)
        {
            return new OkObjectResult(context.Result.ValueOrDefault);
        }
    }
}