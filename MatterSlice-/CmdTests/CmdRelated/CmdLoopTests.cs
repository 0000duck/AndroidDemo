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
    public class CmdLoopTests
    {
        [TestMethod()]
        public void GetLoopStartCmdTest()
        {
            //AA 05 01 C4 05 C5
            CmdLoop cf = new CmdLoop();
            byte[] ret = cf.GetLoopStartCmd(0x05);
            Assert.AreEqual(ret.Count(), 6);
            Assert.AreEqual(ret[5], 0xC5);

            Assert.AreEqual(ret[0], 0xAA);
            Assert.AreEqual(ret[1], 0x05);
            Assert.AreEqual(ret[2], 0x01);
            Assert.AreEqual(ret[3], 0xC4);
        }

        [TestMethod()]
        public void GetLoopStopCmdTest()
        {
            CmdLoop cf = new CmdLoop();
            byte[] ret = cf.GetLoopStopCmd();
            Assert.AreEqual(ret.Count(), 5);
            Assert.AreEqual(ret[4], 0xC0);

            Assert.AreEqual(ret[0], 0xAA);
            Assert.AreEqual(ret[1], 0x04);
            Assert.AreEqual(ret[2], 0x01);
            Assert.AreEqual(ret[3], 0xC5);
        }
    }
}