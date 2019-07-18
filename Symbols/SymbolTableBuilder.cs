using System;
using IronPascal.Parse;
using IronPascal.Interpret;
namespace IronPascal
{
    class SymbolTableBuilder : NodeVisitor // TODO: nodevs need to be in .Interpret?
    {
        SymbolTable SymTab = new SymbolTable();

        public SymbolTableBuilder(){ }

        void VisitBlock(Block node)
        {
            foreach (var decl in node.Declarations)
                Visit(decl);
            Visit(node.CompoundStatement);
        }

        void VisitProgram(Parse.Program node)
        {
            Visit(node.block);
        }

        void VisitBinaryOperation(BinaryOperation node)
        {
            Visit(node.Left);
            Visit(node.Right);
        }

        void VisitNumber(Number node) {  }

        void VisitUnaryOperation(UnaryOperation node)
        {
            Visit(node.Expression);
        }

        void VisitCompound(Compound node)
        {
            foreach (var child in node.Children)
                Visit(child);
        }

        void VisitNoOperation(NoOperation node) { }

        void VisitVariableDeclaration(VariableDeclaration node)
        {
            string typeName = node.typeNode.Value;
            Symbol typeSym = SymTab.Lookup(typeName);
            string varName = node.VariableNode.Value;
            VarSymbol varSym = new VarSymbol(varName, typeSym);
            SymTab.Define(varSym);
        }

        void VisitAssign(Assign node)
        {
            string varName = node.Left.Value;
            Symbol varSym = SymTab.Lookup(varName);
            if (varSym == null)
                throw new Exception($"NameError: {varName}"); // TODO: add custom ex

            Visit(node.Right);
        }

        void VisitVariable(Variable node)
        {
            string varName = node.Value;
            Symbol varSym = SymTab.Lookup(varName);
            if (varSym == null)
                throw new Exception($"NameError: {varName}"); // TODO: add custom ex
        }
    }
}
