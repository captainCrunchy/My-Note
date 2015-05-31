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
            this.HelpOKButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // HelpOKButton
            // 
            this.HelpOKButton.Location = new System.Drawing.Point(198, 453);
            this.HelpOKButton.Name = "HelpOKButton";
            this.HelpOKButton.Size = new System.Drawing.Size(75, 23);
            this.HelpOKButton.TabIndex = 0;
            this.HelpOKButton.Text = "OK";
            this.HelpOKButton.UseVisualStyleBackColor = true;
            this.HelpOKButton.Click += new System.EventHandler(this.HelpOKButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(155, 83);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "My Note Help";
            // 
            // HelpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(477, 488);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.HelpOKButton);
            this.Name = "HelpForm";
            this.Text = "My Note Help";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button HelpOKButton;
        private System.Windows.Forms.Label label1;
    }
}