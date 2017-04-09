using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SimQLTask
{
	class SimQLProgram
	{
		static void Main(string[] args)
		{
            /*TestMain();*/

            var json = Console.In.ReadToEnd();
		    if (IsValidInput(json))
		    {
                Console.WriteLine("Неверный ввод");
                return;
		    }
		    try
		    {
                foreach (var result in ExecuteQueries(json))
                    Console.WriteLine(result);
            }
		    catch (Exception e)
		    {
		        Console.WriteLine(e);
		        throw;
		    }

        }

        static void TestMain()
        {
            var json = @"{
    'data': {
        'a': {
            'x':3.14, 
            'b': {'c':15}, 
            'c': {'c':9}
        }, 
        'z':42
    },
    'queries': [
        'a.b.c',
        'z',
        'a.x'
    ]
}";
            foreach (var result in ExecuteQueries(json))
                Console.WriteLine(result);
            Console.ReadKey();
        }



        public static IEnumerable<string> ExecuteQueries(string json)
		{
			var jObject = JObject.Parse(json);
			var data = (JObject)jObject["data"];
			var queries = jObject["queries"].ToObject<string[]>();
			return queries.Select(q => $"{q}{WEfyhjj(q, data)}");
		}

        public static string WEfyhjj(string path, JObject data)
        {
            data = JObject.Parse(JsonConvert.SerializeObject(data));
            
            var splittedPath = path.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            JToken jToken;
            double? value = null;
            /*foreach (var property in splittedPath)
            {

                bool isTryingSuccessful = data.TryGetValue(property, out jToken);
                if (!isTryingSuccessful)
                {
                    return "";
                }
                try
                {
                    
                    data = (JObject)jToken;
                }
                catch (Exception ex)
                {
                    value = (double)jToken;
                }
            }*/
            for (int i = 0; i < splittedPath.Length; i++)
            {
                var property = splittedPath[i];
                bool isTryingSuccessful = data.TryGetValue(property, out jToken);
                if (!isTryingSuccessful)
                {
                    return "";
                }
                try
                {

                    data = (JObject)jToken;
                }
                catch (Exception ex)
                {
                    if (i == splittedPath.Length - 1)
                    {
                        value = (double)jToken;
                    }
                    else
                    {
                        return "";
                    }
                }
            }
            if (value == null)
            {
                return "";
            }
            
            return $" = {((double)value).ToString(System.Globalization.CultureInfo.InvariantCulture)}";
        }

	    public static bool IsValidInput(string input)
	    {
	        try
	        {
                JObject.Parse("2*-3");
            }
	        catch (Exception)
	        {
	            return false;
	        }
	        return true;
	    }
	}
}
