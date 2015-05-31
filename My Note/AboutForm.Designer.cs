namespace My_Note
{
    partial class AboutForm
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
            this.aboutOKButton = new System.Windows.Forms.Button();
            this.aboutTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // aboutOKButton
            // 
            this.aboutOKButton.Location = new System.Drawing.Point(160, 164);
            this.aboutOKButton.Name = "aboutOKButton";
            this.aboutOKButton.Size = new System.Drawing.Size(75, 23);
            this.aboutOKButton.TabIndex = 2;
            this.aboutOKButton.Text = "OK";
            this.aboutOKButton.UseVisualStyleBackColor = true;
            this.aboutOKButton.Click += new System.EventHandler(this.aboutOKButton_Click);
            // 
            // aboutTextBox
            // 
            this.aboutTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.aboutTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.aboutTextBox.Location = new System.Drawing.Point(12, 12);
            this.aboutTextBox.Multiline = true;
            this.aboutTextBox.Name = "aboutTextBox";
            this.aboutTextBox.ReadOnly = true;
            this.aboutTextBox.Size = new System.Drawing.Size(371, 146);
            this.aboutTextBox.TabIndex = 3;
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(395, 201);
            this.Controls.Add(this.aboutTextBox);
            this.Controls.Add(this.aboutOKButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.Text = "About My Note";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button aboutOKButton;
        private System.Windows.Forms.TextBox aboutTextBox;
    }
}