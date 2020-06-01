// ReSharper disable once CheckNamespace
namespace FluentResults
{
    public interface IResultLogger
    {
        void Log(string context, ResultBase result);
    }
}