using FixOrderConsole.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using QuickFix.Transport;
using QuickFix;
using QuickFix.FIX43;
using System.IO;
using QuickFix.Fields;

namespace FixOrderConsole
{
  
    
    public static class OrderHelper
    {
        private static readonly object objlock = new object();
        //   private static uint glastKnownOrderNo = 0;
        public static List<OrderProcessor> OrderProcessors = new List<OrderProcessor>();
        public static bool ExitFlag = false;
        // private static Random rnd = new Random();
        public static OrderSession _orderSession;
        static SocketInitiator initiator;



        public static void ExecuteOrder(CreateOrderRequest createOrderRequest)
        {
            OrderProcessor orderProcessor = new OrderProcessor();
            orderProcessor.createOrderRequest = createOrderRequest;
            AddOrderProcessor(orderProcessor);
            SendOrder(orderProcessor.createOrderRequest);
            orderProcessor.WaitForOrderExecution();
        }

        public static void AddOrderProcessor(OrderProcessor orderProcessor)
        {
            lock (objlock)
            {
                // glastKnownOrderNo++;
                orderProcessor.OrderNo = orderProcessor.createOrderRequest.ClOrdID;
                orderProcessor.orderStatus = Enums.OrderStatus.PENDING; // OrderStatus.Pending;
                LogWriter.WriteTraceLog($"Order Request Recevied  : {orderProcessor.OrderNo}");
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




        //public static void RandomOrderReceiver()
        //{
        //    while (true)
        //    {
        //        if (ExitFlag) break;
        //        var pendingCompleteOrders = OrderProcessors.Where(x => x.orderStatus != OrderStatus.Complete);
        //        if (pendingCompleteOrders.Any())
        //        {
        //            int count = OrderProcessors.Count();
        //            int randomval = rnd.Next(0, count);

        //            var op = OrderProcessors[randomval];
        //            if (op.orderStatus == OrderStatus.Pending || op.orderStatus == OrderStatus.Processed)
        //            {
        //                if (op.orderStatus == OrderStatus.Pending)
        //                {
        //                    op.orderStatus = OrderStatus.Processed;
        //                }
        //                else if (op.orderStatus == OrderStatus.Processed)
        //                {
        //                    op.orderStatus = OrderStatus.Complete;                       
        //                }
        //                Console.WriteLine($"Order {op.OrderNo} {op.createOrderRequest.ItemName} State change to {op.orderStatus.ToString()}");
        //                if(op.orderStatus == OrderStatus.Complete) ProcessOrder(op.OrderNo);
        //            }
        //        }
        //        System.Threading.Thread.Sleep(5000);
        //    }
        //}

        //public static void ProcessOrder(string orderid)
        //{
        //    var completeOrder = OrderProcessors.FirstOrDefault(x => x.OrderNo == orderid);
        //    completeOrder.manualResetEvent.Set();
        //    completeOrder.orderStatus = OrderStatus.ToBeRemove;
        //}

        public static void Initialize()
        {
            LogWriter.WriteTraceLog("Before session initialize");
            _orderSession = new OrderSession();
            var location = System.Reflection.Assembly.GetEntryAssembly().Location;
            SessionSettings settings = new SessionSettings(Path.GetDirectoryName(location) + @"\OrderConfig.cfg");
            IMessageStoreFactory storeFactory = new QuickFix.FileStoreFactory(settings);
            ILogFactory logFactory = new QuickFix.FileLogFactory(settings);

            initiator = new QuickFix.Transport.SocketInitiator(_orderSession, storeFactory, settings);
            initiator.Start();
            //Give a few seconds to initate session
            System.Threading.Thread.Sleep(2000);
            LogWriter.WriteTraceLog("After session initialize");

        }

        public static void Uninitialize()
        {
            if(!initiator.IsStopped) initiator.Stop();
        }

        public static void SendOrder(CreateOrderRequest orderRequest)
        {



            //NewOrderSingle order = new NewOrderSingle(new ClOrdID("12345"), new HandlInst(HandlInst.AUTOMATED_EXECUTION_ORDER_PRIVATE), new Symbol("XRP/USD"), new Side(Side.BUY), new TransactTime(DateTime.UtcNow), new OrdType(OrdType.MARKET));
            //order.Set(new OrderQty(1));
            //order.Set(new Currency("XRP"));
            //order.Set(new ExecInst("B"));
            //order.Set(new TimeInForce('0'));
            ////order.Set(new MaxShow(10));
            ////order.Set(new MinQty(1));
            //order.Set(new SecurityType("FOR"));
            //order.Set(new Product(4));
            //order.Set(new Price((decimal)1.2));
            //Console.WriteLine("Sending Order to Server");
            //bool flag = Session.SendToTarget(order, sessionID);


            NewOrderSingle newOrder = new NewOrderSingle();
            newOrder.Set(new ClOrdID(orderRequest.ClOrdID));
            newOrder.Set(new HandlInst(orderRequest.HandlInst));
            newOrder.Set(new Symbol(orderRequest.Symbol));
            newOrder.Set(new Side(orderRequest.Side));
            newOrder.Set(new TransactTime(DateTime.UtcNow));
            newOrder.Set(new OrdType(orderRequest.OrdType));
            if (orderRequest.OrdType == OrdType.LIMIT)
                newOrder.Set(new Price(orderRequest.Price.GetValueOrDefault(0)));
            newOrder.Set(new OrderQty(orderRequest.OrderQty));
            newOrder.Set(new Currency(orderRequest.Currency));
            newOrder.Set(new ExecInst(orderRequest.ExecInst));
            newOrder.Set(new TimeInForce(orderRequest.TimeInForce));
            if (orderRequest.TimeInForce == TimeInForce.GOOD_TILL_DATE)
                newOrder.Set(new ExpireTime(DateTime.UtcNow));
            if (orderRequest.MaxShow != null)
                newOrder.Set(new MaxShow(orderRequest.MaxShow.GetValueOrDefault(10)));
            if (orderRequest.MinQty != null)
                newOrder.Set(new MinQty(orderRequest.MinQty.GetValueOrDefault(1)));
            newOrder.Set(new SecurityType(orderRequest.SecurityType));
            newOrder.Set(new Product(orderRequest.Product));
            _orderSession.SendMessage(newOrder);
            LogWriter.WriteTraceLog($"Sending Order : {orderRequest.ClOrdID} to Server");
        }

    }

    //public enum OrderStatus
    //{
    //    Pending = 1,
    //    Processed = 2,
    //    Cancel = 3,
    //    Complete = 4,
    //    ToBeRemove = 5
    //}
}
