using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using EvalTask;
using Newtonsoft.Json;

namespace JsonConversion
{
	class JsonProgram
	{
		static void Main()
		{
			string json = Console.In.ReadToEnd();
			
            Console.Write(ConvertJson(json));
			

		}

	    public static string ConvertJson(string json)
	    {
            JObject v2 = JObject.Parse(json);
	        ProductV3[] newProducts;
            var version = v2["version"];
            var products = v2["products"];
	        try
	        {
	            var constants = JsonConvert.DeserializeObject<IDictionary<string, double>>(v2["constants"].ToString());
                newProducts = GetV3Array(products, constants);
            }
	        catch (NullReferenceException e)
	        {
                newProducts = GetV3Array(products);
            }
	        var result = new  JsonV3(newProducts);
	        return JsonConvert.SerializeObject(result, Formatting.None);
	    }

	    public static ProductV3[] GetV3Array(JToken v2Object, IDictionary<string, double> constantVals )
	    {
            ProductV3[] newProducts = new ProductV3[v2Object.Children().Count()];
	        int index = 0;
            foreach (var obj in v2Object)
            {
                var oldContent = obj.First;
                var id = obj.Path.Split('.')[1];

                var newContent = new ProductV3
                    (int.Parse(id),
                    oldContent["name"].ToString(),
                    double.Parse(ReplaceVarsWithNumbers(constantVals,oldContent["price"].ToString())), 
                    int.Parse(oldContent["count"].ToString()));
                newProducts[index] = newContent;
                index++;
            }
	        return newProducts;
	    }

        public static ProductV3[] GetV3Array(JToken v2Object)
        {
            ProductV3[] newProducts = new ProductV3[v2Object.Children().Count()];
            int index = 0;
            foreach (var obj in v2Object)
            {
                var oldContent = obj.First;
                var id = obj.Path.Split('.')[1];

                var newContent = new ProductV3
                    (int.Parse(id),
                    oldContent["name"].ToString(),
                    double.Parse(oldContent["price"].ToString()),
                    int.Parse(oldContent["count"].ToString()));
                newProducts[index] = newContent;
                index++;
            }
            return newProducts;
        }

        public static string ReplaceVarsWithNumbers(IDictionary<string, double> constatnsVals, string sourceExp)
	    {
	        foreach (var pair in constatnsVals.OrderByDescending((x)=>x.Key.Length))
	        {
                sourceExp = ExpressionParser.GetExpression(sourceExp.Replace(pair.Key, pair.Value.ToString()).Replace(".",",").Replace(" ", "")).ToString();
	        }
	        return sourceExp;
	    }


	}

    class JsonV3
    {
        [JsonProperty("version")] public string id = "3";

        [JsonProperty("products")]
        public ProductV3[] products;

        public JsonV3(ProductV3[] products)
        {
            this.products = products;
        }
    }

    class ProductV3
    {
        [JsonProperty("id")] public int id;
        [JsonProperty("name")] public string name;
        [JsonProperty("price")] public double price;
        [JsonProperty("count")] public int count;

        public ProductV3(int id, string name, double price, int count)
        {
            this.id = id;
            this.name = name;
            this.price = price;
            this.count = count;
        }
    }
}
