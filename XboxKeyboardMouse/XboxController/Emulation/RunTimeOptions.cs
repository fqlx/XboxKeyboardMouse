using System.Windows.Input;

namespace XboxKeyboardMouse
{
    public enum RunTimeOptionType
    {
        CalibrateDeadZone,
        FineTuneDeadZone,
    }

    public delegate void RunTimeOptionDelegate();

    public class RunTimeOption
    {
        public Key Key { get; set; }
        public RunTimeOptionDelegate Run { get; set; }
        public bool Ran { get; set; }
    }
}
