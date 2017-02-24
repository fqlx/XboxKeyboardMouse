namespace XboxKeyboardMouse.Forms.MouseSettings {
    partial class GenericControls {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel2 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel3 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel4 = new MaterialSkin.Controls.MaterialLabel();
            this.mouseMouseModifier = new System.Windows.Forms.NumericUpDown();
            this.mouseYSense = new System.Windows.Forms.NumericUpDown();
            this.mouseXSense = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.mouseMouseModifier)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mouseYSense)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mouseXSense)).BeginInit();
            this.SuspendLayout();
            // 
            // materialLabel1
            // 
            this.materialLabel1.AutoSize = true;
            this.materialLabel1.Depth = 0;
            this.materialLabel1.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel1.Location = new System.Drawing.Point(12, 13);
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(128, 19);
            this.materialLabel1.TabIndex = 1;
            this.materialLabel1.Text = "Mouse Sensitivity";
            // 
            // materialLabel2
            // 
            this.materialLabel2.AutoSize = true;
            this.materialLabel2.Depth = 0;
            this.materialLabel2.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel2.Location = new System.Drawing.Point(146, 13);
            this.materialLabel2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel2.Name = "materialLabel2";
            this.materialLabel2.Size = new System.Drawing.Size(18, 19);
            this.materialLabel2.TabIndex = 2;
            this.materialLabel2.Text = "X";
            // 
            // materialLabel3
            // 
            this.materialLabel3.AutoSize = true;
            this.materialLabel3.Depth = 0;
            this.materialLabel3.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel3.Location = new System.Drawing.Point(146, 42);
            this.materialLabel3.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel3.Name = "materialLabel3";
            this.materialLabel3.Size = new System.Drawing.Size(18, 19);
            this.materialLabel3.TabIndex = 4;
            this.materialLabel3.Text = "Y";
            // 
            // materialLabel4
            // 
            this.materialLabel4.AutoSize = true;
            this.materialLabel4.Depth = 0;
            this.materialLabel4.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel4.Location = new System.Drawing.Point(12, 75);
            this.materialLabel4.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel4.Name = "materialLabel4";
            this.materialLabel4.Size = new System.Drawing.Size(101, 19);
            this.materialLabel4.TabIndex = 5;
            this.materialLabel4.Text = "Final Modifier";
            // 
            // mouseMouseModifier
            // 
            this.mouseMouseModifier.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mouseMouseModifier.DecimalPlaces = 6;
            this.mouseMouseModifier.Location = new System.Drawing.Point(170, 77);
            this.mouseMouseModifier.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.mouseMouseModifier.Name = "mouseMouseModifier";
            this.mouseMouseModifier.Size = new System.Drawing.Size(147, 20);
            this.mouseMouseModifier.TabIndex = 36;
            this.mouseMouseModifier.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.mouseMouseModifier.ValueChanged += new System.EventHandler(this.mouseMouseModifier_ValueChanged);
            // 
            // mouseYSense
            // 
            this.mouseYSense.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mouseYSense.DecimalPlaces = 15;
            this.mouseYSense.Location = new System.Drawing.Point(170, 39);
            this.mouseYSense.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.mouseYSense.Name = "mouseYSense";
            this.mouseYSense.Size = new System.Drawing.Size(147, 20);
            this.mouseYSense.TabIndex = 35;
            this.mouseYSense.Value = new decimal(new int[] {
            -820297523,
            191223,
            0,
            917504});
            this.mouseYSense.ValueChanged += new System.EventHandler(this.mouseYSense_ValueChanged);
            // 
            // mouseXSense
            // 
            this.mouseXSense.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mouseXSense.DecimalPlaces = 15;
            this.mouseXSense.Location = new System.Drawing.Point(170, 13);
            this.mouseXSense.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.mouseXSense.Name = "mouseXSense";
            this.mouseXSense.Size = new System.Drawing.Size(147, 20);
            this.mouseXSense.TabIndex = 34;
            this.mouseXSense.Value = new decimal(new int[] {
            -820297523,
            191223,
            0,
            917504});
            this.mouseXSense.ValueChanged += new System.EventHandler(this.mouseXSense_ValueChanged);
            // 
            // GenericControls
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mouseMouseModifier);
            this.Controls.Add(this.mouseYSense);
            this.Controls.Add(this.mouseXSense);
            this.Controls.Add(this.materialLabel4);
            this.Controls.Add(this.materialLabel3);
            this.Controls.Add(this.materialLabel2);
            this.Controls.Add(this.materialLabel1);
            this.Name = "GenericControls";
            this.Size = new System.Drawing.Size(457, 254);
            ((System.ComponentModel.ISupportInitialize)(this.mouseMouseModifier)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mouseYSense)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mouseXSense)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private MaterialSkin.Controls.MaterialLabel materialLabel2;
        private MaterialSkin.Controls.MaterialLabel materialLabel3;
        private MaterialSkin.Controls.MaterialLabel materialLabel4;
        private System.Windows.Forms.NumericUpDown mouseMouseModifier;
        private System.Windows.Forms.NumericUpDown mouseYSense;
        private System.Windows.Forms.NumericUpDown mouseXSense;
    }
}
