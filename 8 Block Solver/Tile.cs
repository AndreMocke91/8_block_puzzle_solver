using System.Windows.Forms;

namespace _8_Block_Solver
{
    [System.Serializable]
    public class Tile
    {
        public int faceValue { get; set; }
        public int xCoordinate { get; set; }
        public int yCoordinate { get; set; }
        public string boundButtonName;

        public Tile(int _faceValue, int _xCoordinate, int _yCoordinate, string _boundButtonName )
        {
            faceValue = _faceValue;
            xCoordinate = _xCoordinate;
            yCoordinate = _yCoordinate;
            boundButtonName = _boundButtonName;
        }
    }
}
