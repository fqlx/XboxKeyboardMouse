using ScpDriverInterface;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace XboxKeyboardMouse {
    class TranslateKeyboard {

        public enum TriggerType { LeftTrigger, RightTrigger }

        public static Dictionary<Key, short> mapLeftStickY = new Dictionary<Key, short> {
            { Key.W, short.MaxValue },
            { Key.S, short.MinValue },
        };

        public static Dictionary<Key, short> mapLeftStickX = new Dictionary<Key, short> {
            { Key.A, short.MinValue },
            { Key.D, short.MaxValue },
        };

        public static Dictionary<Key, X360Buttons> buttons = new Dictionary<Key, X360Buttons>();
        public static Dictionary<Key, TriggerType> triggers = new Dictionary<Key, TriggerType>();

        private static void KeyInput(X360Controller controller) {
            List<bool> btnStatus = new List<bool>();

            //bool tLeft  = false;
            //bool tRight = false;

            try {
                btnStatus.Clear();
                foreach (KeyValuePair<Key, short> entry in mapLeftStickY) {
                    bool v;

                    if (entry.Key == Key.Escape)
                         v = Hooks.LowLevelKeyboardHook.EscapePressed;
                    else v = Keyboard.IsKeyDown(entry.Key);

                    if (v) controller.LeftStickY = entry.Value;

                    btnStatus.Add(v);
                }

                if (!btnStatus.Contains(true)) {
                    controller.LeftStickY = 0;
                }

                btnStatus.Clear();
                foreach (KeyValuePair<Key, short> entry in mapLeftStickX) {
                    bool v;

                    if (entry.Key == Key.Escape)
                         v = Hooks.LowLevelKeyboardHook.EscapePressed;
                    else v = Keyboard.IsKeyDown(entry.Key);

                    if (v) controller.LeftStickX = entry.Value;
                    btnStatus.Add(v);
                }

                if (!btnStatus.Contains(true)) {
                    controller.LeftStickX = 0;
                }

                foreach (KeyValuePair<Key, X360Buttons> entry in buttons) {
                    if (entry.Key == Key.None) continue;

                    bool v;

                    if (entry.Key == Key.Escape)
                         v = Hooks.LowLevelKeyboardHook.EscapePressed;
                    else v = Keyboard.IsKeyDown(entry.Key);

                    if (v)
                         controller.Buttons = controller.Buttons | entry.Value;
                    else controller.Buttons = controller.Buttons & ~entry.Value;
                }
                
                /*foreach (KeyValuePair<Key, TriggerType> entry in triggers) {
                    var dwn  = Keyboard.IsKeyDown(entry.Key);
                    var left = (entry.Value == TriggerType.LeftTrigger);

                    if (dwn && left)
                        tLeft = true;
                    else if (dwn && !left)
                        tRight = true;

                    if (dwn) {
                        if (left)
                             controller.LeftTrigger = 255;
                        else controller.RightTrigger = 255;
                    }
                }*/

                //if (!tLeft)       controller.LeftTrigger = 0;
                //else if (!tRight) controller.RightTrigger = 0;
            } catch (Exception ex) {
                // This occures when changing presets
            }
        }

        public static void KeyboardInput(X360Controller controller) {
            KeyInput(controller);
        }
    }
}
