using System;

namespace IronPascal.Parse
{
    class VariableDeclaration : AST
    {
        public readonly Variable VariableNode;
        public readonly TypeNode typeNode;

        public VariableDeclaration(AST varNode, AST typeNode)
        {
            VariableNode = (Variable) varNode;
            this.typeNode = (TypeNode) typeNode;
        }
    }
}
