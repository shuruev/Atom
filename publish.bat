@ECHO OFF
SETLOCAL

ECHO -------------------------
DIR nuget /B
ECHO -------------------------
ECHO.

SET /P Check="OK to publish to nuget.org? (Y/N) "
IF /I "%Check%" NEQ "Y" GOTO END

SET nuget=%UserProfile%\.nuget\packages\nuget.commandline\5.4.0\tools\NuGet.exe

FOR %%I IN (nuget\*.nupkg) DO (
    "%nuget%" push "%%~fI" -Source https://api.nuget.org/v3/index.json -SkipDuplicate
)

PAUSE

:END
ENDLOCAL
