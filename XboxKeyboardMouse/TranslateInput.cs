using ScpDriverInterface;
using System.Threading;
using XboxKeyboardMouse;

namespace XboxKeyboardMouse
{
    class TranslateInput
    {
        public static X360Controller StartTranslate(X360Controller controller)
        {
            var threadMouse = new Thread(() => TranslateMouse.StartMouse(controller));

            TranslateKeyboard.StartKeyboard(controller);
            return controller;
        }
    }
}
