using System;
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

        public virtual Boolean IsLocationValid(int x, int y)
        {
            // ensure the location is greater than 0 and less than the dimension of the board
            return x >= 0 && y >= 0 && x < Dimension && y < Dimension;
        }
    }
}
