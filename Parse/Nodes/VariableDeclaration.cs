using System;

namespace IronPascal.Parse
{
    class VariableDeclaration : AST
    {
        public readonly Variable VariableNode;
        public readonly TypeNode typeNode;

        public VariableDeclaration(Variable varNode, TypeNode typeNode)
        {
            VariableNode = varNode;
            this.typeNode = typeNode;
        }
    }
}
