using System;
using System.Collections.Generic;

#nullable enable

namespace Atom.Util
{
    /// <summary>
    /// Plain DTO class representing logical contents for the *.sln file.
    /// </summary>
    /// <remarks>
    /// This may not represent full variety of exotic solution files, but should cover
    /// most of the existing solutions (and can be extended when needed).
    /// </remarks>
    public class SlnFileData
    {
        /// <summary>
        /// Solution file header
        /// </summary>
        public SlnFileHeader Header { get; set; } = new();
        /// <summary>
        /// A list of projects defined within a solution
        /// </summary>
        public List<SlnFileProject> Projects { get; set; } = new();
        /// <summary>
        /// A list of global sections defined within a solution
        /// </summary>
        public List<SlnFileSection> GlobalSections { get; set; } = new();
    }

    /// <summary>
    /// Plain DTO class representing logical contents for the solution header.
    /// </summary>
    public class SlnFileHeader
    {
        /// <summary>
        /// File format version string, e.g. "12.00"
        /// </summary>
        public string FormatVersion { get; set; } = String.Empty;
        /// <summary>
        /// The major version of Visual Studio that (most recently) saved this solution file, e.g. "Visual Studio Version 17"
        /// </summary>
        public string CurrentVisualStudioVersion { get; set; } = String.Empty;
        /// <summary>
        /// The full version of Visual Studio that (most recently) saved the solution file, e.g. "17.2.32519.379".
        /// Won't be change if the solution file is saved by a newer version of Visual Studio that has the same major version.
        /// </summary>
        public string FullVisualStudioVersion { get; set; } = String.Empty;
        /// <summary>
        /// The minimum (oldest) version of Visual Studio that can open this solution file, e.g. "10.0.40219.1".
        /// </summary>
        public string MinimumVisualStudioVersion { get; set; } = String.Empty;

        /// <summary>
        /// Returns some reasonable defaults that an average solution file could use.
        /// </summary>
        public static SlnFileHeader Default() => new()
        {
            FormatVersion = "12.00",
            CurrentVisualStudioVersion = "Visual Studio Version 17",
            FullVisualStudioVersion = "17.0.31710.8",
            MinimumVisualStudioVersion = "10.0.40219.1"
        };
    }

    /// <summary>
    /// Plain DTO class representing logical contents for the solution project.
    /// </summary>
    public class SlnFileProject
    {
        /// <summary>
        /// Project type GUID
        /// </summary>
        public string TypeId { get; set; } = String.Empty;
        /// <summary>
        /// Unique project GUID
        /// </summary>
        public string ProjectId { get; set; } = String.Empty;
        /// <summary>
        /// Project name, e.g. "MyProject"
        /// </summary>
        public string Name { get; set; } = String.Empty;
        /// <summary>
        /// Project location, e.g. "MyProject\MyProject.csproj"
        /// </summary>
        public string Location { get; set; } = String.Empty;
        /// <summary>
        /// A list of project sections
        /// </summary>
        public List<SlnFileSection> ProjectSections { get; set; } = new();

        public SlnFileProject()
        {
        }

        public SlnFileProject(string typeId, string projectId, string name, string location)
        {
            TypeId = typeId;
            ProjectId = projectId;
            Name = name;
            Location = location;
        }
    }

    /// <summary>
    /// Plain DTO class representing logical contents for the solution section.
    /// Can describe both <c>ProjectSection</c> and <c>GlobalSection</c> contents, as they use identical internal structure.
    /// </summary>
    public class SlnFileSection
    {
        /// <summary>
        /// Section name, e.g. "SolutionItems", "ProjectConfigurationPlatforms", "ExtensibilityGlobals", etc.
        /// </summary>
        public string Name { get; set; } = String.Empty;
        /// <summary>
        /// Section type, e.g. "preProject", "postProject", "preSolution", etc.
        /// </summary>
        public string Type { get; set; } = String.Empty;
        /// <summary>
        /// A list of custom properties whose contents may depend on a section.
        /// </summary>
        public List<SlnFileProperty> Properties { get; set; } = new();

        public SlnFileSection()
        {
        }

        public SlnFileSection(string name, string type)
        {
            Name = name;
            Type = type;
        }
    }

    /// <summary>
    /// Plain DTO class representing logical contents for a property key/value pair used within solution.
    /// Can be found under both <c>ProjectSection</c> and <c>GlobalSection</c> contents.
    /// </summary>
    public class SlnFileProperty
    {
        public string Name { get; set; } = String.Empty;
        public string Value { get; set; } = String.Empty;

        public SlnFileProperty()
        {
        }

        public SlnFileProperty(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
