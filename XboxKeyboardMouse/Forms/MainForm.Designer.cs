namespace XboxKeyboardMouse.Forms {
    partial class MainForm {
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.panel1 = new System.Windows.Forms.Panel();
            this.powerButton = new System.Windows.Forms.PictureBox();
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.btnExitApp = new MaterialSkin.Controls.MaterialFlatButton();
            this.materialFlatButton2 = new MaterialSkin.Controls.MaterialFlatButton();
            this.materialFlatButton1 = new MaterialSkin.Controls.MaterialFlatButton();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.powerButton)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.panel1.Controls.Add(this.powerButton);
            this.panel1.Location = new System.Drawing.Point(-1, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(231, 214);
            this.panel1.TabIndex = 0;
            // 
            // powerButton
            // 
            this.powerButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.powerButton.Image = global::XboxKeyboardMouse.Properties.Resources.power_big;
            this.powerButton.Location = new System.Drawing.Point(0, 0);
            this.powerButton.Name = "powerButton";
            this.powerButton.Size = new System.Drawing.Size(231, 214);
            this.powerButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.powerButton.TabIndex = 0;
            this.powerButton.TabStop = false;
            this.powerButton.MouseMove += new System.Windows.Forms.MouseEventHandler(this.updateCursorIcon);
            // 
            // materialLabel1
            // 
            this.materialLabel1.AutoSize = true;
            this.materialLabel1.Depth = 0;
            this.materialLabel1.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel1.Location = new System.Drawing.Point(14, 317);
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(158, 19);
            this.materialLabel1.TabIndex = 4;
            this.materialLabel1.Text = "Xbox Keyboard Mouse";
            // 
            // btnExitApp
            // 
            this.btnExitApp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnExitApp.ControlAutoSize = false;
            this.btnExitApp.Depth = 0;
            this.btnExitApp.FontColor = System.Drawing.Color.Black;
            this.btnExitApp.FontColorDisabled = System.Drawing.Color.Gray;
            this.btnExitApp.Icon = global::XboxKeyboardMouse.Properties.Resources.exit_to_app_black;
            this.btnExitApp.Location = new System.Drawing.Point(175, 309);
            this.btnExitApp.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnExitApp.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnExitApp.Name = "btnExitApp";
            this.btnExitApp.Primary = false;
            this.btnExitApp.SetBackgroundColor = false;
            this.btnExitApp.SetFontColor = false;
            this.btnExitApp.SetFontDisabledColor = false;
            this.btnExitApp.Size = new System.Drawing.Size(41, 36);
            this.btnExitApp.TabIndex = 5;
            this.btnExitApp.Text = "EXIT";
            this.btnExitApp.UseVisualStyleBackColor = true;
            this.btnExitApp.Click += new System.EventHandler(this.materialFlatButton3_Click);
            // 
            // materialFlatButton2
            // 
            this.materialFlatButton2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.materialFlatButton2.ControlAutoSize = false;
            this.materialFlatButton2.Depth = 0;
            this.materialFlatButton2.FontColor = System.Drawing.Color.Black;
            this.materialFlatButton2.FontColorDisabled = System.Drawing.Color.Gray;
            this.materialFlatButton2.Icon = global::XboxKeyboardMouse.Properties.Resources.google_controller;
            this.materialFlatButton2.Location = new System.Drawing.Point(13, 224);
            this.materialFlatButton2.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.materialFlatButton2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialFlatButton2.Name = "materialFlatButton2";
            this.materialFlatButton2.Primary = false;
            this.materialFlatButton2.SetBackgroundColor = false;
            this.materialFlatButton2.SetFontColor = false;
            this.materialFlatButton2.SetFontDisabledColor = false;
            this.materialFlatButton2.Size = new System.Drawing.Size(203, 36);
            this.materialFlatButton2.TabIndex = 3;
            this.materialFlatButton2.Text = "Controller";
            this.materialFlatButton2.UseVisualStyleBackColor = true;
            this.materialFlatButton2.Click += new System.EventHandler(this.materialFlatButton2_Click);
            // 
            // materialFlatButton1
            // 
            this.materialFlatButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.materialFlatButton1.ControlAutoSize = false;
            this.materialFlatButton1.Depth = 0;
            this.materialFlatButton1.FontColor = System.Drawing.Color.Black;
            this.materialFlatButton1.FontColorDisabled = System.Drawing.Color.Gray;
            this.materialFlatButton1.Icon = global::XboxKeyboardMouse.Properties.Resources.ic_settings_black_24dp_2x;
            this.materialFlatButton1.Location = new System.Drawing.Point(13, 269);
            this.materialFlatButton1.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.materialFlatButton1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialFlatButton1.Name = "materialFlatButton1";
            this.materialFlatButton1.Primary = false;
            this.materialFlatButton1.SetBackgroundColor = false;
            this.materialFlatButton1.SetFontColor = false;
            this.materialFlatButton1.SetFontDisabledColor = false;
            this.materialFlatButton1.Size = new System.Drawing.Size(203, 36);
            this.materialFlatButton1.TabIndex = 2;
            this.materialFlatButton1.Text = "Settings";
            this.materialFlatButton1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.materialFlatButton1.UseVisualStyleBackColor = true;
            this.materialFlatButton1.Click += new System.EventHandler(this.materialFlatButton1_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(230, 357);
            this.Controls.Add(this.btnExitApp);
            this.Controls.Add(this.materialLabel1);
            this.Controls.Add(this.materialFlatButton2);
            this.Controls.Add(this.materialFlatButton1);
            this.Controls.Add(this.panel1);
            this.Name = "MainForm";
            this.Text = "XboxKeyboardMouse";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.powerButton)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private MaterialSkin.Controls.MaterialFlatButton materialFlatButton1;
        private MaterialSkin.Controls.MaterialFlatButton materialFlatButton2;
        private System.Windows.Forms.PictureBox powerButton;
        private System.Windows.Forms.Panel panel1;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private MaterialSkin.Controls.MaterialFlatButton btnExitApp;
    }
}