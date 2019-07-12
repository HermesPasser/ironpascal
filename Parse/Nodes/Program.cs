using System;

namespace IronPascal.Parse
{
    public class Program : AST
    {
        public readonly string Name;
        public readonly Block block;
        public Program(string name, AST block)
        {
            Name = name;
            this.block = (Block)block;
        }
    }
}
