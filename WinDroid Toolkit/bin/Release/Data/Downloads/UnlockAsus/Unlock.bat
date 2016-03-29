@echo off

::One-click Zenfone 2 unlock by Sorg
::used some parts of adb/fastboot script made by Social-Design-Concepts

cd tools

:try
call :check_status
if "%status%"=="FASTBOOT-ONLINE" goto :start_recovery
if "%status%"=="ADB-ONLINE" (
	echo Rebooting to bootloader.
	adb reboot bootloader
) else (
	echo waiting for connection...
)
ping -n 3 127.0.0.1 >nul
goto :try

:start_recovery
echo Starting unlock...
fastboot oem start_partitioning >nul 2>nul
fastboot flash /tmp/start.sh unlock1 >nul 2>nul
fastboot flash /tmp/recovery.launcher unlock2 >nul 2>nul
fastboot flash /system/bin/logcat unlock3 >nul 2>nul
fastboot flash /tmp/unlock unlock4 >nul 2>nul
fastboot oem stop_partitioning >nul 2>nul

echo Done.
echo.
echo Bootloader should do the rest unlocking actions 
echo and then will reboot and become unlocked 
echo if boot screen become inverted.
echo.
pause
GOTO:EOF

:check_status
    set tmp=""

    set adbchk="List of devices attached"
    set adbchk2="unknown"
    set fbchk=""
    set deviceinfo=UNKNOWN

:CHECK_ADB
    set tmp=""
    for /f "tokens=1-4" %%a in ( 'adb devices ^2^> nul' ) do (set tmp="%%a %%b %%c %%d")
    if /i %tmp% == %adbchk% ( goto CHECK_FB )
    if /i not %tmp% == %adbchk% ( goto CHECK_AUTHORIZATION )
    set tmp=""
GOTO:EOF

:CHECK_FB
    set tmp=""
    for /f "tokens=1-2" %%a in ( 'fastboot devices ^2^> nul' ) do (set tmp="%%a %%b")
    if /i %tmp% == %fbchk% (set status=UNKNOWN)
    if /i not %tmp% == %fbchk% (set status=FASTBOOT-ONLINE&for /f "tokens=1-2" %%a in ('fastboot devices ^2^> nul' ) do ( set deviceinfo=%%a %%b))
    set tmp=""
GOTO:EOF

:CHECK_AUTHORIZATION
    set tmp=""
    for /f "tokens=1" %%a in ( 'adb get-serialno ^2^> nul' ) do (set tmp="%%a")
    if /i %tmp% == %adbchk2% ( set status=UNAUTHORIZED&for /f "tokens=1-2" %%a in ('adb devices ^2^> nul' ) do ( set deviceinfo=%%a %%b ))
    if /i not %tmp% == %adbchk2% ( set status=ADB-ONLINE&for /f "tokens=1-2" %%a in ('adb devices ^2^> nul' ) do ( set deviceinfo=%%a %%b ))
    set tmp=""
GOTO:EOF
:EOF
