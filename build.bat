@echo off

FOR /f "tokens=1-4 delims=/ " %%a in ('date /t') do (set mm=%%a&set dd=%%b&set yyyy=%%c& (if "%%a:~0,1" gtr "9" set mm=%%b&setdd=%%c&set yyyy=%%d))

REM Get month

if %mm% == 1 (
	set mm=Jan
) else if %mm% == 2 (
	set mm=Feb
) else if %mm% == 3 (
	set mm=Mar
) else if %mm% == 4 (
	set mm=Apr
) else if %mm% == 5 (
	set mm=May
) else if %mm% == 6 (
	set mm=Jun
) else if %mm% == 7 (
	set mm=Jul
) else if %mm% == 8 (
	set mm=Aug
) else if %mm% == 9 (
	set mm=Sep
) else if %mm% == 10 (
	set mm=Oct
) else if %mm% == 11 (
	set mm=Nov
) else if %mm% == 12 (
	set mm=Dec
)

REM setup the msbuild for vs15
REM change the 14.0 to your vs version!
call "C:\Program Files (x86)\Microsoft Visual Studio 14.0\VC\vcvarsall.bat" x86_amd64

echo Building Solution for 32 Bit (x86)...
	echo msbuild "XboxKeyboardMouse.sln" /p:Configuration=Release /p:Platform="x86" /t:Build /p:OutputPath="%~dp0%Downloads"

	REM Build the x32 version
	msbuild "XboxKeyboardMouse.sln" /p:Configuration=Release /p:Platform="x86" /t:Build /p:OutputPath="%~dp0%Downloads"

	REM Delete the output files
	echo Deleting debug files

	REM Setup our special file name
	set fileName=XboxKeyboardMouse %mm% %dd% %yyyy% - x86.exe
	echo Renaming file to %fileName%
	
	REM Rename XboxKeyboardMouse.exe to our desired name
	set renameCommand=move /y "%~dp0
	set renameCommand=%renameCommand%Downloads\XboxKeyboardMouse.exe"
	set renameCommand=%renameCommand% "%~dp0
	set renameCommand=%renameCommand%Downloads\%fileName%"
	echo %renameCommand%
	%renameCommand%
echo. 
echo. 

echo Building Solution for 64 Bit (x86_64)...
	echo msbuild "XboxKeyboardMouse.sln" /p:Configuration=Release /p:Platform="x64" /t:Build /p:OutputPath="%~dp0%Downloads"

	REM Build the x32 version
	msbuild "XboxKeyboardMouse.sln" /p:Configuration=Release /p:Platform="x64" /t:Build /p:OutputPath="%~dp0%Downloads"

	REM Setup our special file name
	set fileName=XboxKeyboardMouse %mm% %dd% %yyyy% - x86_64.exe
	echo Renaming file to %fileName%
	
	REM Rename XboxKeyboardMouse.exe to our desired name
	set renameCommand=move /y "%~dp0
	set renameCommand=%renameCommand%Downloads\XboxKeyboardMouse.exe"
	set renameCommand=%renameCommand% "%~dp0
	set renameCommand=%renameCommand%Downloads\%fileName%"
	echo %renameCommand%
	%renameCommand%
	
rem Finally delete the old files
echo Deleting un-needed files

del "%~dp0%Downloads\SlimDX.dll">NUL
del "%~dp0%Downloads\SlimDX.pdb">NUL
del "%~dp0%Downloads\SlimDX.xml">NUL
del "%~dp0%Downloads\XboxKeyboardMouse.exe.config">NUL
del "%~dp0%Downloads\XboxKeyboardMouse.pdb">NUL
del "%~dp0%Downloads\XboxKeyboardMouse.exe">NUL
del "%~dp0%Downloads\MaterialSkin.pdb">NUL
del "%~dp0%Downloads\MaterialSkinExample.exe">NUL
del "%~dp0%Downloads\MaterialSkinExample.exe.config">NUL
del "%~dp0%Downloads\MaterialSkinExample.pdb">NUL


echo.
echo.
echo.
color A
echo Finished building (Check for any errors!)

rem FINISHED
