﻿using Faze.Abstractions.GameResults;
using Shouldly;
using System;
using Xunit;

namespace Faze.Abstractions.Tests
{
    public class WinLoseDrawResultAggregateTests
    {
        [Fact]
        public void CanCreateDefault()
        {
            var result = new WinLoseDrawResultAggregate();
            uint expectedValue = 0;

            result.Wins.ShouldBe(expectedValue);
            result.Loses.ShouldBe(expectedValue);
            result.Draws.ShouldBe(expectedValue);
        }

        [Fact]
        public void CanSumResults()
        {
            var result = new WinLoseDrawResultAggregate();

            result.Add(WinLoseDrawResult.Win);
            result.Add(WinLoseDrawResult.Win);
            result.Add(WinLoseDrawResult.Win);

            result.Wins.ShouldBe((uint)3);
            result.Draws.ShouldBe((uint)0);
            result.Loses.ShouldBe((uint)0);

            result.Add(WinLoseDrawResult.Lose);
            result.Add(WinLoseDrawResult.Lose);

            result.Wins.ShouldBe((uint)3);
            result.Loses.ShouldBe((uint)2);
            result.Wins.ShouldBe((uint)3);

            result.Add(WinLoseDrawResult.Draw);

            result.Wins.ShouldBe((uint)3);
            result.Loses.ShouldBe((uint)2);
            result.Draws.ShouldBe((uint)1);
        }

        [Fact]
        public void CanAddRange()
        {
            var resultAgg = new WinLoseDrawResultAggregate();
            var results = new[]
            {
                WinLoseDrawResult.Win,
                WinLoseDrawResult.Win,
                WinLoseDrawResult.Win,
                WinLoseDrawResult.Lose,
                WinLoseDrawResult.Lose,
                WinLoseDrawResult.Draw
            };

            resultAgg.AddRange(results);

            resultAgg.Wins.ShouldBe((uint)3);
            resultAgg.Loses.ShouldBe((uint)2);
            resultAgg.Draws.ShouldBe((uint)1);
        }

        [Fact]
        public void CanAddRangeOfAggregates()
        {
            var resultAgg = new WinLoseDrawResultAggregate();
            var results = new[]
            {
                new WinLoseDrawResultAggregate(3, 2, 1),
                new WinLoseDrawResultAggregate(3, 2, 1),
                new WinLoseDrawResultAggregate(3, 2, 1)
            };

            resultAgg.AddRange(results);

            resultAgg.Wins.ShouldBe((uint)9);
            resultAgg.Loses.ShouldBe((uint)6);
            resultAgg.Draws.ShouldBe((uint)3);
        }

        [Fact]
        public void ErrorsForUnknownResult()
        {
            WinLoseDrawResult unknownResult = (WinLoseDrawResult)5;
            var results = new WinLoseDrawResultAggregate();

            Should.Throw<Exception>(() =>
            {
                results.Add(unknownResult);
            });
        }
    }

}
