using IronPascal.Lex;

namespace IronPascal.Parse
{
    public class TypeNode : AST
    {
        public readonly Token token;
        public readonly object Value;

        public TypeNode(Token token)
        {
            this.token = token;
            Value = this.token.Value;
        }
    }
}