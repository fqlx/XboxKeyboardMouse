using System;

namespace XboxKeyboardMouse.Libs
{
    class Logger {

        public enum Type {
            Info,
            Debug,
            Error,
            SCPBus,
            Controller
        }

        /// <summary>
        /// This will append text into the multiple logging systems that is used throughout the server!
        /// </summary>
        /// <param name="log">The type of log category this fits into!</param>
        /// <param name="Tag">The left hand statement (NULL or "" = no tag)</param>
        /// <param name="Text">The text to write</param>
        /// <param name="newLine">This dictates if a newline will be appended at the end... If using your own set to false.</param>
        public static void Write(Type log, string Tag, string Text, bool newLine = false) => appendLog(Tag, Text, log, newLine);

        /// <summary>
        /// This will append text into the multiple logging systems that is used throughout the server!
        /// </summary>
        /// <param name="log">The type of log category this fits into!</param>
        /// <param name="Tag">The left hand statement (NULL or "" = no tag)</param>
        /// <param name="Text">The text to write</param>
        public static void WriteLine(Type log, string Tag, string Text) => appendLog(Tag, Text, log, true);

        /// <summary>
        /// This will append text into the multiple logging systems that is used throughout the server!
        /// </summary>
        /// <param name="log">The type of log category this fits into!</param>
        /// <param name="Text">The text to write</param>
        /// <param name="newLine">This dictates if a newline will be appended at the end... If using your own set to false.</param>
        public static void Write(Type log, string Text, bool newLine = false) => appendLog(null, Text, log, newLine);

        /// <summary>
        /// This will append text into the multiple logging systems that is used throughout the server!
        /// </summary>
        /// <param name="log">The type of log category this fits into!</param>
        /// <param name="Text">The text to write</param>
        public static void WriteLine(Type log, string Text) => appendLog(null, Text, log, true);

        /// <summary>
        /// This will append text into the multiple logging systems that is used throughout the server!
        /// </summary>
        /// <param name="tag">The left hand statement (NULL or "" = no tag)</param>
        /// <param name="txt">The text to write</param>
        /// <param name="log">The type of log category this fits into!</param>
        public static void appendLogLine(string tag, string txt, Type log) => appendLog(tag, txt, log, true);


        /// <summary>
        /// This will append text into the multiple logging systems that is used throughout the server!
        /// </summary>
        /// <param name="tag">The left hand statement (NULL or "" = no tag)</param>
        /// <param name="txt">The text to write</param>
        /// <param name="appendNewLine">This dictates if a newline will be appended at the end... If using your own set to false.</param>
        /// <param name="log">The type of log category this fits into!</param>
        public static void appendLog(string tag, string txt, Type log, bool appendNewLine = true) {
        #if (DEBUG)
            try {
                string
                    time = DateTime.Now.ToString("h:mm:sstt"),
                    outp = txt,
                    logText = (log.ToString());

                if (!string.IsNullOrWhiteSpace(tag)) {
                    if (log == Type.Error)           Console.ForegroundColor = ConsoleColor.Red;
                    else if (log == Type.Debug)      Console.ForegroundColor = ConsoleColor.Yellow;
                    else if (log == Type.SCPBus)     Console.ForegroundColor = ConsoleColor.Green;
                    else if (log == Type.Controller) Console.ForegroundColor = ConsoleColor.Cyan;

                    Console.Write(string.Format("{0}   {1}   {2}", time, logText.PadRight(10, ' '), tag).PadRight(35, ' '));

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" ");
                } else {
                    Console.ForegroundColor = ConsoleColor.White;
                }

                string finalText = txt + (appendNewLine ? "\n" : "");
                Console.Write(txt + (appendNewLine ? "\n" : ""));

                System.Diagnostics.Debugger.Log(0, "", finalText + (appendNewLine ? "" : "\n"));
            } catch { }
        #endif
        }
    }
}
