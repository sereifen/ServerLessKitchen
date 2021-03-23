using MainWs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NodeExplorer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class recipes : ControllerBase
    {
        // GET: api/<RecipeOptimization>
        [HttpPost]
        [Route("optimize-total-count")]
        public RecipesOptimization OptimizeCount([FromBody] OptimizationRequest request)
        {
            return NodeExplorator.CalculateCountByRecipeOptimizeCount(request);
        }

        [HttpPost]
        [Route("optimize-total-waste")]
        public RecipesOptimization OptimizeWaste([FromBody] OptimizationRequest request)
        {
            return NodeExplorator.CalculateCountByRecipeOptimizeWaste(request);
        }

        [HttpGet]
        [Route("ping")]
        public string test()
        {
            return "pong";
        }

    }
}
