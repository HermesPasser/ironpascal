using System;
using System.Collections.Generic;
using IronPascal.Lex;

namespace IronPascal
{
    class SymbolTable
    {
        private Dictionary<string, Symbol> Symbols = new Dictionary<string, Symbol>();

        public SymbolTable()
        {
            InitBuildins();
        }

        private void InitBuildins()
        {
            Define(new BuildinTypeSimbol(TokenKind.Int.ToString()));
            Define(new BuildinTypeSimbol(TokenKind.Real.ToString()));
        }

        public void Define(Symbol sym)
        {
#if DEBUG
            Console.WriteLine($"Define: {sym}");
#endif
            Symbols.Add(sym.Name, sym);
        }

        public Symbol Lookup(string name)
        {
#if DEBUG
            Console.WriteLine($"Lookup: {name}");
#endif
            if (Symbols.ContainsKey(name))
                return Symbols[name];
            return null;
        }

        public override string ToString()
        {
            string str = "Symbols: {";

            foreach (var key in Symbols.Values)
                str += $" {key}";

            str += "}";
            return str;
        }
    }
}
