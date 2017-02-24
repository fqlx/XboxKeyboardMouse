using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using MaterialSkin.Animations;

namespace MaterialSkin.Controls {
    public class MaterialFlatButton : Button, IMaterialControl {
        [Browsable(false)]
        public int Depth { get; set; }
        [Browsable(false)]
        public MaterialSkinManager SkinManager => MaterialSkinManager.Instance;
        [Browsable(false)]
        public MouseState MouseState { get; set; }
        public bool Primary { get; set; }

        private readonly AnimationManager _animationManager;
        private readonly AnimationManager _hoverAnimationManager;

        private SizeF _textSize;

        private Image _icon;
        public Image Icon {
            get { return _icon; }
            set {
                _icon = value;
                if (AutoSize)
                    Size = GetPreferredSize();
                Invalidate();
            }
        }

        private bool _AutoSize = true;

        public override bool AutoSize {
            get {
                return _AutoSize;
            }

            // Disable because
            // i need to manually size
            // the controls and even if
            // changed to the variable
            // it causes damn issues, so
            // i am bypassing it altogether!
            set { }
        }

        // Overrides background color
        public bool SetBackgroundColor { get; set; } = false;



        // Overrides the font color
        private Color _fontColor = Color.Black;
        private Brush _fontBrush = Brushes.Black;
        public Color FontColor {
            get {
                return _fontColor;
            } 

            set {
                _fontColor = value;
                _fontBrush = new SolidBrush(value);
            }
        }

        private Color _fontColorD = Color.Gray;
        private Brush _fontBrushD = Brushes.Gray;
        public Color FontColorDisabled {
            get {
                return _fontColorD;
            }

            set {
                _fontColorD = value;
                _fontBrushD = new SolidBrush(value);
            }
        }

        public bool SetFontColor { get; set; } = false;
        public bool SetFontDisabledColor { get; set; } = false;

        // The real autosize
        public bool ControlAutoSize {
            get {
                return _AutoSize;
            }
            set {
                _AutoSize = value;
            }
        }

        public MaterialFlatButton() {
            Primary = false;

            _animationManager = new AnimationManager(false) {
                Increment = 0.03,
                AnimationType = AnimationType.EaseOut
            };
            _hoverAnimationManager = new AnimationManager {
                Increment = 0.07,
                AnimationType = AnimationType.Linear
            };

            _hoverAnimationManager.OnAnimationProgress += sender => Invalidate();
            _animationManager.OnAnimationProgress += sender => Invalidate();

            AutoSizeMode = AutoSizeMode.GrowAndShrink;

            // Removed to allow manual sizing.
            //AutoSize = true;

            Margin = new Padding(4, 6, 4, 6);
            Padding = new Padding(0);
        }

        public override string Text {
            get { return base.Text; }
            set {
                base.Text = value;
                _textSize = CreateGraphics().MeasureString(value.ToUpper(), SkinManager.ROBOTO_MEDIUM_10);
                if (AutoSize)
                    Size = GetPreferredSize();
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs pevent) {
            var g = pevent.Graphics;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            if (SetBackgroundColor)
                 g.Clear(BackColor);
            else g.Clear(Parent.BackColor);

            //Hover
            Color c = SkinManager.GetFlatButtonHoverBackgroundColor();
            using (Brush b = new SolidBrush(Color.FromArgb((int)(_hoverAnimationManager.GetProgress() * c.A), c.RemoveAlpha())))
                g.FillRectangle(b, ClientRectangle);

            //Ripple
            if (_animationManager.IsAnimating()) {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                for (var i = 0; i < _animationManager.GetAnimationCount(); i++) {
                    var animationValue = _animationManager.GetProgress(i);
                    var animationSource = _animationManager.GetSource(i);

                    using (Brush rippleBrush = new SolidBrush(Color.FromArgb((int)(101 - (animationValue * 100)), Color.Black))) {
                        var rippleSize = (int)(animationValue * Width * 2);
                        g.FillEllipse(rippleBrush, new Rectangle(animationSource.X - rippleSize / 2, animationSource.Y - rippleSize / 2, rippleSize, rippleSize));
                    }
                }
                g.SmoothingMode = SmoothingMode.None;
            }

            //Icon
            var iconRect = new Rectangle(8, 6, 24, 24);

            if (string.IsNullOrEmpty(Text))
                // Center Icon
                iconRect.X += 2;

            if (Icon != null)
                g.DrawImage(Icon, iconRect);

            //Text
            var textRect = ClientRectangle;

            if (Icon != null) {
                //
                // Resize and move Text container
                //

                // First 8: left padding
                // 24: icon width
                // Second 4: space between Icon and Text
                // Third 8: right padding
                textRect.Width -= 8 + 24 + 4 + 8;

                // First 8: left padding
                // 24: icon width
                // Second 4: space between Icon and Text
                textRect.X += 8 + 24 + 4;
            }

            // BRUSH = 
            // Enabled ? 
            //      (Primary ? SkinManager.ColorScheme.PrimaryBrush : SkinManager.GetPrimaryTextBrush()) : 
            //      SkinManager.GetFlatButtonDisabledTextBrush();

            Brush brush;

            if (Enabled) {
                if (SetFontColor)
                     brush = _fontBrush;
                else brush = (Primary ? SkinManager.ColorScheme.PrimaryBrush : SkinManager.GetPrimaryTextBrush());
            } else {
                if (SetFontDisabledColor)
                     brush = _fontBrushD;
                else brush = SkinManager.GetFlatButtonDisabledTextBrush();
            }

            g.DrawString(
                Text.ToUpper(),
                SkinManager.ROBOTO_MEDIUM_10,
                brush,
                textRect,
                new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center }
            );
        }

        private Size GetPreferredSize() {
            return GetPreferredSize(new Size(0, 0));
        }

        public override Size GetPreferredSize(Size proposedSize) {
            // Provides extra space for proper padding for content
            var extra = 16;

            if (Icon != null)
                // 24 is for icon size
                // 4 is for the space between icon & text
                extra += 24 + 4;

            return new Size((int)Math.Ceiling(_textSize.Width) + extra, 36);
        }

        protected override void OnCreateControl() {
            base.OnCreateControl();
            if (DesignMode) return;

            MouseState = MouseState.OUT;
            MouseEnter += (sender, args) => {
                MouseState = MouseState.HOVER;
                _hoverAnimationManager.StartNewAnimation(AnimationDirection.In);
                Invalidate();
            };
            MouseLeave += (sender, args) => {
                MouseState = MouseState.OUT;
                _hoverAnimationManager.StartNewAnimation(AnimationDirection.Out);
                Invalidate();
            };
            MouseDown += (sender, args) => {
                if (args.Button == MouseButtons.Left) {
                    MouseState = MouseState.DOWN;

                    _animationManager.StartNewAnimation(AnimationDirection.In, args.Location);
                    Invalidate();
                }
            };
            MouseUp += (sender, args) => {
                MouseState = MouseState.HOVER;

                Invalidate();
            };
        }
    }
}
