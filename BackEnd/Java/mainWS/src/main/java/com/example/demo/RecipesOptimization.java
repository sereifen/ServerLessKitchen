package com.example.demo;

import com.google.gson.Gson;

import java.util.ArrayList;

public class RecipesOptimization {
    public ArrayList<RecipeCount> recipes = new ArrayList<>();
    public int recipeCount;
    public int unusedInventoryCount;

    public RecipesOptimization Clone(){
        return new Gson().fromJson(new Gson().toJson(this), RecipesOptimization.class);
    }
}
