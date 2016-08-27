Keyboard and mouse for Xbox One streaming to Windows 10.

It sends keyboard and mouse inputs to an emulated Xbox controller using SCP driver.

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
| Keyboard/Mouse| Xbox Controller   |
| ------------- |:-----------------:|
| Space         | A                 |
| F             | B                 |
| E             | X                 |
| Q             | Y                 |
|               |                   |
| 1             | Right Bumper      |
| 2             | Left Bumper       |
|               |                   |
| Left Shift    | Left Stick Click  |
| Left Control  | Right Stick Click |
|               |                   |
| ~             | Start/Menu        |
| B           | Select/Back/Nav   |
| Enter/Return  | Xbox Logo         |
|               |                   |
| Arrow Keys    | DPAD              |
|               |                   |
| Left Click    | Right Trigger     |
| Right Click   | Left Trigger      |
|               |                   |
| WASD          | Left Stick        |
| Mouse         | Right Stick       |

#Lag
1.  Run ScpToolkit Global Configuration
2.  Use asynchronoous HID Report Processing
3.  Disable Rumble and Native Feed

#Compile
1.  SlimDx SDK installed - https://slimdx.org/
2.  SCPToolkit driver installed

#Could be better
1.  Problems translating mouse movement into the right stick directional look.  Very challenging to make it a smooth experience.  

#Todo
1.  Check if SlimDX and SCP driver are installed and if not throw an understandable error.
2.  Add custom configs
3.  Work on smoothing mouse translation/normalization
