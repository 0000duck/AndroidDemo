using Microsoft.VisualStudio.TestTools.UnitTesting;
using myconn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cmd.Tests
{
    [TestClass()]
    public class DataChangeTests
    {
        [TestMethod()]
        public void byteToHexStrTest()
        {
            byte[] tt = new byte[3];
            tt[0] = 0x65;
            tt[1] = 0x66;
            tt[2] = 0x00;
            string data = DataChange.byteToHexOXStr(tt);
            Assert.AreEqual(data.Trim(), "0x65 0x66 0x00");
        }
    }
}