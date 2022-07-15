using GameOfLife.Data;
using System;
using System.Drawing;
using System.Windows.Forms;
using static GameOfLife.Utilities.Utilities;

namespace GameOfLife.Forms
{
    public partial class GameofLifeForm : Form
    {
        // The universe array
        private bool[,] universe;

        // Drawing colors
        private readonly Color gridColor = Color.Black;
        private readonly Color cellColor = Color.Gray;

        // The Timer class
        private readonly Timer timer = new Timer();

        // Generation count
        private int generations = 0;


        public GameofLifeForm()
        {
            InitializeComponent();

            // Setup the timer
            timer.Interval = 100; // milliseconds
            timer.Tick += Timer_Tick;
            timer.Enabled = false;

            LoadSettings();

            universe = new bool[AppSettings.UniverseWidth, AppSettings.UniverseHeight];

            showGridToolStripMenuItem.Checked = AppSettings.DrawGrid;
            showNeighborCountToolStripMenuItem.Checked = AppSettings.ShowNeighbors;
            showCellsAliveToolStripMenuItem.Checked = AppSettings.ShowCellsAlive;
            cellsAliveStatusLabel.Visible = AppSettings.ShowCellsAlive;

            randomizeToolStripMenuItem.PerformClick();
        }

        // The event called by the timer every Interval milliseconds.
        private void Timer_Tick(object sender, EventArgs e)
        {
            universe = NextGeneration(universe);

            // Increment generation count
            generations++;

            // Update status strip generations
            UpdateGenerationLabel(toolStripStatusLabelGenerations, ref generations);

            graphicsPanel.Invalidate();
        }

        private void graphicsPanel_Paint(object sender, PaintEventArgs e)
        {
            // Calculate the width and height of each cell in pixels
            // CELL WIDTH = WINDOW WIDTH / NUMBER OF CELLS IN X
            var cellWidth = (float)graphicsPanel.ClientSize.Width / universe.GetLength(0);
            // CELL HEIGHT = WINDOW HEIGHT / NUMBER OF CELLS IN Y
            var cellHeight = (float)graphicsPanel.ClientSize.Height / universe.GetLength(1);

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
                    if(AppSettings.ShowNeighbors)
                    {
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

                        var neighbors = AppSettings.Toroidal ? CountNeighborsToroidal(x, y, universe) : CountNeighbors(x, y, universe);

                        e.Graphics.DrawString(neighbors.ToString(), font, neighborBrush, cellRect, format);

                        neighborBrush.Dispose();
                        font.Dispose();
                        format.Dispose();
                    }

                    // Outline the cell with a pen
                    if(AppSettings.DrawGrid)
                    {
                        e.Graphics.DrawRectangle(gridPen, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);
                    }
                }
            }

            if(AppSettings.ShowCellsAlive)
            {
                UpdateCellsAliveLabel(cellsAliveStatusLabel, universe);
            }

