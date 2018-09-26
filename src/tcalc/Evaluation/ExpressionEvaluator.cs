using System;
using System.Collections.Generic;
using tcalc.Expressions;
using tcalc.Parsing;

namespace tcalc.Evaluation
{
    public static class ExpressionEvaluator
    {
        public static Result Evaluate(Expression expression, Dictionary<Guid, int> ticketTypeAmounts)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            switch (expression)
            {
                case NumericValue numeric:
                    return new NumericResult(numeric.Value);
                case TicketTypeExpression tt:
                    return new NumericResult(ticketTypeAmounts[Guid.Parse(tt.TicketTypeId)]);
                case BinaryExpression binary:
                    return DispatchOperator(Evaluate(binary.Left, ticketTypeAmounts), Evaluate(binary.Right, ticketTypeAmounts), binary.Operator);
                case EqualityExpression equality:
                    return DispatchEquality(Evaluate(equality.LeftExpression, ticketTypeAmounts), Evaluate(equality.RightExpression, ticketTypeAmounts), equality.EqualityOperator);
                case LogicExpression logic:
                    return DispatchLogic(Evaluate(logic.Left, ticketTypeAmounts), Evaluate(logic.Right, ticketTypeAmounts), logic.Operator);
                default:
                    throw new ArgumentException($"Unsupported expression {expression}.");
            }
        }

        private static Result DispatchLogic(Result left, Result right, LogicOperator logicOperator)
        {
            if (left == null) throw new ArgumentNullException(nameof(left));
            if (right == null) throw new ArgumentNullException(nameof(right));

            switch (logicOperator)
            {
                case LogicOperator.Or:
                    return DispatchLogicOr(left, right);
                case LogicOperator.And:
                    return DispatchLogicAnd(left, right);
                default:
                    throw new ArgumentException($"Unsupported operator {logicOperator}.");
            }
        }

        private static Result DispatchLogicAnd(Result left, Result right)
        {
            if (left is BooleanResult ln && right is BooleanResult rn)
                return new BooleanResult(ln.Result && rn.Result);

            throw new EvaluationException($"Values {left} and {right} cannot be `and`ed.");
        }

        private static Result DispatchLogicOr(Result left, Result right)
        {
            if (left is BooleanResult ln && right is BooleanResult rn)
                return new BooleanResult(ln.Result || rn.Result);

            throw new EvaluationException($"Values {left} and {right} cannot be `or`ed.");
        }

        static Result DispatchEquality(Result left, Result right, EqualityOperator equalityOperator)
        {
            if (left == null) throw new ArgumentNullException(nameof(left));
            if (right == null) throw new ArgumentNullException(nameof(right));

            switch (equalityOperator)
            {
                case EqualityOperator.Equal:
                    return DispatchEqualityEqual(left, right);
                case EqualityOperator.LessThan:
                    return DispatchEqualityLessThan(left, right);
                case EqualityOperator.GreaterThan:
                    return DispatchEqualityLessThan(right, left);
                default:
                    throw new ArgumentException($"Unsupported operator {equalityOperator}.");
            }
        }

        private static Result DispatchEqualityLessThan(Result left, Result right)
        {
            if (left is NumericResult ln && right is NumericResult rn)
                return new BooleanResult(ln.Value < rn.Value);

            throw new EvaluationException($"Values {left} and {right} cannot be added.");
        }

        private static Result DispatchEqualityEqual(Result left, Result right)
        {
            if (left is NumericResult ln && right is NumericResult rn)
                return new BooleanResult(ln.Value == rn.Value);

            throw new EvaluationException($"Values {left} and {right} cannot be added.");
        }

        static Result DispatchOperator(Result left, Result right, Operator @operator)
        {
            if (left == null) throw new ArgumentNullException(nameof(left));
            if (right == null) throw new ArgumentNullException(nameof(right));

            switch (@operator)
            {
                case Operator.Add:
                    return DispatchAdd(left, right);
                case Operator.Subtract:
                    return DispatchSubtract(left, right);
                case Operator.Multiply:
                    return DispatchMultiply(left, right);
                case Operator.Divide:
                    return DispatchDivide(left, right);
                default:
                    throw new ArgumentException($"Unsupported operator {@operator}.");
            }
        }

        static Result DispatchAdd(Result left, Result right)
        {
            if (left is NumericResult ln && right is NumericResult rn)
                return new NumericResult(ln.Value + rn.Value);

            throw new EvaluationException($"Values {left} and {right} cannot be added.");
        }

        static Result DispatchSubtract(Result left, Result right)
        {
            if (left is NumericResult ln && right is NumericResult rn)
                return new NumericResult(ln.Value - rn.Value);

            throw new EvaluationException($"Value {right} cannot be subtracted from {left}.");
        }

        static Result DispatchMultiply(Result left, Result right)
        {
            if (left is NumericResult ln && right is NumericResult rn)
                return new NumericResult(ln.Value * rn.Value);

            throw new EvaluationException($"Values {left} and {right} cannot be multiplied.");
        }

        static Result DispatchDivide(Result left, Result right)
        {
            if (left is NumericResult ln && right is NumericResult rn)
                return new NumericResult(ln.Value / rn.Value);

            throw new EvaluationException($"Value {left} cannot be divided by {right}.");
        }
    }
}
