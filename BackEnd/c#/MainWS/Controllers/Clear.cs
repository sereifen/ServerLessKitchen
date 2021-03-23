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
    public class Clear : ControllerBase
    {
        /// <summary>
        /// clear function to delete everything
        /// </summary>
        [HttpPost]
        public void Post()
        {
            DataComunications.Recipes.Clear();
            DataComunications.Ingredients.Clear();
        }

    }
}
