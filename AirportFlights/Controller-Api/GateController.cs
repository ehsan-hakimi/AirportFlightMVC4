using AirportFlights.Controllers;
using AirportFlights.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AirportFlights.Controller_Api
{
    public class GateController : ApiController
    {
        [HttpGet]
        [ActionName("AllGates")]
        public IHttpActionResult Get()
        {
            IHttpActionResult result = null;
            GateDAO dao = new GateDAO();

        if (dao.gatesList.Count() > 0)
            {
                result = Ok(dao.gatesList);
            }
            else
            {
                result = NotFound();
            }

            return result;
        }
    }
}
