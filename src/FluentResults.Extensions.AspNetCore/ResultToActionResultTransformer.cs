using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace FluentResults.Extensions.AspNetCore
{
    public class FailedResultToActionResultTransformationContext
    {
        public ResultBase Result { get; }

        public FailedResultToActionResultTransformationContext(ResultBase result)
        {
            Result = result;
        }
    }

    public class OkResultToActionResultTransformationContext<TResult>
        where TResult : ResultBase
    {
        public TResult Result { get; }

        public OkResultToActionResultTransformationContext(TResult result)
        {
            Result = result;
        }
    }

    public interface IErrorDto
    {
        string Message { get; set; }
    }

    public class ErrorDto : IErrorDto
    {
        public string Message { get; set; }
    }

    public class OkResponse
    {
        public IEnumerable<ISuccessDto> Successes { get; set; }
    }

    public class OkResponse<T> : OkResponse
    {
        public T Value { get; set; }
    }

    public interface ISuccessDto
    {
        string Message { get; set; }
    }

    public class SuccessDto : ISuccessDto
    {
        public string Message { get; set; }
    }

    public class ResultToActionResultTransformer
    {
        public ActionResult Transform(Result result, IAspNetCoreResultEndpointProfile profile)
        {
            if (result.IsFailed)
            {
                return profile.TransformFailedResultToActionResult(new FailedResultToActionResultTransformationContext(result));
            }

            return profile.TransformOkNoValueResultToActionResult(new OkResultToActionResultTransformationContext<Result>(result));
        }

        public ActionResult Transform<T>(Result<T> result, IAspNetCoreResultEndpointProfile profile)
        {
            if (result.IsFailed)
            {
                return profile.TransformFailedResultToActionResult(new FailedResultToActionResultTransformationContext(result));
            }

            return profile.TransformOkValueResultToActionResult(new OkResultToActionResultTransformationContext<Result<T>>(result));
        }
    }
}