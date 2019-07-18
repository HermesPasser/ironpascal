using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronPascal.Lex;

namespace IronPascal
{
    class VarSymbol : Symbol
    {
        public VarSymbol(string name, Symbol type) : base(name, type) { }
        public override string ToString() => $"<{Name}:{Type}>";
    }
}
