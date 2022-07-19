namespace GameOfLife.Data
{
    internal struct Cell
    {
        public int X;
        public int Y;
        public int AliveNeighbors;
        public bool IsAlive;
    }
}
