using System;
using System.Linq.Expressions;

namespace EvalTask
{
	class EvalProgram
	{
		static void Main(string[] args)
		{
		    string input = Console.In.ReadToEnd();
		    input = input.Replace(" ", "").Replace("\t", "");
		    var res = ExpressionParser.GetExpression(input);
            Console.WriteLine(res);           
		}
	}
}
