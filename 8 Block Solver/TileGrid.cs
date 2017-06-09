using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8_Block_Solver
{
    [System.Serializable]
    public class TileGrid : IEquatable<TileGrid>
    {
        public Tile[,] tileGridArray { get; set; }
        public List<int> faceValues { get; set; }

        public TileGrid()
        {
            faceValues = new List<int>() {
                1,2,3,4,5,6,7,8,0
            };
            tileGridArray = new Tile[3, 3];            
        }

        public TileGrid(TileGrid _tileGrid)
        {
            tileGridArray = _tileGrid.tileGridArray;
            faceValues = _tileGrid.faceValues;
        }

        public bool IsAdjacentToBlank(TileGrid _tileGrid, Tile prospectTile)
        {
            Tile blankTile = GetBlankTile();

            if (
                // If Tile is directly above or below the blank tile
                (prospectTile.xCoordinate == blankTile.xCoordinate &&
                    (
                        prospectTile.yCoordinate == blankTile.yCoordinate + 1 ||
                        prospectTile.yCoordinate == blankTile.yCoordinate - 1
                    )
                ) 

                ||
                // If Tile is directly to the right or left
                (prospectTile.yCoordinate == blankTile.yCoordinate &&
                    (
                        prospectTile.xCoordinate == blankTile.xCoordinate + 1 ||
                        prospectTile.xCoordinate == blankTile.xCoordinate - 1
                    )
                )

                )
            {
                return true;
            }
            return false;
        }

        public Tile GetBlankTile()
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (tileGridArray[x, y].faceValue == 0)
                    {
                        return tileGridArray[x, y];
                    }
                }
            }

            return null;
        }

        public void SwapTileWithBlank(int faceValue)
        {
            Tile blankTile = GetBlankTile();
            Tile selectedTile = GetTileFromFaceValue(faceValue);

            blankTile.faceValue = faceValue;
            selectedTile.faceValue = 0;          

        }        

        public Tile GetTileFromFaceValue(int _facevalue)
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (tileGridArray[x, y].faceValue == _facevalue)
                    {
                        return tileGridArray[x, y];
                    }
                }
            }

            return null;
        }

        public bool Equals(TileGrid other)
        {          

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (tileGridArray[x,y].faceValue != other.tileGridArray[x,y].faceValue)
                    {
                        return false;                        
                    }
                }
            }

            return true;
        }
    }
}
