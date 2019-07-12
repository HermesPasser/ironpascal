namespace IronPascal.Lex
{
    public enum TokenKind
    {
        Int,
        Real,

        RealConst,
        IntConst,

        Plus,
        Minus,
        Mul,
        IntDiv,
        FloatDiv,
        
        LParen,
        RParen,
        
        Id, // var id
        Assign,
        Semi,
        Dot,

        KeyProgram,
        KeyVar,
        KeyBegin,
        KeyEnd,

        Colon,
        Comma,

        Eof
    }
}