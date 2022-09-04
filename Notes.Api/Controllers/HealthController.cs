using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Api.Controllers
{
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok("SuccessFull");
    }
}
