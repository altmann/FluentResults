using FluentResults.Extensions;
using FluentResults.Extensions.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace FluentResults.Samples.WebHost.Controllers
{
    public class RequestDto
    {
        public string FailureType { get; set; }
    }

    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastWithControllerSpecificProfileController : ControllerBase
    {
        private readonly CustomAspNetCoreResultEndpointProfile _profile;

        public WeatherForecastWithControllerSpecificProfileController()
        {
            _profile = new CustomAspNetCoreResultEndpointProfile();
        }

        [HttpPost]
        public ActionResult<OkResponse<WeatherForecastDto>> Query(RequestDto request)
        {
            return Domain.DomainQuery(request.FailureType)
                         .ToResult(value => new WeatherForecastDto
                                            {
                                                Date = DateTime.Now,
                                                Summary = "Hello World",
                                                TemperatureC = value
                                            })
                         .ToActionResult(_profile);
        }

        [HttpPost]
        public async Task<ActionResult<OkResponse<WeatherForecastDto>>> QueryAsync(RequestDto request)
        {
            return await Domain.DomainQueryAsync(request.FailureType)
                               .ToResultX(value => new WeatherForecastDto
                                                   {
                                                       Date = DateTime.Now,
                                                       Summary = "Hello World",
                                                       TemperatureC = value
                                                   })
                               .ToActionResult(_profile);
        }

        [HttpPost]
        public ActionResult<OkResponse> Command(RequestDto request)
        {
            return Domain.DomainCommand(request.FailureType)
                         .ToActionResult(_profile);
        }

        [HttpPost]
        public async Task<ActionResult<OkResponse>> CommandAsync(RequestDto request)
        {
            return await Domain.DomainCommandAsync(request.FailureType)
                               .ToActionResult(_profile);
        }
    }
}