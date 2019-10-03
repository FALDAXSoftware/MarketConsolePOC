using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketConsolePOC.ViewModel
{
    public class CreateMarketOrderResponse
    {
        public int RequestID { get; set; }
        public int ItemId { get; set; }
        
        public bool StatusFlag { get; set; } 
    }
}
