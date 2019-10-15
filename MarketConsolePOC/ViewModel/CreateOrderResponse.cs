using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixOrderConsole.ViewModel
{
    public class CreateOrderResponse
    {
        //public int RequestID { get; set; }
        //public int ItemId { get; set; }

        //public bool StatusFlag { get; set; } 
        public string ExecutionReport { get; set; }

        public string OrderID { get; set; }
        public string ClOrdID { get; set; }
        public string ExecID { get; set; }
        public string ExecInst { get; set; }


        //41 OrigClOrd C —  String(11)
        //Conditionally required for response to a Cancel or
        //ID Cancel/Replace request.ClOrdID(#11) of the previously      
        //accepted order when canceling or replacing an order. This
        //order ID is invalid for further modification operations.
        public string OrigClOrd { get; set; }

        // 150 ExecType                 Y           char(1)
        //Describes the purpose of the execution report. 
        //0=New  5=Replaced   E = Pending Replace           
        public char ExecType { get; set; }

        //39    OrdStatus       Y       char (1) 
        //the order status as accepted or replaced 
        //0=New Identifies 1=Partially Filled   2=Fully Filled   4=Canceled    C = Expired      8=Rejected           
        public char OrdStatus { get; set; }

        //55 Symbol     Y    String(7)         
        //Currency pair for the order 
        public string Symbol { get; set; }

        // 54 Side           char(1)    Y 
        //The customer buys Currency(#15). The customer sells Currency (#15).
        //1=Buy 1 (Buy):  2=Sell 2 (Sell): .          
        public char Side { get; set; }



        //38 OrderQty       Y           Qty(20)         
        // Amount to be traded on the order 
        public decimal OrderQty { get; set; }

        //40    OrdType     Y   char(1) 
        //Type of order
        //1=At Market  2=Limit          
        public char OrdType { get; set; }

        //44 Price N — Price(20)
        //Specifies the limit price for limit orders (OrdType (#40)=2). 
        public decimal Price { get; set; }

        //15 Currency Y(API)   Currency(3) 
        //Identifies currency used for amount 
        public string Currency { get; set; }

        //167 SecurityType Y(API)   String(4)
        //Indicates type of security
        //FOR= Foreign Contract 
        public string SecurityType { get; set; }

        //59 TimeInFor Y  
        //Specifies how long the order remains in effect char (1) 
        //0=DAY  1=Good Till Cancel(GTC) 3=Immediate Or  Cancel(IOC) 4=Fill Or Kill(FOK) 6=Good Till Date(GTD)
        public char TimeInForce { get; set; }

        //31 LastPx C 0 (zero)    Price(20) 
        //Price for the fill.Populated if ExecType (#150)=F (Trade), 0 (zero) otherwise.     
        public decimal LastPx { get; set; }

        // 32 LastQty C
        //Quantity for the fill. Populated if ExecType (#150)=F 

        public decimal LastQty { get; set; }
        public decimal LeavesQty { get; set; }

        public decimal CumQty { get; set; }

        public decimal AvgPx { get; set; }

        public DateTime TransactTime {get;set;}

        //Minimum fill quantity for an order
        public decimal MinQty { get; set; }

        //Time/date of order expiration. Required if TimeInForce 
        public DateTime ExpireTime { get; set; }

        //Maximum quantity of the order shown to other customers
        public decimal MaxShow { get; set; }

        //Identifies the type of the instrument
        public int Product { get; set; }





    }
}
