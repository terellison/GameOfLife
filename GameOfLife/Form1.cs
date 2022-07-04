using System;
using System.Drawing;
using System.Windows.Forms;

namespace GameOfLife
{
    public partial class Form1 : Form
    {
        // The universe array
        private bool[,] universe = new bool[50, 50];

        // Drawing colors
        private readonly Color gridColor = Color.Black;
        private readonly Color cellColor = Color.Gray;

        // The Timer class
        private readonly Timer timer = new Timer();

        // Generation count
        private int generations = 0;

        public Form1()
        {
            InitializeComponent();

            // Setup the timer
            timer.Interval = 100; // milliseconds
            timer.Tick += Timer_Tick;
            timer.Enabled = false;
        }

        // Calculate the next generation of cells
        private void NextGeneration()
        {
            var scratchpad = (bool[,])universe.Clone();
            for(var y = 0; y < universe.GetLength(1); ++y)
            {
                for(var x = 0; x < universe.GetLength(0); ++x)
                {
                    var cell = universe[x, y];

                    var neighbors = CountNeighbors(x, y);

                    if(cell && neighbors < 2)
                    {
                        scratchpad[x, y] = false;
                    }

                    else if(cell && neighbors > 3)
                    {
                        scratchpad[x, y] = false;
                    }

                    else if(cell && (neighbors == 2 || neighbors == 3))
                    {
                        continue;
                    }

                    else if(cell == false && neighbors == 3)
                    {
                        scratchpad[x, y] = true;
                    }
                }
            }

            universe = scratchpad;

            // Increment generation count
            generations++;

            // Update status strip generations
            UpdateGenerationLabel();

            graphicsPanel1.Invalidate();
        }

        private void UpdateGenerationLabel()
        {
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
        }

        // The event called by the timer every Interval milliseconds.
        private void Timer_Tick(object sender, EventArgs e)
        {
            NextGeneration();
        }

        private void graphicsPanel1_Paint(object sender, PaintEventArgs e)
        {
            // Calculate the width and height of each cell in pixels
            // CELL WIDTH = WINDOW WIDTH / NUMBER OF CELLS IN X
            var cellWidth = (float)graphicsPanel1.ClientSize.Width / universe.GetLength(0);
            // CELL HEIGHT = WINDOW HEIGHT / NUMBER OF CELLS IN Y
            var cellHeight = (float)graphicsPanel1.ClientSize.Height / universe.GetLength(1);

            // A Pen for drawing the grid lines (color, width)
            var gridPen = new Pen(gridColor, 1);

            // A Brush for filling living cells interiors (color)
            Brush cellBrush = new SolidBrush(cellColor);

            // Iterate through the universe in the y, top to bottom
            for(var y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for(var x = 0; x < universe.GetLength(0); x++)
                {
                    // A rectangle to represent each cell in pixels
                    RectangleF cellRect = Rectangle.Empty;
                    cellRect.X = x * cellWidth;
                    cellRect.Y = y * cellHeight;
                    cellRect.Width = cellWidth;
                    cellRect.Height = cellHeight;

                    // Fill the cell with a brush if alive
                    if(universe[x, y] == true)
                    {
                        e.Graphics.FillRectangle(cellBrush, cellRect);
                    }

                    var font = new Font("Arial", 10f);

                    var format = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };

                    var neighborBrush =
                        universe[x, y] ?
                        new SolidBrush(Color.Green)
                        : new SolidBrush(Color.Red);
                    var neighbors = CountNeighbors(x, y);

                    e.Graphics.DrawString(neighbors.ToString(), font, neighborBrush, cellRect, format);

                    // Outline the cell with a pen
                    e.Graphics.DrawRectangle(gridPen, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);

                    neighborBrush.Dispose();
                    font.Dispose();
                    format.Dispose();
                }
            }

            // Cleaning up pens and brushes
            gridPen.Dispose();
            cellBrush.Dispose();
        }

        private void graphicsPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            // If the left mouse button was clicked
            if(e.Button == MouseButtons.Left)
            {
                // Calculate the width and height of each cell in pixels
                var cellWidth = (float)graphicsPanel1.ClientSize.Width / universe.GetLength(0);
                var cellHeight = (float)graphicsPanel1.ClientSize.Height / universe.GetLength(1);

                // Calculate the cell that was clicked in
                // CELL X = MOUSE X / CELL WIDTH
                var x = (int)(e.X / cellWidth);
                // CELL Y = MOUSE Y / CELL HEIGHT
                var y = (int)(e.Y / cellHeight);

                // Toggle the cell's state
                universe[x, y] = !universe[x, y];

                // Tell Windows you need to repaint
                graphicsPanel1.Invalidate();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private int CountNeighbors(int x, int y)
        {
            var count = 0;
            var xLength = universe.GetLength(0);
            var yLength = universe.GetLength(1);

            for(var yOffset = -1; yOffset <= 1; ++yOffset)
            {
                for(var xOffset = -1; xOffset <= 1; ++xOffset)
                {
                    var xCheck = x + xOffset;
                    var yCheck = y + yOffset;

                    if
                    (
                        xCheck < 0
                        || yCheck < 0
                        || xCheck >= xLength
                        || yCheck >= yLength
                    )
                    { continue; }

                    else if(xCheck == x && yCheck == y) { continue; }

                    else if(universe[xCheck, yCheck] == true)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        private void PlayPauseButton_Click(object sender, EventArgs e)
        {
            timer.Enabled = !timer.Enabled;
        }

        private void nextGenerationButton_Click(object sender, EventArgs e)
        {
            NextGeneration();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;

            for(var y = 0; y < universe.GetLength(1); ++y)
            {
                for(var x = 0; x < universe.GetLength(0); ++x)
                {
                    universe[x, y] = false;
                }
            }
            generations = 0;
            UpdateGenerationLabel();
            graphicsPanel1.Invalidate();
        }

        private void randomizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var rand = new Random();
            timer.Enabled = false;

            for(var y = 0; y < universe.GetLength(1); ++y)
            {
                for(var x = 0; x < universe.GetLength(0); ++x)
                {
                    universe[x, y] = Convert.ToBoolean(rand.Next(0, 2));
                }
            }
            generations = 0;
            UpdateGenerationLabel();
            graphicsPanel1.Invalidate();
        }
    }
}
