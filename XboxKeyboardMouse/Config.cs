using System.Windows.Input;

namespace XboxKeyboardMouse
{
    class Config
    {
        private class Input 
        {
            private class LeftStick
            {
                private Key up;
                private Key down;
                private Key right;
                private Key left;
            }
            
            private class Buttons
            {
                private Key a;
                private Key b;
                private Key x;
                private Key y;
            }

            private class Bumpers
            {
                private Key right;
                private Key left;
            }
            
            private class Triggers
            {
                private uint right;
                private uint left;
            }
            
            private class StickClick
            {
                private Key right;
                private Key left;
            }    
            
            private class Dpad
            {
                private Key up;
                private Key down;
                private Key right;
                private Key left;
            }
            
            private Key menu;  //aka Start
            private Key view;  //aka Back or Select or Nav
            private Key home;  //aka Guide 
        }
        
        private class App
        {
            private uint framePerTicks;
            
          //private uint innerDeadzone
          //private uint outerDeadzone;
        }
        
        public static void LoadConfig()
        {
            
        }
        
      //todo getters and setters
    }
}
