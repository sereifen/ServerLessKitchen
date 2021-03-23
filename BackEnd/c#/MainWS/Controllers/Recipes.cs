using MainWs.Excepcions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using System.Web.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MainWs.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class recipes : ControllerBase
    {
        /// <summary>
        /// get all recipes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Recipe> Get()
        {
            return DataComunications.Recipes.Values;
        }

        /// <summary>
        /// get one recipe by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public Recipe Get(string id)
        {
            if (DataComunications.Recipes.ContainsKey(id))
                return DataComunications.Recipes[id];
            else
                throw new HttpResponseException(404,"There not exists any recipe with ID " +id);
        }

        /// <summary>
        /// create a recipe and give the id
        /// </summary>
        /// <param name="recipe"> recipe to be added </param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public Recipe Post([FromBody] Recipe recipe)
        {
            recipe.id = (DataComunications.Recipes.Keys.Count > 0 ? int.Parse(DataComunications.Recipes.Keys.Max()) :0) + 1;
            DataComunications.Recipes.Add(recipe.id.ToString(), recipe);
            return recipe;
        }

        /// <summary>
        /// make tha recipe 
        /// </summary>
        /// <param name="id"> ID from recipe</param>
        /// <returns>"Yummy! if everything is ok (ingredients and ID)</returns>
        [HttpPost]
        [Route("{id}/make")]
        public string Make(string id)
        {
            if (DataComunications.Recipes.ContainsKey(id))
                if (DataComunications.Recipes[id].CheckEnoughIngredientsForRecipe(DataComunications.Ingredients))
                    return "Yummy!";
                else
                    throw new HttpResponseException(400,"Not enought ingredients for make the recipe");
            else
                throw new HttpResponseException(404, "There not exists any recipe with ID " + id);
        }

        /// <summary>
        /// path one recipe
        /// </summary>
        /// <param name="id"> ID to be pathced </param>
        /// <param name="recipe"> Recipe (only the fields to be patch)</param>
        /// <returns>recipe patched</returns>
        [HttpPatch("{id}")]
        public Recipe Put(string id, [FromBody] Recipe recipe)
        {
            if (DataComunications.Recipes.ContainsKey(id))
                return DataComunications.Recipes[id].PatchRecipe(recipe);
            else
                throw new HttpResponseException(404, "There not exists any recipe with ID " + id);
        }

        /// <summary>
        /// Delete a recipe by ID
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            if (DataComunications.Recipes.ContainsKey(id))
                DataComunications.Recipes.Remove(id);
        }

        /// <summary>
        /// give the optimization count by the inventory and recipes that it have
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("optimize-total-count")]
        public RecipesOptimization GetCountByRecipeOptimize()
        {
            return OptimizationRequest.SendRequestToOptimization("http://localhost:8081/recipes/optimize-total-count");
        }

        /// <summary>
        /// give the optimization waste by the inventory and recipes that it have
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("optimize-total-waste")]
        public RecipesOptimization GetCountByRecipeOptimizeWaste()
        {
            return OptimizationRequest.SendRequestToOptimization("http://localhost:8081/recipes/optimize-total-waste");
        }

        /// <summary>
        /// return the number of every recipe can be done with the actual ingredients
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-count-by-recipe")]
        public IEnumerable<RecipeCount> GetCountByRecipe()
        {
            return Recipe.CalculateCountByRecipe(DataComunications.Recipes, DataComunications.Ingredients);
        }
    }
}
