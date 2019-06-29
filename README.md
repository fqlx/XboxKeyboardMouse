**Help support this project by ⭐️'ing it! 😍**

# XboxKeyboardMouse
Keyboard and mouse for Xbox One streaming to Windows 10.

It sends keyboard and mouse inputs to an emulated Xbox controller using SCP driver.  

Note: This app only starts when the Xbox streaming app is in the foreground.

![Screenshot](https://cloud.githubusercontent.com/assets/6545688/23099037/50091be8-f655-11e6-838c-b94d7a62572b.PNG "Image of the GUI as of 11/12/16")

# Prerequisites
1. [You need SlimDX Runtime installed (x86)](/SlimDX%20Runtime%20.NET%204.0%20x86%20(January%202012).msi?raw=true). (direct download link)

[Original SlimDX website](https://slimdx.org/download.php) (currently down)

# Installation
Simply run the installer: https://github.com/fqlx/XboxKeyboardMouse/raw/master/Installer/setup.exe
* The application will check for updates upon each successive launch.

# Current Layout
You may customize your own controller layout through the GUI but the default layout is:

| Keyboard/Mouse| Xbox Controller   |
| ------------- |:-----------------:|
| Space         | A                 |
| Left Ctrl     | B                 |
| R             | X                 |
| Number 1      | Y                 |
|               |                   |
| Q             | Right Bumper      |
| E             | Left Bumper       |
|               |                   |
| Left Shift    | Left Stick Click  |
| C             | Right Stick Click |
|               |                   |
| B             | Start/Menu        |
| V             | Select/Back/Nav   |
| Tilde         | Xbox Logo/Guide   |
|               |                   |
| Arrow Keys    | DPAD              |
|               |                   |
| Left Click    | Right Trigger     |
| Right Click   | Left Trigger      |
|               |                   |
| WASD          | Left Stick        |
| Mouse         | Right Stick       |
|               |                   |
| F12           | (Set Dead Zone)   |
| F11           | (Adjust Dead Zone)|

# Dead Zones
The default mouse engine has the ability to accommodate for a game's dead zone for effective, accurate mouse control.
However, XboxKeyboardMouse must be calibrated to learn the running game's dead zone. This takes a few seconds.
Once learned, the saved profile will retain the current dead zone size, reducing the need to recalibrate every time, if you frequently return to the same game or games that happen to use very similar dead zones.
To calibrate, follow these instructions, while running a game:
* Use keyboard controls for a moment to ensure your XboxKeyboardMouse is active.
* Check your game menus to see if you have any in-game sensitivity settings (which are common) and/or dead zone settings (which are rare). XboxKeyboardMouse generally works best when in-game sensitivities are at their maximum setting, and in-game dead zones are at their minimum setting.
* Back out of any menus so that your character is ready to aim.
* Press F12 to begin calibration. Wait with your finger over F12 and press it again when your aim starts moving.
* Press F11 to begin fine-tuning the calibration. Wait with your finger over F11 and press it again when your aim starts moving. (If aim started moving right away for the above step, you can repeat the step to try fine tuning again.)
* At this point, XboxKeyboardMouse has learned the game's dead zone size, and even single-pixel mouse movements should now translate into tiny amounts of aim. If you switch games, or aiming doesn't feel natural yet, you can simply repeat the process to recalibrate.

# Lag
1.  Run ScpToolkit Settings Manager
2.  Use asynchronous HID Report Processing
3.  Disable Rumble and Native Feed

# Todo
* Check if SlimDX is installed and if not throw an understandable error.
* Scroll Wheel movement has not been added (Main usage maybe RTS).
