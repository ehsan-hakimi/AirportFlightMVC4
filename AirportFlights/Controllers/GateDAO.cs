using AirportFlights.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace AirportFlights.Controllers
{
    public class GateDAO
    {
        public Gate[] gatesList = new Gate[] 
        { 
            new Gate { GateName = "Gate A", GateNumber = FlightsPool.GateA}, 
            new Gate { GateName = "Gate B", GateNumber = FlightsPool.GateB}
        };
        public bool add(String gate, DailyFlights fligth)
        {
            List<FreeTimes> list = FlightsPool.availableTimes[gate];
            FreeTimes item=null;
            
            foreach (FreeTimes value in list)
            {
                if (fligth.ArrivalTime >= value.StartTime && fligth.DepartueTime <= value.EndTime)
                {
                    item = value;
                   
                    break;
                }
            }
            if (item!=null)
            {
                list.Remove(item);
                if (fligth.ArrivalTime > item.StartTime)
                {
                    list.Add(new FreeTimes(item.StartTime, fligth.ArrivalTime));
                }
                if (fligth.DepartueTime < item.EndTime)
                {
                    list.Add(new FreeTimes(fligth.DepartueTime, item.EndTime));
                }
                FlightsPool.todayFlights[gate].Add(fligth);
                return true;
            }
            return false;

        }
        public DailyFlights get( string flightNumber)
        {
            DailyFlights result =null;
            foreach (Gate gate in gatesList) { 
                foreach (DailyFlights item in FlightsPool.todayFlights[gate.GateNumber]){
                    if (item.FlightNumber.Equals(flightNumber))
                    {
                        result = item;
                        result.GateNumber = gate.GateNumber;
                        break;
                    }
                }
            }
            return result;
            
        }
        public List<DailyFlights> List(String gate)
        {
            return FlightsPool.todayFlights[gate];
    }

        public void cancel(string gate, DailyFlights fligth)
        {
            List<FreeTimes> list = FlightsPool.availableTimes[gate];
            FreeTimes findItem = null;
            bool findFlg = false;
            List<FreeTimes> removedList = new List<FreeTimes>();

            foreach (FreeTimes value in list)
            {
                if (fligth.ArrivalTime == value.EndTime && findFlg==false)
                {
                    value.EndTime=fligth.DepartueTime;
                    findFlg=true;
                    findItem=value;
                }else if (findFlg==true && findItem.EndTime== value.StartTime){
                    findItem.EndTime=value.EndTime;
                    removedList.Add(value);
                }
            }
            if (findFlg==false){
                list.Add(new FreeTimes(fligth.ArrivalTime,fligth.DepartueTime));
            }
            else
            {
                FlightsPool.availableTimes[gate]=list.Except(removedList).ToList();
                
            }
            FlightsPool.todayFlights[gate].Remove(fligth);

        }

        public bool update(String gate,TimeSpan arrivalTime ,TimeSpan departueTime,string flightNumber)
        {
            List<DailyFlights> list = FlightsPool.todayFlights[gate];
            DailyFlights oldItem=null;
            bool result = false;
            foreach (DailyFlights value in list)
            {
                if (value.FlightNumber == flightNumber)
                {
                    oldItem = value;
                    break;

                }
            }
            if (oldItem != null)
            {
                cancel(gate, oldItem);
                DailyFlights newItem = new DailyFlights(arrivalTime, departueTime, flightNumber);
                result =  add(gate, newItem);
               if (!result)
               {
                                     gate = FlightsPool.getAnotherGateName(gate);
                   if (gate != null)
                   {
                       result= add(gate,newItem);
                       if (result == false)
                       {
                           add(gate, oldItem);

                       }
                   }
                   else
                   {
                       add(gate, oldItem);

                   }

               }
            }
            return result;
        }
    }
}