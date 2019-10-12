using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixOrderConsole
{
    public class Enums
    {
        public enum OrderStatus
        {
            NEW = 0,
            PARTIALLY_FILLED = 1,
            FILLED = 2,
            DONE_FOR_DAY = 3,
            CANCELED = 4,
            REPLACED = 5,
            PENDING_CANCELREPLACE = 6,
            STOPPED = 7,
            REJECTED = 8,
            SUSPENDED = 9,
            PENDING = 98
        }
    }
}

