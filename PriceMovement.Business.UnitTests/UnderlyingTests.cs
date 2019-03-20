namespace Tests
{
    using NUnit.Framework;
    using Moq;

    using PriceMovement.Business;
    using PriceMovement.Data;

    [TestFixture]
    public class UnderlyingTests
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
    }
}