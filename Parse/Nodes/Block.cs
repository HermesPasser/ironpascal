using System;
using System.Collections.Generic;

namespace IronPascal.Parse
{
    public class Block : AST
    {
        public readonly List<AST> Declarations;
        public readonly AST CompoundStatement;

        public Block(List<AST> decl, AST compState)
        {
            Declarations = decl;
            CompoundStatement = compState;
        }
    }
}
