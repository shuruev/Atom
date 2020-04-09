@ECHO OFF
SETLOCAL

SET nuget=%UserProfile%\.nuget\packages\nuget.commandline\5.4.0\tools\NuGet.exe

IF EXIST "nuget\*" DEL "nuget\*" /Q

"%nuget%" pack Atom\Util\Base64Url\Atom.Base64Url.nuspec -OutputDirectory nuget
"%nuget%" pack Atom\Util\CheckType\Atom.CheckType.nuspec -OutputDirectory nuget
"%nuget%" pack Atom\Util\Parse\Atom.Parse.nuspec -OutputDirectory nuget

ENDLOCAL
PAUSE
