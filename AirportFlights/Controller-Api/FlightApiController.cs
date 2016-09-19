using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AirportFlights.Controllers;
using AirportFlights.Models;

namespace AirportFlights.Controller_Api
{
    public class FlightApiController : ApiController
    {
        // GET api/<controller>/Gate1
        [HttpGet] 
        [ActionName("AllFlights")]
        public IHttpActionResult GetAllFlights(string id)
        {
            IHttpActionResult result = null;
            GateDAO dao = new GateDAO();
            List<DailyFlights> dfs;
            string gateNumber = id;

            if (String.IsNullOrEmpty(gateNumber) || !(FlightsPool.todayFlights.ContainsKey(gateNumber)))
                dfs = null;
            else
            {
                dfs = dao.List(gateNumber);
            }

            if ((dfs == null) || (dfs.Count == 0) ){
                dfs = new List<DailyFlights>();
                result = Ok(dfs);
            }else
                result = Ok(dfs);

            return result;
        }



        // GET api/<controller>/5
        [HttpGet]
        [ActionName("GetFlight")]
        public IHttpActionResult GetFlight(string id)  
        {
            IHttpActionResult result;
            DailyFlights df = null;

            GateDAO dao = new GateDAO();
            df = dao.get(id);
            if (df != null)
            {
                result = Ok(df);
            }
            else
            {
                result = NotFound();
            }

            return result;
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut()]
        [ActionName("UpdateFlight")]
        public IHttpActionResult PutFlight(DailyFlights dailyFlight)
        {
            IHttpActionResult result = null;
            GateDAO dao = new GateDAO();
            bool isUpdated = dao.update(dailyFlight.GateNumber, dailyFlight.ArrivalTime, dailyFlight.DepartueTime, dailyFlight.FlightNumber);
            if (isUpdated)
            {
                result = Ok(dailyFlight);
            }
            else
            {
                result = NotFound();
            }

            return result;
        }

        // DELETE api/<controller>/5
        [HttpDelete()]
        [ActionName("DeleteFlight")]
        public IHttpActionResult Delete(string id)
        {
            IHttpActionResult result = null;

            GateDAO dao = new GateDAO();
            // Get the flight
            DailyFlights df = dao.get(id);
            // Did we find the flight?
            if (df.FlightNumber != null)
            {
                // Delete the flight
                dao.cancel(df.GateNumber,df);

                result = Ok(true);
            }
            else
            {
                result = NotFound();
            }

            return result;
        }
    }
}