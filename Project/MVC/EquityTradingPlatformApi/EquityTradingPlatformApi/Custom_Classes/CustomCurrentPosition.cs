using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EquityTradingPlatformApi.Custom_Classes
{
    public class CustomCurrentPosition
    {
        public int StockId { get; set; }
        public string Trader_Name { get; set; }
        public string Stock_Name { get; set; }
        public string Symbol { get; set; }
        public double Quantity { get; set; }
        public double Buying_Price { get; set; }
        public double Current_Price { get; set; }
        public double Total_Value { get; set; }
        public string Date { get; set; }
    }
}