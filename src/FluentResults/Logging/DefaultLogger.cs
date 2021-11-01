// ReSharper disable once CheckNamespace
namespace FluentResults
{
    public class DefaultLogger : IResultLogger
    {
        public void Log(string context, ResultBase<IError> result)
        {

        }

        public void Log<TContext>(ResultBase<IError> result)
        {

        }
    }
}