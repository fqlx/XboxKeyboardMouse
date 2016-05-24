using ScpDriverInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace XboxMouse_Keyboard
{
    class TranslateKeyboard
    {
        private static readonly Dictionary<Key, short> mapLeftStickY = new Dictionary<Key, short>
            {
                { Key.W, short.MaxValue },
                { Key.S, short.MinValue },
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

        private static void keyboardInput(X360Controller controller)
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

            return;
        }

        public static void translateKeyboard(X360Controller controller)
        {
            keyboardInput(controller);
        }
    }
}
