
package com.example.demo;
import com.google.gson.Gson;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.web.bind.annotation.*;

import java.io.*;
import java.net.HttpURLConnection;
import java.net.URL;
import java.net.URLConnection;
import java.nio.charset.StandardCharsets;
import java.util.*;
import java.util.stream.Collectors;

@SpringBootApplication
@RestController
public class MainWS {

	/**
	 * for avoid create a DB with this propose its used this static variable, contains all the recipes with the ID as key
	 */
	public static HashMap<Integer,Recipe> Recipes = new HashMap<Integer,Recipe>();

	/**
	 * for avoid create a DB with this propose its used this static variable, contains all the Ingredients with the name as KEy
	 */
	public static HashMap<String,Ingredient> Ingredients = new HashMap<String,Ingredient>();
	public int maxId = 1;

	public static void main(String[] args) {
		SpringApplication.run(MainWS.class, args);
	}

	/**
	 * its just a test function
	 * @return "pong"
	 */
	@GetMapping("/ping")
	public String Ping() {
		return "pong";
	}

	/**
	 * return all the recipes
	 * @return all the recipes
	 */
	@GetMapping("/recipes")
	public String Recipes() {
		return new Gson().toJson(Recipes.values());
	}

	/**
	 * delete recipes adn Ingredients
	 */
	@PostMapping("/clear")
	public void Clear() {
		Recipes.clear();
		Ingredients.clear();
	}

	/**
	 * create a new recipe and return it with the ID
	 * @param recipe recipe to be added
	 * @return the recipe with ID
	 */
	@PostMapping("/recipes/create")
	public String Recipe(@RequestBody Recipe recipe) {

		recipe.id = maxId;
		Recipes.put(recipe.id,recipe);
		maxId = maxId+ 1;
		return recipe.toString();
	}

	/**
	 * Patch the recipe, 404 on not found and 405 on try to fix ID
	 * @param id id to be patch
	 * @param recipe data that need to be fixed, (the id cannot be fixed)
	 * @return the recipe with the fix
	 */
	@PatchMapping("/recipes/{id}")
	public String RecipesPathRecipe(@PathVariable int id,@RequestBody Recipe recipe) {
		if (Recipes.containsKey(id))
			return Recipes.get(id).PatchRecipe(recipe);
		else
			throw new RecipeNotFoundException();
	}

	/**
	 * get the recipe with that Id 404 on not found
	 * @param id id that is need
	 * @return the recipe
	 */
	@GetMapping("/recipes/{id}")
	public String RecipesGet1(@PathVariable int id) {
		if (Recipes.containsKey(id))
			return new Gson().toJson(Recipes.get(id));
		else
			throw new RecipeNotFoundException();
	}

	/**
	 * delete the recipe with that ID
	 * @param id id to be deleted
	 */
	@DeleteMapping("/recipes/{id}")
	public void RecipesDelete(@PathVariable int id) {
		if (Recipes.containsKey(id))
			Recipes.remove(id);
	}

	/**
	 * make the recipe 404 on not found  405 on not enough ingredients
	 * @param id id to be done
	 * @return "yummy!" if it can be done
	 */
	@PostMapping("/recipes/{id}/make")
	public String RecipesMake(@PathVariable int id) {
		if (Recipes.containsKey(id))
			if (CheckEnoughIngredientsForRecipe(id))
				return "Yummy!";
			else
				throw new NotEnougthIngredientsException();
		else
			throw new RecipeNotFoundException();
	}

	/**
	 * get all ingredients
	 * @return all ingredients
	 */
	@GetMapping("/inventory")
	public String GetInventory() {
		return new Gson().toJson(Ingredients.values());
	}

	/**
	 * fill the ingredients with new if already exist its sum
	 * @param ingredientsList ingredients to be add
	 */
	@PostMapping("/inventory/fill")
	public void InventoryFill(@RequestBody ArrayList<Ingredient> ingredientsList) {
		if (CheckInventoryFill(ingredientsList))
			FillInventory(ingredientsList);
	}

