using System.Windows.Forms;

// Change the namespace to your project's namespace.
namespace GameOfLife.Forms
{
    internal class GraphicsPanel : Panel
    {
        // Default constructor
        public GraphicsPanel()
        {
            // Turn on double buffering.
            DoubleBuffered = true;

            // Allow repainting when the window is resized.
            SetStyle(ControlStyles.ResizeRedraw, true);
        }
    }
}
