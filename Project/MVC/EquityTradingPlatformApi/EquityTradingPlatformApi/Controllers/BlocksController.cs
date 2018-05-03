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
using EquityTradingPlatformApi.Layers;

namespace EquityTradingPlatformApi.Controllers
{
    public class BlocksController : ApiController
    {
        private ProjectContext db = new ProjectContext();
        private CustomBlockCreation CustomBlocks = new CustomBlockCreation();
        private BlockLayer blockLayer = new BlockLayer();
        // GET: api/Blocks
        public IHttpActionResult GetBlocks()
        {

            return Ok(CustomBlocks.CreateList( db.Blocks.ToList())); 
        }

        // GET 
        [Route("api/Trader/ExecuteBlock")]
        public IHttpActionResult GetBlockExecution(int blockId)
        {
            bool result = false;

            Exchange.Exchange exchange = new Exchange.Exchange(blockId);
            result = exchange.FillBlock();

            return Ok(result);
        }


        
        // TYPE OF BLOCKS FOR UserId and BlockStatus
        [Route("api/Trader/Block")]
        public IHttpActionResult GetTraderPendingBlocks(int userId, string blockStatus)
        {
            
            List<CustomBlockModel> blockList = blockLayer.GetBlocksWithStatus(userId, blockStatus);
            return Ok(blockList);
        }


        [Route("api/Trader/AddToBlock")]
        public IHttpActionResult AddtoBlocks(int orderId, int blockId)
        {
            if (blockLayer.AddNewOrderToBlock(orderId, blockId))
                return Ok(true);
            else
                return BadRequest();
            
        }



        // ADD NEW BLOCK FROM ORDER ID
        [Route("api/Trader/NewBlock")]
        public IHttpActionResult PostNewBlock(int orderId)
        {
            if (blockLayer.AddNewBlock(orderId))
                return Ok(true);
            else
                return Ok(false);
          
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BlockExists(int id)
        {
            return db.Blocks.Count(e => e.Id == id) > 0;
        }
    }
}