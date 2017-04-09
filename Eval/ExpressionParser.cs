using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using NUnit.Framework;

namespace EvalTask
{
    public static class ExpressionParser
    {
        //public static List<Token> tokens = new List<Token>();
        public static Dictionary<string, Func<double, double, double>> dict =
            new Dictionary<string, Func<double, double, double>>()
            {
                {"+", (a, b) => a + b},
                {"-", (a, b) => a - b},
                {"*", (a, b) => a * b},
                {"/", (a, b) => a / b}
            };

        [TestFixture]
        public class Tests
        {
            public List<Token> token;
            public List<Token> tokenInRPN;
            [SetUp]
            public void SetUp()
            {
                token = new List<Token>()
                {
                    new Token() {type = TokenType.Value, value = "-45"},
                    new Token() {type = TokenType.SecondaryOperator, value = "-"},
                    new Token() {type = TokenType.OpenBracket, value = "("},
                    new Token() {type = TokenType.Value, value = "2"},
                    new Token() {type = TokenType.SecondaryOperator, value = "+"},
                    new Token() {type = TokenType.Value, value = "3"},
                    new Token() {type = TokenType.CloseBracket, value = ")"}
                };
                tokenInRPN = new List<Token>()
                {
                    new Token() {type = TokenType.Value, value = "-45"}, 
                    new Token() {type = TokenType.Value, value = "2"},
                    new Token() {type = TokenType.Value, value = "3"},
                    new Token() {type = TokenType.SecondaryOperator, value = "+"},
                    new Token() {type = TokenType.SecondaryOperator, value = "-"}                    
                };


            }
            [Test]
            public void Test1()
            {
                var m = FormTokens("-aa-(b+c)");
                Assert.AreEqual(token.Count, m.Count);
                Assert.AreEqual(m[0].value, "-aa");
                Assert.AreEqual(m[4].value, "+");
                Assert.AreEqual(m[3].value, "b");
                Assert.AreEqual(m[5].value, "c");
            }

            [Test]
            public void Test2()
            {
                var m = GetExpressionInReversePolishNotation(token);
                Assert.AreEqual(5, m.Count);
                Assert.AreEqual(m[0].value, "-45");
                Assert.AreEqual(m[1].value, "2");
                Assert.AreEqual(m[2].value, "3");
                Assert.AreEqual(m[3].value, "+");
                Assert.AreEqual(m[4].value, "-");
            }

            [Test]
            public void Test3()
            {
                Assert.AreEqual(-50, GetResult(tokenInRPN));
            }
        }

        public static List<Token> GetExpressionInReversePolishNotation(List<Token> inputTokens)
        {
            var resultTokens = new List<Token>();
            var tokenStack = new Stack<Token>();
            for (int i = 0; i < inputTokens.Count; i++)
            {
                if (inputTokens[i].type == TokenType.Value) resultTokens.Add(inputTokens[i]);
                if (inputTokens[i].type == TokenType.OpenBracket) tokenStack.Push(inputTokens[i]);
                if (inputTokens[i].type == TokenType.CloseBracket)
                {
                    while (true)
                    {

                        var tokenFromStack = tokenStack.Pop();
                        if (tokenFromStack.type != TokenType.OpenBracket)
                        {
                            resultTokens.Add(tokenFromStack);                            
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                if (inputTokens[i].type == TokenType.SecondaryOperator)
                {
                    while (true)
                    {
                        if (tokenStack.Count != 0)
                        {
                            var tokenFromStack = tokenStack.Peek();
                            if (tokenFromStack.type == TokenType.SecondaryOperator ||
                                tokenFromStack.type == TokenType.PrimaryOperator)
                            {
                                tokenFromStack = tokenStack.Pop();
                                resultTokens.Add(tokenFromStack);
                            }
                            else
                            {
                                break;
                            }
                        }
                        else break;
                    }
                    tokenStack.Push(inputTokens[i]);
                }
                if (inputTokens[i].type == TokenType.PrimaryOperator)
                {
                    while (true)
                    {
                        if (tokenStack.Count != 0)
                        {
                            var tokenFromStack = tokenStack.Peek();
                            if (tokenFromStack.type == TokenType.PrimaryOperator)
                            {
                                tokenFromStack = tokenStack.Pop();
                                resultTokens.Add(tokenFromStack);
                            }
                            else
                            {
                                break;
                            }
                        }
                        else break;
                    }
                    tokenStack.Push(inputTokens[i]);
                }
            }
            while (tokenStack.Count!=0)
            {
                resultTokens.Add(tokenStack.Pop());
            }
            return resultTokens;
        }

        public static List<Token> FormTokens(string str)
        {
            var t = new List<Token>();
            var l = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (i != 0)
                {
                    if (str[i].IsPrimaryOperator())
                    {
                        t.Add(new Token() { type = TokenType.Value, value = str.Substring(l, i - l) });
                        AddOperatorToTokens(TokenType.PrimaryOperator, str[i].ToString(), t);
                        l = i + 1;
                    }
                    if (str[i].IsSecondaryOperator() && !(str[i - 1].IsSecondaryOperator() || str[i - 1].IsPrimaryOperator()))
                    {
                        t.Add(new Token() { type = TokenType.Value, value = str.Substring(l, i - l) });
                        AddOperatorToTokens(TokenType.SecondaryOperator, str[i].ToString(), t);
                        l = i + 1;
                    }
                
                if (str[i] == '(')
                    {
                        AddOperatorToTokens(TokenType.OpenBracket, "(", t);
                        l = i + 1;
                    }
                    if (str[i] == ')')
                    {
                        t.Add(new Token() { type = TokenType.Value, value = str.Substring(l, i - l) });
                        AddOperatorToTokens(TokenType.CloseBracket, ")", t);
                        l = i + 1;
                    }
                }

            }
            if (l <= str.Length - 1)
            {
                t.Add(new Token() {type = TokenType.Value, value = str.Substring(l, str.Length - l)});
            }
            return t;
        }

        public static double GetResult(List<Token> inputTokens)
        {
            var stack = new Stack<Token>();
            for (int i = 0; i < inputTokens.Count; i++)
            {
                if (inputTokens[i].type == TokenType.Value)
                {
                    stack.Push(inputTokens[i]);
                }
                else
                {
                    var arg1 = double.Parse(stack.Pop().value);
                    var arg2 = double.Parse(stack.Pop().value);
                    var operation = inputTokens[i].value;
                    stack.Push(new Token() {type = TokenType.Value, value = dict[operation](arg2,arg1).ToString()});
                }                
            }
            return double.Parse(stack.Pop().value);
        }

        public static double GetExpression(string str)
        {
            var tokens =  FormTokens(str);
            tokens = GetExpressionInReversePolishNotation(tokens);
            return GetResult(tokens);
        }

        public static void AddOperatorToTokens(TokenType type, string value, List<Token> t)
        {
            t.Add(new Token() { type = type, value = value });
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
