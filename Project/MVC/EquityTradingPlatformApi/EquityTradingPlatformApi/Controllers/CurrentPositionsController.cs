using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using EquityTradingPlatformApi.Models;
using EquityTradingPlatformApi.Custom_Classes;

namespace EquityTradingPlatformApi.Controllers
{
    public class CurrentPositionsController : ApiController
    {
        private ProjectContext db = new ProjectContext();

        // GET CURRENT POSITION FOR USER ?userId
        [Route("api/Position/Approved")]
        [HttpGet]
        public IHttpActionResult GetCurrentPositionForUser(int userId)
        {
            var getUserOrders = from order in db.Orders
                                where (order.UserId == userId && (order.OrderStatus == OrderStatus.Executed || order.OrderStatus == OrderStatus.Partial))
                                select order;

            if (getUserOrders == null)
                return Ok(false);
            else
            { 
                List<Order> currentUserOrderIds = getUserOrders.ToList();

                List<CustomCurrentPosition> returnPositions = new List<CustomCurrentPosition>();
                List<CurrentPosition> currentPositions = db.CurrentPositions.ToList();
                foreach (Order o in currentUserOrderIds)
                {
                    foreach(CurrentPosition cp in currentPositions)
                                            {
                        if (o.Id == cp.OrderId)
                        {
                            CustomCurrentPosition currentPos = new CustomCurrentPosition();

                            Stocks s = db.Stocks.Find(o.StocksId);
                            User u = db.Users.Find(o.UserId);

                            currentPos.Trader_Name = u.Name;
                            currentPos.Stock_Name = s.Name;
                            currentPos.Symbol = s.Symbol;
                            currentPos.Buying_Price = cp.PriceExecuted;
                            currentPos.Quantity = cp.VolumeExecuted;
                            currentPos.StockId = s.Id;
                            currentPos.Date = cp.Date.ToShortDateString();
                            currentPos.Current_Price = s.CurrentPrice;
                            currentPos.Total_Value = currentPos.Quantity * currentPos.Current_Price;
                            returnPositions.Add(currentPos);
                        }
                    }
                }
                return Ok(returnPositions);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CurrentPositionExists(int id)
        {
            return db.CurrentPositions.Count(e => e.Id == id) > 0;
        }
    }
}