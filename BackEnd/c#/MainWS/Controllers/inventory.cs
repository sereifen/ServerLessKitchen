using MainWs.Excepcions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MainWs.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class inventory : ControllerBase
    {
        /// <summary>
        /// get for get all inventory
        /// </summary>
        /// <returns></returns>
        // GET: api/<inventory>
        [HttpGet]
        public IEnumerable<Ingredient> Get()
        {
            return DataComunications.Ingredients.Values;
        }

        /// <summary>
        /// add an ingredients list
        /// </summary>
        /// <param name="ingredientsList"> ingredients to add into list</param>
        [HttpPost]
        [Route("fill")]
        public void Post([FromBody] List<Ingredient> ingredientsList)
        {
            if (Ingredient.CheckInventoryFill(ingredientsList))
                Ingredient.FillInventory(ingredientsList,DataComunications.Ingredients);
            else
                throw new HttpResponseException(400,"There is ingredients with negative value");
        }
    }
}
