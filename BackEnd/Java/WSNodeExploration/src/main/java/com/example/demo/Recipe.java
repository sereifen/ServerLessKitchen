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
}
