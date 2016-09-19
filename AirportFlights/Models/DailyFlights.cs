using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AirportFlights.Models
{
    public class DailyFlights
    {
        public TimeSpan ArrivalTime { get; set; }
        public TimeSpan DepartueTime { get; set; }
        public string FlightNumber{ get; set; }
        public string GateNumber { get; set; }
        public List<SelectListItem> listGates = new List<SelectListItem>();

        public DailyFlights()
        {
        }
        public DailyFlights(TimeSpan arrivalTime, TimeSpan departueTime, string flightNumber)
        {
            this.ArrivalTime = arrivalTime;
            this.DepartueTime = departueTime;
            this.FlightNumber = flightNumber;

        }
    }
}