// ReSharper disable once CheckNamespace
namespace FluentResults
{
    public interface IResultLogger
    {
        void Log(string context, ResultBase<IError> result);
        void Log<TContext>(ResultBase<IError> result);
    }
}