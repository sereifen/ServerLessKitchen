using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainWs
{
    public class DataComunications
    {
        /// <summary>
        /// actual recipes
        /// </summary>
        public static Dictionary<string, Recipe> Recipes = new Dictionary<string, Recipe>();
        /// <summary>
        /// actual ingredients
        /// </summary>
        public static Dictionary<string, Ingredient> Ingredients = new Dictionary<string, Ingredient>();
    }
}
