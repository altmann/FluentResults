using System.Collections.Generic;
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
            return new OkObjectResult(new OkResponse
                                      {
                                          Successes = TransformSuccessDtos(context.Result.Successes)
                                      });
        }

        public virtual ActionResult TransformOkValueResultToActionResult<T>(OkResultToActionResultTransformationContext<Result<T>> context)
        {
            return new OkObjectResult(new OkResponse<T>
                                      {
                                          Value = context.Result.ValueOrDefault,
                                          Successes = TransformSuccessDtos(context.Result.Successes)
                                      });
        }

        private IEnumerable<SuccessDto> TransformSuccessDtos(IEnumerable<ISuccess> successes)
        {
            return successes.Select(s => new SuccessDto
                                         {
                                             Message = s.Message
                                         });
        }
    }
}