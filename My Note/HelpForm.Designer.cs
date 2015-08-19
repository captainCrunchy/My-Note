namespace My_Note
{
    partial class HelpForm
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
            if (disposing && (components != null))
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HelpForm));
            this.HelpOKButton = new System.Windows.Forms.Button();
            this.helpPanel = new System.Windows.Forms.Panel();
            this.helpPictureBox = new System.Windows.Forms.PictureBox();
            this.helpPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.helpPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // HelpOKButton
            // 
            this.HelpOKButton.Location = new System.Drawing.Point(401, 682);
            this.HelpOKButton.Name = "HelpOKButton";
            this.HelpOKButton.Size = new System.Drawing.Size(119, 23);
            this.HelpOKButton.TabIndex = 0;
            this.HelpOKButton.Text = "Exit Help Menu";
            this.HelpOKButton.UseVisualStyleBackColor = true;
            this.HelpOKButton.Click += new System.EventHandler(this.HelpOKButton_Click);
            // 
            // helpPanel
            // 
            this.helpPanel.AutoScroll = true;
            this.helpPanel.Controls.Add(this.helpPictureBox);
            this.helpPanel.Location = new System.Drawing.Point(0, 0);
            this.helpPanel.Name = "helpPanel";
            this.helpPanel.Size = new System.Drawing.Size(921, 676);
            this.helpPanel.TabIndex = 1;
            // 
            // helpPictureBox
            // 
            this.helpPictureBox.Image = global::My_Note.Properties.Resources.Help_Picture;
            this.helpPictureBox.Location = new System.Drawing.Point(0, 0);
            this.helpPictureBox.Name = "helpPictureBox";
            this.helpPictureBox.Size = new System.Drawing.Size(904, 2549);
            this.helpPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.helpPictureBox.TabIndex = 0;
            this.helpPictureBox.TabStop = false;
            // 
            // HelpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(921, 717);
            this.Controls.Add(this.helpPanel);
            this.Controls.Add(this.HelpOKButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HelpForm";
            this.Text = "My Note Help";
            this.helpPanel.ResumeLayout(false);
            this.helpPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.helpPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button HelpOKButton;
        private System.Windows.Forms.Panel helpPanel;
        private System.Windows.Forms.PictureBox helpPictureBox;
    }
}