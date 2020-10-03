// ReSharper disable once CheckNamespace
namespace FluentResults
{
    public class ResultSettingsBuilder
    {
        /// <summary>
        /// Set the ResultLogger
        /// </summary>
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