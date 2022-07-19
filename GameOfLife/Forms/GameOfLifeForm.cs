using GameOfLife.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static GameOfLife.Utilities.Utilities;

namespace GameOfLife.Forms
{
    public partial class GameofLifeForm : Form
    {
        // The universe array
        private HashSet<Cell> universe;

        // The Timer class
        private readonly Timer timer = new Timer();

        // Generation count
        private int generations = 0;


        public GameofLifeForm()
        {
            InitializeComponent();

            LoadSettings();

            // Setup the timer
            timer.Interval = AppSettings.GenerationLength; // milliseconds
            timer.Tick += Timer_Tick;
            timer.Enabled = false;

            universe = new HashSet<Cell>();

            showGridToolStripMenuItem.Checked = AppSettings.DrawGrid;
            showNeighborCountToolStripMenuItem.Checked = AppSettings.ShowNeighbors;
            showCellsAliveToolStripMenuItem.Checked = AppSettings.ShowCellsAlive;
            cellsAliveStatusLabel.Visible = AppSettings.ShowCellsAlive;

            randomizeToolStripMenuItem.PerformClick();
        }

        // The event called by the timer every Interval milliseconds.
        private void Timer_Tick(object sender, EventArgs e)
        {
            universe = new HashSet<Cell>(NextGeneration(universe));

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
            var cellWidth = (float)graphicsPanel.ClientSize.Width / AppSettings.UniverseWidth;
            // CELL HEIGHT = WINDOW HEIGHT / NUMBER OF CELLS IN Y
            var cellHeight = (float)graphicsPanel.ClientSize.Height / AppSettings.UniverseHeight;

            // A Pen for drawing the grid lines (color, width)
            var gridPen = new Pen(AppSettings.GridColor, 1);

            // A Brush for filling living cells interiors (color)
            Brush cellBrush = new SolidBrush(AppSettings.CellColor);

            // Iterate through the universe in the y, top to bottom
            universe.ToList().ForEach((Cell cell) =>
            {
                var x = cell.X;
                var y = cell.Y;
                // A rectangle to represent each cell in pixels
                RectangleF cellRect = Rectangle.Empty;
                cellRect.X = x * cellWidth;
                cellRect.Y = y * cellHeight;
                cellRect.Width = cellWidth;
                cellRect.Height = cellHeight;

                // Fill the cell with a brush if alive
                if(cell.IsAlive)
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
                        cell.IsAlive ?
                        new SolidBrush(Color.Green)
                        : new SolidBrush(Color.Red);

                    cell.AliveNeighbors = AppSettings.Toroidal ? CountNeighborsToroidal(x, y, universe) : CountNeighbors(x, y, universe);

                    e.Graphics.DrawString(cell.AliveNeighbors.ToString(), font, neighborBrush, cellRect, format);

                    neighborBrush.Dispose();
                    font.Dispose();
                    format.Dispose();
                }

                // Outline the cell with a pen
                if(AppSettings.DrawGrid)
                {
                    e.Graphics.DrawRectangle(gridPen, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);
                }
            });

            if(AppSettings.ShowCellsAlive)
            {
                UpdateCellsAliveLabel(cellsAliveStatusLabel, universe.Where(x => x.IsAlive).Count());
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
                var cellWidth = (float)graphicsPanel.ClientSize.Width / AppSettings.UniverseWidth;
                var cellHeight = (float)graphicsPanel.ClientSize.Height / AppSettings.UniverseHeight;

                // Calculate the cell that was clicked in
                // CELL X = MOUSE X / CELL WIDTH
                var x = (int)(e.X / cellWidth);
                // CELL Y = MOUSE Y / CELL HEIGHT
                var y = (int)(e.Y / cellHeight);

                // Toggle the cell's state
                var cell = universe.First(c => c.X == x && c.Y == y);
                universe.RemoveWhere(c => c.X == x && c.Y == y);

                cell.IsAlive = !cell.IsAlive;

                universe.Add(cell);

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
            universe = new HashSet<Cell>(NextGeneration(universe));

            // Increment generation count
            generations++;

            // Update status strip generations
            UpdateGenerationLabel(toolStripStatusLabelGenerations, ref generations);

            graphicsPanel.Invalidate();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;

            universe = new HashSet<Cell>();

            for(var y = 0; y < AppSettings.UniverseHeight; ++y)
            {
                for(var x = 0; x < AppSettings.UniverseWidth; ++x)
                {
                    universe.Add(new Cell { IsAlive = false, X = x, Y = y, AliveNeighbors = 0 });
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

            universe = new HashSet<Cell>();

            for(var y = 0; y < AppSettings.UniverseHeight; ++y)
            {
                for(var x = 0; x < AppSettings.UniverseWidth; ++x)
                {
                    universe.Add(
                        new Cell
                        {
                            X = x, Y = y,
                            IsAlive = Convert.ToBoolean(rand.Next(0, 2))
                        });
                }
            }

            generations = 0;
            UpdateGenerationLabel(toolStripStatusLabelGenerations, ref generations);

            if(AppSettings.ShowCellsAlive)
            {
                UpdateCellsAliveLabel(cellsAliveStatusLabel, universe.Where(x => x.IsAlive == true).Count());
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
                UniverseWidth = AppSettings.UniverseWidth,
                UniverseHeight = AppSettings.UniverseHeight
            })
            {
                if(dialog.ShowDialog() == DialogResult.OK)
                {
                    timer.Enabled = false;

                    AppSettings.UniverseWidth = dialog.UniverseWidth;
                    AppSettings.UniverseHeight = dialog.UniverseHeight;

                    universe = new HashSet<Cell>();

                    for(var y = 0; y < AppSettings.UniverseHeight; ++y)
                    {
                        for(var x = 0; x < AppSettings.UniverseWidth; ++x)
                        {
                            universe.Add(new Cell
                            {
                                IsAlive = false,
                                AliveNeighbors = 0,
                                X = x,
                                Y = y
                            });
                        }
                    }

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
                    AppSettings.GenerationLength = dialog.GenerationLength;
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

        private void changeGridColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using(var dialog = new ColorDialog())
            {
                if(dialog.ShowDialog() == DialogResult.OK)
                {
                    AppSettings.GridColor = dialog.Color;
                    graphicsPanel.Invalidate();
                }
            }
        }

        private void changeCellColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using(var dialog = new ColorDialog())
            {
                if(dialog.ShowDialog() == DialogResult.OK)
                {
                    AppSettings.CellColor = dialog.Color;
                    graphicsPanel.Invalidate();
                }
            }
        }
    }
}
