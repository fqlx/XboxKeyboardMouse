using ScpDriverInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
namespace XboxKeyboardMouse
{
    class TranslateInput
    {
        private static readonly Dictionary<Key, short> leftstickmap = new Dictionary<Key, short>
        {
            { Key.W, short.MaxValue },
            { Key.S, short.MinValue },
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
            foreach(KeyValuePair<Key, short> entry in leftstickmap)
            {
                if(Keyboard.IsKeyDown(entry.Key))
                    controller.LeftStickY = entry.Value;
                else
                    controller.LeftStickY = 0;
            }
        
            foreach(KeyValuePair<Key, X360Buttons> entry in buttonsmap)
            {
                if(Keyboard.IsKeyDown(entry.Key))
                    controller.Buttons = controller.Buttons | entry.Value;
                else
                    controller.Buttons = controller.Buttons ^ entry.Value;
            }

            return controller;
        }
    }
}
