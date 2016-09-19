using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirportFlights.Models
{
    public class DailyFlightsJsonFormat
    {
        public string id { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string title{ get; set; }
        public string resourceId{ get; set; }

    }
}