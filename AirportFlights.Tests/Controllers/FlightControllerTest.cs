using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AirportFlights;
using AirportFlights.Controllers;

namespace AirportFlights.Tests.Controllers
{
    [TestClass]
    public class FlightControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            FlightController controller = new FlightController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.AreEqual("Modify this template to jump-start your ASP.NET MVC application.", result.ViewBag.Message);
        }

        [TestMethod]
        public void About()
        {
            // Arrange
            FlightController controller = new FlightController();

            // Act
            ViewResult result = controller.FillSampleDate() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Contact()
        {
            // Arrange
            FlightController controller = new FlightController();

            // Act
            ViewResult result = controller.Contact() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
