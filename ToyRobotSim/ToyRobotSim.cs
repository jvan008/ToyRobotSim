﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyRobotSim
{
    class ToyRobotSim
    {
        static void Main(string[] args)
        {
            Board board = new Board(5);
            Robot robot = new Robot(board);
            RobotCommandProvider robotCommandProvider = new RobotCommandProvider(() => Console.ReadLine());
            RobotController robotController = new RobotController(robot, robotCommandProvider);
            robotController.Control();
        }
    }

    /// <summary>
    /// A Board describes a robot area of X units.
    /// 
    /// Boards are always square, hence same size in both dimensions
    /// </summary>
    public class Board
    {
        public int Dimension { get; }

        public Board(int dimension)
        {
            this.Dimension = dimension;
        }

        public virtual bool IsLocationValid(int x, int y)
        {
            // ensure the location is greater than 0 and less than the dimension of the board
            return x >= 0 && y >= 0 && x < Dimension && y < Dimension;
        }
    }

    /// <summary>
    /// Domo arigato
    /// </summary>
    public class Robot
    {
        // The heading the robot faces
        public enum Heading
        {
            NORTH, EAST, SOUTH, WEST
        };

        // The directions the robot can turn
        private enum Direction
        {
            LEFT = -1, RIGHT = +1
        };

        // X,Y coordinates, we could go coordinate object but we're not doing any math so seems overkill
        private int x, y;

        public int X { get { return x; } }

        public int Y { get { return y; } }

        // The heading the robot faces
        private Heading face;

        public Heading Face { get { return face; } }

        // The board the robot is placed on
        private readonly Board board;

        // Track whether the robot has been placed in a valid location, invalid locations ignore commands until a valid placement
        private bool placed = false;

        public bool Placed { get { return placed; } }

        public Robot(Board board)
        {
            this.board = board;
        }

        public virtual void Place(int x, int y, Heading face)
        {
            // Placed is the same as saying the robot is at a valid location
            this.placed = board.IsLocationValid(x, y);

            // If we've placed then set the location and facing
            if (placed)
            {
                this.x = x;
                this.y = y;
                this.face = face;
            }
        }

        /// <summary>
        /// Move the robot by 1 unit in the direction it's pointing given it's current location, if the proposed location is invalid then the move will not occur.
        /// </summary>
        public virtual void Move()
        {
            // We can't move if we haven't been successfully placed
            if (!placed)
            {
                return;
            }

            int proposedX = this.x;
            int proposedY = this.y;

            switch (face)
            {
                case Heading.NORTH:
                    proposedY++;
                    break;
                case Heading.EAST:
                    proposedX++;
                    break;
                case Heading.SOUTH:
                    proposedY--;
                    break;
                case Heading.WEST:
                    proposedX--;
                    break;
                default:
                    break;
            }

            // Check if the board position is valid for the proposed location and if ok make the move
            if (board.IsLocationValid(proposedX, proposedY))
            {
                this.x = proposedX;
                this.y = proposedY;
            }
        }

        /// <summary>
        /// Rotate Left, aka anti-clockwise
        /// </summary>
        public virtual void Left()
        {
            Turn(Direction.LEFT);
        }

        /// <summary>
        /// Rotate Right, aka clockwise
        /// </summary>
        public virtual void Right()
        {
            Turn(Direction.RIGHT);
        }

        private void Turn(Direction direction)
        {
            // We can't turn if we haven't been successfully placed
            if (!placed)
            {
                return;
            }

            // Turn in the specified direction, current face with direction added
            int faceValue = (int)face + (int)direction;

            // This assumes that the enums are 0 indexed and valued sequentially
            int numFaces = Enum.GetNames(typeof(Heading)).Length;
            if (faceValue < 0)
            {
                faceValue = numFaces - 1;
            }

            // Modulo the number of faces and cast back to a Face enum
            faceValue = faceValue % numFaces;
            this.face = (Heading)faceValue;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Report()
        {
            Console.WriteLine("{0}, {1}, {2}", x, y, face);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RobotCommand
    {
        public string Command { get; }

        public string[] Arguments { get; }

        public RobotCommand(string command)
        {
            this.Command = command;
        }

        public RobotCommand(string command, string[] arguments)
        {
            this.Command = command;
            this.Arguments = arguments;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RobotCommandProvider
    {
        private readonly Func<string> stringProvider;

        public RobotCommandProvider(Func<string> stringProvider)
        {
            this.stringProvider = stringProvider;
        }

        public virtual RobotCommand GetNextCommand()
        {
            // An empty line means we are done
            string line = stringProvider();
            if (line == null || "".Equals(line.Trim()))
            {
                return null;
            }

            line = line.Trim();

            // Single command, no arguments
            if (line.IndexOf(" ") < 0)
            {
                return new RobotCommand(line);
            }

            // Get the command name
            int index = line.IndexOf(' ');
            string cmd = line.Substring(0, index);

            // Get the command arguments
            string[] arguments = line.Remove(0, index)
                .Trim()
                .Replace(" ", string.Empty)
                .Split(',');

            return new RobotCommand(cmd, arguments);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RobotController
    {
        private readonly Robot robot;

        private readonly RobotCommandProvider robotCommandProvider;

        public RobotController(Robot robot, RobotCommandProvider robotCommandProvider)
        {
            this.robot = robot;
            this.robotCommandProvider = robotCommandProvider;
        }

        public void Control()
        {
            RobotCommand command = robotCommandProvider.GetNextCommand();
            while (command != null)
            {
                // If we add more command then we should convert this to the command pattern
                switch (command.Command)
                {
                    case "PLACE":
                        // This is a little ugly but it's the only place we need to convert strings -> types
                        robot.Place(
                            Int32.Parse(command.Arguments[0]),
                            Int32.Parse(command.Arguments[1]),
                            (Robot.Heading)Enum.Parse(typeof(Robot.Heading), command.Arguments[2]));
                        break;
                    case "MOVE":
                        robot.Move();
                        break;
                    case "LEFT":
                        robot.Left();
                        break;
                    case "RIGHT":
                        robot.Right();
                        break;
                    case "REPORT":
                        robot.Report();
                        break;
                    default:
                        // TODO handle this properly using a real exception
                        throw new Exception("Unknown robot command");
                };

                // Get the next command to execute
                command = robotCommandProvider.GetNextCommand();
            }
        }
    }
}
