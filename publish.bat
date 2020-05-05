@ECHO OFF
SETLOCAL

ECHO -------------------------
DIR nuget /B
ECHO -------------------------
ECHO.

SET /P Check="OK to publish to nuget.org? (Y/N) "
IF /I "%Check%" NEQ "Y" GOTO END

FOR %%I IN (nuget\*.nupkg) DO (
    dotnet nuget push "%%~fI" --source https://api.nuget.org/v3/index.json --skip-duplicate
)

PAUSE

:END
ENDLOCAL
