namespace Tests
{
    using NUnit.Framework;
    using Moq;

    using PriceMovement.Business;
    using PriceMovement.Data;

    [TestFixture]
    public class PricesTests
    {
        private Mock<IPriceDataCommand> command;

        [SetUp]
        public void Setup()
        {
            this.command = new Mock<IPriceDataCommand>();
        }

        [Test]
        public void CanCreate()
        {
            var o = new Prices(this.command.Object);
            Assert.Pass();
        }
    }
}