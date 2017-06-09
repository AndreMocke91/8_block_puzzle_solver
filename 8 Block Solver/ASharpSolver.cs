using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace _8_Block_Solver
{
    class ASharpSolver
    {
        public TileGrid tileGrid { get; set; }
        List<TargetCoordinate> targetCoordinateList;

        public ASharpSolver()
        {
            tileGrid = new TileGrid();
            targetCoordinateList = new List<TargetCoordinate>();
        }

        public ASharpSolver(TileGrid _tileGrid, List<TargetCoordinate> _targetCoordinateList)
        {
            tileGrid = _tileGrid;
            targetCoordinateList = _targetCoordinateList;
            Console.WriteLine("Current Heuristic is : " + GetHeuristic(tileGrid));
        }

        public List<BoardState> SeekSolution()
        {
            List<BoardState> openBoardStates = new List<BoardState>();
            List<BoardState> closedBoardStates = new List<BoardState>();
            List<BoardState> possibleNewStates = new List<BoardState>();
            bool solutionFound = false;

            BoardState currentState = new BoardState(tileGrid, null, GetHeuristic(tileGrid));

            if (!CheckSolvability(currentState.tileGridState))
                return null;

            openBoardStates.Add(currentState);

            do
            {
                currentState = openBoardStates.OrderBy(z => z.heuristic).FirstOrDefault();               
                closedBoardStates.Add(currentState);
                openBoardStates.Remove(currentState);

                if (closedBoardStates.Where(x => x.heuristic == 0).FirstOrDefault() != null)
                {
                    solutionFound = true;                   
                    break;
                }

                possibleNewStates = possibleStates(currentState);

                foreach(BoardState possibleNewState in possibleNewStates)
                {
                    BoardState openListMatch = openBoardStates.Where(x => x.tileGridState.Equals(possibleNewState.tileGridState)).FirstOrDefault();
                    if( openListMatch == null)
                    {
                        BoardState closedListMatch = closedBoardStates.Where(x => x.tileGridState.Equals(possibleNewState.tileGridState)).FirstOrDefault();
                        if (closedListMatch == null)
                        {
                            openBoardStates.Add(possibleNewState);
                        } else
                        {
                            //Console.WriteLine("Found in closed list");
                        }
                    } else
                    {
                        //Console.WriteLine("Found in open list");
                    } 
                }

            }
            while (openBoardStates.Count > 0 && closedBoardStates.Count < 3000);

            if (solutionFound)
            {

                Console.WriteLine(currentState);

                List<BoardState> solution = new List<BoardState>();

                while (currentState.predecessor != null)
                {
                    solution.Add(currentState);
                    currentState = currentState.predecessor;
                }

                Console.WriteLine("Found solution in : " + solution.Count + " steps");
                return solution;
            }
            
            return null;
        }

        private List<BoardState> possibleStates(BoardState boardState)
        {
            List<BoardState> tempPossibleStates = new List<BoardState>();
            BoardState predecessor = new BoardState(boardState.tileGridState, boardState.predecessor, boardState.heuristic);
            Tile currentBlankTile = boardState.tileGridState.GetBlankTile();

            // blank tile can move left
            if (currentBlankTile.xCoordinate > 0)
            {
                // Get the tile directly to the left of the blank
                Tile designatedTile = boardState.tileGridState.tileGridArray[currentBlankTile.xCoordinate - 1, currentBlankTile.yCoordinate];
                AddStateToTemplist(tempPossibleStates, designatedTile, boardState);
               
            }

            // blank tile can move right
            if (currentBlankTile.xCoordinate < 2)
            {
                // Get the tile directly to the right of the blank
                Tile designatedTile = boardState.tileGridState.tileGridArray[currentBlankTile.xCoordinate + 1, currentBlankTile.yCoordinate];
                AddStateToTemplist(tempPossibleStates, designatedTile, boardState);
            }

            // blank tile can move up
            if (currentBlankTile.yCoordinate > 0)
            {
                // Get the tile directly below of the blank
                Tile designatedTile = boardState.tileGridState.tileGridArray[currentBlankTile.xCoordinate, currentBlankTile.yCoordinate - 1];
                AddStateToTemplist(tempPossibleStates, designatedTile, boardState);
            }

            // blank tile can move down
            if (currentBlankTile.yCoordinate < 2)
            {
                // Get the tile directly below of the blank
                Tile designatedTile = boardState.tileGridState.tileGridArray[currentBlankTile.xCoordinate, currentBlankTile.yCoordinate + 1];
                AddStateToTemplist(tempPossibleStates, designatedTile, boardState);
            }
            return tempPossibleStates;
        }

        private void AddStateToTemplist(List<BoardState> tempPossibleStates, Tile designatedTile, BoardState predecessor)
        {
            int faceValue = designatedTile.faceValue;
            TileGrid newBoardTileGrid = DeepClone<TileGrid>( predecessor.tileGridState );

            // Create new grid with swapped tiles
            newBoardTileGrid.SwapTileWithBlank(faceValue);

            // Add new state to templist
            BoardState newBoardState = new BoardState(newBoardTileGrid, predecessor, GetHeuristic(newBoardTileGrid));
            tempPossibleStates.Add(newBoardState);                     
        }

        private int GetHeuristic(TileGrid tileGridState)
        {
            // Currently using plain Manhattan Distance - should be improved
            int heuristic = 0;
            int targetFaceValue = 0;

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    targetFaceValue = tileGridState.tileGridArray[x, y].faceValue;
                    TargetCoordinate targetCoordinate = targetCoordinateList.Where(z => z.faceValue == targetFaceValue).FirstOrDefault();

                    heuristic = heuristic + Math.Abs(targetCoordinate.xCoordinate - tileGridState.tileGridArray[x, y].xCoordinate);
                    heuristic = heuristic + Math.Abs(targetCoordinate.yCoordinate - tileGridState.tileGridArray[x, y].yCoordinate);
                }
            }

            return heuristic;
        }

        public static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }

        public bool CheckSolvability(TileGrid boardState)
        {
            int inversions = 0;
            List<int> linearList = new List<int>();

            for (int y = 0; y < 3; y++)                
            {
                for (int x = 0; x < 3; x++)
                {
                    if(boardState.tileGridArray[x,y].faceValue != 0)
                    {
                        linearList.Add(boardState.tileGridArray[x, y].faceValue);
                    }
                }
            }

            for(int i = 0; i < linearList.Count; i++)
            {
                for(int j = i+1; j < linearList.Count; j++)
                {
                    if (linearList[j] > linearList[i])
                        inversions++;
                }
            }

            if (inversions % 2 == 1)
            {
                Console.WriteLine("It is unsolvable with an inversion count of: " + inversions.ToString());
                return false;
            }
            
                return true;
        }
    }
}
