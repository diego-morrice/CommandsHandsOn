using System.Collections.Generic;
using Bogus;
using Command.Application.Command;
using Command.Application.Enums;
using Command.Application.Receiver;
using FluentAssertions;
using Restaurant;
using Xunit;

namespace CustomerTest
{
    public class MakeOrdersTest
    {

        private readonly Waitress Waitress;

        public MakeOrdersTest()
        {
            Waitress = new Waitress();
        }

        [Theory]
        [InlineData(20)]
        [InlineData(50)]
        [InlineData(100)]
        public async void Take_Valid_Orders_Success(int numberOfOrders)
        {
            var orders = FakerOrders(numberOfOrders);

            orders.ForEach(order => Waitress.TakeOrder(new CreateOrderCommand(order)));

            Waitress.HasOrdersToPlace().Should().BeTrue();

            await Waitress.PlaceOrders();

            Waitress.HasMissedAnyOrders().Should().BeFalse();
            Waitress.HasOrdersToPlace().Should().BeFalse();
            Waitress.GetMissingOrdersCount().Should().Be(0);
        }

        [Theory]
        [InlineData(20)]
        [InlineData(50)]
        [InlineData(100)]
        public async void Take_Invalid_Orders_Success(int numberOfOrders)
        {
            var orders = FakeOrdersInvalid(numberOfOrders);

            orders.ForEach(order => Waitress.TakeOrder(new CreateOrderCommand(order)));
            Waitress.HasOrdersToPlace().Should().BeTrue();
            await Waitress.PlaceOrders();

            Waitress.HasMissedAnyOrders().Should().BeTrue();
            Waitress.HasOrdersToPlace().Should().BeFalse();
            Waitress.GetMissingOrdersCount().Should().Be(numberOfOrders);
        }

      
        #region Fakers

        private static List<Order> FakerOrders(int numTests)
        {
            var fakeOrders = new Faker<Order>(locale: "pt_BR").StrictMode(false)
                .RuleFor(p => p.WaitressName, (f, p) => f.Name.FirstName())
                .RuleFor(p => p.Type, f => f.PickRandom<DishTypeEnum>().ToString());
                

            return fakeOrders.Generate(numTests);
        }

        private static List<Order> FakeOrdersInvalid(int numTests)
        {
            var fakeOrders = new Faker<Order>(locale: "pt_BR").StrictMode(false)
                .RuleFor(p => p.WaitressName, (f, p) => f.Name.FirstName());

            return fakeOrders.Generate(numTests);
        }

        #endregion
    }

}
