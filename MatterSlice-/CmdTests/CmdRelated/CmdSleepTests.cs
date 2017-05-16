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
    public class CmdSleepTests
    {
        [TestMethod()]
        public void GetSleepOneCmdTest()
        {
            //AA 05 01 C0 99 5D
            CmdSleep cf = new CmdSleep();
            byte[] ret = cf.GetSleepOneCmd( 0x99);
            Assert.AreEqual(ret.Count(), 6);
            Assert.AreEqual(ret[5], 0x5D);

            Assert.AreEqual(ret[0], 0xAA);
            Assert.AreEqual(ret[1], 0x05);
            Assert.AreEqual(ret[2], 0x01);
            Assert.AreEqual(ret[3], 0xC0);
        }

        [TestMethod()]
        public void GetSleepTwoCmdTest()
        {
            //AA 05 01 C1 99 5C
            CmdSleep cf = new CmdSleep();
            byte[] ret = cf.GetSleepTwoCmd(0x99);
            Assert.AreEqual(ret.Count(), 6);
            Assert.AreEqual(ret[5], 0x5C);

            Assert.AreEqual(ret[0], 0xAA);
            Assert.AreEqual(ret[1], 0x05);
            Assert.AreEqual(ret[2], 0x01);
            Assert.AreEqual(ret[3], 0xC1);
        }

        [TestMethod()]
        public void GetWaitAxisResetCmdTest()
        {
            //AA 05 01 C2 01 C7
            CmdSleep cf = new CmdSleep();
            byte[] ret = cf.GetWaitAxisResetCmd(0x01);
            Assert.AreEqual(ret.Count(), 6);
            Assert.AreEqual(ret[5], 0xC7);

            Assert.AreEqual(ret[0], 0xAA);
            Assert.AreEqual(ret[1], 0x05);
            Assert.AreEqual(ret[2], 0x01);
            Assert.AreEqual(ret[3], 0xC2);
        }
    }
}