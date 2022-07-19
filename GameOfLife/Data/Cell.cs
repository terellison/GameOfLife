namespace GameOfLife.Data
{
    internal struct Cell
    {
        public bool isAlive;
        public Point location;
        public int aliveNeighbors;
    }
}
