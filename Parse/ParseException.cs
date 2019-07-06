using System;

namespace IronPascal.Parse
{
    public class ParseException : Exception
    {
        public ParseException(string text) : base(text) { }
    }
}
