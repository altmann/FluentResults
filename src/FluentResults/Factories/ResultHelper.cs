namespace FluentResults
{
    internal static class ResultHelper
    {
        public static TResult Merge<TResult>(params ResultBase[] results)
            where TResult : ResultBase<TResult>, new()
        {
            var finalResult = new TResult();

            foreach (var result in results)
            {
                foreach (var reason in result.Reasons)
                {
                    finalResult.WithReason(reason);
                }
            }

            return finalResult;
        }
    }
}