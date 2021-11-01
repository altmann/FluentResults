using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace FluentResults.Samples.MediatR.MediatRLogic.PipelineBehaviors
{
    public class ValidationPipelineBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TResponse : ResultBase<IError>, new()
    {
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            var validationResult = await ValidateAsync(request);
            if (validationResult.IsFailed)
            {
                var result = new TResponse();

                foreach (var reason in validationResult.Reasons)
                    result.Reasons.Add(reason);

                return result;
            }

            return await next();
        }

        private Task<Result> ValidateAsync(TRequest request)
        {
            // do here some validation, for example with fluentvalidation
            return Task.FromResult(Result.Fail("Validation failed"));
            // return Result.Ok();
        }
    }
}