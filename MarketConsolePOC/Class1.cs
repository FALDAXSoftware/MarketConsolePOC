using MarketConsolePOC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MarketConsolePOC
{
    public enum OrderStatus
    {
        Pending = 1,
        Processed = 2,
        Cancel = 3,
        Complete = 4,
        ToBeRemove = 5
    }
    public class OrderProcessor
    {
        public ManualResetEvent manualResetEvent = new ManualResetEvent(false);
        public uint OrderNo;
        public OrderStatus orderStatus;
        public CreateMarketOrderRequest CreateMarketrequest;
        public CreateMarketOrderResponse CreateMarketresponse;



        public void ExecuteOrder()
        {
            //code for sending message
            Console.WriteLine($"enter into wait one of Order {OrderNo}" );
            manualResetEvent.WaitOne();
            Console.WriteLine($"exit from wait one of Order {OrderNo}");
        }


    }

    public static class OrderHelper
    {
        private static readonly object objlock = new object();
        private static uint glastKnownOrderNo = 0;
        public static List<OrderProcessor> OrderProcessors = new List<OrderProcessor>();
        public static bool ExitFlag = false;
        private static Random rnd = new Random();



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
                orderProcessor.orderStatus = OrderStatus.Pending;
                Console.WriteLine($"Order Request {orderProcessor.OrderNo} of {orderProcessor.CreateMarketrequest.ItemName}");
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



        public static void RandomOrderReceiver()
        {
            while (true)
            {
                if (ExitFlag) break;
                var pendingCompleteOrders = OrderProcessors.Where(x => x.orderStatus != OrderStatus.Complete);
                if (pendingCompleteOrders.Any())
                {
                    int count = OrderProcessors.Count();
                    int randomval = rnd.Next(0, count);

                    var op = OrderProcessors[randomval];
                    if (op.orderStatus == OrderStatus.Pending || op.orderStatus == OrderStatus.Processed)
                    {
                        if (op.orderStatus == OrderStatus.Pending)
                        {
                            op.orderStatus = OrderStatus.Processed;
                        }
                        else if (op.orderStatus == OrderStatus.Processed)
                        {
                            op.orderStatus = OrderStatus.Complete;                       
                        }
                        Console.WriteLine($"Order {op.OrderNo} {op.CreateMarketrequest.ItemName} State change to {op.orderStatus.ToString()}");
                        if(op.orderStatus == OrderStatus.Complete) ProcessOrder(op.OrderNo);
                    }
                }
                System.Threading.Thread.Sleep(5000);
            }
        }

        public static void ProcessOrder(uint orderid)
        {            
            var completeOrder = OrderProcessors.FirstOrDefault(x => x.OrderNo == orderid);
            completeOrder.manualResetEvent.Set();
            completeOrder.orderStatus = OrderStatus.ToBeRemove;
        }



    }
}
