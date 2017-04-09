using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace JsonConversion
{
    class JsonProcessor
    {
        public static string ReplaceVarsWithNumbers(IDictionary<string, double> constatnsVals, string sourceExp)
        {
            foreach (var pair in constatnsVals.OrderByDescending((x) => x.Key.Length))
            {
                sourceExp = sourceExp.Replace(pair.Key, pair.Value.ToString());
            }
            return sourceExp;
        }

        public static IDictionary<string, double> GetConstantDictionaryFromEval2Json(string json)
        {
            return JsonConvert.DeserializeObject<IDictionary<string, double>>(json);
        }
    }
}
