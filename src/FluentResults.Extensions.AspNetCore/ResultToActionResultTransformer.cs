using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace FluentResults.Extensions.AspNetCore
{
    public interface IAspNetCoreResultEndpointProfile
    {
        ActionResult TransformFailedResultToActionResult(FailedResultToActionResultTransformationContext context);

        ActionResult TransformOkNoValueResultToActionResult(OkResultToActionResultTransformationContext<Result> context);

        ActionResult TransformOkValueResultToActionResult<T>(OkResultToActionResultTransformationContext<Result<T>> context);

        IErrorDto TransformErrorToErrorDto(ErrorToErrorDtoTransformatonContext context);

        ISuccessDto TransformSuccessToSuccessDto(SuccessToSuccessDtoTransformatonContext context);
    }

    public class DefaultAspNetCoreResultEndpointProfile : IAspNetCoreResultEndpointProfile
    {
        public virtual ActionResult TransformFailedResultToActionResult(FailedResultToActionResultTransformationContext context)
        {
            return new BadRequestObjectResult(context.GetErrors());
        }

        public virtual ActionResult TransformOkNoValueResultToActionResult(OkResultToActionResultTransformationContext<Result> context)
        {
            return new OkObjectResult(new OkResponse
                                      {
                                          Successes = context.GetSuccesses()
                                      });
        }

        public virtual ActionResult TransformOkValueResultToActionResult<T>(OkResultToActionResultTransformationContext<Result<T>> context)
        {
            return new OkObjectResult(new OkResponse<T>
                                      {
                                          Value = context.Result.ValueOrDefault,
                                          Successes = context.GetSuccesses()
                                      });
        }

        public virtual IErrorDto TransformErrorToErrorDto(ErrorToErrorDtoTransformatonContext context)
        {
            return new ErrorDto
                   {
                       Message = context.Error.Message
                   };
        }

        public ISuccessDto TransformSuccessToSuccessDto(SuccessToSuccessDtoTransformatonContext context)
        {
            return new SuccessDto
                   {
                       Message = context.Success.Message
                   };
        }
    }

    public class FailedResultToActionResultTransformationContext
    {
        public ResultBase Result { get; }

        private Func<IEnumerable<IErrorDto>> GetErrorsLogic { get; }

        public FailedResultToActionResultTransformationContext(ResultBase result, Func<IEnumerable<IErrorDto>> getErrors)
        {
            Result = result;
            GetErrorsLogic = getErrors;
        }

        public IEnumerable<IErrorDto> GetErrors()
        {
            return GetErrorsLogic();
        }
    }

    public class OkResultToActionResultTransformationContext<TResult>
        where TResult : ResultBase
    {
        public TResult Result { get; }

        private Func<IEnumerable<ISuccessDto>> Successes { get; }

        public OkResultToActionResultTransformationContext(TResult result, Func<IEnumerable<ISuccessDto>> successes)
        {
            Result = result;
            Successes = successes;
        }

        public IEnumerable<ISuccessDto> GetSuccesses()
        {
            return Successes();
        }
    }

    public class SuccessToSuccessDtoTransformatonContext
    {
        public IResultBase Result { get; }

        public ISuccess Success { get; }

        public SuccessToSuccessDtoTransformatonContext(ISuccess success, IResultBase result)
        {
            Success = success;
            Result = result;
        }
    }

    public class ErrorToErrorDtoTransformatonContext
    {
        public IResultBase Result { get; }

        public IError Error { get; }

        public ErrorToErrorDtoTransformatonContext(IError error, IResultBase result)
        {
            Error = error;
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
                Func<IEnumerable<IErrorDto>> getErrors = () => result.Errors.Select(e => profile.TransformErrorToErrorDto(new ErrorToErrorDtoTransformatonContext(e, result)));
                return profile.TransformFailedResultToActionResult(new FailedResultToActionResultTransformationContext(result, getErrors));
            }

            Func<IEnumerable<ISuccessDto>> getSuccesses = () => result.Successes.Select(s => profile.TransformSuccessToSuccessDto(new SuccessToSuccessDtoTransformatonContext(s, result)));
            return profile.TransformOkNoValueResultToActionResult(new OkResultToActionResultTransformationContext<Result>(result, getSuccesses));
        }

        public ActionResult Transform<T>(Result<T> result, IAspNetCoreResultEndpointProfile profile)
        {
            if (result.IsFailed)
            {
                Func<IEnumerable<IErrorDto>> getErrors = () => result.Errors.Select(e => profile.TransformErrorToErrorDto(new ErrorToErrorDtoTransformatonContext(e, result)));
                return profile.TransformFailedResultToActionResult(new FailedResultToActionResultTransformationContext(result, getErrors));
            }

            Func<IEnumerable<ISuccessDto>> getSuccesses = () => result.Successes.Select(s => profile.TransformSuccessToSuccessDto(new SuccessToSuccessDtoTransformatonContext(s, result)));
            return profile.TransformOkValueResultToActionResult(new OkResultToActionResultTransformationContext<Result<T>>(result, getSuccesses));
        }
    }
}