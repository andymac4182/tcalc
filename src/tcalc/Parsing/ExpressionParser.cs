using Superpower;
using Superpower.Model;
using Superpower.Parsers;
using tcalc.Expressions;

using ExpressionTokenParser = Superpower.TokenListParser<tcalc.Parsing.ExpressionToken, tcalc.Expressions.Expression>;

namespace tcalc.Parsing
{
    public static class ExpressionParser
    {
        public static ExpressionTokenParser Number { get; } =
            Token.EqualTo(ExpressionToken.Number)
                .Apply(Numerics.DecimalDouble)
                .Select(d => (Expression)new NumericValue(d));

        private static ExpressionTokenParser TicketTypeAmount { get; } =
            from lcurly in Token.EqualTo(ExpressionToken.LCurly)
            from expr in Token.EqualTo(ExpressionToken.Guid)
            from rcurly in Token.EqualTo(ExpressionToken.RCurly)
            select (Expression)new TicketTypeExpression(expr.ToStringValue());

        public static TokenListParser<ExpressionToken, Operator> Op(ExpressionToken token, Operator op) => 
            Token.EqualTo(token)
                .Value(op);

        public static TokenListParser<ExpressionToken, LogicOperator> Op(ExpressionToken token, LogicOperator op) =>
            Token.EqualTo(token)
                .Value(op);

        public static TokenListParser<ExpressionToken, EqualityOperator> Op(ExpressionToken token, EqualityOperator op) =>
            Token.EqualTo(token)
                .Value(op);

        public static TokenListParser<ExpressionToken, Operator> Add { get; } = Op(ExpressionToken.Plus, Operator.Add);
        public static TokenListParser<ExpressionToken, Operator> Subtract { get; } = Op(ExpressionToken.Minus, Operator.Subtract);
        public static TokenListParser<ExpressionToken, Operator> Multiply { get; } = Op(ExpressionToken.Asterisk, Operator.Multiply);
        public static TokenListParser<ExpressionToken, Operator> Divide { get; } = Op(ExpressionToken.Slash, Operator.Divide);

        public static TokenListParser<ExpressionToken, LogicOperator> Or { get; } = Op(ExpressionToken.Or, LogicOperator.Or);
        public static TokenListParser<ExpressionToken, LogicOperator> And { get; } = Op(ExpressionToken.And, LogicOperator.And);
        public static TokenListParser<ExpressionToken, EqualityOperator> Equal { get; } = Op(ExpressionToken.Equal, EqualityOperator.Equal);
        public static TokenListParser<ExpressionToken, EqualityOperator> LessThan { get; } = Op(ExpressionToken.LessThan, EqualityOperator.LessThan);
        public static TokenListParser<ExpressionToken, EqualityOperator> GreaterThan { get; } = Op(ExpressionToken.LessThan, EqualityOperator.GreaterThan);

        public static ExpressionTokenParser Literal { get; } = Number.Or(TicketTypeAmount);

        static ExpressionTokenParser Factor { get; } =
            (from lparen in Token.EqualTo(ExpressionToken.LParen)
             from expr in Parse.Ref(() => LogicalOperator)
             from rparen in Token.EqualTo(ExpressionToken.RParen)
             select expr)
            .Or(Literal);
        
        static ExpressionTokenParser Term { get; } = Parse.Chain(Multiply.Or(Divide), Factor, BinaryExpression.Create);
        static ExpressionTokenParser Expression { get; } = Parse.Chain(Add.Or(Subtract), Term, BinaryExpression.Create);
        private static ExpressionTokenParser EqualityCheck { get; } = Parse.Chain(Equal.Or(LessThan).Or(GreaterThan), Expression, (equalityOp, lexpr, rexpr) => new EqualityExpression(lexpr, rexpr, equalityOp));
        private static ExpressionTokenParser LogicalOperator { get; } = Parse.Chain(Or.Or(And), EqualityCheck, LogicExpression.Create);

        static ExpressionTokenParser Source { get; } = LogicalOperator.AtEnd();

        public static bool TryParse(TokenList<ExpressionToken> tokens, out Expression expr, out string error, out Position errorPosition)
        {
            var result = Source(tokens);
            if (!result.HasValue)
            {
                expr = null;
                error = result.ToString();
                errorPosition = result.ErrorPosition;
                return false;
            }

            expr = result.Value;
            error = null;
            errorPosition = Position.Empty;
            return true;
        }
    }

    internal class EqualityExpression : Expression
    {
        public Expression LeftExpression { get; }
        public Expression RightExpression { get; }
        public EqualityOperator EqualityOperator { get; }

        public EqualityExpression(Expression leftExpression, Expression rightExpression,
            EqualityOperator equalityOperator)
        {
            LeftExpression = leftExpression;
            RightExpression = rightExpression;
            EqualityOperator = equalityOperator;
        }
    }

    public class TicketTypeExpression : Expression
    {
        public string TicketTypeId { get; }

        public TicketTypeExpression(string ticketTypeId)
        {
            TicketTypeId = ticketTypeId;
        }
    }
}
