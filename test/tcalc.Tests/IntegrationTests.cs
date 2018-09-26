using System;
using System.Collections.Generic;
using Superpower.Model;
using tcalc.Evaluation;
using tcalc.Parsing;
using Xunit;

namespace tcalc.Tests
{
    public class IntegrationTests
    {
        [Theory]
        [InlineData("(1 + 2 = 3)", "True")]
        [InlineData("1 + 2 = 3", "True")]
        [InlineData("1 + 2 * 3 = 7", "True")]
        [InlineData("(1 + 2) * 3 = 9", "True")]
        [InlineData("(1 + 2) * 3 = 9 || 9 = 9", "True")]
        [InlineData("(1 + 2) * 3 = 9 && 9 = 9", "True")]
        [InlineData("(1 + 2) * 3 = 9 || 9 = 10", "True")]
        [InlineData("(1 + 2) * 3 = 9 && 9 = 10", "False")]
        [InlineData("(1 = 1 || 1 = 2)", "True")]
        [InlineData("((1 + 2) * 3 = 9) || (9 = 10)", "True")]
        [InlineData("((1 + 2) * 3 = 9 || 9 = 10) && 3 = 4", "False")]
        [InlineData("{37f3350f-dcfc-4be3-93c0-3014cfffaa02} = {37f3350f-dcfc-4be3-93c0-3014cfffaa02}", "True")]
        [InlineData("{37f3350f-dcfc-4be3-93c0-3014cfffaa02} * 3 = 9", "True")]
        [InlineData("({37f3350f-dcfc-4be3-93c0-3014cfffaa02} * {37f3350f-dcfc-4be3-93c0-3014cfffaa02}) = 9", "True")]
        public void ValidResultsAreComputed(string source, string result)
        {
            var tokens = ExpressionTokenizer.TryTokenize(source);
            Assert.True(tokens.HasValue, tokens.ToString());

            Assert.True(ExpressionParser.TryParse(tokens.Value, out var expr, out var err, out var errorPosition), err);
            var actual = ExpressionEvaluator.Evaluate(expr, TicketTypeAmounts);
            Assert.Equal(result, actual.ToString());
            Assert.Equal(errorPosition, new Position(0, 0, 0));
        }

        readonly Dictionary<Guid, int> TicketTypeAmounts = new Dictionary<Guid, int>
        {
            { Guid.Parse("37f3350f-dcfc-4be3-93c0-3014cfffaa02"), 3 }
        };
    }
}
