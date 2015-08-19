namespace My_Note
{
    partial class RenameSubjectForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RenameSubjectForm));
            this.renameLabel = new System.Windows.Forms.Label();
            this.renameTextBox = new System.Windows.Forms.TextBox();
            this.renameOKButton = new System.Windows.Forms.Button();
            this.renameCancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // renameLabel
            // 
            this.renameLabel.AutoSize = true;
            this.renameLabel.Location = new System.Drawing.Point(12, 9);
            this.renameLabel.Name = "renameLabel";
            this.renameLabel.Size = new System.Drawing.Size(213, 13);
            this.renameLabel.TabIndex = 0;
            this.renameLabel.Text = "Enter Name of Subject (12 chacacters max)";
            // 
            // renameTextBox
            // 
            this.renameTextBox.Location = new System.Drawing.Point(15, 25);
            this.renameTextBox.MaxLength = 12;
            this.renameTextBox.Name = "renameTextBox";
            this.renameTextBox.Size = new System.Drawing.Size(210, 20);
            this.renameTextBox.TabIndex = 1;
            this.renameTextBox.TextChanged += new System.EventHandler(this.renameTextBox_TextChanged);
            this.renameTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.renameTextBox_KeyPress);
            // 
            // renameOKButton
            // 
            this.renameOKButton.Location = new System.Drawing.Point(15, 51);
            this.renameOKButton.Name = "renameOKButton";
            this.renameOKButton.Size = new System.Drawing.Size(75, 23);
            this.renameOKButton.TabIndex = 2;
            this.renameOKButton.Text = "OK";
            this.renameOKButton.UseVisualStyleBackColor = true;
            this.renameOKButton.Click += new System.EventHandler(this.renameOKButton_Click);
            // 
            // renameCancelButton
            // 
            this.renameCancelButton.Location = new System.Drawing.Point(150, 51);
            this.renameCancelButton.Name = "renameCancelButton";
            this.renameCancelButton.Size = new System.Drawing.Size(75, 23);
            this.renameCancelButton.TabIndex = 3;
            this.renameCancelButton.Text = "Cancel";
            this.renameCancelButton.UseVisualStyleBackColor = true;
            this.renameCancelButton.Click += new System.EventHandler(this.renameCancelButton_Click);
            // 
            // RenameSubjectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(237, 86);
            this.Controls.Add(this.renameCancelButton);
            this.Controls.Add(this.renameOKButton);
            this.Controls.Add(this.renameTextBox);
            this.Controls.Add(this.renameLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RenameSubjectForm";
            this.Text = "Rename Subject";
            this.Load += new System.EventHandler(this.RenameSubjectForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label renameLabel;
        private System.Windows.Forms.TextBox renameTextBox;
        private System.Windows.Forms.Button renameOKButton;
        private System.Windows.Forms.Button renameCancelButton;
    }
}