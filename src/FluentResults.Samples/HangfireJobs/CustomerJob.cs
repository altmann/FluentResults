using System;
using System.Threading.Tasks;
using FluentResults.Samples.WebController;
using Hangfire;

namespace FluentResults.Samples.HangfireJobs
{
    public class CustomerJob
    {
        public static void AddOrUpdateJob()
        {
            RecurringJob.AddOrUpdate(() => Dispatch(), Cron.Daily);
        }

        public static async Task<ResultDto> Dispatch()
        {
            // some logic
            Console.WriteLine("Fire-and-forget!");

            // Use an custom ResultDto class so that the serialization is in your control
            // Transform the FluentResult Result object to an custom ResultDto object as last as possible.
            return Result.Ok().ToResultDto();
        }
    }
}