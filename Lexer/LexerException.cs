using System;

namespace ExpressionInterpreter.Lex
{
    class LexerException : Exception
    {
        public LexerException(string text) : base(text) { }
    }
}