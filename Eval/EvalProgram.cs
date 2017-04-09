using System;
using System.Linq.Expressions;

namespace EvalTask
{
	class EvalProgram
	{
		static void Main(string[] args)
		{
            string input = Console.In.ReadToEnd();
		    //var input = " 5+ 34";
            input = input.Replace(" ", "").Replace("\t", "").Replace(".", ",");
            var res = ExpressionParser.GetExpression(input);
            Console.WriteLine(res);
		}
	}
}
