using EquityTradingPlatformApi.Custom_Classes;
using EquityTradingPlatformApi.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EquityTradingPlatformApi.Layers
{
    public class UserLayer
    {
        ProjectContext db;
        public UserLayer()
        {
            this.db = new ProjectContext();
        }
        public List<User> GetApprovedUsers(UserType type)
        {
            var approvedUsers = from user in db.Users
                                  where user.Approved == true && (user.Type == type || user.Type == UserType.Both)
                                  select user;
            return (approvedUsers.ToList());
        }

        public List<User> GetUnapprovedUsers(UserType type)
        {
            var unapprovedUsers = from user in db.Users
                                  where user.Approved == false && (user.Type == type || user.Type == UserType.Both)
                                  select user;
            return (unapprovedUsers.ToList());
        }
        public JObject Login(LoginUser user)
        {
            var result = JObject.Parse(@"{}");
            result["response"] = false;

            foreach (User u in db.Users)
            {
                if (u.UserName == user.UserName && u.Password == user.Password)
                {
                    if (u.Type == user.Type)
                    {
                        result["response"] = true;
                        result["id"] = u.Id;
                        result["type"] = u.Type.ToString();
                        result["error"] = "";
                    }
                    else
                    {
                        result["response"] = false;
                        result["type"] = "";
                        result["error"] = "Incorrect UserType";
                    }
                    return (result);
                }
            }
            result["error"] = "Incorrect UserName or Password";
            return (result);
        }
    }
   
}