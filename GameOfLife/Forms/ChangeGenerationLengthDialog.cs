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
    public partial class ChangeGenerationLengthDialog : Form
    {
        private int genLength = 0;

        public int GenerationLength
        {
            get => genLength;

            set
            {
                if(value <= 0)
                {
                    genLength = 1;
                }
                else
                {
                    genLength = value;
                }

                newLengthTextBox.Text = genLength.ToString();
            }
        }

        public ChangeGenerationLengthDialog()
        {
            InitializeComponent();
        }

        private void ChangeGenerationLengthDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(DialogResult == DialogResult.OK)
            {
                try
                {
                    GenerationLength = Convert.ToInt32(newLengthTextBox.Text);
                }
                catch(Exception)
                {
                    MessageBox.Show("Invalid input for new generation length. Try again.");
                }
            }
        }
    }
}
