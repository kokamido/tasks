using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;

namespace SimQLTask
{
    [TestFixture]
    class SimQl_Tests
    {
        [Test]
        public void TestInTask()
        {
            //arrenge
            var input =@"{
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
            var expected = @"a.b.c = 15
z = 42
a.x = 3.14";
            //act
            var resultFull = "";
            foreach (var result in SimQLProgram.ExecuteQueries(input))
                resultFull += $"{result}\r\n";

            //Assert
            Assert.AreEqual(expected, resultFull.Substring(0, resultFull.Length-2));

        }
        [Test]
        public void TestInRelease1()
        {
            //arrenge
            var input = @"{""data"":{""empty"":{},""ab"":0,""x1"":1,""x2"":2,""y1"":{""y2"":{""y3"":3}}},""queries"":[""empty"",""xyz"",""x1.x2"",""y1.y2.z"",""empty.foobar""]}";
            var expected = @"empty =
xyz = 
x1.x2 = 
y1.y2.z = 
empty.foobar = 
";
            //act
            var resultFull = "";
            foreach (var result in SimQLProgram.ExecuteQueries(input))
                resultFull += $"{result}\r\n";

            //Assert
            Assert.AreEqual(expected, resultFull.Substring(0, resultFull.Length - 2));

        }
    }
}
