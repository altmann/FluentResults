using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FluentResults.Extensions.AspNetCore
{
    

    public class TransformResultToActionResultAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var maybeResult = (context.Result as ObjectResult)?.Value;
            if (!(maybeResult is IResultBase result))
                return;

            new ResultToActionResultTransformer().Transform(new AbstractedResult(result.Successes,
                                                                                 result.Errors,
                                                                                 result.))
        }
    }
}