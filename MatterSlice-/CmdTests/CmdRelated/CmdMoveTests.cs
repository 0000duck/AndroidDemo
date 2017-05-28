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
    public class CmdMoveTests
    {
        [TestMethod()]
        public void GetOneStepCmdTest()
        {
            //AA 06 01 B0 00 01 B6
            //AA 06 01 B0 00 81 36
            CmdMove cf = new CmdMove();
            byte[] ret = cf.GetOneStepCmd(0x00, true);
            Assert.AreEqual(ret.Count(), 7);
            Assert.AreEqual(ret[6], 0xB6);
            Assert.AreEqual(ret[0], 0xAA);
            Assert.AreEqual(ret[1], 0x06);
            Assert.AreEqual(ret[2], 0x01);
            Assert.AreEqual(ret[3], 0xB0);

            byte[] ret1 = cf.GetOneStepCmd(0x00, false);
            Assert.AreEqual(ret1.Count(), 7);
            Assert.AreEqual(ret1[6], 0x36);
            Assert.AreEqual(ret1[0], 0xAA);
            Assert.AreEqual(ret1[1], 0x06);
            Assert.AreEqual(ret1[2], 0x01);
            Assert.AreEqual(ret1[3], 0xB0);
        }

        [TestMethod()]
        public void GetUniformStepCmdTest()
        {
            //AA 0B 01 B1 00 10 00 00 00 20 00 8B
            //CmdMove cf = new CmdMove();
            //byte[] ret = cf.GetUniformStepCmd(0x00, true);
            //Assert.AreEqual(ret.Count(), 12);
            //Assert.AreEqual(ret[6], 0xB6);
            //Assert.AreEqual(ret[0], 0xAA);
            //Assert.AreEqual(ret[1], 0x06);
            //Assert.AreEqual(ret[2], 0x01);
            //Assert.AreEqual(ret[3], 0xB0);

            ////AA 0B 01 B1 00 10 00 80 00 20 00 0B
            //byte[] ret1 = cf.GetUniformStepCmd(0x00, false);
            //Assert.AreEqual(ret1.Count(), 12);
            //Assert.AreEqual(ret1[6], 0x36);
            //Assert.AreEqual(ret1[0], 0xAA);
            //Assert.AreEqual(ret1[1], 0x06);
            //Assert.AreEqual(ret1[2], 0x01);
            //Assert.AreEqual(ret1[3], 0xB0);
        }
    }
}