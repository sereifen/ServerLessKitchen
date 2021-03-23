using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace MainWs
{
    public class Recipe
    {
        public int id { set; get; }
        public string name { set; get; }
        public string instructions { set; get; }
        public List<Ingredient> ingredients { set; get; }

        /// <summary>
        /// constructor
        /// </summary>
        public Recipe(){
            id = -1;
            name = "";
            instructions = "";
            ingredients = new List<Ingredient>();
        }

        /// <summary>
        /// change the object into a JSon
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// check the fileds from the class and patch them
        /// </summary>
        /// <param name="recipe">recipe that contais the correct fields</param>
        /// <returns></returns>
        public Recipe PatchRecipe(Recipe recipe)
        {
            
            if (recipe.id != -1)
                throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest);
            if (!string.IsNullOrEmpty(recipe.instructions))
                this.instructions = recipe.instructions;
            if (recipe.ingredients.Count() > 0)
                this.ingredients = recipe.ingredients;

            if (!string.IsNullOrEmpty(recipe.name))
                this.name = recipe.name;
            return this;
        }

        /// <summary>
        /// check if there is enought ingredients to bild this recipe
        /// </summary>
        /// <param name="Ingredients">actual ingredient lis</param>
        /// <returns>true on enougth</returns>
        public bool CheckEnoughIngredientsForRecipe(Dictionary<string, Ingredient> Ingredients)
        {
            foreach (Ingredient ingredient  in ingredients)
            {
                if (!Ingredients.ContainsKey(ingredient.name))
                    return false;
                if (Ingredients[ingredient.name].quantity < ingredient.quantity)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// calculate how much recipes can be done with actual ingredients
        /// </summary>
        /// <param name="recipes"> actual recipes</param>
        /// <param name="ingredients"> actual ingredients</param>
        /// <returns></returns>
        internal static IEnumerable<RecipeCount> CalculateCountByRecipe(Dictionary<string, Recipe> recipes, Dictionary<string, Ingredient> ingredients)
        {
            List<RecipeCount> result = new List<RecipeCount>();

            foreach (Recipe recipe in recipes.Values)
            {
                RecipeCount aux = new RecipeCount(recipe.id);
                int min = ingredients[recipe.ingredients[0].name].quantity / recipe.ingredients[0].quantity;
                foreach (Ingredient ingredient in  recipe.ingredients)
                {
                    min = Math.Min(min, ingredients[ingredient.name].quantity / ingredient.quantity);
                }
                aux.count = min;
                result.Add(aux);
            }
            return result;
        }
    }
}
