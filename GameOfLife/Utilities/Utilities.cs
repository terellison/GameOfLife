using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace GameOfLife.Utilities
{
    internal static class Utilities
    {
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

                for(var y = 0; y < universe.GetLength(1); ++y)
                {
                    var line = string.Empty;

                    for(var x = 0; x < universe.GetLength(0); ++x)
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

            var data = new bool[width, height];

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
    }
}
