using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MarketConsolePOC.ViewModel;

namespace MarketConsolePOC
{
    public class OrderProcessor
    {
        public ManualResetEvent manualResetEvent = new ManualResetEvent(false);
        public string OrderNo;
        public Enums.OrderStatus orderStatus;
        public CreateMarketOrderRequest CreateMarketrequest;
        public CreateMarketOrderResponse CreateMarketresponse;



        public void ExecuteOrder()
        {
            //code for sending message
            Console.WriteLine($"enter into wait one of Order {OrderNo}");
            manualResetEvent.WaitOne();
            Console.WriteLine($"exit from wait one of Order {OrderNo}");
        }
    }
}
