using ScpDriverInterface;
using XboxMouse_Keyboard;

namespace XboxKeyboardMouse
{
    class TranslateInput
    {
        public static X360Controller TranslateInput(X360Controller controller)
        {
            TranslateKeyboard.TranslateKeyboard(controller);
            TranslateMouse.TranslateMouse(controller);
            return controller;
        }
    }
}
