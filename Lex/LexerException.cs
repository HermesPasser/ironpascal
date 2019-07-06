using System;

namespace IronPascal.Lex
{
    class LexerException : Exception
    {
        public LexerException(string text) : base(text) { }
    }
}