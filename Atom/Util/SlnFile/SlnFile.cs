using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#nullable enable

namespace Atom.Util
{
    /// <summary>
    /// Parse or create Visual Studio Solution (*.sln) files.
    /// </summary>
    public static class SlnFile
    {
        /// <summary>
        /// Quick wrapper for reading *.sln files, using the default line-break handling specific to the current OS and runtime.
        /// Use <see cref="ParseLines"/> when more granular control is needed.
        /// </summary>
        public static SlnFileData ReadFromFile(string path) => ParseLines(File.ReadAllLines(path));

        /// <summary>
        /// Parses textual lines for the *.sln file into in-memory solution data.
        /// </summary>
        public static SlnFileData ParseLines(IEnumerable<string> lines)
        {
            var data = new SlnFileData();

            SlnFileProject? currentProject = null;
            SlnFileSection? currentProjectSection = null;
            SlnFileSection? currentGlobalSection = null;
            foreach (var line in lines)
            {
                if (String.IsNullOrEmpty(line))
                    continue;

                if (currentProject != null)
                {
                    if (currentProjectSection != null)
                    {
                        if (ParseProjectSectionEnd(line))
                        {
                            currentProject.ProjectSections.Add(currentProjectSection);
                            currentProjectSection = null;
                            continue;
                        }

                        var property = ParseSectionPropertyLine(line);
                        currentProjectSection.Properties.Add(property);
                        continue;
                    }

                    if (ParseProjectSectionBegin(line, out var projSec))
                    {
                        currentProjectSection = projSec;
                        continue;
                    }

                    if (ParseProjectEnd(line))
                    {
                        data.Projects.Add(currentProject);
                        currentProject = null;
                        continue;
                    }
                }

                if (currentGlobalSection != null)
                {
                    if (ParseGlobalSectionEnd(line))
                    {
                        data.GlobalSections.Add(currentGlobalSection);
                        currentGlobalSection = null;
                        continue;
                    }

                    var property = ParseSectionPropertyLine(line);
                    currentGlobalSection.Properties.Add(property);
                    continue;
                }

                if (ParseHeaderLine(data.Header, line))
                    continue;

                if (ParseProjectBegin(line, out var proj))
                {
                    currentProject = proj;
                    continue;
                }

                if (ParseGlobal(line))
                    continue;

                if (ParseGlobalSectionBegin(line, out var globSec))
                {
                    currentGlobalSection = globSec;
                    continue;
                }

                throw new InvalidOperationException($"Unexpected solution line: {line}");
            }

            return data;
        }

        private const string FormatVersionPrefix = "Microsoft Visual Studio Solution File, Format Version ";
        private const string CurrentVisualStudioVersionPrefix = "# Visual Studio Version ";
        private const string FullVisualStudioVersionPrefix = "VisualStudioVersion = ";
        private const string MinimumVisualStudioVersionPrefix = "MinimumVisualStudioVersion = ";
        private const string ProjectBeginPrefix = "Project(";
        private const string ProjectEnd = "EndProject";
        private const string ProjectSectionBeginPrefix = "ProjectSection(";
        private const string ProjectSectionEnd = "EndProjectSection";
        private const string GlobalBegin = "Global";
        private const string GlobalEnd = "EndGlobal";
        private const string GlobalSectionBeginPrefix = "GlobalSection(";
        private const string GlobalSectionEnd = "EndGlobalSection";

        private static bool ParseHeaderLine(SlnFileHeader header, string line)
        {
            if (line.StartsWith(FormatVersionPrefix))
            {
                header.FormatVersion = line.Replace(FormatVersionPrefix, String.Empty);
                return true;
            }

            if (line.StartsWith(CurrentVisualStudioVersionPrefix))
            {
                header.CurrentVisualStudioVersion = line.Trim(' ', '#');
                return true;
            }

            if (line.StartsWith(FullVisualStudioVersionPrefix))
            {
                header.FullVisualStudioVersion = line.Replace(FullVisualStudioVersionPrefix, String.Empty);
                return true;
            }

            if (line.StartsWith(MinimumVisualStudioVersionPrefix))
            {
                header.MinimumVisualStudioVersion = line.Replace(MinimumVisualStudioVersionPrefix, String.Empty);
                return true;
            }

            return false;
        }

        private static bool ParseProjectBegin(string line, out SlnFileProject? project)
        {
            if (!line.StartsWith(ProjectBeginPrefix))
            {
                project = null;
                return false;
            }

            var parts = line.Split('"');
            project = new SlnFileProject
            {
                TypeId = parts[1],
                Name = parts[3],
                Location = parts[5],
                ProjectId = parts[7]
            };

            return true;
        }

        private static bool ParseProjectEnd(string line) =>
            line.Equals(ProjectEnd);

        private static bool ParseGlobal(string line) =>
            line.Equals(GlobalBegin) || line.Equals(GlobalEnd);

        private static bool ParseProjectSectionBegin(string line, out SlnFileSection? section) =>
            ParseSectionBegin(ProjectSectionBeginPrefix, line, out section);

        private static bool ParseProjectSectionEnd(string line)
            => ParseSectionEnd(ProjectSectionEnd, line);

