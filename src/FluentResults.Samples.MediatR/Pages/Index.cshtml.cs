using System.Threading.Tasks;
using FluentResults.Samples.MediatR.MediatRLogic.Messages;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace FluentResults.Samples.MediatR.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IMediator _mediator;

        public IndexModel(ILogger<IndexModel> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task OnGet()
        {
            var result1 = await _mediator.Send(new CommandWithResult());
            _logger.LogInformation(result1.ToString());

            var result2 = await _mediator.Send(new QueryWithResult());
            _logger.LogInformation(result2.ToString());

            var result3 = await _mediator.Send(new CommandWithoutResult());
            _logger.LogInformation(result3.ToString());

            var result4 = await _mediator.Send(new QueryWithoutResult());
            _logger.LogInformation(result4.ToString());
        }
    }
}
