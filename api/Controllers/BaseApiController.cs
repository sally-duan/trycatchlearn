using api.Data;
using Microsoft.AspNetCore.Mvc;
using api.Entities;
using Microsoft.EntityFrameworkCore;
using api.Helpers;

namespace api.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [ApiController]

    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
    }

}