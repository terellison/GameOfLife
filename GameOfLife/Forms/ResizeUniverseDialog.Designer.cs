
namespace GameOfLife.Forms
{
    partial class ResizeUniverseDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Label widthLabel;
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.heightLabel = new System.Windows.Forms.Label();
            this.heightTextBox = new System.Windows.Forms.TextBox();
            this.widthTextbox = new System.Windows.Forms.TextBox();
            widthLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // widthLabel
            // 
            widthLabel.AutoSize = true;
            widthLabel.Location = new System.Drawing.Point(280, 147);
            widthLabel.Name = "widthLabel";
            widthLabel.Size = new System.Drawing.Size(54, 20);
            widthLabel.TabIndex = 4;
            widthLabel.Text = "Width:";
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(661, 235);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(107, 44);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(530, 235);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(107, 44);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // heightLabel
            // 
            this.heightLabel.AutoSize = true;
            this.heightLabel.Location = new System.Drawing.Point(280, 100);
            this.heightLabel.Name = "heightLabel";
            this.heightLabel.Size = new System.Drawing.Size(60, 20);
            this.heightLabel.TabIndex = 2;
            this.heightLabel.Text = "Height:";
            // 
            // heightTextBox
            // 
            this.heightTextBox.Location = new System.Drawing.Point(347, 100);
            this.heightTextBox.Name = "heightTextBox";
            this.heightTextBox.Size = new System.Drawing.Size(100, 26);
            this.heightTextBox.TabIndex = 3;
            this.heightTextBox.UseWaitCursor = true;
            // 
            // widthTextbox
            // 
            this.widthTextbox.Location = new System.Drawing.Point(347, 141);
            this.widthTextbox.Name = "widthTextbox";
            this.widthTextbox.Size = new System.Drawing.Size(100, 26);
            this.widthTextbox.TabIndex = 5;
            this.widthTextbox.UseWaitCursor = true;
            // 
            // ResizeUniverseForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(780, 291);
            this.Controls.Add(this.widthTextbox);
            this.Controls.Add(widthLabel);
            this.Controls.Add(this.heightTextBox);
            this.Controls.Add(this.heightLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ResizeUniverseForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Resize Universe";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ResizeUniverseDialog_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label heightLabel;
        private System.Windows.Forms.TextBox heightTextBox;
        private System.Windows.Forms.TextBox widthTextbox;
    }
}