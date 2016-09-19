using System.Web.Script.Serialization;
using AirportFlights.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;

namespace AirportFlights.Controllers
{
    public class FlightController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Flight Page";
            if ((FlightsPool.todayFlights != null) && (FlightsPool.todayFlights.Count > 0))
                ViewBag.isListFull = true;
            else
                ViewBag.isListFull = false;
            return View();
        }

        public ActionResult FillSampleDate()
        {
            int counter = FlightsPool.FlightsPoolInit();
            if (counter > 0)
                ViewBag.Message = counter + " sample dates has been added.";
            else
                ViewBag.Message = "";

            FlightsPool.Flights flights = new FlightsPool.Flights();
            flights.allFlights = new Dictionary<string,List<DailyFlights>>();
            foreach (var dfs in FlightsPool.todayFlights)
            {
                List<DailyFlights> dfsOrdered = dfs.Value.OrderBy(o => o.ArrivalTime).ToList();
                flights.allFlights.Add(dfs.Key, dfsOrdered);
            }

            return View(flights);
        }

        public JsonResult GetJsonData()
        {
            List<DailyFlightsJsonFormat> flightJsonList = new List<DailyFlightsJsonFormat>();
            string gateNumber = (string) Request.RequestContext.RouteData.Values["id"];
            if (String.IsNullOrEmpty(gateNumber))
                gateNumber = "0";
            GateDAO dao = new GateDAO();
            if (!String.Equals("0", gateNumber)) { 
                if ((FlightsPool.todayFlights != null) && (FlightsPool.todayFlights.Count > 0))
                { 
                    List<DailyFlights> list =dao.List(gateNumber);
                    string todayDate = DateTime.Now.ToString("yyyy-MM-dd")+'T';
                    foreach (var item in list)
                    {
                        DailyFlightsJsonFormat dailyFlightJsonFormat = new DailyFlightsJsonFormat();

                        dailyFlightJsonFormat.resourceId = gateNumber;
                        dailyFlightJsonFormat.title = item.FlightNumber;
                        dailyFlightJsonFormat.start = todayDate +item.ArrivalTime.ToString(@"hh\:mm\:ss");
                        dailyFlightJsonFormat.end = todayDate + item.DepartueTime.ToString(@"hh\:mm\:ss");
                        flightJsonList.Add(dailyFlightJsonFormat);
                    }
                }
            }
            return Json(flightJsonList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddFlight()
        {
            DailyFlights df = new DailyFlights();
            GateDAO dao = new GateDAO();
            foreach(Gate item in dao.gatesList){
                df.listGates.Add(new SelectListItem{Text = item.GateName , Value = item.GateNumber});
            }

            return View(df);
        }

        [HttpPost]
        public ActionResult AddFlight( DailyFlights df)
        {
            if (ModelState.IsValid)
            {
                GateDAO dao = new GateDAO();

                bool isAdded = dao.add(df.GateNumber,df);
                if(isAdded)
                    return RedirectToAction("Index");
            }
            return View(df);
        }
        [HttpGet]
        public ActionResult EditFlight(string gateNumber, string flightNumber)
        {
            if (String.IsNullOrEmpty(flightNumber))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GateDAO dao = new GateDAO();
            DailyFlights df = dao.get( flightNumber);
            if (df == null)
            {
                return HttpNotFound();
            }
            return View(df);
        }

        public ActionResult Contact()
        {
            return View();
        }

    }
}
