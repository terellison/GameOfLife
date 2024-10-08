﻿using GameOfLife.Data;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace GameOfLife.Utilities
{
    internal static class Utilities
    {
        public static Settings AppSettings;

        public static bool[,] ReadCellsFile()
        {
            // Open file fialog box
            var fileContents = new List<string>();

            using(var dialog = new OpenFileDialog())
            {
                dialog.Filter = "CELLS files (*.CELLS)|";
                dialog.FilterIndex = 0;
                dialog.RestoreDirectory = true;

                if(dialog.ShowDialog() == DialogResult.OK)
                {
                    var fileStream = dialog.OpenFile();

                    using(var reader = new StreamReader(fileStream))
                    {
                        var i = 0;

                        while(!reader.EndOfStream)
                        {
                            fileContents.Add(reader.ReadLine());
                            ++i;
                        }
                        reader.Close();
                    }
                    fileStream.Dispose();
                }
            }

            var data = ProcessFileContents(fileContents);

            return data;
        }

        public static void Save(bool[,] universe)
        {
            var dialog = new SaveFileDialog
            {
                Filter = "CELLS files (*.CELLS)|",
                FilterIndex = 0,
                DefaultExt = "cells"
            };

            if(dialog.ShowDialog() == DialogResult.OK)
            {
                var writer = new StreamWriter(dialog.FileName);

                for(var y = 0; y < AppSettings.UniverseHeight; ++y)
                {
                    var line = string.Empty;

                    for(var x = 0; x < AppSettings.UniverseWidth; ++x)
                    {
                        line += universe[x, y] ? 'O' : '.';
                    }

                    writer.WriteLine(line);
                }
                writer.Close();
            }
        }

        private static bool[,] ProcessFileContents(List<string> fileContents)
        {
            var height = 0;
            var width = 0;
            foreach(var line in fileContents)
            {
                if(line[0] != '!')
                {
                    height += 1;

                    if(width < line.Length)
                    {
                        width = line.Length;
                    }
                }
            }
            AppSettings.UniverseWidth = width;
            AppSettings.UniverseHeight = height;

            var data = new bool[AppSettings.UniverseWidth, AppSettings.UniverseHeight];

            for(var y = 0; y < data.GetLength(1); ++y)
            {
                var line = fileContents[y];

                for(var x = 0; x < data.GetLength(0); ++x)
                {
                    data[x, y] = line[x] != '.';
                }
            }

            return data;
        }

        // Calculate the next generation of cells
        public static bool[,] NextGeneration(bool[,] universe)
        {
            var scratchpad = (bool[,])universe.Clone();
            for(var y = 0; y < AppSettings.UniverseHeight; ++y)
            {
                for(var x = 0; x < AppSettings.UniverseWidth; ++x)
                {
                    var cell = universe[x, y];

                    var neighbors = AppSettings.Toroidal ? CountNeighborsToroidal(x, y, universe) : CountNeighbors(x, y, universe);

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

            return scratchpad;
        }

        public static int CountNeighbors(int x, int y, bool[,] universe)
        {
            var count = 0;
            var xLength = AppSettings.UniverseWidth;
            var yLength = AppSettings.UniverseHeight;

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

        public static int CountNeighborsToroidal(int x, int y, bool[,] universe)
        {
            var count = 0;
            var xLength = AppSettings.UniverseWidth;
            var yLength = AppSettings.UniverseHeight;

            for(var yOffset = -1; yOffset <= 1; ++yOffset)
            {
                for(var xOffset = -1; xOffset <= 1; ++xOffset)
                {
                    var xCheck = x + xOffset;
                    var yCheck = y + yOffset;

                    if(xOffset == 0 && yOffset == 0)
                    {
                        continue;
                    }
                    else
                    {
                        if(xCheck < 0)
                        {
                            xCheck = xLength - 1;
                        }

                        if(yCheck < 0)
                        {
                            yCheck = yLength - 1;
                        }

                        if(xCheck >= xLength)
                        {
                            xCheck = 0;
                        }

                        if(yCheck >= yLength)
                        {
                            yCheck = 0;
                        }

                        if(universe[xCheck, yCheck] == true)
                        {
                            ++count;
                        }
                    }
                }
            }
            return count;
        }

        public static void UpdateGenerationLabel(ToolStripStatusLabel toolStripStatusLabelGenerations, ref int generations)
        {
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
        }

        public static void UpdateCellsAliveLabel(ToolStripStatusLabel toolStripStatusLabelCellsAlive, bool[,] universe)
        {
            var count = 0;

            for(var y = 0; y < AppSettings.UniverseHeight; ++y)
            {
                for(var x = 0; x < AppSettings.UniverseWidth; ++x)
                {
                    if(universe[x, y])
                    {
                        ++count;
                    }
                }
            }

            toolStripStatusLabelCellsAlive.Text = $"Cells Alive = {count}";
        }

        public static void LoadSettings()
        {
            AppSettings = new Settings
            {
                DrawGrid = Properties.Settings.Default.DrawGrid,
                ShowCellsAlive = Properties.Settings.Default.ShowCellsAlive,
                CellColor = Properties.Settings.Default.CellColor,
                GridColor = Properties.Settings.Default.GridColor,
                ShowNeighbors = Properties.Settings.Default.ShowNeighbors,
                Toroidal = Properties.Settings.Default.Toroidal,
                UniverseHeight = Properties.Settings.Default.UniverseHeight,
                UniverseWidth = Properties.Settings.Default.UniverseWidth,
                GenerationLength = Properties.Settings.Default.GenerationLength
            };
        }

        public static void SaveSettings()
        {
            Properties.Settings.Default.DrawGrid = AppSettings.DrawGrid;
            Properties.Settings.Default.ShowCellsAlive = AppSettings.ShowCellsAlive;
            Properties.Settings.Default.GridColor = AppSettings.GridColor;
            Properties.Settings.Default.CellColor = AppSettings.CellColor;
            Properties.Settings.Default.ShowNeighbors = AppSettings.ShowNeighbors;
            Properties.Settings.Default.Toroidal = AppSettings.Toroidal;
            Properties.Settings.Default.UniverseHeight = AppSettings.UniverseHeight;
            Properties.Settings.Default.UniverseWidth = AppSettings.UniverseWidth;
            Properties.Settings.Default.GenerationLength = AppSettings.GenerationLength;

            Properties.Settings.Default.Save();
        }
    }
}
