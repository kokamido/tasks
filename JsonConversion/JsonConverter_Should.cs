using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit;

using NUnit.Framework;

namespace JsonConversion
{
    [TestFixture]
    public class JsonConverter_Should
    {

        [Test]
        public void Simple()
        {
            var res = JsonProgram.ConvertJson(@"
{
	""version"": ""2"",
	""products"": {
		""1"": {
			""name"": ""Pen"",
			""price"": 12,
			""count"": 100
		},
		""2"": {
			""name"": ""Pencil"",
			""price"": 8,
			""count"": 1000
		},
		""3"": {
			""name"": ""Box"",
			""price"": 12.1,
			""count"": 50
		}
	}
}
");

            Assert.That(res, Is.EqualTo(CleanFormatting(@"
{
	""version"": ""3"",
	""products"": [
		{
			""id"": 1,
			""name"": ""Pen"",
			""price"": 12.0,
			""count"": 100
		},
		{
			""id"": 2,
			""name"": ""Pencil"",
			""price"": 8.0,
			""count"": 1000
		},
		{
			""id"": 3,
			""name"": ""Box"",
			""price"": 12.1,
			""count"": 50
		}
	]
}")));
        }
        [Test]
        public void WorkCorrectly_WithJson1Task()
        {
            var res = JsonProgram.ConvertJson(@"{""version"":""2"",""products"":{""642572671"":{""name"":""\\t\\t\\t\\t\\t\\t\\t\\t\\t\\t"",""price"":26755360,""count"":2147483647},""462028247"":{""name"":""\\t\\t\\t\\t\\t\\t\\t\\t\\t\\t"",""price"":1812829817,""count"":1583821338},""1064089862"":{""name"":""jtXpDL4AA"",""price"":1,""count"":1765575149},""441937189"":{""name"":""LPAI"",""price"":2119059550,""count"":260983550},""1493811026"":{""name"":""M"",""price"":1208992471,""count"":1},""1"":{""name"":"""",""price"":1,""count"":1},""1031623038"":{""name"":""XuNL"",""price"":188661436,""count"":0},""0"":{""name"":""Vz"",""price"":2147483647,""count"":1}}}");
            Assert.That(res, Is.EqualTo(CleanFormatting(@"
{
  ""version"": ""3"",
  ""products"": [
    {
      ""id"": 642572671,
      ""name"": ""\\t\\t\\t\\t\\t\\t\\t\\t\\t\\t"",
      ""price"": 26755360.0,
      ""count"": 2147483647
    },
    {
      ""id"": 462028247,
      ""name"": ""\\t\\t\\t\\t\\t\\t\\t\\t\\t\\t"",
      ""price"": 1812829817.0,
      ""count"": 1583821338
    },
    {
      ""id"": 1064089862,
      ""name"": ""jtXpDL4AA"",
      ""price"": 1.0,
      ""count"": 1765575149
    },
    {
      ""id"": 441937189,
      ""name"": ""LPAI"",
      ""price"": 2119059550.0,
      ""count"": 260983550
    },
    {
      ""id"": 1493811026,
      ""name"": ""M"",
      ""price"": 1208992471.0,
      ""count"": 1
    },
    {
      ""id"": 1,
      ""name"": """",
      ""price"": 1.0,
      ""count"": 1
    },
    {
      ""id"": 1031623038,
      ""name"": ""XuNL"",
      ""price"": 188661436.0,
      ""count"": 0
    },
    {
      ""id"": 0,
      ""name"": ""Vz"",
      ""price"": 2147483647.0,
      ""count"": 1
    }
  ]
}")));
        }

        [Test]
        public void WorkCorrectly_WithJson2Task()
        {
            var res = JsonProgram.ConvertJson(@"{ ""version"":""2"",""constants"":{ ""pi"":3.14},""products"":{ ""1"":{ ""name"":""product - name"",""price"":""12.3 * pi + pi + 4"",""count"":100} } }"
           );
            Console.WriteLine(JObject.Parse(@"{ ""version"":""2"",""constants"":{ ""pi"":3.14},""products"":{ ""1"":{ ""name"":""product - name"",""price"":""12.3 * pi + pi + 4"",""count"":100} } }"));
            Console.WriteLine(JObject.Parse(res));
            Assert.That(CleanFormatting(res), Is.EqualTo(CleanFormatting(@"
{
    ""version"": ""3"",
    ""products"": [
      {
      ""id"": 1,
      ""name"": ""product-name"",
      ""price"": 45.762,
      ""count"": 100
    }
  ]
}")));
        }

        private static string CleanFormatting(string json)
        {
            return json.Replace(" ", "").Replace("	", "").Replace("\n", "").Replace("\r", "");
        }

    }
}