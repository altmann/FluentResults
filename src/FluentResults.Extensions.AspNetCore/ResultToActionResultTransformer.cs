using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace FluentResults.Extensions.AspNetCore
{
    public interface IAspNetCoreResultEndpointProfile
    {
        ActionResult TransformFailedResultToActionResult(FailedResultToActionResultTransformationContext context);

        ActionResult TransformSuccessResultToActionResult(SuccessResultToActionResultTransformationContext<Result> context);

        ActionResult TransformSuccessValueResultToActionResult<T>(SuccessResultToActionResultTransformationContext<Result<T>> context);

        IErrorDto TransformErrorToErrorDto(ErrorToErrorDtoTransformatonContext context);

        ISuccessDto TransformSuccessToSuccessDto(SuccessToSuccessDtoTransformatonContext context);
    }

    public class DefaultAspNetCoreResultEndpointProfile : IAspNetCoreResultEndpointProfile
    {
        public virtual ActionResult TransformFailedResultToActionResult(FailedResultToActionResultTransformationContext context)
        {
            return new BadRequestObjectResult(context.GetErrors());
        }

        public ActionResult TransformSuccessResultToActionResult(SuccessResultToActionResultTransformationContext<Result> context)
        {
            return new OkObjectResult(new SuccessResponse
                                      {
                                          Successes = context.GetSuccesses()
                                      });
        }

        public ActionResult TransformSuccessValueResultToActionResult<T>(SuccessResultToActionResultTransformationContext<Result<T>> context)
        {
            return new OkObjectResult(new SuccessResponse<T>
                                      {
                                          Value = (T)context.Result.ValueOrDefault,
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
        public IResultBase Result { get; }

        private Func<IEnumerable<IErrorDto>> GetErrorsLogic { get; }

        public FailedResultToActionResultTransformationContext(IResultBase result, Func<IEnumerable<IErrorDto>> getErrors)
        {
            Result = result;
            GetErrorsLogic = getErrors;
        }

        public IEnumerable<IErrorDto> GetErrors()
        {
            return GetErrorsLogic();
        }
    }

    public class SuccessResultToActionResultTransformationContext<TResult>
        where TResult : ResultBase
    {
        public TResult Result { get; }

        private Func<IEnumerable<ISuccessDto>> Successes { get; }

        public SuccessResultToActionResultTransformationContext(TResult result, Func<IEnumerable<ISuccessDto>> successes)
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

    public class SuccessResponse
    {
        public IEnumerable<ISuccessDto> Successes { get; set; }
    }

    public class SuccessResponse<T> : SuccessResponse
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
            return profile.TransformSuccessResultToActionResult(new SuccessResultToActionResultTransformationContext<Result>(result, getSuccesses));
        }

        public ActionResult Transform<T>(Result<T> result, IAspNetCoreResultEndpointProfile profile)
        {
            if (result.IsFailed)
            {
                Func<IEnumerable<IErrorDto>> getErrors = () => result.Errors.Select(e => profile.TransformErrorToErrorDto(new ErrorToErrorDtoTransformatonContext(e, result)));
                return profile.TransformFailedResultToActionResult(new FailedResultToActionResultTransformationContext(result, getErrors));
            }

            Func<IEnumerable<ISuccessDto>> getSuccesses = () => result.Successes.Select(s => profile.TransformSuccessToSuccessDto(new SuccessToSuccessDtoTransformatonContext(s, result)));
            return profile.TransformSuccessValueResultToActionResult(new SuccessResultToActionResultTransformationContext<Result<T>>(result, getSuccesses));
        }
    }
}