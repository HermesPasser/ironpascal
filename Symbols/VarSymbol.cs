
namespace IronPascal
{
    class VarSymbol : Symbol
    {
        public VarSymbol(string name, Symbol type) : base(name, type) { }
        public override string ToString() => $"<{GetType().Name}(Name='{Name}', Type='{Type}')>";
    }
}

		

