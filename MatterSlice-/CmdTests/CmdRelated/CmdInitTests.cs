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
    public class CmdInitTests
    {
        [TestMethod()]
        public void GetAddrCmdTest()
        {
            //AA 05 00 A0 01 A4
            CmdInit cf = new CmdInit();
            Global.gCommandAddr = 0x00;
            byte[] ret = cf.GetAddrCmd(0x01);
            Assert.AreEqual(ret.Count(), 6);
            Assert.AreEqual(ret[5], 0xA4);
            Assert.AreEqual(ret[0], 0xAA);
            Assert.AreEqual(ret[1], 0x05);
            Assert.AreEqual(ret[2], 0x00);
            Assert.AreEqual(ret[3], 0xA0);
            Global.gCommandAddr = 0x01;
        }

        [TestMethod()]
        public void GetBaudRateCmdTest()
        {
            //AA 05 01 A1 00 A5
            CmdInit cf = new CmdInit();
            byte[] ret = cf.GetBaudRateCmd(0x00);
            Assert.AreEqual(ret.Count(), 6);
            Assert.AreEqual(ret[5], 0xA5);
            Assert.AreEqual(ret[0], 0xAA);
            Assert.AreEqual(ret[1], 0x05);
            Assert.AreEqual(ret[2], 0x01);
            Assert.AreEqual(ret[3], 0xA1);
        }

        [TestMethod()]
        public void GetXAxisCmdTest()
        {
            //AA 05 01 A2 00 A6
            CmdInit cf = new CmdInit();
            byte[] ret = cf.GetAxisCmd(0x00);
            Assert.AreEqual(ret.Count(), 6);
            Assert.AreEqual(ret[5], 0xA6);
            Assert.AreEqual(ret[0], 0xAA);
            Assert.AreEqual(ret[1], 0x05);
            Assert.AreEqual(ret[2], 0x01);
            Assert.AreEqual(ret[3], 0xA2);
        }



        [TestMethod()]
        public void GetInLevelUsefulCmdTest()
        {
            //AA 05 01 A3 00 A7
            CmdInit cf = new CmdInit();
            byte[] ret = cf.GetInLevelUsefulCmd(0x00);
            Assert.AreEqual(ret.Count(), 6);
            Assert.AreEqual(ret[5], 0xA7);
            Assert.AreEqual(ret[0], 0xAA);
            Assert.AreEqual(ret[1], 0x05);
            Assert.AreEqual(ret[2], 0x01);
            Assert.AreEqual(ret[3], 0xA3);
        }

        [TestMethod()]
        public void GetOutLevelUsefulCmdTest()
        {
            //AA 05 01 A4 00 A0
            CmdInit cf = new CmdInit();
            byte[] ret = cf.GetOutLevelUsefulCmd(0x00);
            Assert.AreEqual(ret.Count(), 6);
            Assert.AreEqual(ret[5], 0xA0);
            Assert.AreEqual(ret[0], 0xAA);
            Assert.AreEqual(ret[1], 0x05);
            Assert.AreEqual(ret[2], 0x01);
            Assert.AreEqual(ret[3], 0xA4);
        }

        [TestMethod()]
        public void GetRelativeAxLitCmdTest()
        {
            ////AA 05 01 A4 00 A0
            //CmdInit cf = new CmdInit();
            //Global.gCommandAddr = 0x00;
            //byte[] ret = cf.GetRelativeAxLitCmd(0x00);
            //Assert.AreEqual(ret.Count(), 6);
            //Assert.AreEqual(ret[5], 0xA0);
            //Assert.AreEqual(ret[0], 0xAA);
            //Assert.AreEqual(ret[1], 0x05);
            //Assert.AreEqual(ret[2], 0x01);
            //Assert.AreEqual(ret[3], 0xA4);
        }

        [TestMethod()]
        public void GetAbsoluteAxLitCmdTest()
        {
            //Assert.Fail();
        }

        [TestMethod()]
        public void GetNowZeroCmdTest()
        {
            //AA 05 01 A6 00 A2
            CmdInit cf = new CmdInit();
            byte[] ret = cf.GetNowZeroCmd(0x00);
            Assert.AreEqual(ret.Count(), 6);
            Assert.AreEqual(ret[5], 0xA2);
            Assert.AreEqual(ret[0], 0xAA);
            Assert.AreEqual(ret[1], 0x05);
            Assert.AreEqual(ret[2], 0x01);
            Assert.AreEqual(ret[3], 0xA6);
        }

        [TestMethod()]
        public void GetAxisValueCmdTest()
        {
            //AA 05 01 A7 00 A3
            CmdInit cf = new CmdInit();
            byte[] ret = cf.GetAxisValueCmd(0x00);
            Assert.AreEqual(ret.Count(), 6);
            Assert.AreEqual(ret[5], 0xA3);
            Assert.AreEqual(ret[0], 0xAA);
            Assert.AreEqual(ret[1], 0x05);
            Assert.AreEqual(ret[2], 0x01);
            Assert.AreEqual(ret[3], 0xA7);
        }

        [TestMethod()]
        public void GetMachineValueCmdTest()
        {
            //AA 05 01 A8 00 AC
            CmdInit cf = new CmdInit();
            byte[] ret = cf.GetMachineValueCmd(0x00);
            Assert.AreEqual(ret.Count(), 6);
            Assert.AreEqual(ret[5], 0xAC);
            Assert.AreEqual(ret[0], 0xAA);
            Assert.AreEqual(ret[1], 0x05);
            Assert.AreEqual(ret[2], 0x01);
            Assert.AreEqual(ret[3], 0xA8);
        }

        [TestMethod()]
        public void GetSettedLimiteValueCmdTest()
        {
            //AA 05 01 A9 00 AD
            CmdInit cf = new CmdInit();
            byte[] ret = cf.GetSettedLimiteValueCmd(0x00);
            Assert.AreEqual(ret.Count(), 6);
            Assert.AreEqual(ret[5], 0xAD);
            Assert.AreEqual(ret[0], 0xAA);
            Assert.AreEqual(ret[1], 0x05);
            Assert.AreEqual(ret[2], 0x01);
            Assert.AreEqual(ret[3], 0xA9);
        }
    }
}