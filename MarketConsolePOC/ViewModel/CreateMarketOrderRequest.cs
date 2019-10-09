using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketConsolePOC.ViewModel
{
    public class CreateMarketOrderRequest
    {
        //public int RequestID { get; set; }
        //public int ItemId { get; set; }
        //public string ItemName { get; set; }
        //public int ItemPrice { get; set; }
        public string ClOrdID { get; set; }
        public char HandlInst { get; set; }
        public string Symbol { get; set; }
        public char Side { get; set; }
        public decimal OrderQty { get; set; }
        public char OrdType { get; set; }
        public decimal? Price { get; set; }
        public string Currency { get; set; }
        public string ExecInst { get; set; }
        public char TimeInForce { get; set; }

        public decimal? MaxShow { get; set; }
        public decimal? MinQty { get; set; }
        public string SecurityType { get; set; }
        public byte Product { get; set; }
    }
}
