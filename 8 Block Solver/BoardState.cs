using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8_Block_Solver
{
    [System.Serializable]
    class BoardState : IEquatable<BoardState>
    {
        public BoardState predecessor { get; set; }
        public TileGrid tileGridState { get; set; }
        public int heuristic { get; set; }

        public BoardState(TileGrid _tileGrid, BoardState _predecessor, int _heuristic)
        {
            predecessor = _predecessor;
            tileGridState = _tileGrid;
            heuristic = _heuristic;
        }

        public bool Equals(BoardState other)
        {
            if (tileGridState.tileGridArray.Equals(other.tileGridState.tileGridArray))
                return true;

            return false;
        }
    }
}
