using System;
using System.Threading;
using System.Threading.Tasks;
using FluentResults.Samples.MediatR.MediatRLogic.Messages;
using MediatR;

namespace FluentResults.Samples.MediatR.MediatRLogic.Handlers
{
    public class CommandWithResultHandler : IRequestHandler<CommandWithResult, Result>
    {
        public Task<Result> Handle(CommandWithResult request, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Hello {nameof(CommandWithoutResult)}");

            return Task.FromResult(Result.Ok());
        }
    }
}