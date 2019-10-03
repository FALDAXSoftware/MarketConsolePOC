using MarketConsolePOC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MarketConsolePOC
{

    public class OrderProcessor
    {
        public ManualResetEvent manualResetEvent;
        public uint OrderNo;
        public CreateMarketOrderRequest CreateMarketrequest;
        public CreateMarketOrderResponse CreateMarketresponse;
        public void ExecuteOrder()
        {
            //code for sending message
            manualResetEvent.WaitOne();
        }


    }

    public static class OrderHelper
    {
        private static readonly object objlock = new object();
        private static uint glastKnownOrderNo = 0;
        public static List<OrderProcessor> OrderProcessors = new List<OrderProcessor>();

        public static void ExecuteOrder(CreateMarketOrderRequest createMarketrequest)
        {
            OrderProcessor orderProcessor = new OrderProcessor();
            orderProcessor.CreateMarketrequest = createMarketrequest;
            AddOrderProcessor(orderProcessor);
            orderProcessor.ExecuteOrder();
        }

        public static void AddOrderProcessor(OrderProcessor orderProcessor)
        {
            lock (objlock)
            {
                glastKnownOrderNo++;
                orderProcessor.OrderNo = glastKnownOrderNo;
                OrderProcessors.Add(orderProcessor);                
            }
        }

        public static void RemoveOrderProcessor(OrderProcessor orderProcessor)
        {
            lock (objlock)
            {
                OrderProcessors.Remove(orderProcessor);
            }
        }
    }
}
