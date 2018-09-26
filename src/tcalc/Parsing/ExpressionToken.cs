using Superpower.Display;

namespace tcalc.Parsing
{
    public enum ExpressionToken
    {
        Number,

        [Token(Example = "+")]
        Plus,

        [Token(Example = "-")]
        Minus,

        [Token(Example = "*")]
        Asterisk,

        [Token(Example = "/")]
        Slash,

        [Token(Example = "(")]
        LParen,

        [Token(Example = ")")]
        RParen,

        [Token(Example = "{")]
        LCurly,

        [Token(Example = "}")]
        RCurly,

        [Token(Example = "00000000-0000-0000-0000-000000000000")]
        Guid,

        [Token(Example = "=")]
        Equal,

        [Token(Example = "<")]
        LessThan,

        [Token(Example = ">")]
        GreaterThan,

        [Token(Example = "||")]
        Or,

        [Token(Example = "&&")]
        And
    }
}