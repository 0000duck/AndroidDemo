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
    public class CmdInOutTests
    {
        [TestMethod()]
        public void GetIOCmdTest()
        {
            CmdInOut cf = new CmdInOut();
            byte[] ret = cf.GetIOCmd(0x00, 0x01);
            Assert.AreEqual(ret.Count(), 7);
            Assert.AreEqual(ret[6], 0xC5);

            Assert.AreEqual(ret[0], 0xAA);
            Assert.AreEqual(ret[1], 0x06);
            Assert.AreEqual(ret[2], 0x01);
            Assert.AreEqual(ret[3], 0xC3);
        }
    }
}