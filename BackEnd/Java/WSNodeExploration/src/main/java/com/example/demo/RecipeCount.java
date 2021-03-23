package com.example.demo;

public class RecipeCount implements Comparable {
    public int id;
    public int count=0;
    RecipeCount(int id){
        this.id = id;
    }

    @Override
    public int compareTo(Object o) {
        return count - ((RecipeCount)o).count;
    }
}
