namespace XboxKeyboardMouse
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
            this.activate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // activate
            // 
            this.activate.Location = new System.Drawing.Point(138, 69);
            this.activate.Name = "activate";
            this.activate.Size = new System.Drawing.Size(75, 23);
            this.activate.TabIndex = 0;
            this.activate.Text = "Activate";
            this.activate.UseVisualStyleBackColor = true;
            this.activate.Click += new System.EventHandler(this.activate_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 146);
            this.Controls.Add(this.activate);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button activate;
    }
}

