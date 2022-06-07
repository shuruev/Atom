#nullable enable

namespace Atom.Util
{
    /// <summary>
    /// Some known GUIDs for common project types.
    /// </summary>
    public static class ProjectTypeGuids
    {
        /// <summary>Azure Cloud Service project (*.ccproj)</summary>
        public const string AzureCloudService = "{CC5FD16D-436D-48AD-A40C-5A424C6E3E79}";

        /// <summary>Azure Service Fabric project (*.sfproj)</summary>
        public const string AzureServiceFabric = "{A07B5EB6-E848-4116-A8D0-A826331D98C6}";

        /// <summary>C++ or Visual C++ project (*.vcproj or *.vcxproj)</summary>
        public const string Cpp = "{8BC9CEB8-8B4A-11D0-8D11-00A0C91BC942}";

        /// <summary>C# project (*.csproj)</summary>
        /// <remarks>Newer type based on the Common Project System (CPS)</remarks>
        public const string CSharp = "{9A19103F-16F7-4668-BE54-9A1E7A4F7556}";

        /// <summary>C# project (*.csproj)</summary>
        /// <remarks>Legacy type based on the native project system</remarks>
        public const string CSharpLegacy = "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}";

        /// <summary>Database project (*.dbproj)</summary>
        public const string Database = "{C8D11400-126E-41CD-887F-60BD40844F9E}";

        /// <summary>Default (or unknown) project</summary>
        /// <remarks>Was introduced for the Common Project System (CPS)</remarks>
        public const string Default = "{13B669BE-BB05-4DDF-9536-439F39A36129}";

        /// <summary>F# project (*.fsproj)</summary>
        /// <remarks>Newer type based on the Common Project System (CPS)</remarks>
        public const string FSharp = "{6EC3EE1D-3C4E-46DD-8F32-0CC8E7565705}";

        /// <summary>F# project (*.fsproj)</summary>
        /// <remarks>Legacy type based on the native project system</remarks>
        public const string FSharpLegacy = "{F2A71F9B-5D33-465A-A702-920D77279786}";

        /// <summary>J# project (*.vjsproj)</summary>
        public const string JSharp = "{E6FDF86B-F3D1-11D4-8576-0002A516ECE8}";

        /// <summary>LightSwitch project (*.lsproj)</summary>
        public const string LightSwitch = "{ECD6D718-D1CF-4119-97F3-97C25A0DFBF9}";

        /// <summary>MSI installer project (*.vdproj)</summary>
        public const string MsiInstaller = "{54435603-DBB4-11D2-8724-00A0C9A8B90C}";

        /// <summary>MS Test project (*.csproj)</summary>
        /// <remarks>Legacy project type used to indicate unit-test projects</remarks>
        public const string MsTest = "{3AC096D0-A1C2-E12C-1390-A8335801FDAB}";

        /// <summary>Nemerle project (*.nproj)</summary>
        public const string Nemerle = "{EDCC3B85-0BAD-11DB-BC1A-00112FDE8B61}";

        /// <summary>.NET Core project (*.xproj)</summary>
        /// <remarks>Deprecated project type that was used in the early .NET Core versions</remarks>
        public const string NetCore = "{8BB2217D-0F2D-49D1-97BC-3654ED321F3B}";

        /// <summary>Node JS project (*.njsproj)</summary>
        public const string NodeJs = "{9092AA53-FB77-4645-B42D-1CCCA6BD08BD}";

        /// <summary>NuProj project (*.nuproj)</summary>
        public const string NuProj = "{FF286327-C783-4F7A-AB73-9BCBAD0D4460}";

        /// <summary>Python project (*.pyproj)</summary>
        public const string Python = "{888888A0-9F3D-457C-B088-3A5042F75D52}";

        /// <summary>Scope SDK project (*.scopeproj)</summary>
        public const string ScopeSdk = "{202899A3-C531-4771-9089-0213D66978AE}";

        /// <summary>Silverlight project (*.csproj)</summary>
        public const string Silverlight = "{A1591282-1198-4647-A2B1-27E5FF5F6F3B}";

        /// <summary>Shared project (*.shproj)</summary>
        public const string SharedProject = "{D954291E-2A0B-460D-934E-DC6B0785DB48}";

        /// <summary>Visual Studio solution folder</summary>
        public const string SolutionFolder = "{2150E333-8FDC-42A3-9474-1A3956D46DE8}";

        /// <summary>SQL Server Data Tools project (*.sqlproj)</summary>
        public const string SqlDataTools = "{00D1A9C2-B5F0-4AF3-8072-F6C62B433612}";

        /// <summary>SQL Replication project (*.synproj)</summary>
        public const string SqlReplication = "{BBD0F5D1-1CC4-42FD-BA4C-A96779C64378}";

        /// <summary>Universal Windows App project (*.jsproj)</summary>
        public const string UniversalApp = "{262852C6-CD72-467D-83FE-5EEB1973A190}";

        /// <summary>Universal Windows Class Library project (*.csproj)</summary>
        public const string UniversalLibrary = "{A5A43C5B-DE2A-4C0C-9213-0A381AF9435A}";

        /// <summary>Unloaded project</summary>
        public const string UnloadedProject = "{67294A52-A4F0-11D2-AA88-00C04F688DDE}";

        /// <summary>Visual Basic project (*.vbproj)</summary>
        /// <remarks>Newer type based on the Common Project System (CPS)</remarks>
        public const string VisualBasic = "{778DAE3C-4631-46EA-AA77-85C1314464D9}";

        /// <summary>Visual Basic project (*.vbproj)</summary>
        /// <remarks>Legacy type based on the native project system</remarks>
        public const string VisualBasicLegacy = "{F184B08F-C81C-45F6-A57F-5ABD9991F28F}";

        /// <summary>Windows Application Packaging project (*.wapproj)</summary>
        public const string Wap = "{C7167F0D-BC9F-4E6E-AFE1-012C56B48DB5}";

        /// <summary>Windows Communication Foundation project</summary>
        public const string Wcf = "{3D9AD99F-2412-4246-B90B-4EAA41C64699}";

        /// <summary>Windows Presentation Foundation project</summary>
        public const string Wpf = "{60DC8134-EBA5-43B8-BCC9-BB4BC16C2548}";

        /// <summary>Windows Workflow Foundation project</summary>
        public const string Wwf = "{32F31D43-81CC-4C15-9DE6-3FC5453562B6}";

        /// <summary>ASP.NET Web Application project</summary>
        /// <remarks>Sub-type used for many various project types depending on the language</remarks>
        public const string WebApplication = "{349C5851-65DF-11DA-9384-00065B846F21}";

        /// <summary>Web Deployment project (*.wdproj)</summary>
        public const string WebDeployment = "{2CFEAB61-6A3B-4EB8-B523-560B4BEEF521}";

        /// <summary>Web Site project (*.webproj)</summary>
        public const string WebSite = "{E24C65DC-7377-472B-9ABA-BC803B73C61A}";

        /// <summary>WiX installer project (*.wixproj)</summary>
        public const string WixInstaller = "{930C7802-8A8C-48F9-8165-68863BCCD9DD}";
    }
}
