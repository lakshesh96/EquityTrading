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

namespace EquityTradingPlatformApi.Controllers
{
    public class TransactionHistoriesController : ApiController
    {
        private ProjectContext db = new ProjectContext();

        // GET: api/TransactionHistories
        [Route("api/TransactionHistory/{userId}")]
        public IQueryable<TransactionHistory> GetTransactionHistory(int userId)
        {
            var histories = from n in db.TransactionHistory where n.UserId == userId select n;
            return histories;
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}