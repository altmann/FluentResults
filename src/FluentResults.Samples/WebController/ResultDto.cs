using System.Collections.Generic;

namespace FluentResults.Samples.WebController
{
    // The ResultDto look however you want
    // You can store error messages, warnings, success message, error code, ...
    public class ResultDto
    {
        public bool IsSuccess { get; set; }

        public IEnumerable<ErrorDto> Errors { get; set; }

        public ResultDto(bool isSuccess, IEnumerable<ErrorDto> errors)
        {
            IsSuccess = isSuccess;
            Errors = errors;
        }
    }

    public class ErrorDto
    {
        public string Message { get; set; }

        public string Code { get; set; }

        public ErrorDto(string message, string code)
        {
            Message = message;
            Code = code;
        }
    }
}