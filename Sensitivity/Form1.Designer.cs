namespace Sensitivity
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
            this.RunModel = new System.Windows.Forms.Button();
            this.RunExternal = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // RunModel
            // 
            this.RunModel.Location = new System.Drawing.Point(23, 36);
            this.RunModel.Name = "RunModel";
            this.RunModel.Size = new System.Drawing.Size(154, 23);
            this.RunModel.TabIndex = 0;
            this.RunModel.Text = "Run Internal Variables";
            this.RunModel.UseVisualStyleBackColor = true;
            this.RunModel.Click += new System.EventHandler(this.RunModel_Click);
            // 
            // RunExternal
            // 
            this.RunExternal.Location = new System.Drawing.Point(23, 87);
            this.RunExternal.Name = "RunExternal";
            this.RunExternal.Size = new System.Drawing.Size(154, 23);
            this.RunExternal.TabIndex = 1;
            this.RunExternal.Text = "Run External Variables";
            this.RunExternal.UseVisualStyleBackColor = true;
            this.RunExternal.Click += new System.EventHandler(this.RunExternal_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.RunExternal);
            this.Controls.Add(this.RunModel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button RunModel;
        private System.Windows.Forms.Button RunExternal;
    }
}

