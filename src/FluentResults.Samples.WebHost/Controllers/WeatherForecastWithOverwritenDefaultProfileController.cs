using FluentResults.Extensions.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace FluentResults.Samples.WebHost.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastWithOverwritenDefaultProfileController : ControllerBase
    {
        [HttpPost]
        public ActionResult<SuccessResponse<WeatherForecastDto>> Query(RequestDto request)
        {
            return Domain.DomainQuery(request.FailureType)
                         .ToResult(value => new WeatherForecastDto
                                            {
                                                Date = DateTime.Now,
                                                Summary = "Hello World",
                                                TemperatureC = value
                                            })
                         .ToActionResult();
        }

        [HttpPost]
        public ActionResult<SuccessResponse> Command(RequestDto request)
        {
            return Domain.DomainCommand(request.FailureType)
                         .ToActionResult();
        }
    }
}