
using QuickFix;
using QuickFix.FIX43;
using QuickFix.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading;
//using Common;

namespace FixShare
{
    public class OrderDispatcher
    {
        public OrderSession _orderSession;
        QuickFix.Transport.SocketInitiator initiator;


        public OrderDispatcher()
        {
        }
        public void Initialize()
        {
            _orderSession = new OrderSession();
            var location = System.Reflection.Assembly.GetEntryAssembly().Location;
            SessionSettings settings = new SessionSettings(Path.GetDirectoryName(location) + @"\OrderConfig.cfg");
            IMessageStoreFactory storeFactory = new QuickFix.FileStoreFactory(settings);
            ILogFactory logFactory = new QuickFix.FileLogFactory(settings);            

            initiator = new QuickFix.Transport.SocketInitiator(_orderSession, storeFactory, settings);
            initiator.Start();
            //Give a few seconds to initate session
            System.Threading.Thread.Sleep(2000);
            //if (fixOrderSession._session.IsLoggedOn == false)
            //    Console.WriteLine("Order Session Could not be established");

          //  HashSet<SessionID> list = initiator.GetSessionIDs();

           // SessionID sessionID = (SessionID)list.FirstOrDefault(); // .[0];


            //    ClOrdID="12345"
            //HandlInst=1
            //Symbol="XRP/USD"
            //Side="1"
            //* TransactTime = "UTC Time"(You can put utc time in .NET)
            //OrderQty=1
            //OrdType="1"
            //|Price="1.2" (if OrdType="2")
            //Currency="USD"
            //ExecInst="A"
            //TimeInForce="0"
            //|ExpireTime="UTC Time" (if TimeInForce="6")
            //MaxShow="10"
            //MinQty="1"
            //SecurityType="FOR"
            //Product="4"




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
           // bool flag = Session.SendToTarget(order, _orderSession._session);

          



            //  Console.ReadKey();


        }
        public void SendOrder(CreateNewOrderRequest orderRequest)
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
            if(orderRequest.MaxShow != null) 
                newOrder.Set(new MaxShow(orderRequest.MaxShow.GetValueOrDefault(10)));
            if (orderRequest.MinQty != null)
                newOrder.Set(new MinQty(orderRequest.MinQty.GetValueOrDefault(1)));
            newOrder.Set(new SecurityType(orderRequest.SecurityType));
            newOrder.Set(new Product(orderRequest.Product));          
            _orderSession.SendMessage(newOrder);
            Console.WriteLine("Sending Order to Server");   
            



        }

        public void Run()
        {

        }

        //static long childOrderID = 1001;
        //public static string GetNextChildOrderID()
        //{
        //    Interlocked.Increment(ref childOrderID);
        //    return childOrderID.ToString();
        //}
        //public MarketData GetOrder(string symbol)
        //{
        //    //MarketData md = new MarketData();
        //    //md.Bid = fixOrderSession.priceData.Bid;
        //    //md.Ask = fixOrderSession.priceData.Ask;
        //    //md.Symbol = fixOrderSession.priceData.Symbol;
        //    //Helper.Log(md.Bid + " / " + md.Ask);
        //    //return md;
        //    if (_orderSession.priceData.ContainsKey(symbol))
        //        return _orderSession.priceData[symbol];

        //    return new MarketData();
        //}
        //private void SendOrderMessage(QuickFix.Message m)
        //{
        //    if (_orderSession._session != null)
        //        _orderSession._session.Send(m);
        //    else
        //    {
        //        // This probably won't ever happen.
        //        Console.WriteLine("Can't send message: session not created.");
        //    }
        //}
    }


    

}
