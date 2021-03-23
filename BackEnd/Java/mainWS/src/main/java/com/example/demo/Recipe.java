package com.example.demo;

import com.google.gson.Gson;

import java.util.ArrayList;

public class Recipe {
    public int id = -1;
    public String name = "";
    public String instructions = "";
    public ArrayList<Ingredient>  ingredients = new ArrayList<>();

    @Override
    public String toString() {
        return new Gson().toJson(this);
    }

    public String PatchRecipe(Recipe recipe) {

        if (recipe.id != -1)
            throw new RecipeBadRequestException();
        if (!recipe.instructions.isEmpty())
            this.instructions = recipe.instructions;
        if (recipe.ingredients.size() > 0)
            this.ingredients = recipe.ingredients;

        if (!recipe.name.isEmpty())
            this.name = recipe.name;
        return this.toString();
    }
}
