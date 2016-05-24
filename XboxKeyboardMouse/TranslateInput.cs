using ScpDriverInterface;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Windows.Input;
using System.Drawing;

namespace XboxKeyboardMouse
{
    class TranslateInput
    {
        private class TranslateKeyboard
        {
            private static readonly Dictionary<Key, short> mapLeftStickY = new Dictionary<Key, short>
            {
                { Key.W, short.MinValue },
                { Key.S, short.MaxValue },
            };

            private static readonly Dictionary<Key, short> mapLeftStickX = new Dictionary<Key, short>
            {
                { Key.A, short.MinValue },
                { Key.D, short.MaxValue },
            };

            private static readonly Dictionary<Key, X360Buttons> buttonsmap = new Dictionary<Key, X360Buttons>
            {
                { Key.Space, X360Buttons.A },
                { Key.F, X360Buttons.B},
                { Key.E, X360Buttons.X },
                { Key.Tab, X360Buttons.Y},
            };

            public static X360Controller keyboardInput(X360Controller controller)
            {
                foreach (KeyValuePair<Key, short> entry in mapLeftStickY)
                {
                    if (Keyboard.IsKeyDown(entry.Key))
                        controller.LeftStickY = entry.Value;
                }

                //todo fix later use map
                if (Keyboard.IsKeyUp(Key.W) && Keyboard.IsKeyUp(Key.S))
                {
                    controller.LeftStickY = 0;
                }

                foreach (KeyValuePair<Key, short> entry in mapLeftStickX)
                {
                    if (Keyboard.IsKeyDown(entry.Key))
                        controller.LeftStickX = entry.Value;
                }

                //todo fix later use map
                if (Keyboard.IsKeyUp(Key.A) && Keyboard.IsKeyUp(Key.D))
                {
                    controller.LeftStickX = 0;
                }

                foreach (KeyValuePair<Key, X360Buttons> entry in buttonsmap)
                {
                    if (Keyboard.IsKeyDown(entry.Key))
                        controller.Buttons = controller.Buttons | entry.Value;
                    else
                        controller.Buttons = controller.Buttons & ~entry.Value;
                }

                return controller;
            }
        }

        private class TranslateMouse
        {
            static Point previousPosition;

            public static X360Controller mouseInput(X360Controller controller)
            {
                Point mousePos = Control.MousePosition;

                if (mousePos == previousPosition)
                    return controller;

                int deltaX = System.Math.Abs(mousePos.X - previousPosition.X);
                int deltaY = System.Math.Abs(mousePos.Y - previousPosition.Y);

                controller.RightStickX = (short)(deltaX * 5000);
                controller.RightStickY = (short)(-deltaY * 5000);

                previousPosition = mousePos;
                return controller;
            }
        }

        public static X360Controller translateInput(X360Controller controller)
        {
            TranslateMouse.mouseInput(controller);
            TranslateKeyboard.keyboardInput(controller);

            return controller;
        }
    }
}
