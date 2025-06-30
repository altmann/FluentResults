﻿using System.Collections.Generic;
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

        private static IEnumerable<ErrorDto> TransformErrors(IEnumerable<IError> errors)
        {
            return errors.Select(TransformError);
        }

        private static ErrorDto TransformError(IError error)
        {
            var errorCode = TransformErrorCode(error);

            return new ErrorDto(error.Message, errorCode);
        }

        private static string TransformErrorCode(IError error)
        {
            if (error.Metadata.TryGetValue("ErrorCode", out var errorCode))
                return errorCode as string;

            return "";
        }
    }
}