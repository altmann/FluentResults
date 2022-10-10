using FluentResults.Extensions.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace FluentResults.Samples.WebHost.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        public WeatherForecastController()
        {
        }

        [HttpPost]
        public ActionResult<SuccessResponse<WeatherForecastDto>> Query()
        {
            return DomainQuery(false)
                   .ToResult(value => new WeatherForecastDto
                                      {
                                          Date = DateTime.Now,
                                          Summary = "Hello World",
                                          TemperatureC = value
                                      })
                   .ToActionResult();
        }

        private Result<int> DomainQuery(bool failed)
        {
            if (failed)
            {
                return Result.Fail<int>("My Error");
            }

            return Result.Ok(12);
        }

        [HttpPost]
        public ActionResult Command()
        {
            return DomainCommand(false)
                .ToActionResult();
        }

        private Result DomainCommand(bool failed)
        {
            if (failed)
            {
                return Result.Fail("My Error");
            }

            return Result.Ok();
        }
    }


}