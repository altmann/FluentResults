using FluentResults.Extensions.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace FluentResults.Samples.WebHost.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastWithOverwritenDefaultProfileController : ControllerBase
    {
        [HttpPost]
        public ActionResult<OkResponse<WeatherForecastDto>> Query(RequestDto request)
        {
            return Domain.DomainQuery(request.FailureType)
                         .Map(value => new WeatherForecastDto
                                            {
                                                Date = DateTime.Now,
                                                Summary = "Hello World",
                                                TemperatureC = value
                                            })
                         .ToActionResult();
        }

        [HttpPost]
        public ActionResult<OkResponse> Command(RequestDto request)
        {
            return Domain.DomainCommand(request.FailureType)
                         .ToActionResult();
        }
    }
}