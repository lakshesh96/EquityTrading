using EquityTradingPlatformApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EquityTradingPlatformApi.Custom_Classes
{
    public class CustomBlockCreation
    {
        public List<CustomBlockModel> CustomBLockList;
        public ProjectContext DB;
        public CustomBlockCreation()
        {
            DB = new ProjectContext();
            CustomBLockList = new List<CustomBlockModel>();
        }
        public List<CustomBlockModel> CreateList(List<Block> BlockList)
        {
            foreach(var item in BlockList)
            {
                CustomBlockModel block = new CustomBlockModel();
                block.BlockId = item.Id;
                block.BlockSide = item.Side.ToString();
                block.BlockStatus = item.BlockStatus.ToString();
                block.OrderType = item.Type.ToString();
                var quantity = (from n in DB.Orders where n.BlockId == item.Id select n.Quantity).Sum();
                block.Quantity = quantity;
                block.StockName = DB.Stocks.Find(item.StocksId).Name;
                CustomBLockList.Add(block);
            }
            return this.CustomBLockList;
        }
    }
}