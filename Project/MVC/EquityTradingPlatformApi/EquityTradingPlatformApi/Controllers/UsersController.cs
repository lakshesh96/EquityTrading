using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using EquityTradingPlatformApi.Models;
using Newtonsoft.Json.Linq;
using EquityTradingPlatformApi.Layers;
using EquityTradingPlatformApi.Custom_Classes;

namespace EquityTradingPlatformApi.Controllers
{
    public class UsersController : ApiController
    {
        private ProjectContext db = new ProjectContext();
        private UserLayer userLayer = new UserLayer();
        // GET APPROVED TRADERS
        // GET: api/Trader/Approved
        [HttpGet]
        [Route("api/Trader/Approved")]
        [ResponseType(typeof(User))]
        public IHttpActionResult GetApprovedTraders()
        {
            return Ok(userLayer.GetApprovedUsers(UserType.Trader));
        }


        // GET UNAPPROVED TRADERS
        // GET: api/Trader/Unapproved
        [HttpGet]
        [Route("api/Trader/Unapproved")]
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUnapprovedTraders()
        {
            return Ok(userLayer.GetUnapprovedUsers(UserType.Trader));
        }


        // GET APPROVED PM
        // GET: api/PM/Approved
        [HttpGet]
        [Route("api/PM/Approved")]
        [ResponseType(typeof(User))]
        public IHttpActionResult GetApprovedPM()
        {
            return Ok(userLayer.GetApprovedUsers(UserType.PortfolioManager));
        }


        // GET UNAPPROVED PM
        // GET: api/PM/Unapproved
        [HttpGet]
        [Route("api/PM/Unapproved")]
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUnapprovedPM()
        {
            return Ok(userLayer.GetUnapprovedUsers(UserType.PortfolioManager));
        }


        // LOGIN FOR TRADERS AND PM
        // Post: api/Users/Login
        [Route("api/Users/Login")]
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult PostLogin(LoginUser user)
        {
            return Ok(userLayer.Login(user));
        }



        // BATCH ADD USERS
        // Post: api/Users/PutList
        [Route("api/Users/PutList")]
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult PostUserList(List<User> mydata)
        {
            try
            {
                foreach (var item in mydata)
                {
                    db.Users.Add(item);
                }
                db.SaveChanges();
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }



        // USER REGISTRATION 
        // POST: api/Users
        [ResponseType(typeof(User))]
        public IHttpActionResult PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // By default user should'nt be approved
            user.Approved = false;

            foreach(User u in db.Users)
            {
                if (u.UserName == user.UserName)
                    return Ok("UserName already exists");

                if (u.EmployeeId == user.EmployeeId)
                    return Ok("EmployeeID already exists.");
            }

            try
            {
                db.Users.Add(user);
                db.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                // If employee doesn't Exist.
                return Ok("Error. EmployeeId doesn't exist. " + e.Message);
            }
   
            //return CreatedAtRoute("DefaultApi", new { id = user.Id }, user);
            return Ok(user.Id);
        }




        // APPROVE USERS (TOGGLE)
        // POST: api/Users/Approve
        [HttpPost]
        [Route("api/Users/Approve")]
        [ResponseType(typeof(bool))]
        public IHttpActionResult ApproveUser(int id)
        {
            bool result = false;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                foreach(User u in db.Users)
                {
                    if (u.Id == id)
                    {
                        u.Approved = !u.Approved;
                        result = true;
                    }
                }
                db.SaveChanges();
                return Ok(result);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Ok(false);
            }
        }




        // Extra Functions
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.Id == id) > 0;
        }
    }
}