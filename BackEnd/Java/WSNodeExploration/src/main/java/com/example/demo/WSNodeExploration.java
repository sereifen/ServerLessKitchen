
package com.example.demo;
import com.google.gson.Gson;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.web.bind.annotation.*;

@SpringBootApplication
@RestController
public class WSNodeExploration {

	public static void main(String[] args) {
		SpringApplication.run(WSNodeExploration.class, args);
	}

	/**
	 * here the function is reduced to call for optimization and json the result
	 * @param input data for optimize
	 * @return json of result
	 */
	@PostMapping("/optimize-total-count")
	public String GetCountByRecipeOptimize(@RequestBody InputOptimization input) {

		String res  = new Gson().toJson(input.CalculateCountByRecipeOptimizeCount());
		return res;
	}

	/**
	 * here the function is reduced to call for optimization and json the result
	 * @param input data for optimize
	 * @return json of result
	 */
	@PostMapping("/optimize-total-waste")
	public String GetCountByRecipeOptimizeWaste(@RequestBody InputOptimization input) {

		String res = new Gson().toJson(input.CalculateCountByRecipeOptimizeWaste());
		return res;
	}


}
            