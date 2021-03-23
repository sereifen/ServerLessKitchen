using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MainWs
{
    public class OptimizationRequest
    {
        public Dictionary<string, Recipe> Recipes { set; get; }
        public Dictionary<string, Ingredient> Ingredients { set; get; }

        /// <summary>
        /// constructor with initializations
        /// </summary>
        public OptimizationRequest()
        {
            Recipes = new Dictionary<string, Recipe>();
            Ingredients = new Dictionary<string, Ingredient>();
        }

        /// <summary>
        /// send a request to the other server to calcularte the optimized
        /// </summary>
        /// <param name="uri">uri from other server</param>
        /// <returns>object with all the data</returns>
        internal static RecipesOptimization SendRequestToOptimization(string uri)
        {
            OptimizationRequest send = new OptimizationRequest();
            send.Recipes = DataComunications.Recipes;
            send.Ingredients = DataComunications.Ingredients;

            var client = new RestClient(uri);
			// client.Authenticator = new HttpBasicAuthenticator(username, password);
			var request = new RestRequest(Method.POST);
			//request.Resource = "Api/Score";
			request.RequestFormat = DataFormat.Json;
			request.AddJsonBody(Newtonsoft.Json.JsonConvert.SerializeObject(send));
			var response = client.Execute(request);
			var content = response.Content; // Raw content as string

            return JsonConvert.DeserializeObject<RecipesOptimization>(content); 
		}
    }
}
