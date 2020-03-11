using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Memcomb.Controllers
{
    public class HomePageController : Controller
    {
        // GET: HomePage
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateMemory()
        {
            var co_val = Response.Cookies["email"].Value;
            Console.WriteLine(co_val);
            return View();
        }
    }
}