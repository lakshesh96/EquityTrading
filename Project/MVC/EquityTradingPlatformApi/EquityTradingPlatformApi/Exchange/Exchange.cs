using EquityTradingPlatformApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EquityTradingPlatformApi.Exchange
{
    public class Exchange
    {
        Block block;
        ProjectContext db;
        List<Order> orders;
        Stocks stock;
        Side side;
        int totalQuantity=0;
        int FillQuantity = 0;
        int VolumeAvailable = 0;
        int VolumeExecuted = 0;

        public Exchange(int id)                         //constructer to accept 
        {
            this.db = new ProjectContext();
            this.block = db.Blocks.Find(id);
            this.stock = this.db.Stocks.Find(this.block.StocksId);
            this.side = this.block.Side;
            this.VolumeAvailable = this.stock.VolumeAvailable;
        }
        public bool FillBlock()                         //FillBlock
        {
                Random random = new Random();
                GetTotal();                     //Get total quantity for all orders in block 
                
                if (this.totalQuantity == 0)    //check for no orders
                    return false;

                if (!CheckSellValid())          //Check for Sell Quantity Validity AVailable in Holding
                    return false;

                this.FillQuantity = random.Next(2 * this.totalQuantity);      //Random filling within 2*totalquantity
                if ((this.FillQuantity >= this.totalQuantity&&this.VolumeAvailable >=this.totalQuantity&&this.side==Side.Buy)||(this.side==Side.Sell&&this.FillQuantity>=this.totalQuantity) ) 
                {                                                           //if Fully Filled (for both buy and sell)
                    bool FullExecutionFlag = true;
                    foreach (var item in this.orders)
                    {
                        if (item.OrderType == OrderType.Market || CheckStopLimitValid(item) || CheckStopValid(item) || CheckLimitValid(item))
                            AddOrderFull(item);
                        else
                            FullExecutionFlag = false;
                    }
                    if(this.side==Side.Sell)
                    {
                        ExecuteForSell();
                    }
                    if (FullExecutionFlag)
                        this.block.BlockStatus = BlockStatus.Executed;
                    else
                        this.block.BlockStatus = BlockStatus.Partial;
                    db.SaveChanges();
                }
                else
                {
                    this.block.BlockStatus = BlockStatus.Partial;
                    if (this.VolumeAvailable < this.totalQuantity && this.side==Side.Buy)
                    {//if volume available for stock is less than volume asked only for buy side
                        this.FillQuantity = this.VolumeAvailable;
                    }
                //this.VolumeExecuted = this.FillQuantity;
                    foreach (var item in this.orders)
                    {
                        
                        if (FillQuantity < item.Quantity && FillQuantity != 0)
                        {//if cannot fill fully
                            if (item.OrderType == OrderType.Market || CheckStopValid(item) || CheckLimitValid(item) || CheckStopLimitValid(item))
                            {//checking for all type of order validities for both sides seperately in functions below
                                AddOrderPartial(item);
                                break;
                            }
                        }
                        else if (FillQuantity >= item.Quantity)
                        {//if can fill fully
                            if (item.OrderType == OrderType.Market || CheckStopLimitValid(item) || CheckStopValid(item) || CheckLimitValid(item))
                            {
                                this.FillQuantity -= item.Quantity;
                                AddOrderFull(item);
                                
                            }
                        }
                        else if (FillQuantity == 0)
                        {
                            break;
                        }
                    }
                    if(this.side==Side.Sell)
                        ExecuteForSell();
                    db.SaveChanges();
                }
                ChangeMarketPrice();//fluctuate market price
                this.stock.VolumeAvailable = this.VolumeAvailable;
                db.SaveChanges();
                return true;
        }
        public void ChangeMarketPrice()
        {
            double priceVariation = ((double)VolumeExecuted / ((double)this.stock.VolumeAvailable)) * 100;
            if (this.side == Side.Buy)
            {
                this.stock.CurrentPrice += priceVariation;
            }
            else if(this.side == Side.Sell)
            {
                this.stock.CurrentPrice -= priceVariation;
            }
        }
        public bool CheckLimitValid(Order item)
        {
            if (item.OrderType == OrderType.Limit)
            {
                if ((this.stock.CurrentPrice <= item.LimitPrice&&this.side==Side.Buy)||(this.stock.CurrentPrice>=item.LimitPrice&&this.side==Side.Sell))
                {//checking for limit price for buy side or sell side seperately
                    return true;
                }
            }
            return false;

        }
        public bool CheckStopValid(Order item)
        {
            if (item.OrderType == OrderType.Stop)
            {
                if ((this.stock.CurrentPrice >= item.StopPrice&&this.side==Side.Buy)||(this.side==Side.Sell&&this.stock.CurrentPrice <=item.StopPrice))
                {
                    return true;
                }
            }
            return false;
        }
        public bool CheckStopLimitValid(Order item)
        {
            if (item.OrderType == OrderType.StopLimit)
            {
                if ((this.stock.CurrentPrice >= item.StopPrice && this.stock.CurrentPrice <= item.LimitPrice&&this.side==Side.Buy)||(this.side==Side.Sell&& this.stock.CurrentPrice <= item.StopPrice && this.stock.CurrentPrice >= item.LimitPrice))
                {
                    return true;
                }
            }
            return false;
        }
        public void GetTotal()
        {//return total quantity for orders
            if (this.block == null)
                this.totalQuantity = 0;
            else
            {
                this.orders = (from n in db.Orders where n.BlockId == this.block.Id orderby n.DateAdded select n).ToList();
                foreach (var item in this.orders)
                {
                    this.totalQuantity += item.Quantity;
                }
            }
        }
        public void AddOrderFull(Order item)
        {
            item.BlockId = null;
            this.VolumeExecuted += item.Quantity;
            item.OrderStatus = OrderStatus.Executed;
            if (this.side == Side.Buy)
                ExecuteForBuyFull(item);
            else
                this.VolumeAvailable += item.Quantity;                
            item.Quantity = 0;
        }
        public void ExecuteForBuyFull(Order item)
        {
            CurrentPosition currentPositionobject = new CurrentPosition();
            currentPositionobject.Date = System.DateTime.Now;
            currentPositionobject.PriceExecuted = this.stock.CurrentPrice;
            currentPositionobject.OrderId = item.Id;
            currentPositionobject.VolumeExecuted = item.Quantity;
            db.CurrentPositions.Add(currentPositionobject);
            this.VolumeAvailable -= item.Quantity;
        }
        public void ExecuteForSell()
        {
            List<CurrentPosition> currentPositions = (from n in db.CurrentPositions
                                                       join m in db.Orders on
                                                       n.OrderId equals m.Id
                                                       where (m.StocksId == this.stock.Id
                                                       & m.UserId == this.block.UserId)
                                                       orderby n.Date ascending
                                                       select n).ToList();

            int ExecutedItemQuantity = this.VolumeExecuted;
            double TotalBuyPrice = 0;
            foreach (var currentHolding in currentPositions)
            {
                if (currentHolding.VolumeExecuted <= ExecutedItemQuantity)
                {
                    TotalBuyPrice += (currentHolding.PriceExecuted * currentHolding.VolumeExecuted);
                    ExecutedItemQuantity -= currentHolding.VolumeExecuted;
                    db.CurrentPositions.Remove(currentHolding);
                }
                else
                {
                    TotalBuyPrice += (currentHolding.PriceExecuted * ExecutedItemQuantity);
                    currentHolding.VolumeExecuted -= ExecutedItemQuantity;
                    ExecutedItemQuantity = 0;
                }
            }
            TransactionHistory history = new TransactionHistory();
            history.BuyPrice = TotalBuyPrice / VolumeExecuted;
            history.Quantity = VolumeExecuted;
            history.SellPrice = this.stock.CurrentPrice;
            history.StockId = this.stock.Id;
            history.UserId = this.block.UserId;
            db.TransactionHistory.Add(history);
        }
        public void AddOrderPartial(Order item)
        {
            item.OrderStatus = OrderStatus.Partial;
            this.block.BlockStatus = BlockStatus.Partial;
            this.VolumeExecuted += this.FillQuantity;
            if (this.side == Side.Buy)
                ExecuteForBuyPartial(item);
            else
                this.VolumeAvailable += this.FillQuantity;
            this.FillQuantity = 0;
        }
        public void ExecuteForBuyPartial(Order item)
        {
            CurrentPosition currentPositionobject = new CurrentPosition();
            currentPositionobject.Date = System.DateTime.Now;
            currentPositionobject.PriceExecuted = this.stock.CurrentPrice;
            currentPositionobject.OrderId = item.Id;
            currentPositionobject.VolumeExecuted = this.FillQuantity;
            item.Quantity -= this.FillQuantity;
            this.VolumeAvailable -= this.FillQuantity;
            db.CurrentPositions.Add(currentPositionobject);
        }
    public Boolean CheckSellValid()
        {
            if (this.side == Side.Buy)
                return true;
            var stockQuantity = from n in db.CurrentPositions
                                join m in db.Orders
                                on n.OrderId equals m.Id
                                where (m.StocksId == this.stock.Id
                                & m.UserId == this.block.UserId)
                                select (n.VolumeExecuted);
            int TotalQuantityInHolding = 0;
            foreach(var item in stockQuantity)
            {
                TotalQuantityInHolding += item;
            }
            if (TotalQuantityInHolding < this.totalQuantity)
                return false;
            else
                return true;
        }
    }
}