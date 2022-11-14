using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Financier.Domain.Portfolios;
using Financier.Domain.Trades;

namespace Financier.Tests
{
    [TestClass]
    public class TestPnL
    {
        [TestMethod]
        public void TestPnLWhenAmountIsNegative()
        {
            var trades = new List<Trade> {
                new Trade()
                {
                    DateTime = new DateTime(2019, 11, 1),
                    Direction = Direction.Buy,
                    Amount = 200,
                    Price = 220,
                    ExchangeTradeNo = 1,
                },
                new Trade()
                {
                    DateTime = new DateTime(2019, 11, 2),
                    Direction = Direction.Sell,
                    Amount = 100,
                    Price = 230,
                    ExchangeTradeNo = 2,
                },
                new Trade()
                {
                    DateTime = new DateTime(2019, 11, 2),
                    Direction = Direction.Sell,
                    Amount = 150,
                    Price = 240,
                    ExchangeTradeNo = 3,
                },
            };

            var amount = trades.Sum(x => x.Direction == Direction.Buy ? x.Amount : -x.Amount);
            var closedPnL = trades.Sum(x => x.Direction == Direction.Buy ? x.Amount : -x.Amount);

            var position = new Position();

            foreach (var trade in trades)
                position.AddTrade(trade);

            var pnL = position.ClosedTrades.Sum(x => x.GetPnL());

            Assert.IsTrue(pnL == 3000M);
            Assert.IsTrue(position.Amount == -50M);
        }

        [TestMethod]
        public void TestPnLWhenAmountIsZero()
        {
            var trades = new List<Trade> {
                new Trade()
                {
                    DateTime = DateTime.Parse("09.08.2018"),
                    Direction = Direction.Buy,
                    Amount = 300,
                    Price = 102.1M,
                    TotalCommission = 41.14M,
                    ExchangeTradeNo = 1,
                },
                new Trade()
                {
                    DateTime = DateTime.Parse("17.10.2018"),
                    Direction = Direction.Buy,
                    Amount = 300,
                    Price = 98M,
                    TotalCommission = 51.24M,
                    ExchangeTradeNo = 2,
                },
                new Trade()
                {
                    DateTime = DateTime.Parse("22.10.2018"),
                    Direction = Direction.Buy,
                    Amount = 300,
                    Price = 93M,
                    TotalCommission = 37.47M,
                    ExchangeTradeNo = 3,
                },
                new Trade()
                {
                    DateTime = DateTime.Parse("06.03.2019"),
                    Direction = Direction.Sell,
                    Amount = 300,
                    Price = 98.12M,
                    TotalCommission = 2.73M,
                    ExchangeTradeNo = 4,
                },
                new Trade()
                {
                    DateTime = DateTime.Parse("06.03.2019"),
                    Direction = Direction.Sell,
                    Amount = 300,
                    Price = 98M,
                    TotalCommission = 2.73M,
                    ExchangeTradeNo = 5,
                },
                new Trade()
                {
                    DateTime = DateTime.Parse("06.03.2019"),
                    Direction = Direction.Sell,
                    Amount = 50,
                    Price = 97.96M,
                    TotalCommission = 0.45M,
                    ExchangeTradeNo = 6,
                },
                new Trade()
                {
                    DateTime = DateTime.Parse("06.03.2019"),
                    Direction = Direction.Sell,
                    Amount = 10,
                    Price = 97.96M,
                    TotalCommission = 0.09M,
                    ExchangeTradeNo = 7,
                },
                new Trade()
                {
                    DateTime = DateTime.Parse("06.03.2019"),
                    Direction = Direction.Sell,
                    Amount = 240,
                    Price = 97.94M,
                    TotalCommission = 2.19M,
                    ExchangeTradeNo = 8,
                },
            };

            var position = new Position();

            foreach (var trade in trades)
                position.AddTrade(trade);

            var pnL = position.ClosedTrades.Sum(x => x.GetPnL());
            var commission = position.ClosedTrades.Sum(x => x.GetCommission());

            Assert.IsTrue(pnL == 151.16M);
            Assert.IsTrue(position.Amount == 0M);
        }
    }
}
