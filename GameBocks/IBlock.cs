using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameBocks
{
    public class IBlock
    {
        public Point[] BlocksCoordinates { get; set; } = new Point[4];
        private int _size = 4;
        public IBlock()
        {
            for (int i = 0; i < _size; i++)
            {
                BlocksCoordinates[i] = new Point(0, 3 + i);
            }
        }
    }
}
