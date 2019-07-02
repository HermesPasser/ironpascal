using System;

namespace ExpressionInterpreter.Parse
{
    class ParseException : Exception
    {
        public ParseException(string text) : base(text) { }
    }
}
