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
    public class CmdFileManageTests
    {
        [TestMethod()]
        public void GetEraseSectorCmdTest()
        {
            CmdFileManage cf = new CmdFileManage();
            byte[] ret = cf.GetEraseSectorCmd(0x00, 0x00);
            Assert.AreEqual(ret.Count(), 7);
            Assert.AreEqual(ret[6], 0xD7);

        }

        [TestMethod()]
        public void GetEraseBlockCmdTest()
        {
            CmdFileManage cf = new CmdFileManage();
            byte[] ret = cf.GetEraseBlockCmd(0x00, 0x00);
            Assert.AreEqual(ret.Count(), 7);
            Assert.AreEqual(ret[6], 0xD6);
        }

        [TestMethod()]
        public void GetEraseAllCmdTest()
        {
            CmdFileManage cf = new CmdFileManage();
            byte[] ret = cf.GetEraseAllCmd();
            Assert.AreEqual(ret.Count(), 5);
            Assert.AreEqual(ret[4], 0xD7);
        }

        [TestMethod()]
        public void GetWriteToSectorCmdTest()
        {
            CmdFileManage cf = new CmdFileManage();
            byte[] ret = cf.GetWriteToSectorCmd(0x00, 0x00);
            Assert.AreEqual(ret.Count(), 7);
            Assert.AreEqual(ret[6], 0xD4);
        }

        [TestMethod()]
        public void GetWriteToBlockCmdTest()
        {
            CmdFileManage cf = new CmdFileManage();
            byte[] ret = cf.GetWriteToBlockCmd(0x00, 0x00);
            Assert.AreEqual(ret.Count(), 7);
            Assert.AreEqual(ret[6], 0xD3);
        }

        [TestMethod()]
        public void GetReadOnePageCmdTest()
        {
            CmdFileManage cf = new CmdFileManage();
            byte[] ret = cf.GetReadOnePageCmd(0x00, 0x00);
            Assert.AreEqual(ret.Count(), 7);
            Assert.AreEqual(ret[6], 0xD2);

            Assert.AreEqual(ret[0], 0xAA);
            Assert.AreEqual(ret[1], 0x06);
            Assert.AreEqual(ret[2], 0x01);
            Assert.AreEqual(ret[3], 0xD5);
        }

        [TestMethod()]
        public void GetReadSectorCmdTest()
        {
            CmdFileManage cf = new CmdFileManage();
            byte[] ret = cf.GetReadSectorCmd(0x00, 0x00);
            Assert.AreEqual(ret.Count(), 7);
            Assert.AreEqual(ret[6], 0xD1);

            Assert.AreEqual(ret[0], 0xAA);
            Assert.AreEqual(ret[1], 0x06);
            Assert.AreEqual(ret[2], 0x01);
            Assert.AreEqual(ret[3], 0xD6);
        }

        [TestMethod()]
        public void GetReadBlockCmdTest()
        {
            CmdFileManage cf = new CmdFileManage();
            byte[] ret = cf.GetReadBlockCmd(0x00, 0x00);
            Assert.AreEqual(ret.Count(), 7);
            Assert.AreEqual(ret[6], 0xD0);

            Assert.AreEqual(ret[0], 0xAA);
            Assert.AreEqual(ret[1], 0x06);
            Assert.AreEqual(ret[2], 0x01);
            Assert.AreEqual(ret[3], 0xD7);
        }

        [TestMethod()]
        public void GetExcuteCommonCmdTest()
        {
            CmdFileManage cf = new CmdFileManage();
            byte[] ret = cf.GetExcuteCommonCmd(0x00, 0x01);
            Assert.AreEqual(ret.Count(), 7);
            Assert.AreEqual(ret[6], 0xDE);

            Assert.AreEqual(ret[0], 0xAA);
            Assert.AreEqual(ret[1], 0x06);
            Assert.AreEqual(ret[2], 0x01);
            Assert.AreEqual(ret[3], 0xD8);
        }

        [TestMethod()]
        public void GetExcute4AxisCmdTest()
        {
            CmdFileManage cf = new CmdFileManage();
            byte[] ret = cf.GetExcute4AxisCmd(0x01, 0x00);
            Assert.AreEqual(ret.Count(), 7);
            Assert.AreEqual(ret[6], 0xDF);

            Assert.AreEqual(ret[0], 0xAA);
            Assert.AreEqual(ret[1], 0x06);
            Assert.AreEqual(ret[2], 0x01);
            Assert.AreEqual(ret[3], 0xD9);
        }

        [TestMethod()]
        public void GetWriteToPageCmdTest()
        {
            CmdFileManage cf = new CmdFileManage();
            byte[] ret = cf.GetWriteToPageCmd(0x00, 0x00);
            Assert.AreEqual(ret.Count(), 7);
            Assert.AreEqual(ret[6], 0xDD);

            Assert.AreEqual(ret[0], 0xAA);
            Assert.AreEqual(ret[1], 0x06);
            Assert.AreEqual(ret[2], 0x01);
            Assert.AreEqual(ret[3], 0xDA);
        }
    }
}