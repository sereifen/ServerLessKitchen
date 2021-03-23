package com.example.demo;

import com.google.gson.Gson;

import java.util.HashMap;

public class InputOptimization {

    public HashMap<Integer,Recipe> Recipes = new HashMap<Integer,Recipe>();
    public HashMap<String,Ingredient> Ingredients = new HashMap<String,Ingredient>();

    /**
     * optimize by reducing waste
     * @return
     */
    public RecipesOptimization CalculateCountByRecipeOptimizeWaste() {
        HashMap<String,Ingredient> ingredientsLocal = new HashMap<>();

        for (Ingredient ingredient: Ingredients.values()) {
            Ingredient aux = new Ingredient();
            aux.quantity = ingredient.quantity;
            aux.name = ingredient.name;
            ingredientsLocal.put(ingredient.name,aux);
        }

        RecipesOptimization res = DeepExploration(ingredientsLocal,false);
        return  res;
    }

    /**
     * it start the algorithm that explore the graph node by deepness
     * @param ingredientsLocal the ingredients
     * @param maximizeCount if it mus be done for optimize count or for minimize waste
     * @return result
     */
    private RecipesOptimization DeepExploration(HashMap<String, Ingredient> ingredientsLocal, boolean maximizeCount) {

        RecipesOptimization best = new RecipesOptimization();
        best.recipeCount = 0;
        best.unusedInventoryCount = 0;
        for ( Ingredient ing:ingredientsLocal.values()) {
            best.unusedInventoryCount = best.unusedInventoryCount + ing.quantity;
        }
        best = DeepExplorationNode(ingredientsLocal,maximizeCount,best,best);
        return best;
    }

    /**
     * it explore the actual node and send to the next one
     * @param ingredientsLocalOriginal the ingredients that we have so isnt posible to send to a node that isnt ok
     * @param maximizeCount if it mus be done for optimize count or for minimize waste
     * @param origin the where we are now
     * @param best the best that is finded
     * @return the best its find
     */
    private RecipesOptimization DeepExplorationNode(HashMap<String, Ingredient> ingredientsLocalOriginal, boolean maximizeCount, RecipesOptimization origin, RecipesOptimization best) {

        for (Recipe rec: Recipes.values()) {
            RecipesOptimization actual = origin.Clone();

            HashMap<String, Ingredient> ingredientsLocal = new HashMap<>();
            for (Ingredient ingr: ingredientsLocalOriginal.values()) {
                ingredientsLocal.put(ingr.name,ingr.Clone());
            }

            boolean usable = true;
            for ( Ingredient ing:rec.ingredients) {
                if (ingredientsLocal.get(ing.name).quantity< ing.quantity)
                    usable = false;
            }

            if (usable) {
                for ( Ingredient ing:rec.ingredients) {
                    ingredientsLocal.get(ing.name).quantity = ingredientsLocal.get(ing.name).quantity - ing.quantity;
                    actual.unusedInventoryCount =actual.unusedInventoryCount - ing.quantity;
                }
                actual.recipeCount = actual.recipeCount +1;
                boolean find =false;
                for ( RecipeCount Rc: actual.recipes) {
                    if (Rc.id == rec.id){
                        Rc.count = Rc.count +1;
                        find = true;
                    }
                }
                if (!find){
                    RecipeCount aux = new RecipeCount(rec.id);
                    aux.count = 1;
                    actual.recipes.add(aux);
                }
                if (actual.recipeCount > best.recipeCount & maximizeCount)
                    best = actual;
                else if (actual.unusedInventoryCount < best.unusedInventoryCount & !maximizeCount)
                    best = actual;
                best = DeepExplorationNode(ingredientsLocal,maximizeCount,actual,best);
            }
        }
        return best;
    }

    /**
     * optimization by count
     * @return result of optimization
     */
    public RecipesOptimization CalculateCountByRecipeOptimizeCount() {
        HashMap<String,Ingredient> ingredientsLocal = new HashMap<>();

        for (Ingredient ingredient: Ingredients.values()) {
            Ingredient aux = new Ingredient();
            aux.quantity = ingredient.quantity;
            aux.name = ingredient.name;
            ingredientsLocal.put(ingredient.name,aux);
        }

        RecipesOptimization res = DeepExploration(ingredientsLocal,true);

        return  res;
    }

    @Override
    public String toString() {
        return new Gson().toJson(this);
    }
}