            // Cleaning up pens and brushes
            gridPen.Dispose();
            cellBrush.Dispose();
        }

        private void graphicsPanel_MouseClick(object sender, MouseEventArgs e)
        {
            // If the left mouse button was clicked
            if(e.Button == MouseButtons.Left)
            {
                // Calculate the width and height of each cell in pixels
                var cellWidth = (float)graphicsPanel.ClientSize.Width / universe.GetLength(0);
                var cellHeight = (float)graphicsPanel.ClientSize.Height / universe.GetLength(1);

                // Calculate the cell that was clicked in
                // CELL X = MOUSE X / CELL WIDTH
                var x = (int)(e.X / cellWidth);
                // CELL Y = MOUSE Y / CELL HEIGHT
                var y = (int)(e.Y / cellHeight);

                // Toggle the cell's state
                universe[x, y] = !universe[x, y];

                // Tell Windows you need to repaint
                graphicsPanel.Invalidate();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void PlayPauseButton_Click(object sender, EventArgs e)
        {
            timer.Enabled = !timer.Enabled;
        }

        private void nextGenerationButton_Click(object sender, EventArgs e)
        {
            universe = NextGeneration(universe);

            // Increment generation count
            generations++;

            // Update status strip generations
            UpdateGenerationLabel(toolStripStatusLabelGenerations, ref generations);

            graphicsPanel.Invalidate();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;

            universe = new bool[AppSettings.UniverseWidth, AppSettings.UniverseHeight];

            for(var y = 0; y < universe.GetLength(1); ++y)
            {
                for(var x = 0; x < universe.GetLength(0); ++x)
                {
                    universe[x, y] = false;
                }
            }

            generations = 0;
            UpdateGenerationLabel(toolStripStatusLabelGenerations, ref generations);

            if(AppSettings.ShowCellsAlive)
            {
                cellsAliveStatusLabel.Text = "Cells Alive = 0";
            }

            graphicsPanel.Invalidate();
        }

        private void randomizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var rand = new Random();
            timer.Enabled = false;

            universe = new bool[AppSettings.UniverseWidth, AppSettings.UniverseHeight];

            for(var y = 0; y < universe.GetLength(1); ++y)
            {
                for(var x = 0; x < universe.GetLength(0); ++x)
                {
                    universe[x, y] = Convert.ToBoolean(rand.Next(0, 2));
                }
            }

            generations = 0;
            UpdateGenerationLabel(toolStripStatusLabelGenerations, ref generations);

            if(AppSettings.ShowCellsAlive)
            {
                UpdateCellsAliveLabel(cellsAliveStatusLabel, universe);
            }


            graphicsPanel.Invalidate();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
            universe = ReadCellsFile();
            generations = 0;
            UpdateGenerationLabel(toolStripStatusLabelGenerations, ref generations);
            graphicsPanel.Invalidate();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
            Save(universe);
        }

        private void resizeUniverseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using(var dialog = new ResizeUniverseDialog
            {
                UniverseWidth = universe.GetLength(0),
                UniverseHeight = universe.GetLength(1)
            })
            {
                if(dialog.ShowDialog() == DialogResult.OK)
                {
                    timer.Enabled = false;

                    AppSettings.UniverseWidth = dialog.UniverseWidth;
                    AppSettings.UniverseHeight = dialog.UniverseHeight;

                    universe = new bool[AppSettings.UniverseWidth, AppSettings.UniverseHeight];

                    generations = 0;
                    UpdateGenerationLabel(toolStripStatusLabelGenerations, ref generations);

                    cellsAliveStatusLabel.Text = "Cells Alive = 0";

                    graphicsPanel.Invalidate();
                }
            }
        }

        private void toroidalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(!toroidalToolStripMenuItem.Checked)
            {
                AppSettings.Toroidal = true;
                toroidalToolStripMenuItem.Checked = true;
            }
            else
            {
                AppSettings.Toroidal = false;
                toroidalToolStripMenuItem.Checked = false;
            }
            graphicsPanel.Invalidate();
        }

        private void changeGenerationLengthToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;

            using(var dialog = new ChangeGenerationLengthDialog
            {
                GenerationLength = timer.Interval
            })
            {
                if(dialog.ShowDialog() == DialogResult.OK)
                {
                    timer.Interval = dialog.GenerationLength;
                }
            }

        }

        private void showGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(showGridToolStripMenuItem.Checked)
            {
                showGridToolStripMenuItem.Checked = false;
                AppSettings.DrawGrid = false;
            }
            else
            {
                showGridToolStripMenuItem.Checked = true;
                AppSettings.DrawGrid = true;
            }

            graphicsPanel.Invalidate();
        }

        private void showNeighborCountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(showNeighborCountToolStripMenuItem.Checked)
            {
                showNeighborCountToolStripMenuItem.Checked = false;
                AppSettings.ShowNeighbors = false;
            }
            else
            {
                showNeighborCountToolStripMenuItem.Checked = true;
                AppSettings.ShowNeighbors = true;
            }

            graphicsPanel.Invalidate();
        }

        private void showCellsAliveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(showCellsAliveToolStripMenuItem.Checked)
            {
                showCellsAliveToolStripMenuItem.Checked = false;
                AppSettings.ShowCellsAlive = false;
            }
            else
            {
                showCellsAliveToolStripMenuItem.Checked = true;
                AppSettings.ShowCellsAlive = true;
            }

            cellsAliveStatusLabel.Visible = AppSettings.ShowCellsAlive;
            graphicsPanel.Invalidate();
        }

        private void GameofLifeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }
    }
}
