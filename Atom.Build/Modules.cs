using Newtonsoft.Json;

namespace Atom.Build;

public class Modules : SortedDictionary<string, Module>
{
    private const string FILE_PATH = "../../../../Modules.json";

    public static Modules Load()
    {
        var json = File.ReadAllText(FILE_PATH);
        return JsonConvert.DeserializeObject<Modules>(json)!;
    }

    public static void Save(Modules modules)
    {
        var json = JsonConvert.SerializeObject(modules, new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore
        });

        File.WriteAllText(FILE_PATH, json);
    }
}

public class Module
{
    public string Category { get; set; } = "Util";
    public string Description { get; set; } = "The module description goes here.";
    public string? Tags { get; set; }
    public string Version { get; set; } = "1.0.0";
    public string ReleaseNotes { get; set; } = "Initial release.";
    public Guid SourceHash { get; set; }
    public string[]? DependsOn { get; set; }
}
