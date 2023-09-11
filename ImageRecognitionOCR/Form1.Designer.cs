namespace ImageRecognitionOCR
{
    partial class Form1
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
            this.SelectImageFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.SelectFileButton = new System.Windows.Forms.Button();
            this.FilePathLabel = new System.Windows.Forms.Label();
            this.ExportedRichText = new System.Windows.Forms.RichTextBox();
            this.ConvertButton = new System.Windows.Forms.Button();
            this.ConfidenceLabel = new System.Windows.Forms.Label();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SelectImageFileDialog
            // 
            this.SelectImageFileDialog.FileName = "openFileDialog";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.SelectFileButton);
            this.flowLayoutPanel1.Controls.Add(this.FilePathLabel);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(651, 30);
            this.flowLayoutPanel1.TabIndex = 3;
            // 
            // SelectFileButton
            // 
            this.SelectFileButton.Location = new System.Drawing.Point(3, 3);
            this.SelectFileButton.Name = "SelectFileButton";
            this.SelectFileButton.Size = new System.Drawing.Size(149, 23);
            this.SelectFileButton.TabIndex = 1;
            this.SelectFileButton.Text = "Select Image file";
            this.SelectFileButton.UseVisualStyleBackColor = true;
            this.SelectFileButton.Click += new System.EventHandler(this.SelectFileButton_Click);
            // 
            // FilePathLabel
            // 
            this.FilePathLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FilePathLabel.AutoSize = true;
            this.FilePathLabel.ForeColor = System.Drawing.Color.Red;
            this.FilePathLabel.Location = new System.Drawing.Point(158, 0);
            this.FilePathLabel.Name = "FilePathLabel";
            this.FilePathLabel.Size = new System.Drawing.Size(0, 29);
            this.FilePathLabel.TabIndex = 2;
            this.FilePathLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ExportedRichText
            // 
            this.ExportedRichText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ExportedRichText.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExportedRichText.Location = new System.Drawing.Point(12, 139);
            this.ExportedRichText.Name = "ExportedRichText";
            this.ExportedRichText.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ExportedRichText.Size = new System.Drawing.Size(779, 300);
            this.ExportedRichText.TabIndex = 4;
            this.ExportedRichText.Text = "";
            // 
            // ConvertButton
            // 
            this.ConvertButton.Location = new System.Drawing.Point(669, 12);
            this.ConvertButton.Name = "ConvertButton";
            this.ConvertButton.Size = new System.Drawing.Size(126, 29);
            this.ConvertButton.TabIndex = 5;
            this.ConvertButton.Text = "Convert To OCR";
            this.ConvertButton.UseVisualStyleBackColor = true;
            this.ConvertButton.Click += new System.EventHandler(this.ConvertButton_Click);
            // 
            // ConfidenceLabel
            // 
            this.ConfidenceLabel.AutoSize = true;
            this.ConfidenceLabel.Location = new System.Drawing.Point(9, 54);
            this.ConfidenceLabel.Name = "ConfidenceLabel";
            this.ConfidenceLabel.Size = new System.Drawing.Size(0, 13);
            this.ConfidenceLabel.TabIndex = 6;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(803, 451);
            this.Controls.Add(this.ConfidenceLabel);
            this.Controls.Add(this.ConvertButton);
            this.Controls.Add(this.ExportedRichText);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog SelectImageFileDialog;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button SelectFileButton;
        private System.Windows.Forms.Label FilePathLabel;
        private System.Windows.Forms.RichTextBox ExportedRichText;
        private System.Windows.Forms.Button ConvertButton;
        private System.Windows.Forms.Label ConfidenceLabel;
    }
}

