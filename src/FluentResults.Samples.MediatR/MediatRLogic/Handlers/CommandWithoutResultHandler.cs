using System;
using System.Threading;
using System.Threading.Tasks;
using FluentResults.Samples.MediatR.MediatRLogic.Messages;
using MediatR;

namespace FluentResults.Samples.MediatR.MediatRLogic.Handlers
{
    public class CommandWithoutResultHandler : AsyncRequestHandler<CommandWithoutResult>
    {
        protected override async Task Handle(CommandWithoutResult request, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Hello {nameof(CommandWithoutResult)}");
        }
    }
}
