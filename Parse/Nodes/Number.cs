using System.Globalization;
using IronPascal.Lex;

namespace IronPascal.Parse
{
    public class Number : AST
    {
        readonly Token token;

        public Number(Token token)
        {
            this.token = token;
        }

        public double Value => double.Parse(token.Value, CultureInfo.InvariantCulture);
    }
}