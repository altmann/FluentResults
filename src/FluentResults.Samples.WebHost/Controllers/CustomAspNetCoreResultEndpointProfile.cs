using FluentResults.Extensions.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace FluentResults.Samples.WebHost.Controllers;

public class CustomAspNetCoreResultEndpointProfile : DefaultAspNetCoreResultEndpointProfile
{
    public override ActionResult TransformFailedResultToActionResult(FailedResultToActionResultTransformationContext context)
    {
        var result = context.Result;

        if (result.HasError<UnauthorizedError>(out var unauthorizedErrors))
        {
#if DEBUG
            // Here you can also use your custom dto to transfer information
            return new UnauthorizedObjectResult(unauthorizedErrors.First().Message);
#endif
            return new UnauthorizedResult();
        }

        if (result.HasError<NotFoundError>(out var notFoundErrors))
        {
#if DEBUG
            // Here you can also use your custom dto to transfer information
            return new NotFoundObjectResult(notFoundErrors.First().Message);
#endif
            return new NotFoundResult();
        }

        if (result.HasError<DomainError>(out var domainErrors))
        {
            return new BadRequestObjectResult(TransformDomainErrors(domainErrors));
        }

        return new StatusCodeResult(500);
    }

    private IEnumerable<BadRequestErrorDto> TransformDomainErrors(IEnumerable<DomainError> domainErrors)
    {
        return domainErrors.Select(e => new BadRequestErrorDto(e.Message, e.ErrorCode));
    }

    public class BadRequestErrorDto : ErrorDto
    {
        public string ErrorCode { get; }

        public BadRequestErrorDto(string message, string errorCode)
        {
            Message = message;
            ErrorCode = errorCode;
        }
    }
}