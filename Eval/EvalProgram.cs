﻿using System;
using System.Linq.Expressions;

namespace EvalTask
{
	class EvalProgram
	{
		static void Main(string[] args)
		{
            //string input = Console.In.ReadToEnd();
		    var input = "2+2*2";
            input = input.Replace(" ", "").Replace("\token", "").Replace(".", ",");
            var res = ExpressionParser.GetExpression(input);
            Console.WriteLine(res);
		}
	}
}
