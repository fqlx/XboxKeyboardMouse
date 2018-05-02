using System;

namespace XboxKeyboardMouse
{
    /// <summary>Calibrator for Dead Zone settings.</summary>
    public class DeadZoneCalibrator
    {
        /// <summary>Cap dead zones somewhere nearing short max value to avoid short overflows.</summary>
        private const short MaxRespectedDeadZoneSize = 30000;

        /// <summary>The current dead zone size being tested by the calibrator.</summary>
        /// <remarks>
        /// In the end, this should be nearer to matching the game's true dead zone. Some example
        /// values found with the calibrator:
        ///   5500 for Halo 1
        ///   5950 for GoW 4 (when in-game "Inner Dead Zone setting" is at default of "10")
        ///   0    for GoW 4 (when in-game "Inner Dead Zone setting" is at "0" instead; best for mouse here)
        ///   9650 for Minecraft (both new and XboxOne ed.)
        /// </remarks>
        public short CurrentDeadZone { get; set; }

        private short CalibrationIncrement { get; set; }

        public DeadZoneCalibrator(short startingDeadZoneSize, short startingIncrementSize)
        {
            this.CurrentDeadZone = startingDeadZoneSize;
            this.CalibrationIncrement = startingIncrementSize;
        }

        public int AdvanceDeadZoneSize()
        {
            this.CurrentDeadZone += this.CalibrationIncrement;
            
            // Cap to avoid short conversion issues when user leaves calibrator running too long.
            if (this.CurrentDeadZone > MaxRespectedDeadZoneSize)
                this.CurrentDeadZone = MaxRespectedDeadZoneSize;

            return this.CurrentDeadZone;
        }
    }
}
