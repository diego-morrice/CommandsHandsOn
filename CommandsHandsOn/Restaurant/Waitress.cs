using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Command.Application.Interfaces;

namespace Restaurant
{
    public class Waitress
    {
        private readonly List<IOrderCommand> OrderList;
        private Dictionary<IOrderCommand, Exception> OrdersNotPlaced;
        private int MissingOrderCount;

        public Waitress()
        {
            OrderList = new List<IOrderCommand>();
        }

        public void TakeOrder(IOrderCommand order)
        {
            OrderList.Add(order);
        }

        public async Task PlaceOrders()
        {
            await PlaceOrders(OrderList);

            if (OrdersNotPlaced.Any())
                await PlaceOrders(OrdersNotPlaced.Keys.ToList());
        }

        private async Task PlaceOrders(List<IOrderCommand> ordersList)
        {
            OrdersNotPlaced = new Dictionary<IOrderCommand, Exception>();
            MissingOrderCount = 0;

            foreach (var order in ordersList)
            {
                try
                {
                    var result = await order.Prepare();
                    if (result) continue;
                    MissingOrderCount += 1;
                    OrdersNotPlaced.Add(order, null);
                }
                catch (Exception ex)
                {
                    MissingOrderCount += 1;
                    OrdersNotPlaced.Add(order, ex);
                }
            }
        }

        public bool HasOrdersToPlace()
        {
            return OrderList.Any(c => c.IsPrepared == false);
        }

        public bool HasMissedAnyOrders()
        {
            return MissingOrderCount > 0;
        }

        public int GetMissingOrdersCount()
        {
            return MissingOrderCount;
        }
    }
}
