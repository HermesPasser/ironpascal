using System;
using System.Collections.Generic;

namespace IronPascal.Parse
{
    public class Compound : AST
    {
        public readonly List<AST> Children = new List<AST>();
        
        public void Add(AST node)
        {
            Children.Add(node);
        }
    }
}
