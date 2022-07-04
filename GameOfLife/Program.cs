using GameOfLife.Forms;
using System;
using System.Windows.Forms;

namespace GameOfLife
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GameofLifeForm());
        }
    }
}
