namespace IronPascal.Lex
{
    public enum TokenKind
    {
        Int, 
        
        Plus,
        Minus,
        Mul,
        Div,
        
        LParen,
        RParen,
        
        Id, // var id
        Assign,
        Semi,
        Dot,

        KeyBegin,
        KeyEnd,

        Eof
    }
}