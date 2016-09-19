using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirportFlights.Models
{
    public class FreeTimes
    {
        public TimeSpan StartTime{ get; set; }
        public TimeSpan EndTime { get; set; }
        public FreeTimes(TimeSpan sTime, TimeSpan eTime)
        {
            this.StartTime = sTime;
            this.EndTime = eTime;
        }
    }
}