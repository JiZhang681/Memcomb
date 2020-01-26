﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Memcomb.Controllers
{
	public class HomeController : Controller
	{

		public ActionResult Submit()
		{

			return View();
		}
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
		public new ActionResult Profile() {
			ViewBag.Message = "Profile page.";
			return View();
		}
	}
}