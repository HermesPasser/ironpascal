using IronPascal.Lex;

namespace IronPascal
{
    class Symbol
    {
        public readonly string Name;
        public readonly Symbol Type;

        public Symbol (string name, Symbol type)
        {
            Name = name;
            Type = type;
        }
    }
}
