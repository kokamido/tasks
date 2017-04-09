using System;
using System.Collections.Generic;

namespace EvalTask
{
    public static class ExpressionParser
    {
        public static List<Token> Tokens = new List<Token>();
        public static Dictionary<char, Func<double, double, double>> dict =
            new Dictionary<char, Func<double, double, double>>()
            {
                {'+', (a, b) => a + b},
                {'-', (a, b) => a - b},
                {'*', (a, b) => a * b},
                {'/', (a, b) => a / b}
            };

        public static double GetExpression(string str)
        {
            var l = 0;
            for (int i = 0; i < str.Length; i++)//456+5675
            {

                if (i != 0)
                {
                    if (str[i].IsPrimaryOperator())
                    {
                        Tokens.Add(new Token() { type = TokenType.Value, value = str.Substring(l, i - l) });
                        AddOperatorToTokens(TokenType.PrimaryOperator, str[i].ToString());
                        l = i + 1;
                    }
                    if (str[i].IsSecondaryOperator())
                    {
                        Tokens.Add(new Token() { type = TokenType.Value, value = str.Substring(l, i - l) });
                        AddOperatorToTokens(TokenType.SecondaryOperator, str[i].ToString());
                        
                        l = i + 1;
                    }
                }

            }
            Tokens.Add(new Token() { type = TokenType.Value, value = str.Substring(l, str.Length - l) });
            
            for (int i = 0; i < Tokens.Count; i++)
            {
                if (Tokens[i].type == TokenType.PrimaryOperator)
                {
                    var result = dict[Tokens[i].value[0]](double.Parse(Tokens[i - 1].value),
                        double.Parse(Tokens[i + 1].value));
                    Tokens.RemoveRange(i,2);
                    Tokens[i - 1] = new Token() {type = TokenType.Value, value = result.ToString()};
                    i = i - 1;
                }
            }
            for (int i = 0; i < Tokens.Count; i++)
            {
                if (Tokens[i].type == TokenType.SecondaryOperator)
                {
                    var result = dict[Tokens[i].value[0]](double.Parse(Tokens[i - 1].value),
                        double.Parse(Tokens[i + 1].value));
                    Tokens.RemoveRange(i, 2);
                    Tokens[i - 1] = new Token() { type = TokenType.Value, value = result.ToString() };
                    i = i - 1;
                }
            }
            return double.Parse(Tokens[0].value);
        }

        public static void AddOperatorToTokens(TokenType type, string value)
        {
            Tokens.Add(new Token() { type = type, value = value });
        }
        public static bool IsPrimaryOperator(this char c)
        {
            return (c == '*' || c == '/');
        }
        public static bool IsSecondaryOperator(this char c)
        {
            return (c == '+' || c == '-');
        }
    }
}
