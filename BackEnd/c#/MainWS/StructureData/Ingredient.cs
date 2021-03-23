using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainWs
{
    public class Ingredient
    {
        public string name { set; get; }
        public int quantity { set; get; }

        /// <summary>
        /// clone the ingredient
        /// </summary>
        /// <returns></returns>
        public Ingredient Clone()
        {
            Ingredient aux = new Ingredient();
            aux.quantity = this.quantity;
            aux.name = this.name;
            return aux;
        }

        /// <summary>
        /// check if the given ingredients are ok to be joined to the list
        /// </summary>
        /// <param name="ingredientsList"></param>
        /// <returns></returns>
        public static bool CheckInventoryFill(List<Ingredient> ingredientsList) {
            foreach (Ingredient ingredient in ingredientsList)
            {
                if (ingredient.quantity < 0)
                    return false;
            }
		    return true;
	    }

        /// <summary>
        /// fill the actual inventory of ingredients
        /// </summary>
        /// <param name="ingredientsList">ingredients to be add</param>
        /// <param name="Ingredients">actual list of ingredients</param>
        internal static void FillInventory(List<Ingredient> ingredientsList, Dictionary<string, Ingredient> Ingredients)
        {         
            foreach (Ingredient ingredient in ingredientsList){
                if (!Ingredients.ContainsKey(ingredient.name))
                    Ingredients.Add(ingredient.name, ingredient);
                else
                {
                    Ingredients[ingredient.name].quantity = Ingredients[ingredient.name].quantity + ingredient.quantity;
                }
            }
        }
    }
}
