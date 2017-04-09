using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvalTask
{
    public enum TokenType
    {
        Value,
        PrimaryOperator,
        SecondaryOperator
    }
    public class Token
    {
        //public Token(string value, TokenType type)
        public string value { get; set; }
        public TokenType type;
    }
}
