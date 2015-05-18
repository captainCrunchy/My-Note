namespace My_Note
{
    partial class VertTextOptionsForm
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
            this.FontStyleLabel = new System.Windows.Forms.Label();
            this.FontStyleComboBox = new System.Windows.Forms.ComboBox();
            this.FontSizeComboBox = new System.Windows.Forms.ComboBox();
            this.FontSizeLabel = new System.Windows.Forms.Label();
            this.FontColorLabel = new System.Windows.Forms.Label();
            this.TextLabel = new System.Windows.Forms.Label();
            this.TextRichTextBox = new System.Windows.Forms.RichTextBox();
            this.OKOptionsButton = new System.Windows.Forms.Button();
            this.CancelOptionsButton = new System.Windows.Forms.Button();
            this.FontColorComboBox = new My_Note.VTextColorComboBox();
            this.SuspendLayout();
            // 
            // FontStyleLabel
            // 
            this.FontStyleLabel.AutoSize = true;
            this.FontStyleLabel.Location = new System.Drawing.Point(9, 9);
            this.FontStyleLabel.Name = "FontStyleLabel";
            this.FontStyleLabel.Size = new System.Drawing.Size(54, 13);
            this.FontStyleLabel.TabIndex = 0;
            this.FontStyleLabel.Text = "Font Style";
            // 
            // FontStyleComboBox
            // 
            this.FontStyleComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FontStyleComboBox.FormattingEnabled = true;
            this.FontStyleComboBox.Items.AddRange(new object[] {
            "Calibri",
            "Consolas",
            "Microsoft Sans Serif",
            "Times New Roman"});
            this.FontStyleComboBox.Location = new System.Drawing.Point(12, 25);
            this.FontStyleComboBox.Name = "FontStyleComboBox";
            this.FontStyleComboBox.Size = new System.Drawing.Size(121, 21);
            this.FontStyleComboBox.TabIndex = 1;
            this.FontStyleComboBox.SelectedIndexChanged += new System.EventHandler(this.FontStyleComboBox_SelectedIndexChanged);
            // 
            // FontSizeComboBox
            // 
            this.FontSizeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FontSizeComboBox.FormattingEnabled = true;
            this.FontSizeComboBox.Items.AddRange(new object[] {
            "9",
            "10",
            "12",
            "14"});
            this.FontSizeComboBox.Location = new System.Drawing.Point(139, 25);
            this.FontSizeComboBox.Name = "FontSizeComboBox";
            this.FontSizeComboBox.Size = new System.Drawing.Size(51, 21);
            this.FontSizeComboBox.TabIndex = 2;
            this.FontSizeComboBox.SelectedIndexChanged += new System.EventHandler(this.FontSizeComboBox_SelectedIndexChanged);
            // 
            // FontSizeLabel
            // 
            this.FontSizeLabel.AutoSize = true;
            this.FontSizeLabel.Location = new System.Drawing.Point(139, 9);
            this.FontSizeLabel.Name = "FontSizeLabel";
            this.FontSizeLabel.Size = new System.Drawing.Size(51, 13);
            this.FontSizeLabel.TabIndex = 3;
            this.FontSizeLabel.Text = "Font Size";
            // 
            // FontColorLabel
            // 
            this.FontColorLabel.AutoSize = true;
            this.FontColorLabel.Location = new System.Drawing.Point(196, 9);
            this.FontColorLabel.Name = "FontColorLabel";
            this.FontColorLabel.Size = new System.Drawing.Size(55, 13);
            this.FontColorLabel.TabIndex = 4;
            this.FontColorLabel.Text = "Font Color";
            // 
            // TextLabel
            // 
            this.TextLabel.AutoSize = true;
            this.TextLabel.Location = new System.Drawing.Point(9, 58);
            this.TextLabel.Name = "TextLabel";
            this.TextLabel.Size = new System.Drawing.Size(130, 13);
            this.TextLabel.TabIndex = 7;
            this.TextLabel.Text = "Text ( 24 characters max )";
            // 
            // TextRichTextBox
            // 
            this.TextRichTextBox.Location = new System.Drawing.Point(12, 74);
            this.TextRichTextBox.MaxLength = 24;
            this.TextRichTextBox.Multiline = false;
            this.TextRichTextBox.Name = "TextRichTextBox";
            this.TextRichTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.TextRichTextBox.Size = new System.Drawing.Size(305, 30);
            this.TextRichTextBox.TabIndex = 8;
            this.TextRichTextBox.Text = "";
            this.TextRichTextBox.TextChanged += new System.EventHandler(this.TextRichTextBox_TextChanged);
            // 
            // OKOptionsButton
            // 
            this.OKOptionsButton.Location = new System.Drawing.Point(53, 121);
            this.OKOptionsButton.Name = "OKOptionsButton";
            this.OKOptionsButton.Size = new System.Drawing.Size(75, 23);
            this.OKOptionsButton.TabIndex = 9;
            this.OKOptionsButton.Text = "OK";
            this.OKOptionsButton.UseVisualStyleBackColor = true;
            this.OKOptionsButton.Click += new System.EventHandler(this.OKOptionsButton_Click);
            // 
            // CancelOptionsButton
            // 
            this.CancelOptionsButton.Location = new System.Drawing.Point(200, 121);
            this.CancelOptionsButton.Name = "CancelOptionsButton";
            this.CancelOptionsButton.Size = new System.Drawing.Size(75, 23);
            this.CancelOptionsButton.TabIndex = 10;
            this.CancelOptionsButton.Text = "Cancel";
            this.CancelOptionsButton.UseVisualStyleBackColor = true;
            this.CancelOptionsButton.Click += new System.EventHandler(this.CancelOptionsButton_Click);
            // 
            // FontColorComboBox
            // 
            this.FontColorComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.FontColorComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FontColorComboBox.FormattingEnabled = true;
            this.FontColorComboBox.Location = new System.Drawing.Point(196, 25);
            this.FontColorComboBox.Name = "FontColorComboBox";
            this.FontColorComboBox.SelectedItem = null;
            this.FontColorComboBox.SelectedValue = System.Drawing.Color.Black;
            this.FontColorComboBox.Size = new System.Drawing.Size(121, 21);
            this.FontColorComboBox.TabIndex = 6;
            this.FontColorComboBox.SelectedIndexChanged += new System.EventHandler(this.FontColorComboBox_SelectedIndexChanged);
            // 
            // VertTextOptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(329, 156);
            this.Controls.Add(this.CancelOptionsButton);
            this.Controls.Add(this.OKOptionsButton);
            this.Controls.Add(this.TextRichTextBox);
            this.Controls.Add(this.TextLabel);
            this.Controls.Add(this.FontColorComboBox);
            this.Controls.Add(this.FontColorLabel);
            this.Controls.Add(this.FontSizeLabel);
            this.Controls.Add(this.FontSizeComboBox);
            this.Controls.Add(this.FontStyleComboBox);
            this.Controls.Add(this.FontStyleLabel);
            this.Name = "VertTextOptionsForm";
            this.Text = "Text Options";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VertTextOptionsForm_FormClosing);
            this.Load += new System.EventHandler(this.VertTextOptionsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label FontStyleLabel;
        private System.Windows.Forms.ComboBox FontStyleComboBox;
        private System.Windows.Forms.ComboBox FontSizeComboBox;
        private System.Windows.Forms.Label FontSizeLabel;
        private System.Windows.Forms.Label FontColorLabel;
        private VTextColorComboBox FontColorComboBox;
        private System.Windows.Forms.Label TextLabel;
        private System.Windows.Forms.RichTextBox TextRichTextBox;
        private System.Windows.Forms.Button OKOptionsButton;
        private System.Windows.Forms.Button CancelOptionsButton;
    }
}