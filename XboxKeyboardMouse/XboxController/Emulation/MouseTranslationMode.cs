using System;

namespace XboxKeyboardMouse
{
    public enum MouseTranslationMode : int
    {
        INVALID = -1,
        Percentage = 0,
        Relative = 1,
        Raw = 2,
        RawSens = 3,
        NONE = 4,
        DeadZoning = 5,
    }
}
