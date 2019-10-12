using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FixOrderConsole.ViewModel;
using QuickFix.FIX43;

namespace FixOrderConsole
{
    public class OrderProcessor
    {
        private ManualResetEvent manualResetEvent = new ManualResetEvent(false);
        public string OrderNo;
        public Enums.OrderStatus orderStatus;
        public CreateOrderRequest createOrderRequest;
        public CreateOrderResponse createOrderResponse;
        public ExecutionReport executionReport;




        public void WaitForOrderExecution()
        {
            
                //code for sending message
                Console.WriteLine($"enter into wait one of Order {OrderNo}");
                manualResetEvent.WaitOne(30000);
                Console.WriteLine($"exit from wait one of Order {OrderNo}");
        }

        public void SetForOrderExecution()
        {

            //code for sending message
            Console.WriteLine($"enter into set one of Order {OrderNo}");
            manualResetEvent.Set();
            Console.WriteLine($"exit from set one of Order {OrderNo}");
        }

    }
}
