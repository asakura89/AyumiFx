namespace TestForm
{
    partial class CompanyForm
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
            this.companyIdTxt = new System.Windows.Forms.TextBox();
            this.nameTxt = new System.Windows.Forms.TextBox();
            this.addressTxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.insertButton = new System.Windows.Forms.Button();
            this.updateButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.activeChk = new System.Windows.Forms.CheckBox();
            this.activeDatePicker = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // companyIdTxt
            // 
            this.companyIdTxt.Location = new System.Drawing.Point(86, 24);
            this.companyIdTxt.Name = "companyIdTxt";
            this.companyIdTxt.Size = new System.Drawing.Size(150, 20);
            this.companyIdTxt.TabIndex = 0;
            this.companyIdTxt.Leave += new System.EventHandler(this.companyIdTxt_Leave);
            // 
            // nameTxt
            // 
            this.nameTxt.Location = new System.Drawing.Point(86, 51);
            this.nameTxt.Name = "nameTxt";
            this.nameTxt.Size = new System.Drawing.Size(150, 20);
            this.nameTxt.TabIndex = 1;
            // 
            // addressTxt
            // 
            this.addressTxt.Location = new System.Drawing.Point(86, 78);
            this.addressTxt.Name = "addressTxt";
            this.addressTxt.Size = new System.Drawing.Size(150, 20);
            this.addressTxt.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Company Id";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Address";
            // 
            // insertButton
            // 
            this.insertButton.Location = new System.Drawing.Point(16, 172);
            this.insertButton.Name = "insertButton";
            this.insertButton.Size = new System.Drawing.Size(75, 23);
            this.insertButton.TabIndex = 6;
            this.insertButton.Text = "Insert";
            this.insertButton.UseVisualStyleBackColor = true;
            this.insertButton.Click += new System.EventHandler(this.insertButton_Click);
            // 
            // updateButton
            // 
            this.updateButton.Location = new System.Drawing.Point(105, 172);
            this.updateButton.Name = "updateButton";
            this.updateButton.Size = new System.Drawing.Size(75, 23);
            this.updateButton.TabIndex = 7;
            this.updateButton.Text = "Update";
            this.updateButton.UseVisualStyleBackColor = true;
            this.updateButton.Click += new System.EventHandler(this.updateButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Location = new System.Drawing.Point(197, 172);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(75, 23);
            this.deleteButton.TabIndex = 8;
            this.deleteButton.Text = "Delete";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // activeChk
            // 
            this.activeChk.AutoSize = true;
            this.activeChk.Location = new System.Drawing.Point(86, 105);
            this.activeChk.Name = "activeChk";
            this.activeChk.Size = new System.Drawing.Size(56, 17);
            this.activeChk.TabIndex = 9;
            this.activeChk.Text = "Active";
            this.activeChk.UseVisualStyleBackColor = true;
            this.activeChk.CheckedChanged += new System.EventHandler(this.activeChk_CheckedChanged);
            // 
            // activeDatePicker
            // 
            this.activeDatePicker.Enabled = false;
            this.activeDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.activeDatePicker.Location = new System.Drawing.Point(86, 128);
            this.activeDatePicker.Name = "activeDatePicker";
            this.activeDatePicker.Size = new System.Drawing.Size(150, 20);
            this.activeDatePicker.TabIndex = 10;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 223);
            this.Controls.Add(this.activeDatePicker);
            this.Controls.Add(this.activeChk);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.updateButton);
            this.Controls.Add(this.insertButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.addressTxt);
            this.Controls.Add(this.nameTxt);
            this.Controls.Add(this.companyIdTxt);
            this.Name = "Form1";
            this.Text = "Company";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.form1_FormClosing);
            this.Load += new System.EventHandler(this.form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox companyIdTxt;
        private System.Windows.Forms.TextBox nameTxt;
        private System.Windows.Forms.TextBox addressTxt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button insertButton;
        private System.Windows.Forms.Button updateButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.CheckBox activeChk;
        private System.Windows.Forms.DateTimePicker activeDatePicker;

    }
}

