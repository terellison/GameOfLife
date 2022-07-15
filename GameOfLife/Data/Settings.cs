using System.Drawing;

namespace GameOfLife.Data
{
    internal struct Settings
    {
        public bool DrawGrid;
        public bool ShowNeighbors;
        public bool ShowCellsAlive;
        public Color CellColor;
        public Color GridColor;
        public bool Toroidal;
        public int UniverseWidth;
        public int UniverseHeight;
    }
}
