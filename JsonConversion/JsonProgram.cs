using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.Linq;
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
            var version = v2["version"];
            var products = v2["products"];
	        ProductV3[] newProducts = GetV3Array(products);
	        var result = new  JsonV3(newProducts);
	        return json;
	    }

	    public static ProductV3[] GetV3Array(JToken v2Object)
	    {
            ProductV3[] newProducts = new ProductV3[v2Object.Children().Count()];
            foreach (var obj in v2Object)
            {
                var oldContent = obj.First;
                var id = obj.Path.Split('.')[1];
                var newContent = new ProductV3
                    (int.Parse(id),
                    oldContent["name"].ToString(),
                    double.Parse(oldContent["price"].ToString()), 
                    int.Parse(oldContent["count"].ToString()));
                newProducts[int.Parse(id) - 1] = newContent;
            }
	        return newProducts;
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
