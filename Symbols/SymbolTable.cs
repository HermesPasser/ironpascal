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
            // TODO: the name here must be the same as in Lexer.ReservedKeywords

            //Insert(new BuildinTypeSimbol(TokenKind.Int.ToString()));
            //Insert(new BuildinTypeSimbol(TokenKind.Real.ToString()));
            Insert(new BuildinTypeSimbol("INTEGER"));
            Insert(new BuildinTypeSimbol("REAL"));
        }

        public void Insert(Symbol sym)
        {
#if DEBUG
            Console.WriteLine($"Insert: {sym}");
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
// TODO: emulate this behavior
/*
        symtab_header = 'Symbol table contents'
		
        lines = ['\n', symtab_header, '_' * len(symtab_header)]
        lines.extend(
            ('%7s: %r' % (key, value))
            for key, value in self._symbols.items()
        )
        lines.append('\n')
        s = '\n'.join(lines)
        return s
		
		Symbol table contents
_____________________
INTEGER: <BuiltinTypeSymbol(name='INTEGER')>
   REAL: <BuiltinTypeSymbol(name='REAL')>
 number: <VarSymbol(name='number', type='INTEGER')>
      a: <VarSymbol(name='a', type='INTEGER')>
      b: <VarSymbol(name='b', type='INTEGER')>
      y: <VarSymbol(name='y', type='REAL')>
*/
        public override string ToString() 
		{
			string symtabHeader = "Symbol table contents";
			//terminar isso
			return $"Symbols: [{string.Join(", ", Symbols.Values)}]";
		}
        
    }
}
