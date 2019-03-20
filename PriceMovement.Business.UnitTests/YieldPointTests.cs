namespace Tests
{
    using System;
    using System.Collections.Generic;
    using Moq;
    using NUnit.Framework;
    using PriceMovement.Business;
    using PriceMovement.Data;
    using PriceMovement.Domain;

    [TestFixture]
    public class YieldPointTests
    {
        private Mock<IYieldPointDataCommand> command;

        [SetUp]
        public void Setup()
        {
            this.command = new Mock<IYieldPointDataCommand>();
        }

        [Test]
        public void CanCreate()
        {
            var o = new YieldPoint(this.command.Object);
            Assert.Pass();
        }

        [Test]
        public void CanCall_NoFilters_ReturnsEmpty()
        {
            this.command.Setup(o => o.GetYieldPointData(It.IsAny<DateTime>())).Returns(new List<YieldPointRecord>());

            var y = new YieldPoint(this.command.Object);
            var result = y.GetYieldPointRecords(new DateTime()).Result;

            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void CanCall_SingleAssetClassFilters_ReturnsEmpty()
        {
            this.command.Setup(o => o.GetYieldPointData(It.IsAny<DateTime>())).Returns(new List<YieldPointRecord>());
            var a = "Fund";

            var y = new YieldPoint(this.command.Object);
            var result = y.GetYieldPointRecords(new DateTime(), a).Result;

            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void CanCall_MultipleAssetClassesFilters_ReturnsEmpty()
        {
            this.command.Setup(o => o.GetYieldPointData(It.IsAny<DateTime>())).Returns(new List<YieldPointRecord>());
            var a = "Fund,Bond";

            var y = new YieldPoint(this.command.Object);
            var result = y.GetYieldPointRecords(new DateTime(), a).Result;

            Assert.AreEqual(0, result.Count);
        }


        [Test]
        public void CanCall_SinlgeCurrencyFilters_ReturnsEmpty()
        {
            this.command.Setup(o => o.GetYieldPointData(It.IsAny<DateTime>())).Returns(new List<YieldPointRecord>());
            var c = "EUR";

            var y = new YieldPoint(this.command.Object);
            var result = y.GetYieldPointRecords(new DateTime(), c).Result;

            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void CanCall_MultipleCurrenciesFilters_ReturnsEmpty()
        {
            this.command.Setup(o => o.GetYieldPointData(It.IsAny<DateTime>())).Returns(new List<YieldPointRecord>());
            var c = "EUR,GBP";

            var y = new YieldPoint(this.command.Object);
            var result = y.GetYieldPointRecords(new DateTime(), c).Result;

            Assert.AreEqual(0, result.Count);
        }


        [Test]
        public void CanCall_AssetClassAndCurrencyFilters_ReturnsEmpty()
        {
            this.command.Setup(o => o.GetYieldPointData(It.IsAny<DateTime>())).Returns(new List<YieldPointRecord>());
            var a = "Fund,Bond";
            var c = "EUR,GBP";

            var y = new YieldPoint(this.command.Object);
            var result = y.GetYieldPointRecords(new DateTime(), a, c).Result;

            Assert.AreEqual(0, result.Count);
        }


        [Test]
        public void CanCall_AssetClassAndCurrencyFilters_ReturnsSinlgeData()
        {
            this.command.Setup(o => o.GetYieldPointData(It.IsAny<DateTime>())).Returns(new List<YieldPointRecord>
                                                                                       {
                                                                                           new YieldPointRecord{PriceCurrency = "EUR", AssetClass = "Bond"},
                                                                                           new YieldPointRecord { PriceCurrency = "USD", AssetClass = "FX Option" },
                                                                                           new YieldPointRecord { PriceCurrency = "JPY", AssetClass = "IRS" }
                                                                                       });
            var a = "Fund,Bond";
            var c = "EUR,GBP";

            var y = new YieldPoint(this.command.Object);
            var result = y.GetYieldPointRecords(new DateTime(), a, c).Result;

            Assert.AreEqual(1, result.Count);
        }


        [Test]
        public void CanCall_AssetClassAndCurrencyFilters_ReturnsMultipleData()
        {
            this.command.Setup(o => o.GetYieldPointData(It.IsAny<DateTime>())).Returns(new List<YieldPointRecord>
                                                                                       {
                                                                                           new YieldPointRecord{PriceCurrency = "EUR", AssetClass = "Bond"},
                                                                                           new YieldPointRecord { PriceCurrency = "GBP", AssetClass = "Fund" },
                                                                                           new YieldPointRecord { PriceCurrency = "JPY", AssetClass = "Bond" },
                                                                                           new YieldPointRecord { PriceCurrency = "EUR", AssetClass = "FX Option" }
                                                                                       });
            var a = "Fund,Bond";
            var c = "EUR,GBP";

            var y = new YieldPoint(this.command.Object);
            var result = y.GetYieldPointRecords(new DateTime(), a, c).Result;

            Assert.AreEqual(2, result.Count);
        }
    }
}