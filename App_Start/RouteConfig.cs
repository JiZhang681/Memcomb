﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Memcomb
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "LoginPage", action = "Index", id = UrlParameter.Optional }
			);
			routes.MapRoute(
				name: "Profile",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Profile", action = "Index", id = UrlParameter.Optional}
			);
			routes.MapRoute(
				name: "Followings",
				url: "{controller}/{action}/{email}/",
				defaults: new { controller = "Followings", action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}
