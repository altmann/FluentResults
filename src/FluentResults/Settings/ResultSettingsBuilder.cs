// ReSharper disable once CheckNamespace
namespace FluentResults
{
    public class ResultSettingsBuilder
    {
        public IResultLogger Logger { get; set; }

        public ResultSettingsBuilder()
        {
            // set defaults
            Logger = new DefaultLogger();
        }

        public ResultSettings Build()
        {
            return new ResultSettings
            {
                Logger = Logger
            };
        }
    }
}