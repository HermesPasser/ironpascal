using System;
using IronPascal.Parse;
using IronPascal.Interpret;

namespace IronPascal
{
    class SemanticAnalyzer : NodeVisitor
    {
        // TODO: create get and private set
        public SymbolTable SymTab = new SymbolTable();

        public SemanticAnalyzer(){ }

        public void VisitProgram(Parse.Program node) => Visit(node.block);

        public void VisitBinaryOperation(BinaryOperation node)
        {
            Visit(node.Left);
            Visit(node.Right);
        }

        public void VisitNumber(Number node) {  }

        public void VisitUnaryOperation(UnaryOperation node) => Visit(node.Expression);


        public void VisitCompound(Compound node)
        {
            foreach (var child in node.Children)
                Visit(child);
        }

        public void VisitNoOperation(NoOperation node) { }

        public void VisitVariableDeclaration(VariableDeclaration node)
        {
            string typeName = node.typeNode.Value;
            // TODO: throw if null?
            Symbol typeSym = SymTab.Lookup(typeName);
			
            string varName = node.VariableNode.Value;
            VarSymbol varSym = new VarSymbol(varName, typeSym);
			//TODO: create custom error
			if (SymTab.Lookup(varName) != null)
				throw new Exception($"Error: Duplicate indentifier '{varName}' found.");
            SymTab.Insert(varSym);
        }

        public void VisitAssign(Assign node)
        {
			// right-hand side
			Visit(node.Right);
			
			// left-hand side
			Visit(node.Left);            
        }

        public void VisitVariable(Variable node)
        {
            string varName = node.Value;
            Symbol varSym = SymTab.Lookup(varName);
            if (varSym == null)
                throw new Exception($"Error: Symbol(identifier) not found '{varName}'"); // TODO: add custom ex
        }

        public void VisitProcedureDeclaration(AST node)
        {
            //
        }

        public void VisitBlock(Block node)
        {
            foreach (var decl in node.Declarations)
                Visit(decl);
            Visit(node.CompoundStatement);
        }
    }
}
