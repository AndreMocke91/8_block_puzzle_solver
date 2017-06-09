using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _8_Block_Solver
{
    public partial class Form1 : Form
    {
        public Random rnd = new Random();
        public TileGrid tileGrid;
        List<TargetCoordinate> targetCoordinatesList;

        public Form1()
        {
            InitializeComponent();
        }       

        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeTiles();
            BindTileGridToUI(tileGrid);
            AddMethodsToTileButtons(this);
            ConstructTargetCoordinates();
        }
        
        public void InitializeTiles()
        {
            
            String buttonName = "";
            int faceValueCounter = 0;
            tileGrid = new TileGrid();

            // Create ASharpSolverInstance to use checkSolvabilty function
            ASharpSolver solver = new ASharpSolver();

            tileGrid.faceValues = getRandomizedFaceValues(tileGrid.faceValues);
            
            // Iterate through 2d grid array to instantiate new tiles
            for(int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    buttonName = "tile" + x.ToString() + y.ToString();

                    // Instantiate new tile and add to the grid
                    Tile newTile = new Tile(tileGrid.faceValues[faceValueCounter], x, y, buttonName);
                    tileGrid.tileGridArray[x, y] = newTile;                   

                    faceValueCounter++;
                }
            }

            // Recall function if current grid is unsolvable : odd number of inversions
            if (!solver.CheckSolvability(tileGrid))
                InitializeTiles();

        }

        public void BindTileGridToUI(TileGrid _tileGrid)
        {
            int faceValueCounter = 0;
            String buttonName = "";

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    buttonName = _tileGrid.tileGridArray[x, y].boundButtonName;

                    // Bind tiles to UI                     
                    var tileButton = Controls.Find(buttonName, true)[0];
                    tileButton.Text = _tileGrid.tileGridArray[x, y].faceValue.ToString();

                    faceValueCounter++;

                }
            }

            this.Refresh();
        }

        public List<int> getRandomizedFaceValues(List<int> faceValues)
        {
            return faceValues.OrderBy(x => rnd.Next()).ToList();
        }

        public void AttemptSwap(Object sender, EventArgs e)
        {

            String senderName = ((Control)sender).Name;
           
            int xCoordinate = Int16.Parse(senderName[4].ToString());
            int yCoordinate = Int16.Parse(senderName[5].ToString());

            Tile prospectTile = tileGrid.tileGridArray[xCoordinate, yCoordinate];
            bool canSwap = tileGrid.IsAdjacentToBlank(tileGrid, prospectTile);

            if (canSwap)
            {
                tileGrid.SwapTileWithBlank(prospectTile.faceValue);
                BindTileGridToUI(tileGrid);
            }
        }
       
        public void AddMethodsToTileButtons(Control formControl)
        {
            foreach(Control control in formControl.Controls)
            {
                if(control.GetType() == typeof(Button))
                {
                    if (((Button)control).Name.Contains("tile"))
                    {
                        ((Button)control).Click += new EventHandler(AttemptSwap);
                    }
                }
            }
        }

        public void ConstructTargetCoordinates()
        {
            targetCoordinatesList = new List<TargetCoordinate>();

            for(int faceValue = 0; faceValue < 9; faceValue++)
            {
                targetCoordinatesList.Add(new TargetCoordinate(faceValue));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ASharpSolver aSharpSolver = new ASharpSolver(tileGrid, targetCoordinatesList);
            List<BoardState> solution = aSharpSolver.SeekSolution();            
            
            if (solution != null)
            {
                solution.Reverse();
                foreach (BoardState boardState in solution)
                {
                    System.Threading.Thread.Sleep(150);
                    BindTileGridToUI(boardState.tileGridState);
                }
            }
            else
            {
                MessageBox.Show("No Result found, reload please");               
            }
        }        

        private void button2_Click(object sender, EventArgs e)
        {
            InitializeTiles();
            BindTileGridToUI(tileGrid);
        }
    }
}
