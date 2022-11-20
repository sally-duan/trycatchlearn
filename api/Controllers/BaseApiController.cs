using api.Data;
using Microsoft.AspNetCore.Mvc;
using api.Entities;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [ApiController]

    [Route("api/[controller]")]
    public class BaseApiController:ControllerBase
    {
    }

}