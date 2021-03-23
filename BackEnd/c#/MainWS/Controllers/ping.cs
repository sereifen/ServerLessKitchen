using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainWs.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ping : ControllerBase
    {
        [HttpGet]
        public string test()
        {
            return "pong";
        }
    }
}
