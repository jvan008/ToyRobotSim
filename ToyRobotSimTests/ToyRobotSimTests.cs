using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using ToyRobotSim;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace ToyRobotSimTests
{
    [TestClass]
    public class BoardTest
    {
        [TestMethod]
        public void TestValidLocation()
        {
            Board b = new Board(10);
            bool valid = b.IsLocationValid(0, 0);
            IsTrue(valid);
        }

        [TestMethod]
        public void TestInValidLocation()
        {
            Board b = new Board(5);
            bool valid = b.IsLocationValid(4, 5);
            IsFalse(valid);
        }

        [TestMethod]
        public void TestNegativeLocation()
        {
            Board b = new Board(5);
            bool valid = b.IsLocationValid(-1, 4);
            IsFalse(valid);
        }
    }
}
