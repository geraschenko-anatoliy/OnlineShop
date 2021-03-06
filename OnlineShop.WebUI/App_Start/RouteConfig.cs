﻿using OnlineShop.WebUI.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OnlineShop.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            ArrayAwareRoute route = new ArrayAwareRoute("{controller}/{action}/{id}", new MvcRouteHandler());
            object defaults = new
            {
                controller = "Product",
                action = "Index",
                id = UrlParameter.Optional
            };
            route.Defaults = new RouteValueDictionary(defaults);

            routes.Add("Default", route);
        }
    }
}