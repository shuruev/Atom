@ECHO OFF
SETLOCAL

IF EXIST "nuget\*" DEL "nuget\*" /Q

dotnet build
dotnet test

dotnet pack Atom\Atom.csproj /p:NuspecFile=Util\AsyncUtil\Atom.AsyncUtil.nuspec --no-build --output nuget
dotnet pack Atom\Atom.csproj /p:NuspecFile=AWS\AWSClient\Atom.AWSClient.nuspec --no-build --output nuget
dotnet pack Atom\Atom.csproj /p:NuspecFile=AWS\AWSQueue\Atom.AWSQueue.nuspec --no-build --output nuget
dotnet pack Atom\Atom.csproj /p:NuspecFile=Azure\AzureBlob\Atom.AzureBlob.nuspec --no-build --output nuget
dotnet pack Atom\Atom.csproj /p:NuspecFile=Azure\AzureClient\Atom.AzureClient.nuspec --no-build --output nuget
dotnet pack Atom\Atom.csproj /p:NuspecFile=Azure\AzureQueue\Atom.AzureQueue.nuspec --no-build --output nuget
dotnet pack Atom\Atom.csproj /p:NuspecFile=Util\Base64Url\Atom.Base64Url.nuspec --no-build --output nuget
dotnet pack Atom\Atom.csproj /p:NuspecFile=Util\Batch\Atom.Batch.nuspec --no-build --output nuget
dotnet pack Atom\Atom.csproj /p:NuspecFile=Util\CheckType\Atom.CheckType.nuspec --no-build --output nuget
dotnet pack Atom\Atom.csproj /p:NuspecFile=Util\CommaList\Atom.CommaList.nuspec --no-build --output nuget
dotnet pack Atom\Atom.csproj /p:NuspecFile=Util\HashBuilder\Atom.HashBuilder.nuspec --no-build --output nuget
dotnet pack Atom\Atom.csproj /p:NuspecFile=Util\NormalizedPath\Atom.NormalizedPath.nuspec --no-build --output nuget
dotnet pack Atom\Atom.csproj /p:NuspecFile=Util\Parse\Atom.Parse.nuspec --no-build --output nuget
dotnet pack Atom\Atom.csproj /p:NuspecFile=Util\PathUtil\Atom.PathUtil.nuspec --no-build --output nuget
dotnet pack Atom\Atom.csproj /p:NuspecFile=Util\RunProcess\Atom.RunProcess.nuspec --no-build --output nuget
dotnet pack Atom\Atom.csproj /p:NuspecFile=Util\SlnFile\Atom.SlnFile.nuspec --no-build --output nuget
dotnet pack Atom\Atom.csproj /p:NuspecFile=Util\TypeName\Atom.TypeName.nuspec --no-build --output nuget
dotnet pack Atom\Atom.csproj /p:NuspecFile=Util\VSConstants\Atom.VSConstants.nuspec --no-build --output nuget
dotnet pack Atom\Atom.csproj /p:NuspecFile=Util\XConsole\Atom.XConsole.nuspec --no-build --output nuget
dotnet pack Atom\Atom.csproj /p:NuspecFile=Util\XDoc\Atom.XDoc.nuspec --no-build --output nuget

ENDLOCAL
PAUSE
