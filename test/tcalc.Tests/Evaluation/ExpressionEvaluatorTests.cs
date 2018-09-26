using System;
using System.Collections.Generic;
using tcalc.Evaluation;
using tcalc.Expressions;
using Xunit;

namespace tcalc.Tests.Evaluation
{
    public class ExpressionEvaluatorTests
    {
        [Fact]
        public void AddingNumbersYieldsANumber()
        {
            var left = new NumericValue(5);
            var right = new NumericValue(3);
            var expr = new BinaryExpression(Operator.Add, left, right);
            var result = ExpressionEvaluator.Evaluate(expr, TicketTypeAmounts);
            var actual = Assert.IsType<NumericResult>(result);
            Assert.Equal(8, actual.Value);
        }
        readonly Dictionary<Guid, int> TicketTypeAmounts = new Dictionary<Guid, int>
        {
            { Guid.Parse("37f3350f-dcfc-4be3-93c0-3014cfffaa02"), 3 }
        };
    }
}
