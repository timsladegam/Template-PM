namespace Tests
{
    using NUnit.Framework;
    using Moq;

    using PriceMovement.Business;
    using PriceMovement.Data;
    using Microsoft.Extensions.Caching.Memory;

    [TestFixture]
    public class PriceMovementTests
    {
        private Mock<IPortfolioDataCommand> portfolioDataCommand;
        private Mock<IPriceDataCommand> priceDataCommand;
        private Mock<IStaticDataCommand> staticDataCommand;
        private Mock<IMemoryCache> cache;

        [SetUp]
        public void Setup()
        {
            this.portfolioDataCommand = new Mock<IPortfolioDataCommand>();
            this.priceDataCommand = new Mock<IPriceDataCommand>();
            this.staticDataCommand = new Mock<IStaticDataCommand>();
            this.cache = new Mock<IMemoryCache>();
        }

        [Test]
        public void CanCreate()
        {
            var o = new PriceMovements(this.portfolioDataCommand.Object, this.priceDataCommand.Object, this.staticDataCommand.Object, this.cache.Object);
            Assert.Pass();
        }
    }
}