        private static bool ParseGlobalSectionBegin(string line, out SlnFileSection? section) =>
            ParseSectionBegin(GlobalSectionBeginPrefix, line, out section);

        private static bool ParseGlobalSectionEnd(string line)
            => ParseSectionEnd(GlobalSectionEnd, line);

        private static bool ParseSectionBegin(string beginPrefix, string line, out SlnFileSection? section)
        {
            line = line.TrimStart();
            if (!line.StartsWith(beginPrefix))
            {
                section = null;
                return false;
            }

            var parts = line.Substring(beginPrefix.Length).Split(") = ");
            section = new SlnFileSection
            {
                Name = parts[0],
                Type = parts[1]
            };

            return true;
        }

        private static bool ParseSectionEnd(string end, string line) => line.TrimStart().Equals(end);

        private static SlnFileProperty ParseSectionPropertyLine(string line)
        {
            line = line.TrimStart();

            if (!line.Contains(" = "))
                throw new InvalidOperationException("Cannot find expected section property line.");

            var parts = line.Split(" = ");
            return new SlnFileProperty
            {
                Name = parts[0],
                Value = parts[1]
            };
        }

        /// <summary>
        /// Quick wrapper for writing *.sln files, using the default line-break handling specific to the current OS and runtime.
        /// Use <see cref="RenderLines"/> when more granular control is needed.
        /// </summary>
        public static void WriteToFile(SlnFileData data, string path) => File.WriteAllLines(path, RenderLines(data));

        /// <summary>
        /// Renders in-memory solution data into a list of textual lines for the *.sln file.
        /// </summary>
        public static List<string> RenderLines(SlnFileData data)
        {
            var list = new List<string>();
            list.AddRange(RenderHeader(data.Header));
            list.AddRange(data.Projects.SelectMany(RenderProject));
            list.Add("Global");
            list.AddRange(data.GlobalSections.SelectMany(RenderGlobalSection));
            list.Add("EndGlobal");
            return list;
        }

        private static List<string> RenderHeader(SlnFileHeader header)
        {
            if (String.IsNullOrEmpty(header.FormatVersion))
                throw new ArgumentNullException(nameof(header.FormatVersion));
            if (String.IsNullOrEmpty(header.CurrentVisualStudioVersion))
                throw new ArgumentNullException(nameof(header.CurrentVisualStudioVersion));
            if (String.IsNullOrEmpty(header.FullVisualStudioVersion))
                throw new ArgumentNullException(nameof(header.FullVisualStudioVersion));
            if (String.IsNullOrEmpty(header.MinimumVisualStudioVersion))
                throw new ArgumentNullException(nameof(header.MinimumVisualStudioVersion));

            return new()
        {
            $"Microsoft Visual Studio Solution File, Format Version {header.FormatVersion}",
            $"# {header.CurrentVisualStudioVersion}",
            $"VisualStudioVersion = {header.FullVisualStudioVersion}",
            $"MinimumVisualStudioVersion = {header.MinimumVisualStudioVersion}"
        };
        }

        private static List<string> RenderProject(SlnFileProject project)
        {
            if (String.IsNullOrEmpty(project.TypeId))
                throw new ArgumentNullException(nameof(project.TypeId));
            if (String.IsNullOrEmpty(project.Name))
                throw new ArgumentNullException(nameof(project.Name));
            if (String.IsNullOrEmpty(project.Location))
                throw new ArgumentNullException(nameof(project.Location));
            if (String.IsNullOrEmpty(project.ProjectId))
                throw new ArgumentNullException(nameof(project.ProjectId));

            var list = new List<string>();
            list.Add($"Project(\"{project.TypeId}\") = \"{project.Name}\", \"{project.Location}\", \"{project.ProjectId}\"");
            list.AddRange(project.ProjectSections.SelectMany(RenderProjectSection));
            list.Add("EndProject");
            return list;
        }

        private static List<string> RenderProjectSection(SlnFileSection section) => RenderSection("Project", section);
        private static List<string> RenderGlobalSection(SlnFileSection section) => RenderSection("Global", section);

        private static List<string> RenderSection(string code, SlnFileSection section)
        {
            if (String.IsNullOrEmpty(section.Name))
                throw new ArgumentNullException(nameof(section.Name));
            if (String.IsNullOrEmpty(section.Type))
                throw new ArgumentNullException(nameof(section.Type));

            var list = new List<string>();
            list.Add($"\t{code}Section({section.Name}) = {section.Type}");
            list.AddRange(section.Properties.Select(p => $"\t\t{p.Name} = {p.Value}"));
            list.Add($"\tEnd{code}Section");
            return list;
        }

        /// <summary>
        /// Wraps the solution data into <see cref="SlnFileBrowser"/> that allows reading through
        /// the data in a cached manner.
        /// </summary>
        public static SlnFileBrowser Browse(this SlnFileData data) => new(data);
    }

