using MediatR;

namespace FluentResults.Samples.MediatR.MediatRLogic.Messages
{
    public class QueryWithResult : IRequest<Result<QueryResponse>>
    {

    }
}