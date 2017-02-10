using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace XboxKeyboardMouse.Forms.Controls {
    public class CircleControl : UserControl {

        // Axis
        short axisX, axisY;

        private Color color  = Color.FromArgb(0x5D, 0xC2, 0x1E);
        private Pen colorB;

        [DefaultValue(false)]
        public bool DrawAxisDot { get; set; } = false;
        
        public Color AxisDotColor {
            get { return  color;  }
            set {
                color = value;
                colorB = new Pen(value);
            }
        }

        Size lastSize;
        public CircleControl() {
            lastSize = Size;

            this.DoubleBuffered = true;

            colorB = new Pen(color);
        }

        protected override void OnPaint(PaintEventArgs e) {          
            // Draw base control
            base.OnPaint(e);

            var bounds = ClientSize;
            var g = e.Graphics;
            var p = new Pen(ForeColor);
            var r = new Rectangle(0, 0, Size.Width-1, Size.Height-1);

            // Draw the axis dot
            if (DrawAxisDot) {
                int max = short.MaxValue;
                double cur = (double)Width / 2; // Either will do
                double ScaleFactor = cur / max; // ??? Pretty sure its this xD

                double x = Math.Round(axisX * ScaleFactor) + cur;
                double y = Math.Round(axisY * ScaleFactor) * -1 + cur;

                g.DrawRectangle(colorB, new Rectangle((int)x - 5, (int)y - 5, 10, 10));

                // Draw rect so our current 
                // location does not go outside the 
                // bounds of the circle
                g.DrawRectangle(p, r);
            } else {
                g.DrawEllipse(p, r);
            }
        }
        
        protected override void OnResize(EventArgs e) {
            base.OnResize(e);

            // Check what has changed
            var oh = lastSize.Height;
            var nh = Size.Height;
            var ow = lastSize.Width;
            var nw = Size.Width;
            var s  = Size;

            // Ensure the control is a square
            if (oh != nh && nw == ow) {
                s.Width = s.Height;
                Size = s;
            } else if (ow != nw && nh == oh) {
                s.Height = s.Width;
                Size = s;
            } else {
                // Just choose the width
                s.Height = s.Width;
                Size = s;
            }
        }

        public void SetAxis(short x, short y) {
            if (axisX != x || axisY != y) {
                axisX = x;
                axisY = y;

                //System.Diagnostics.Debugger.Log(0, "", $"JOYSTICK UPDATE ({Name}) : X {axisX}, Y {axisY}\n");

                this.Invalidate();
            }
        }

        public void ResetAxis() {
            // Set to 0 (CENTER)
            axisX = 0;
            axisY = 0;
        }
    }
}
