using MainWs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NodeExplorer
{
    public class NodeExplorator
    {
        internal static RecipesOptimization CalculateCountByRecipeOptimizeWaste(OptimizationRequest request)
        {
            Dictionary<string, Ingredient> ingredientsLocal = new Dictionary<string, Ingredient>();

            foreach (Ingredient ingredient in request.Ingredients.Values)
            {
                Ingredient aux = new Ingredient();
                aux.quantity = ingredient.quantity;
                aux.name = ingredient.name;
                ingredientsLocal.Add(ingredient.name, aux);
            }

            RecipesOptimization res = DeepExploration(ingredientsLocal, false, request.Recipes);
            return res;
        }

        internal static RecipesOptimization CalculateCountByRecipeOptimizeCount(OptimizationRequest request)
        {
            Dictionary<string, Ingredient> ingredientsLocal = new Dictionary<string, Ingredient>();

            foreach (Ingredient ingredient in request.Ingredients.Values)
            {
                Ingredient aux = new Ingredient();
                aux.quantity = ingredient.quantity;
                aux.name = ingredient.name;
                ingredientsLocal.Add(ingredient.name, aux);
            }

            RecipesOptimization res = DeepExploration(ingredientsLocal, true,request.Recipes);

            return res;
        }

        private static RecipesOptimization DeepExploration(Dictionary<string, Ingredient> ingredientsLocal, bool maximizeCount, Dictionary<string,Recipe> Recipes)
        {

            RecipesOptimization best = new RecipesOptimization();
            best.recipeCount = 0;
            best.unusedInventoryCount = 0;
            foreach (Ingredient ing in ingredientsLocal.Values)
            {
                best.unusedInventoryCount = best.unusedInventoryCount + ing.quantity;
            }
            best = DeepExplorationNode(ingredientsLocal, maximizeCount, best, best,Recipes);
            return best;
        }

        private static RecipesOptimization DeepExplorationNode(Dictionary<string, Ingredient> ingredientsLocalOriginal, bool maximizeCount, RecipesOptimization origin, RecipesOptimization best, Dictionary<string, Recipe> Recipes)
        {

            foreach (Recipe rec in Recipes.Values)
            {
                RecipesOptimization actual = origin.Clone();

                Dictionary<string, Ingredient> ingredientsLocal = new Dictionary<string, Ingredient>();
                foreach (Ingredient ingr in ingredientsLocalOriginal.Values)
                {
                    ingredientsLocal.Add(ingr.name, ingr.Clone());
                }

                bool usable = true;
                foreach (Ingredient ing in rec.ingredients)
                {
                    if (ingredientsLocal[ing.name].quantity < ing.quantity)
                        usable = false;
                }

                if (usable)
                {
                    foreach (Ingredient ing in rec.ingredients)
                    {
                        ingredientsLocal[ing.name].quantity = ingredientsLocal[ing.name].quantity - ing.quantity;
                        actual.unusedInventoryCount = actual.unusedInventoryCount - ing.quantity;
                    }
                    actual.recipeCount = actual.recipeCount + 1;
                    bool find = false;
                    foreach (RecipeCount Rc in  actual.recipes)
                    {
                        if (Rc.id == rec.id)
                        {
                            Rc.count = Rc.count + 1;
                            find = true;
                        }
                    }
                    if (!find)
                    {
                        RecipeCount aux = new RecipeCount(rec.id);
                        aux.count = 1;
                        actual.recipes.Add(aux);
                    }
                    if (actual.recipeCount > best.recipeCount & maximizeCount)
                        best = actual;
                    else if (actual.unusedInventoryCount < best.unusedInventoryCount & !maximizeCount)
                        best = actual;
                    best = DeepExplorationNode(ingredientsLocal, maximizeCount, actual, best,Recipes);
                }
            }
            return best;
        }
    }
}
