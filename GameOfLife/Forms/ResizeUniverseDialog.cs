using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameOfLife.Forms
{
    public partial class ResizeUniverseDialog : Form
    {
        private int height = 0;

        private int width = 0;
        public int UniverseHeight
        {
            get => height;
            set
            {
                if(value < 0) height = 5;
                else height = value;

                heightTextBox.Text = height.ToString();
            }
        }

        public int UniverseWidth
        {
            get => width;
            set
            {
                if(value < 0) width = 5;
                else width = value;

                widthTextbox.Text = width.ToString();
            }
        }

        public ResizeUniverseDialog()
        {
            InitializeComponent();
        }

        private void ResizeUniverseDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(DialogResult == DialogResult.OK)
            {
                try
                {
                    UniverseWidth = Convert.ToInt32(widthTextbox.Text);
                    UniverseHeight = Convert.ToInt32(heightTextBox.Text);
                }
                catch(Exception)
                {
                    MessageBox.Show("Invalid input for one or more numerical inputs. Try again.");
                }
            }
        }
    }
}
