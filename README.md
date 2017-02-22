#XboxKeyboardMouse
Keyboard and mouse for Xbox One streaming to Windows 10.

It sends keyboard and mouse inputs to an emulated Xbox controller using SCP driver.

![Screenshot](https://cloud.githubusercontent.com/assets/6545688/23099037/50091be8-f655-11e6-838c-b94d7a62572b.PNG)

#Prereq
1.  You need SCPToolkit driver installed to use
2.  You need SlimDX Runtime installed (x86)
3.  https://github.com/nefarius/ScpToolkit
4.  https://slimdx.org/download.php

#Download
1. Go to download folder
2. Get the latest exe
3. 'View Raw'

#Current Layout
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

#Lag
1.  Run ScpToolkit Settings Manager
2.  Use asynchronoous HID Report Processing
3.  Disable Rumble and Native Feed

#Compile
1.  SlimDx SDK installed - https://slimdx.org/
2.  SCPToolkit driver installed

#Could be better
1.  Problems translating mouse movement into the right stick directional look.  Very challenging to make it a smooth experience.  
    - Was altered from the original to be smoother but still a 50/50 experience.
2.  Live preview improvements (Nicer gui?)

#Todo
1.  Check if SlimDX and SCP driver are installed and if not throw an understandable error.
2.  Work on smoothing mouse translation/normalization
3.  Mouse input buttons dont work as of yet.
4.  Scroll Wheel movement has not been added (Main usage maybe RTS).
5.  You cant use the keyboard for triggers... Yet
6.  Add Jenkins job to compile merges
