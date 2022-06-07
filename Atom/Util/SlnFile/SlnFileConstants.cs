#nullable enable

namespace Atom.Util
{
    /// <summary>
    /// Some common built-in section names used in Visual Studio solution files.
    /// </summary>
    public static class SlnFileSectionNames
    {
        public const string ProjectConfigurationPlatforms = "ProjectConfigurationPlatforms";
        public const string ProjectDependencies = "ProjectDependencies";
        public const string SolutionConfigurationPlatforms = "SolutionConfigurationPlatforms";
        public const string SolutionItems = "SolutionItems";
        public const string SolutionNotes = "SolutionNotes";
        public const string SolutionProperties = "SolutionProperties";
        public const string NestedProjects = "NestedProjects";
        public const string ExtensibilityGlobals = "ExtensibilityGlobals";
    }

    /// <summary>
    /// Some common built-in section types used in Visual Studio solution files.
    /// </summary>
    public static class SlnFileSectionTypes
    {
        public const string PreProject = "preProject";
        public const string PostProject = "postProject";
        public const string PreSolution = "preSolution";
        public const string PostSolution = "postSolution";
    }

    /// <summary>
    /// Some common strings that are often used for section properties or values.
    /// </summary>
    public static class SlnFileSectionProps
    {
        public const string HideSolutionNode = "HideSolutionNode";
        public const string SolutionGuid = "SolutionGuid";
        public const string DebugAnyCpu = "Debug|Any CPU";
        public const string ReleaseAnyCpu = "Release|Any CPU";
        public const string DebugX64 = "Debug|x64";
        public const string ReleaseX64 = "Release|x64";
        public const string DebugX86 = "Debug|x86";
        public const string ReleaseX86 = "Release|x86";
        public const string ActiveCfg = "ActiveCfg";
        public const string Build0 = "Build.0";
    }
}
