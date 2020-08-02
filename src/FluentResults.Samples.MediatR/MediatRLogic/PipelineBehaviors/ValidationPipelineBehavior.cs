using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace FluentResults.Samples.MediatR.MediatRLogic.PipelineBehaviors
{
    public class ValidationPipelineBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TResponse : ResultBase
    {
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var validationResult = await ValidateAsync(request);
            if (validationResult.IsFailed)
            {
                var res = validationResult as TResponse;

                return res;
            }

            return await next();
        }

        private async Task<Result> ValidateAsync(TRequest request)
        {
            // do here some validation, for example with fluentvalidation
            return Result.Fail("Validation failed");
        }
    }
}