using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace JsonConversion
{
    [TestFixture]
    class JsonProcessor_should
    {
        [Test]
        public void ReturnDictionary_WhenArgIsJson()
        {
            var res = JsonProcessor.GetConstantDictionaryFromEval2Json(@"{a:2,b:4}");
            Assert.That(res, Is.EqualTo(new Dictionary<string, int>(){ { "a",2},{ "b",4}}));
        }
    }
}
