using System;

namespace FluentResults.Extensions.AspNetCore
{
    public static class AspNetCoreResult
    {
        internal static AspNetCoreResultSettings Settings { get; private set; }

        static AspNetCoreResult()
        {
            Settings = new AspNetCoreResultSettings();
        }

        /// <summary>
        /// Setup global settings
        /// </summary>
        public static void Setup(Action<AspNetCoreResultSettings> setupFunc)
        {
            var settingsBuilder = new AspNetCoreResultSettings();
            setupFunc(settingsBuilder);

            Settings = settingsBuilder;
        }

    }
}