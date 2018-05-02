using SimWinInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using XboxKeyboardMouse.Libs;

namespace XboxKeyboardMouse {
    class TranslateKeyboard {
        public static bool TRIGGER_LEFT_PRESSED = false;
        public static bool TRIGGER_RIGHT_PRESSED = false;

        public enum TriggerType { LeftTrigger, RightTrigger }

        public static void ClearAllDicts()
        {
            mapLeftStickX.Clear();
            mapLeftStickY.Clear();
            mapRightStickX.Clear();
            mapRightStickY.Clear();
            buttons.Clear();
            triggers.Clear();
        }

        public static Dictionary<Key, short> mapLeftStickY = new Dictionary<Key, short>();
        public static Dictionary<Key, short> mapLeftStickX = new Dictionary<Key, short>();
        public static Dictionary<Key, short> mapRightStickX = new Dictionary<Key, short>();
        public static Dictionary<Key, short> mapRightStickY = new Dictionary<Key, short>();

        public static Dictionary<Key, GamePadControl> buttons = new Dictionary<Key, GamePadControl>();
        public static Dictionary<Key, TriggerType> triggers = new Dictionary<Key, TriggerType>();

        public static Dictionary<RunTimeOptionType, RunTimeOption> mapRunTimeOptions = new Dictionary<RunTimeOptionType, RunTimeOption>();

        private static void KeyInput(SimulatedGamePadState controller) {
            List<bool> btnStatus = new List<bool>();
            
            try {
                btnStatus.Clear();

                bool mouseDisabled = Program.ActiveConfig.Mouse_Eng_Type == MouseTranslationMode.NONE;

                // -------------------------------------------------------------------------------
                //                                STICKS
                // -------------------------------------------------------------------------------
                if (mouseDisabled || Program.ActiveConfig.Mouse_Is_RightStick)
                {
                    controller.LeftStickX = StickValueFromKeyboardState(mapLeftStickX);
                    controller.LeftStickY = StickValueFromKeyboardState(mapLeftStickY);
                }
                if (mouseDisabled || !Program.ActiveConfig.Mouse_Is_RightStick)
                {
                    controller.RightStickX = StickValueFromKeyboardState(mapRightStickX);
                    controller.RightStickY = StickValueFromKeyboardState(mapRightStickY);
                }

                // -------------------------------------------------------------------------------
                //                                BUTTONS
                // -------------------------------------------------------------------------------
                foreach (var control in GamePadControls.BinaryControls)
                {
                    // Explicitly set the state of every binary button we care about: If it's in our map
                    // and the key is currently pressed, set the button to pressed, else set it to unpressed.
                    if (AnyKeyIsPressedForControl(buttons, control))
                        controller.Buttons |= control;
                    else
                        controller.Buttons &= ~control;
                }
                
                // -------------------------------------------------------------------------------
                //                                TRIGGERS
                // -------------------------------------------------------------------------------
                foreach (KeyValuePair<Key, TriggerType> entry in triggers) {
                    if (entry.Key == Key.None) continue;

                    bool v = Keyboard.IsKeyDown(entry.Key);
                    bool ir = entry.Value == TriggerType.RightTrigger;

                    if (v) {
                        if (ir)
                             controller.RightTrigger = 255;
                        else controller.LeftTrigger = 255;
                    } else {
                        if (!TranslateKeyboard.TRIGGER_RIGHT_PRESSED && ir)
                            controller.RightTrigger = 0;
                        else if (!TranslateKeyboard.TRIGGER_LEFT_PRESSED && ir)
                            controller.LeftTrigger = 0;
                    }
                }

                // -------------------------------------------------------------------------------
                //                                CALIBRATION / RUNTIME OPTIONS
                // -------------------------------------------------------------------------------
                // Calibration / runtime options will detect the first key-down state for their associated button, but 
                // will ignore further iterations where the button is still being held. This can be used to advance
                // through calibration states, or have ways to swap configurations on the fly, etc.
                foreach (var option in mapRunTimeOptions.Values)
                {
                    if (option.Key != Key.None && Keyboard.IsKeyDown(option.Key))
                    {
                        if (!option.Ran)
                        {
                            option.Run();
                            option.Ran = true;
                        }
                    }
                    else
                    {
                        option.Ran = false;
                    }
                }
            }
            catch (Exception) { /* This occures when changing presets */ }
        }

        private static short StickValueFromKeyboardState(Dictionary<Key, short> map)
        {
            return (from entry in map
                    where entry.Key != Key.None && Keyboard.IsKeyDown(entry.Key)
                    select entry.Value).FirstOrDefault();
        }

        private static bool AnyKeyIsPressedForControl(Dictionary<Key, GamePadControl> map, GamePadControl control)
        {
            return map.Any(entry => entry.Value == control && Keyboard.IsKeyDown(entry.Key));
        }

        private static void Debug_TimeTracer(SimulatedGamePadState Controller) {
            // Get the start time
            var watch = System.Diagnostics.Stopwatch.StartNew();

            // Call the key input
            KeyInput(Controller);

            // Get the end time
            watch.Stop();
            string time = watch.ElapsedMilliseconds + " MS";
            
            if (time != "0 MS") {
                // Display the time
                Logger.appendLogLine("KeyboardInput", $"Timed @ {time}", Logger.Type.Debug);
            }
        }

        public static void KeyboardInput(SimulatedGamePadState controller) =>
            #if (DEBUG)
                    // Only enable if you have timing issues AKA Latency on 
                    // the keyboard inputs
                    Debug_TimeTracer(controller);
                    //KeyInput(controller);
            #else
                    KeyInput(controller);
            #endif
    }
}
