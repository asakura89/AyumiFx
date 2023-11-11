namespace WK.Forms
{
    partial class ErrorMessageForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ErrorMessageForm));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.errorMessageLabel = new System.Windows.Forms.Label();
            this.stackTraceTxt = new System.Windows.Forms.TextBox();
            this.closeButton = new System.Windows.Forms.Button();
            this.showDetailButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(94, 94);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // errorMessageLabel
            // 
            this.errorMessageLabel.AutoSize = true;
            this.errorMessageLabel.Location = new System.Drawing.Point(131, 12);
            this.errorMessageLabel.Name = "errorMessageLabel";
            this.errorMessageLabel.Size = new System.Drawing.Size(35, 13);
            this.errorMessageLabel.TabIndex = 1;
            this.errorMessageLabel.Text = "label1";
            // 
            // stackTraceTxt
            // 
            this.stackTraceTxt.Location = new System.Drawing.Point(12, 126);
            this.stackTraceTxt.Multiline = true;
            this.stackTraceTxt.Name = "stackTraceTxt";
            this.stackTraceTxt.Size = new System.Drawing.Size(571, 213);
            this.stackTraceTxt.TabIndex = 2;
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(427, 83);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 3;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // showDetailButton
            // 
            this.showDetailButton.Location = new System.Drawing.Point(508, 83);
            this.showDetailButton.Name = "showDetailButton";
            this.showDetailButton.Size = new System.Drawing.Size(75, 23);
            this.showDetailButton.TabIndex = 4;
            this.showDetailButton.Text = "Details >>";
            this.showDetailButton.UseVisualStyleBackColor = true;
            this.showDetailButton.Click += new System.EventHandler(this.showDetailButton_Click);
            // 
            // ErrorMessageForm
            // 
            this.AcceptButton = this.closeButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(595, 354);
            this.Controls.Add(this.showDetailButton);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.stackTraceTxt);
            this.Controls.Add(this.errorMessageLabel);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ErrorMessageForm";
            this.Text = "Error";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label errorMessageLabel;
        private System.Windows.Forms.TextBox stackTraceTxt;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button showDetailButton;
    }
}