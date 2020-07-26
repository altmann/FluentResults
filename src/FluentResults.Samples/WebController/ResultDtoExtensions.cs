using System.Collections.Generic;
using System.Linq;

namespace FluentResults.Samples.WebController
{
    public static class ResultDtoExtensions
    {
        public static ResultDto ToResultDto(this Result result)
        {
            if (result.IsSuccess)
                return new ResultDto(true, Enumerable.Empty<ErrorDto>());

            return new ResultDto(false, TransformErrors(result.Errors));
        }

        private static IEnumerable<ErrorDto> TransformErrors(List<Error> errors)
        {
            return errors.Select(TransformError);
        }

        private static ErrorDto TransformError(Error error)
        {
            var errorCode = TransformErrorCode(error);

            return new ErrorDto(error.Message, errorCode);
        }

        private static string TransformErrorCode(Error error)
        {
            if (error.Metadata.TryGetValue("ErrorCode", out var errorCode))
                return errorCode as string;

            return "";
        }
    }
}