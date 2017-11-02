# XboxKeyboardMouse
Keyboard and mouse for Xbox One streaming to Windows 10.

It sends keyboard and mouse inputs to an emulated Xbox controller using SCP driver.

![Screenshot](https://cloud.githubusercontent.com/assets/6545688/23099037/50091be8-f655-11e6-838c-b94d7a62572b.PNG "Image of the GUI as of 11/12/16")

# Prerequisites
1.  [You need SCPToolkit driver installed to use](https://github.com/nefarius/ScpToolkit)
2.  [You need SlimDX Runtime installed (x86)](https://slimdx.org/download.php)

# Download
1. [Download the repository as a zip](https://github.com/fqlx/XboxKeyboardMouse/archive/master.zip)
2. Extract the download folder to anywhere on your pc
3. Rename the download folder to something of your choice
4. Run `XboxKeyboardMouse 24 02 2017 - x86.exe` or `XboxKeyboardMouse 24 02 2017 - x86_64.exe`

# Current Layout
You may customise your own controller layout through the gui but the default layout is 

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
| Escape        | Xbox Logo/Guide   |
|               |                   |
| Arrow Keys    | DPAD              |
|               |                   |
| Left Click    | Right Trigger     |
| Right Click   | Left Trigger      |
|               |                   |
| WASD          | Left Stick        |
| Mouse         | Right Stick       |

# Lag
1.  Run ScpToolkit Settings Manager
2.  Use asynchronoous HID Report Processing
3.  Disable Rumble and Native Feed

# Compile
1.  SlimDx SDK installed - https://slimdx.org/
2.  SCPToolkit driver installed

# Todo
1.  Check if SlimDX and SCP driver are installed and if not throw an understandable error.
2.  Mouse input buttons dont work as of yet.
3.  Scroll Wheel movement has not been added (Main usage maybe RTS).
4.  Better Mouse Engines
