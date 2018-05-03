using EquityTradingPlatformApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EquityTradingPlatformApi.Custom_Classes
{
    public class LoginUser
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public UserType Type { get; set; }
    }
}