	/**
	 * the max that it can be done by recipe
	 * @return the max can be done from any recipe
	 */
	@GetMapping("/recipes/get-count-by-recipe")
	public String GetCountByRecipe() {
		return new Gson().toJson(CalculateCountByRecipe(Ingredients));
	}

	/**
	 * get an optimization from other WS that will calculate considering a reduction on ingredients to maximize the number of recipes to be done
	 * @return result of optimization
	 * @throws IOException
	 */
	@GetMapping("/recipes/optimize-total-count")
	public String GetCountByRecipeOptimize() throws IOException {

		return	SendRequestToOptimization("http://localhost:8081/optimize-total-count");
	}

	/**
	 * optimize the waste of ingredients
	 * @return result of optimization
	 * @throws IOException
	 */
	@GetMapping("/recipes/optimize-total-waste")
	public String GetCountByRecipeOptimizeWaste() throws IOException {

		return	SendRequestToOptimization("http://localhost:8081/optimize-total-waste");
	}

	/**
	 * call to other webservice for optimization
	 * @param Uri url for the other webservice
	 * @return the result of optimization
	 * @throws IOException
	 */
	private String SendRequestToOptimization(String Uri) throws IOException {

		InputOptimization send = new InputOptimization();
		send.Recipes = this.Recipes;
		send.Ingredients = this.Ingredients;

		URL url = new URL(Uri);
		URLConnection con = url.openConnection();
		HttpURLConnection http = (HttpURLConnection)con;
		http.setRequestMethod("POST"); // PUT is another valid option
		http.setDoOutput(true);

		byte[] out = new Gson().toJson(send).getBytes(StandardCharsets.UTF_8);
		int length = out.length;

		http.setFixedLengthStreamingMode(length);
		http.setRequestProperty("Content-Type", "application/json;");
		http.connect();
		try(OutputStream os = http.getOutputStream()) {
			os.write(out);
		}

		return new BufferedReader(new InputStreamReader(http.getInputStream()))
				.lines().collect(Collectors.joining("\n"));
	}

	/**
	 * calculate the number of recipe that can be done with actual ingredients
	 * @param ingredientsLocal the ingredients
	 * @return result of calculation
	 */
	private ArrayList<RecipeCount> CalculateCountByRecipe(HashMap<String,Ingredient> ingredientsLocal) {
		ArrayList<RecipeCount> result = new ArrayList<>();

		for (Recipe recipe:Recipes.values()) {
			RecipeCount aux = new RecipeCount(recipe.id);
			int min = ingredientsLocal.get(recipe.ingredients.get(0).name).quantity/recipe.ingredients.get(0).quantity;
			for (Ingredient ingredient: recipe.ingredients) {
				min = Math.min(min,ingredientsLocal.get(ingredient.name).quantity/ingredient.quantity);
			}
			aux.count = min;
			result.add(aux);
		}
		return result;
	}

	/**
	 * add the ingredients to our ingredients list
	 * @param ingredientsList ingredients to be added
	 */
	private void FillInventory(ArrayList<Ingredient> ingredientsList) {
		for (Ingredient ingredient:ingredientsList){
			if (!Ingredients.containsKey(ingredient.name))
				Ingredients.put(ingredient.name,ingredient);
			else {
				Ingredients.get(ingredient.name).quantity =Ingredients.get(ingredient.name).quantity + ingredient.quantity;
			}
		}
	}

	/**
	 * check if there is any ingredient where quantity is <0
	 * @param ingredientsList ingredients to be check
	 * @return true if everything is ok
	 */
	private boolean CheckInventoryFill(ArrayList<Ingredient> ingredientsList) {
		for (Ingredient ingredient:ingredientsList){
			if (ingredient.quantity<0)
				return false;
		}
		return true;
	}

	/**
	 * check if there is enough ingredients to do the recipe
	 * @param id id from recipe
	 * @return true if can be done the recipe
	 */
	private boolean CheckEnoughIngredientsForRecipe(int id) {
		for (Ingredient ingredient:Recipes.get(id).ingredients) {
			if (!Ingredients.containsKey(ingredient.name))
				return false;
			if (Ingredients.get(ingredient.name).quantity <ingredient.quantity)
				return false;
		}
		return true;
	}

}
            