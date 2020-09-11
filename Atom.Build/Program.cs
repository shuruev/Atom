using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Atom.Util;

namespace Atom.Build
{
    public static class Program
    {
        private static bool _prerelease = false;

        public static void Main()
        {
            //_prerelease = true;

            if (_prerelease)
            {
                XConsole
                    .NewPara()
                    .Warning.WriteLine("Prerelease mode is used")
                    .Yellow.WriteLine("Don't forget to turn it off and build packages again")
                    .NewPara();
            }

            var modules = Modules.Load();

            foreach (var module in modules)
            {
                XConsole.Write($"Process \"{module.Key}\"... ");
                var updated = ProcessModule(module.Key, module.Value);

                if (updated)
                {
                    XConsole.Warning.WriteLine("UPDATED");
                }
                else
                {
                    XConsole.Green.WriteLine("OK");
                }
            }

            XConsole.Write("Update pack scripts... ");
            UpdatePackBat(modules);
            XConsole.Green.WriteLine("OK");

            Modules.Save(modules);
            XConsole.NewPara().WriteLine("Done.");
        }

        private static bool ProcessModule(string name, Module info)
        {
            var modulePath = $"../../../../Atom/{info.Category}/{name}";
            var moduleFiles = Directory.GetFiles(modulePath);

            var codeFiles = moduleFiles.Where(f => Path.GetExtension(f) == ".cs").OrderBy(f => f).ToList();
            var sourceHash = new HashBuilder().AddRange(codeFiles.Select(File.ReadAllText)).GetHash();

            // normalize tags, if needed
            info.Tags = info.Tags.AsCommaList().ToCommaList();

            // sort dependencies, if needed
            if (info.DependsOn != null)
                info.DependsOn = info.DependsOn.OrderBy(i => i).ToArray();

            // if source files have changed then increase build number
            var updated = false;
            if (sourceHash != info.SourceHash)
            {
                var ver = new Version(info.Version);
                var upd = new Version(ver.Major, ver.Minor, ver.Build + 1);

                info.Version = upd.ToString();
                info.SourceHash = sourceHash;
                updated = true;
            }

            var nuspecFile = Path.Combine(modulePath, $"Atom.{name}.nuspec");
            var nuspecXml = CreateNuspecXml(name, info, codeFiles);
            File.WriteAllText(nuspecFile, nuspecXml);

            return updated;
        }

        private static string CreateNuspecXml(string name, Module info, IReadOnlyCollection<string> codeFiles)
        {
            var doc = XDoc.New(XDocDeclaration.UTF8)
                .AddNamespace("ng", "http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd");

            var package = new XElement(doc.Name("ng", "package"));

            var metadata = new XElement(doc.Name("ng", "metadata"));

            metadata.Add(new XElement(doc.Name("ng", "id"), $"Atom.{name}"));
            metadata.Add(new XElement(doc.Name("ng", "version"), _prerelease ? $"{info.Version}-v{DateTime.Now:MddHHmm}" : info.Version));
            metadata.Add(new XElement(doc.Name("ng", "releaseNotes"), info.ReleaseNotes));

            metadata.Add(new XElement(doc.Name("ng", "tags"), $"atom, {info.Category.ToLower()}, {info.Tags}"));
            metadata.Add(new XElement(doc.Name("ng", "description"), info.Description + $@"

Add this package to your core library, so it would get the source code of this module without installing a binary dependency. Then proceed with using corresponding functionality from Atom.{info.Category} namespace, like if it was installed using binary assembly.

Check out GitHub for more docs and usage examples."));

            metadata.Add(new XElement(doc.Name("ng", "authors"), "Oleg Shuruev"));
            metadata.Add(new XElement(doc.Name("ng", "projectUrl"), "https://github.com/shuruev/Atom"));
            metadata.Add(new XElement(doc.Name("ng", "icon"), "images/icon.png"));

            metadata.Add(new XElement(
                doc.Name("ng", "license"),
                new XAttribute("type", "expression"),
                "MIT"));

            var contentFiles = new XElement(doc.Name("ng", "contentFiles"));
            foreach (var codeFile in codeFiles)
            {
                var fileName = Path.GetFileName(codeFile);
                contentFiles.Add(new XElement(
                    doc.Name("ng", "files"),
                    new XAttribute("include", $"any/any/{fileName}"),
                    new XAttribute("buildAction", "Compile"),
                    new XAttribute("copyToOutput", "false")));
            }

            metadata.Add(contentFiles);

            if (info.DependsOn != null)
            {
                var dependencies = new XElement(doc.Name("ng", "dependencies"));

                foreach (var item in info.DependsOn)
                {
                    var parts = item.Split(' ');
                    var id = parts[0];
                    var version = parts[1];

                    var dependency = new XElement(
                        doc.Name("ng", "dependency"),
                        new XAttribute("id", id),
                        new XAttribute("version", version));

                    if (id.StartsWith("Atom."))
                    {
                        dependency.Add(new XAttribute("include", "contentFiles"));
                    }

                    dependencies.Add(dependency);
                }

                metadata.Add(dependencies);
            }

            var files = new XElement(doc.Name("ng", "files"));
            files.Add(new XElement(
                doc.Name("ng", "file"),
                new XAttribute("src", "../../../icon.png"),
                new XAttribute("target", "images/")));

            foreach (var codeFile in codeFiles)
            {
                var fileName = Path.GetFileName(codeFile);
                files.Add(new XElement(
                    doc.Name("ng", "file"),
                    new XAttribute("src", $"{fileName}"),
                    new XAttribute("target", $"contentFiles/any/any/Atom/{info.Category}/")));
            }

            package.Add(metadata);
            package.Add(files);

            doc.Add(package);

            return doc.ToXml(XDocFormatting.Indented);
        }

        private static void UpdatePackBat(Modules modules)
        {
            var sb = new StringBuilder(@"@ECHO OFF
SETLOCAL

IF EXIST ""nuget\*"" DEL ""nuget\*"" /Q

dotnet build
dotnet test

");

            foreach (var module in modules)
            {
                var pack = $@"dotnet pack Atom\Atom.csproj /p:NuspecFile={module.Value.Category}\{module.Key}\Atom.{module.Key}.nuspec --no-build --output nuget";
                sb.AppendLine(pack);
            }

            sb.AppendLine(@"
ENDLOCAL
PAUSE");

            File.WriteAllText("../../../../pack.bat", sb.ToString());
        }
    }
}
