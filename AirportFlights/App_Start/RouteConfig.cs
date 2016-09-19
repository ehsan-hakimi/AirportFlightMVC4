using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AirportFlights
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Flight", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "FlightEdit",
                url: "{controller}/{action}/{gate}/{flight}",
                defaults: new { controller = "Flight", action = "EditFlight", gate = "Gate1", flight = "QF102" }
            );

        }
    }
}