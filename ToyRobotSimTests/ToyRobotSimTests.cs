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

    [TestClass]
    public class RobotTest
    {
        [TestMethod]
        public void TestFace()
        {
            Mock<Board> mockBoard = new Mock<Board>(5);
            Robot robot = new Robot(mockBoard.Object);
            robot.Place(0, 0, Robot.Heading.NORTH);

            AreEqual(Robot.Heading.NORTH, robot.Face);
        }

        [TestMethod]
        public void TestValidMove()
        {
            Mock<Board> mockBoard = new Mock<Board>(5);
            mockBoard.Setup(b => b.IsLocationValid(0, 0)).Returns(true);
            mockBoard.Setup(b => b.IsLocationValid(0, 1)).Returns(true);

            Robot robot = new Robot(mockBoard.Object);
            robot.Place(0, 0, Robot.Heading.NORTH);
            robot.Move();

            AreEqual(0, robot.X);
            AreEqual(1, robot.Y);
            AreEqual(Robot.Heading.NORTH, robot.Face);
            AreEqual(true, robot.Placed);

            mockBoard.VerifyAll();
        }

        [TestMethod]
        public void TestInValidMove()
        {
            Mock<Board> mockBoard = new Mock<Board>(5);
            mockBoard.Setup(b => b.IsLocationValid(4, 4)).Returns(true);
            mockBoard.Setup(b => b.IsLocationValid(4, 5)).Returns(false);

            Robot robot = new Robot(mockBoard.Object);
            robot.Place(4, 4, Robot.Heading.NORTH);
            robot.Move();

            mockBoard.VerifyAll();

            // Location and face haven't changed but we're still placed
            AreEqual(4, robot.X);
            AreEqual(4, robot.Y);
            AreEqual(Robot.Heading.NORTH, robot.Face);
            AreEqual(true, robot.Placed);
        }

        [TestMethod]
        public void TestValidPlace()
        {
            Mock<Board> mockBoard = new Mock<Board>(5);
            mockBoard.Setup(b => b.IsLocationValid(4, 4)).Returns(true);
            Robot robot = new Robot(mockBoard.Object);
            robot.Place(4, 4, Robot.Heading.NORTH);

            mockBoard.VerifyAll();
            AreEqual(true, robot.Placed);
        }

        [TestMethod]
        public void TestInValidPlace()
        {
            Mock<Board> mockBoard = new Mock<Board>(5);
            mockBoard.Setup(b => b.IsLocationValid(4, 4)).Returns(false);
            Robot robot = new Robot(mockBoard.Object);
            robot.Place(4, 4, Robot.Heading.NORTH);

            mockBoard.VerifyAll();
            AreEqual(false, robot.Placed);
        }

        [TestMethod]
        public void TestLeft()
        {
            Mock<Board> mockBoard = new Mock<Board>(5);
            mockBoard.Setup(b => b.IsLocationValid(0, 0)).Returns(true);

            Robot robot = new Robot(mockBoard.Object);
            robot.Place(0, 0, Robot.Heading.NORTH);

            robot.Left();
            AreEqual(Robot.Heading.WEST, robot.Face);

            robot.Left();
            AreEqual(Robot.Heading.SOUTH, robot.Face);

            robot.Left();
            AreEqual(Robot.Heading.EAST, robot.Face);

            robot.Left();
            AreEqual(Robot.Heading.NORTH, robot.Face);

            robot.Left();
            AreEqual(Robot.Heading.WEST, robot.Face);
        }

        [TestMethod]
        public void TestRight()
        {
            Mock<Board> mockBoard = new Mock<Board>(5);
            mockBoard.Setup(b => b.IsLocationValid(0, 0)).Returns(true);

            Robot robot = new Robot(mockBoard.Object);
            robot.Place(0, 0, Robot.Heading.NORTH);

            robot.Right();
            AreEqual(Robot.Heading.EAST, robot.Face);

            robot.Right();
            AreEqual(Robot.Heading.SOUTH, robot.Face);

            robot.Right();
            AreEqual(Robot.Heading.WEST, robot.Face);

            robot.Right();
            AreEqual(Robot.Heading.NORTH, robot.Face);

            robot.Right();
            AreEqual(Robot.Heading.EAST, robot.Face);

            mockBoard.VerifyAll();
        }
    }
}
