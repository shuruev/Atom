@ECHO OFF
SETLOCAL

ECHO -------------------------
DIR nuget /B
ECHO -------------------------
ECHO.

SET /P Check="OK to publish all Atom packages to nuget.org? (Y/N) "
IF /I "%Check%" NEQ "Y" GOTO END

SET nuget=%UserProfile%\.nuget\packages\nuget.commandline\5.4.0\tools\NuGet.exe

"%nuget%" push nuget\Atom.Base64Url.1.0.2.nupkg -Source https://api.nuget.org/v3/index.json -SkipDuplicate
"%nuget%" push nuget\Atom.CheckType.1.2.0.nupkg -Source https://api.nuget.org/v3/index.json -SkipDuplicate
"%nuget%" push nuget\Atom.Parse.1.2.0.nupkg -Source https://api.nuget.org/v3/index.json -SkipDuplicate
"%nuget%" push nuget\Atom.TypeName.1.0.0.nupkg -Source https://api.nuget.org/v3/index.json -SkipDuplicate
"%nuget%" push nuget\Atom.XConsole.1.0.0.nupkg -Source https://api.nuget.org/v3/index.json -SkipDuplicate

PAUSE

:END
ENDLOCAL
