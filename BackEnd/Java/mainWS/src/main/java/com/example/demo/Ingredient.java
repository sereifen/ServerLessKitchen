package com.example.demo;

public class Ingredient {
    public String name;
    public int quantity;

    public Ingredient Clone(){
        Ingredient aux = new Ingredient();
        aux.quantity = this.quantity;
        aux.name = this.name;
        return aux;
    }
}