    /// <summary>
    /// Separate class which deconstructs the <see cref="SlnFileData"/> data into some cached representation
    /// for quicker processing. Should be discarded once any contents of the <see cref="SlnFileData"/> was changed.
    /// </summary>
    public class SlnFileBrowser
    {
        /// <summary>
        /// Gets underlying solution file data.
        /// </summary>
        /// <remarks>
        /// Note that the cached browsing contents might become invalid after any part of this data is changed.
        /// </remarks>
        public SlnFileData Data { get; }

        private readonly Lazy<Dictionary<string, SlnFileProject>> _projectInfos;
        private readonly Lazy<HashSet<string>> _solutionFolders;
        private readonly Lazy<Dictionary<string, string>> _nestedProjects;

        public SlnFileBrowser(SlnFileData data)
        {
            Data = data;

            _projectInfos = new Lazy<Dictionary<string, SlnFileProject>>(LoadProjectInfos);
            _solutionFolders = new Lazy<HashSet<string>>(LoadSolutionFolders);
            _nestedProjects = new Lazy<Dictionary<string, string>>(LoadNestedProjects);
        }

        private Dictionary<string, SlnFileProject> LoadProjectInfos() =>
            Data.Projects.ToDictionary(p => p.ProjectId);

        private HashSet<string> LoadSolutionFolders() =>
            Data.Projects
                .Where(p => p.TypeId == ProjectTypeGuids.SolutionFolder)
                .Select(p => p.ProjectId)
                .ToHashSet();

        private Dictionary<string, string> LoadNestedProjects() =>
            Data.GlobalSections
                .Where(s => s.Name == SlnFileSectionNames.NestedProjects)
                .SelectMany(s => s.Properties)
                .ToDictionary(p => p.Name, p => p.Value);

        private SlnFileProject GetProject(string projectId) => _projectInfos.Value[projectId];

        /// <summary>
        /// Gets a name for any project within a solution file.
        /// </summary>
        /// <remarks>
        /// Note the "project" here may not necessarily be a source code project, e.g. it can be a solution folder.
        /// See <see cref="ProjectTypeGuids"/> for some known project types.
        /// </remarks>
        public string GetProjectName(string projectId) => GetProject(projectId).Name;

        /// <summary>
        /// Gets a location for any project within a solution file.
        /// </summary>
        /// <remarks>
        /// Note the "project" here may not necessarily be a source code project, e.g. it can be a solution folder.
        /// See <see cref="ProjectTypeGuids"/> for some known project types.
        /// </remarks>
        public string GetProjectLocation(string projectId) => GetProject(projectId).Location;

        /// <summary>
        /// Gets solution path for the specified project (the name of the specified project will not be included).
        /// </summary>
        /// <remarks>
        /// Note the "project" here may not necessarily be a source code project, e.g. it can be a solution folder.
        /// See <see cref="ProjectTypeGuids"/> for some known project types.
        /// </remarks>
        public string GetSolutionPath(string projectId)
        {
            if (!_nestedProjects.Value.TryGetValue(projectId, out var parentId))
                return "/";

            return GetFullPath(parentId);
        }

        /// <summary>
        /// Gets full path of nested projects, including specified project name.
        /// </summary>
        /// <remarks>
        /// Note the "project" here may not necessarily be a source code project, e.g. it can be a solution folder.
        /// See <see cref="ProjectTypeGuids"/> for some known project types.
        /// </remarks>
        public string GetFullPath(string projectId)
        {
            var name = GetProjectName(projectId);
            var path = GetSolutionPath(projectId);
            return path == "/"
                ? $"/{name}"
                : $"{path}/{name}";
        }

        /// <summary>
        /// Gets a list of all solution folders defined within a current solution.
        /// </summary>
        public List<string> GetSolutionFolders()
        {
            return _solutionFolders.Value
                .Select(GetFullPath)
                .OrderBy(i => i)
                .ToList();
        }

        /// <summary>
        /// Gets a list of all projects defined within a current solution.
        /// </summary>
        /// <remarks>
        /// Note the "project" here may not necessarily be a source code project, e.g. it can be a solution folder.
        /// See <see cref="ProjectTypeGuids"/> for some known project types.
        /// </remarks>
        public List<string> GetAllProjects()
        {
            return Data.Projects
                .OrderBy(p => p.Name)
                .Select(p => p.ProjectId)
                .ToList();
        }

        /// <summary>
        /// Gets a list of all projects defined within a current solution,
        /// that point to a file with a specific extension.
        /// </summary>
        /// <remarks>
        /// The extension can be specified with or without dot (e.g. <c>".csproj"</c> or just <c>"csproj"</c>),
        /// and compared in a case-insensitive manner.
        /// Use <c>"."</c> or <c>""</c> (<see cref="String.Empty"/>) for files without extensions.
        /// </remarks>
        public List<string> GetProjectsByExtension(string extension)
        {
            if (!String.IsNullOrEmpty(extension))
            {
                extension = extension.TrimStart('.');
                if (!String.IsNullOrEmpty(extension))
                    extension = $".{extension}";
            }

            return Data.Projects
                .Where(p => Path.GetExtension(p.Location).Equals(extension, StringComparison.OrdinalIgnoreCase))
                .OrderBy(p => p.Name)
                .Select(p => p.ProjectId)
                .ToList();
        }
    }
}
