using ScpDriverInterface;
using XboxMouse_Keyboard;

namespace XboxKeyboardMouse
{
    class TranslateInput
    {
        public static X360Controller translateInput(X360Controller controller)
        {
            TranslateKeyboard.translateKeyboard(controller);
            TranslateMouse.translateMouse(controller);
            return controller;
        }
    }
}
