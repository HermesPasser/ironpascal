using System;

namespace IronPascal.Parse
{
    class ProcedureDeclaration : AST
    {
        public readonly string ProcedureName;
        public readonly Block BlockNode;

        public ProcedureDeclaration(string name, Block node)
        {
            ProcedureName = name;
            BlockNode = node;
        }
    }
}
