using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using EquityTradingPlatformApi.Models;
using Newtonsoft.Json.Linq;

namespace EquityTradingPlatformApi.Controllers
{
    public class AdminController : ApiController
    {
        private ProjectContext db = new ProjectContext();

        // POST: api/Admin
        [ResponseType(typeof(Admin))]
        public IHttpActionResult PostAdmin(Admin admin)
        {
            var result = JObject.Parse(@"{}");
            result["response"] = false;
            if (admin.UserName == db.Admins.First().UserName && admin.Password == db.Admins.First().Password)
                result["response"] = true;
            return Ok(result);
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