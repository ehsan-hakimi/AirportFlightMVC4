using AirportFlights.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AirportFlights.Models
{
    public class FlightsPool
    {
        public const string GateA = "Gate1";
        public const string GateB = "Gate2";


        public class Flights
        {
            public Dictionary<string, List<DailyFlights>> allFlights { get; set; }
        }
        public  static List<string> gates = new List<string>();

        public  static Dictionary<string, List<DailyFlights>> todayFlights = new Dictionary<string, List <DailyFlights>>();
        public  static Dictionary<string, List<FreeTimes>> availableTimes = new Dictionary<string, List<FreeTimes>>();
        
        public static String getAnotherGateName(String name)
        {
            foreach (String value in gates)
            {
                if (value != name)
                {
                    return value;
                }
            }
            return null;
        }

        public static int FlightsPoolInit()
	    {
            int counter = 0;
            if(gates.Count == 0){
                gates.Add(FlightsPool.GateA);
                gates.Add(FlightsPool.GateB);
                if (availableTimes.Count == 0)
                {
                    foreach (var gate in gates)
                    {
                        List<FreeTimes> initialFreeTimes = new List<FreeTimes>();
                        initialFreeTimes.Add(new FreeTimes(TimeSpan.Zero, new TimeSpan(23, 59, 0)));
                        availableTimes.Add(gate, initialFreeTimes);
                    }
                }
                if (todayFlights.Count == 0)
                {
                    GateDAO dao = new GateDAO();
                    string fNumber = "";
                    string flightPrefix = "";
                    TimeSpan aTime = TimeSpan.Zero;
                    TimeSpan dTime = TimeSpan.Zero;

                    foreach (var gate in gates)
                    {
                        
                        todayFlights.Add(gate,new List<DailyFlights>());

                        if (gate.Equals(FlightsPool.GateA))
                            flightPrefix = "QF";
                        else
                            flightPrefix = "EK";
                        bool isAdded;
                        for (var i = 1; i < 11; i++)
                        {
                            isAdded = false;
                            fNumber = flightPrefix + (i + 50);
                            aTime = aTime.Add(TimeSpan.FromHours(i));
                            dTime = aTime.Add(TimeSpan.FromMinutes(30));
                            isAdded = dao.add(gate, new DailyFlights( aTime, dTime, fNumber));
                            if (isAdded)
                                counter++;
                            aTime = TimeSpan.Zero;
                            dTime = TimeSpan.Zero;
                        }
                    }
                }
            }
            return counter;
	    }
 

    }
}