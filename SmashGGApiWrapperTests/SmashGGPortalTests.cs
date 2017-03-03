using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmashGGApiWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashGGApiWrapper.Tests
{
    [TestClass()]
    public class SmashGGPortalTests
    {
        [TestMethod()]
        public void GetEventsTest()
        {
            SmashGGPortal port = new SmashGGPortal("genesis-3");
            IEnumerable<Event> events = port.GetEvents();
            Assert.Fail();
        }
    }
}