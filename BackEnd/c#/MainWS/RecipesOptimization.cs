using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MainWs;
using Newtonsoft.Json;

namespace MainWs
{
    public class RecipesOptimization
    {
        /// <summary>
        /// List of recipe counts 
        /// </summary>
        public List<RecipeCount> recipes { set; get; }
        public int recipeCount { set; get; }
        public int unusedInventoryCount { set; get; }

        /// <summary>
        /// void constructor with set default values
        /// </summary>
        public RecipesOptimization()
        {
            recipes = new List<RecipeCount>();
        }

        /// <summary>
        /// object to JSon
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// object to json and back to Json, so the reference breaks
        /// </summary>
        /// <returns></returns>
        public RecipesOptimization Clone()
        {
            return JsonConvert.DeserializeObject<RecipesOptimization>(this.ToString());
        }

    }
}
