using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ADAdmin.Controllers
{
    public class HomeController : Controller
    {
        [Route("Home/Test")]
        public string Test()
        {
            return "Success";
        }
    }
}