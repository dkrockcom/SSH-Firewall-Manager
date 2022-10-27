@ECHO OFF
CLS
ECHO Please select platform to build/Release OSS
ECHO.
ECHO 1.Windows-x64
ECHO 2.Linux-x64
ECHO 3.Linux-arm
ECHO 4.Linux-arm64
ECHO 5.All
ECHO.

CHOICE /C 12345 /M "Enter your choice:"

:: Note - list ERRORLEVELS in decreasing order
IF ERRORLEVEL 5 GOTO All
IF ERRORLEVEL 4 GOTO Linux-arm64
IF ERRORLEVEL 3 GOTO Linux-arm
IF ERRORLEVEL 2 GOTO Linux-x64
IF ERRORLEVEL 1 GOTO Windows-x64

:Windows-x64
ECHO Build Windows-x64
dotnet publish -c release -r win-x64 --self-contained false
GOTO End

:Linux-x64
ECHO Build Linux-x64
dotnet publish -c release -r linux-x64 --self-contained false
GOTO End

:Linux-arm
ECHO Build Linux-arm
dotnet publish -c release -r linux-arm --self-contained false
GOTO End

:Linux-arm64
ECHO Build Linux-arm64
dotnet publish -c release -r linux-arm64 --self-contained false
GOTO End

:All
ECHO Build ALL
dotnet publish -c release -r win-x64 --self-contained false
dotnet publish -c release -r linux-x64 --self-contained false
dotnet publish -c release -r linux-arm --self-contained false
dotnet publish -c release -r linux-arm64 --self-contained false
GOTO End


:End
ECHO.
ECHO Build/Release Process completed.
pause