using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronPascal
{
    class BuildinTypeSimbol : Symbol
    {
        public BuildinTypeSimbol(string name) : base(name, null) { }
        public override string ToString() => $"<{GetType().Name}(Name='{Name}')>";
        
    }

}
