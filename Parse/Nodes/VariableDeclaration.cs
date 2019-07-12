using System;

namespace IronPascal.Parse
{
    class VariableDeclaration : AST
    {
        public readonly Variable VariableNode;
        public readonly TypeNode TypeNode;

        public VariableDeclaration(AST varNode, AST typeNode)
        {
            VariableNode = (Variable) varNode;
            TypeNode = (TypeNode) typeNode;
        }
    }
}
