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
                         .Map(value => new WeatherForecastDto
                                       {
                                           Date = DateTime.Now,
                                           Summary = "Hello World",
                                           TemperatureC = value
                                       })
                         .ToActionResult(_profile);
        }

        [HttpPost]
        public async Task<ActionResult<OkResponse<WeatherForecastDto>>> QueryWithTask(RequestDto request)
        {
            return await Domain.DomainQueryAsync(request.FailureType)
                               .Map(value => new WeatherForecastDto
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
        public async Task<ActionResult<OkResponse>> CommandWithTask(RequestDto request)
        {
            return await Domain.DomainCommandAsync(request.FailureType)
                               .ToActionResult(_profile);
        }

        //[HttpPost]
        //public async Task<ActionResult<PersonDto>> CreatePerson(CreatePersonCommand request)
        //{
        //    var result = await Domain.CreatePerson(request);

        //    return ????;
        //}

        //[HttpPost]
        //public async Task<ActionResult<PersonDto>> CreatePerson(CreatePersonCommand request)
        //{
        //    return await Domain.CreatePerson(request)
        //                                        .ToResultX(person => new PersonDto
        //                                                             {
        //                                                                 Id = person.Id,
        //                                                                 Vorname = person.Vorname,
        //                                                                 Nachname = person.Nachname
        //                                                             })
        //                                        .ToActionResult();
        //}
    }
}