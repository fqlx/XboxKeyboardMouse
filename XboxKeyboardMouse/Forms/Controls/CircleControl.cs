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
        Size lastSize;
        public CircleControl() {
            lastSize = Size;
        }

        protected override void OnPaint(PaintEventArgs e) {          
            // Draw base control
            base.OnPaint(e);

            var bounds = ClientSize;
            var g = e.Graphics;
            var p = new Pen(ForeColor);
            var r = new Rectangle(0, 0, Size.Width-1, Size.Height-1);
            g.DrawEllipse(p, r);
        }
        
        protected override void OnResize(EventArgs e) {
            base.OnResize(e);

            // Check what has changed
            var oh = lastSize.Height;
            var nh = Size.Height;
            var ow = lastSize.Width;
            var nw = Size.Width;

            // Ensure the control is a square
            if (oh != nh && nw == ow) {
                var s = Size;
                s.Width = s.Height;
                Size = s;
            } else if (ow != nw && nh == oh) {
                var s = Size;
                s.Height = s.Width;
                Size = s;
            } else {
                // Just choose the width
                var s = Size;
                s.Height = s.Width;
                Size = s;
            }
        }
    }
}
