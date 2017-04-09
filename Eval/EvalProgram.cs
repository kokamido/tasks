using System;
using System.Linq.Expressions;

namespace EvalTask
{
	class EvalProgram
	{
		static void Main(string[] args)
		{
		    string input = Console.In.ReadToEnd();
		    var res = ExpressionParser.GetExpression(input);
            Console.WriteLine(res);           
		}
	}
}
