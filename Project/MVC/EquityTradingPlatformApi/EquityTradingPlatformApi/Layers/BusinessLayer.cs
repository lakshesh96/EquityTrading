using EquityTradingPlatformApi.Custom_Classes;
using EquityTradingPlatformApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace EquityTradingPlatformApi.Layers
{
    public class BusinessLayer
    {
        ProjectContext db;
        public BusinessLayer()
        {
            db = new ProjectContext();
        }
        public List<CustomBlockModel> GetBlocksWithStatus(int userId,string blockStatus)
        {
            CustomBlockCreation CustomBlocks = new CustomBlockCreation();
            if (blockStatus.Equals("PendingAndPartial"))
            {
                var blocks = from n in db.Blocks
                             join m in db.Orders on n.Id equals m.BlockId
                             where (n.BlockStatus == BlockStatus.Partial
                             ||
                             n.BlockStatus == BlockStatus.Pending)
                             &
                             m.UserId == userId
                             select n;
                return CustomBlocks.CreateList(blocks.ToList());
            }
            var userblocks = from n in db.Blocks
                             join m in db.Orders on n.Id equals m.BlockId
                             where n.BlockStatus.ToString() == blockStatus & m.UserId == userId
                             select n;
            return CustomBlocks.CreateList(userblocks.ToList());
        }

        public Boolean AddNewOrderToBlock(int orderId,int blockId)
        {
            var order = db.Orders.Find(orderId);
            var block = db.Blocks.Find(blockId);
            if (order.OrderSide == block.Side && order.OrderType == block.Type && order.StocksId == block.StocksId)
            {
                order.BlockId = blockId;
                try
                {
                    db.SaveChanges();
                    return (true);
                }
                catch (Exception)
                {
                    return (false);
                }
            }
            else
                return false;
        }  

        public bool AddNewBlock(int orderId)
        {
            Order order = db.Orders.Find(orderId);
            Block block = new Block
            {
                BlockStatus = BlockStatus.Pending,
                Side = order.OrderSide,
                UserId = order.UserId,
                StocksId = order.StocksId
            };
            try
            {
                db.Blocks.Add(block);
                db.SaveChanges();
                order.BlockId = block.Id;
                db.SaveChanges();
                return true;
            }
            catch (DbUpdateException)
            {
                return (false);
            }
            catch(Exception)
            {
                return false;
            }
                
        }
    }
}