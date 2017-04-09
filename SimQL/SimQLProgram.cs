﻿using System;
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
            foreach (var result in ExecuteQueries(json))
                Console.WriteLine(result);

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
			// TODO
			return queries.Select(q => $"{q} = {WEfyhjj(q, data)}");
		}

        public static string WEfyhjj(string path, JObject data)
        {
            data = JObject.Parse(JsonConvert.SerializeObject(data));
            
            var splittedPath = path.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            JToken jToken;
            double value = 0;
            foreach (var property in splittedPath)
            {

                bool isTryingSuccessful = data.TryGetValue(property, out jToken);
                if (!isTryingSuccessful)
                {
                    throw new Exception("No field");
                }
                try
                {
                    data = (JObject)jToken;
                }
                catch (Exception ex)
                {
                    value = (double)jToken;
                }
            }
            return value.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }
	}
}
