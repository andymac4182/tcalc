using Superpower;
using Superpower.Model;
using Superpower.Parsers;
using Superpower.Tokenizers;

namespace tcalc.Parsing
{
    public static class ExpressionTokenizer
    {
        public static TextParser<TextSpan> Guid { get; } =
            Span.Regex(@"[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");

        static Tokenizer<ExpressionToken> Tokenizer { get; } = new TokenizerBuilder<ExpressionToken>()
            .Match(Guid, ExpressionToken.Guid)
            .Match(Character.EqualTo('+'), ExpressionToken.Plus)
            .Match(Character.EqualTo('-'), ExpressionToken.Minus)
            .Match(Character.EqualTo('*'), ExpressionToken.Asterisk)
            .Match(Character.EqualTo('/'), ExpressionToken.Slash)
            .Match(Numerics.Decimal, ExpressionToken.Number, requireDelimiters: true)
            .Match(Character.EqualTo('('), ExpressionToken.LParen)
            .Match(Character.EqualTo(')'), ExpressionToken.RParen)
            .Match(Character.EqualTo('{'), ExpressionToken.LCurly)
            .Match(Character.EqualTo('}'), ExpressionToken.RCurly)
            .Match(Character.EqualTo('='), ExpressionToken.Equal)
            .Match(Character.EqualTo('<'), ExpressionToken.LessThan)
            .Match(Character.EqualTo('>'), ExpressionToken.GreaterThan)
            .Match(Span.EqualTo("||"), ExpressionToken.Or)
            .Match(Span.EqualTo("&&"), ExpressionToken.And)
            .Ignore(Span.WhiteSpace)
            .Build();

        public static Result<TokenList<ExpressionToken>> TryTokenize(string source)
        {
            return Tokenizer.TryTokenize(source);
        }
    }
}
