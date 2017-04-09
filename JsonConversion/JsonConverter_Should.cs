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

Assert.That(res, Is.EqualTo(@"
{
	""version"": ""3"",
	""products"": [
		{
			""id"": 1,
			""name"": ""Pen"",
			""price"": 12,
			""count"": 100
		},
		{
			""id"": 2,
			""name"": ""Pencil"",
			""price"": 8,
			""count"": 1000
		},
		{
			""id"": 3,
			""name"": ""Box"",
			""price"": 12.1,
			""count"": 50
		}
	]
}"));
        }
        
    }
}