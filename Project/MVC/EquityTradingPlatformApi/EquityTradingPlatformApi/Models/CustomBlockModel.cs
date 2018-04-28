using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EquityTradingPlatformApi.Models
{
    public class CustomBlockModel
    {
        public string BlockStatus { get; set; }
        public int BlockId { get; set; }
        public string BlockSide { get; set; }
        public string StockName { get; set; }
        public string OrderType { get; set; }
        public int Quantity { get; set; }

    }
}