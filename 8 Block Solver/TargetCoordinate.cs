using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8_Block_Solver
{
    class TargetCoordinate
    {
        public int xCoordinate { get; set; }
        public int yCoordinate { get; set; }
        public int faceValue;
        public TargetCoordinate(int _faceValue)
        {
            faceValue = _faceValue;

            switch (faceValue)
            {                
                case 1:
                    {
                        xCoordinate = 0;
                        yCoordinate = 0;
                        break;
                    }
                case 2:
                    {
                        xCoordinate = 1;
                        yCoordinate = 0;
                        break;
                    }
                case 3:
                    {
                        xCoordinate = 2;
                        yCoordinate = 0;
                        break;
                    }
                case 4:
                    {
                        xCoordinate = 0;
                        yCoordinate = 1;
                        break;
                    }
                case 5:
                    {
                        xCoordinate = 1;
                        yCoordinate = 1;
                        break;
                    }
                case 6:
                    {
                        xCoordinate = 2;
                        yCoordinate = 1;
                        break;
                    }
                case 7:
                    {
                        xCoordinate = 0;
                        yCoordinate = 2;
                        break;
                    }
                case 8:
                    {
                        xCoordinate = 1;
                        yCoordinate = 2;
                        break;
                    }
                case 0:
                    {
                        xCoordinate = 2;
                        yCoordinate = 2;
                        break;
                    }
            }
        }
    }
}
