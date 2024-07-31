namespace Atom.Tests.Util;

public class NormalizedPathTest
{
    [Test]
    public void Basic_Properties()
    {
        var dir = new NormalizedPath("/MyDirectory/");
        dir.IsDirectory.Should().BeTrue();
        dir.IsRoot.Should().BeFalse();
        dir.RawPath.Should().Be("/MyDirectory/");
        dir.InnerPath.Should().Be("MyDirectory");

        var file = new NormalizedPath("/MyDirectory/MyFile.txt");
        file.IsDirectory.Should().BeFalse();
        file.IsRoot.Should().BeFalse();
        file.RawPath.Should().Be("/MyDirectory/MyFile.txt");
        file.InnerPath.Should().Be("MyDirectory/MyFile.txt");
    }

    [Test]
    public void Root_Path()
    {
        NormalizedPath.Root.RawPath.Should().Be("/");
        NormalizedPath.Root.InnerPath.Should().Be(String.Empty);
        NormalizedPath.Root.IsRoot.Should().BeTrue();
        NormalizedPath.Root.IsDirectory.Should().BeTrue();

        NormalizedPath.Root.Should().Be("/");
        NormalizedPath.Root.GetParent().Should().Be("/");

        NormalizedPath.New().Should().Be(NormalizedPath.Root);
    }

    [Test]
    public void Throws_If_Value_Is_Null()
    {
        this.Invoking(_ => new NormalizedPath(null!))
            .Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'path')")
            .And.ParamName.Should().Be("path");
    }

    [TestCase("", true)]
    [TestCase(" ", true)]
    [TestCase(" /", true)]
    [TestCase("/ ", false)]
    [TestCase(" / ", true)]
    public void Throws_If_Value_Does_Not_Start_With_Slash(string path, bool error)
    {
        if (error)
        {
            this.Invoking(_ => new NormalizedPath(path))
                .Should().Throw<ArgumentException>()
                .WithMessage($"Path '{path}' is expected to start with '/'.*")
                .And.ParamName.Should().Be("path");
        }
        else
        {
            this.Invoking(_ => new NormalizedPath(path)).Should().NotThrow();
        }
    }

    [TestCase("//", "//")]
    [TestCase("/ /", null)]
    [TestCase("/ / ", null)]
    [TestCase("//", "//")]
    [TestCase("// ", "//")]
    [TestCase("//MyDirectory/", "//")]
    [TestCase("/MyDirectory//", "//")]
    [TestCase("/MyDirectory//MyFile.txt", "//")]
    [TestCase("/Path\\", "\\")]
    [TestCase("/Path:", ":")]
    [TestCase("/Path*", "*")]
    [TestCase("/Path?", "?")]
    [TestCase("/Path\"", "\"")]
    [TestCase("/Path<", "<")]
    [TestCase("/Path>", ">")]
    [TestCase("/Path|", "|")]
    public void Throws_If_Value_Contains_Invalid_Characters(string path, string? invalid)
    {
        if (invalid != null)
        {
            this.Invoking(_ => new NormalizedPath(path))
                .Should().Throw<ArgumentException>()
                .WithMessage($"Path '{path}' is not expected to contain '{invalid}'.*")
                .And.ParamName.Should().Be("path");
        }
        else
        {
            this.Invoking(_ => new NormalizedPath(path)).Should().NotThrow();
        }
    }

    [TestCase("/", "/")]
    [TestCase("/MyDirectory/", "/")]
    [TestCase("/MyFile", "/")]
    [TestCase("/MyDirectory/MyFile.txt", "/MyDirectory/")]
    [TestCase("/MyDirectory/SubDir/", "/MyDirectory/")]
    [TestCase("/MyDirectory/SubDir/MyFile.txt", "/MyDirectory/SubDir/")]
    public void Get_Parent(string path, string parent)
    {
        new NormalizedPath(path).GetParent().Should().Be(parent);
    }

    [Test]
    public void Append_Directory()
    {
        NormalizedPath.New()
            .AppendDirectory("MyDirectory")
            .AppendDirectory("SubDir")
            .Should().Be("/MyDirectory/SubDir/");

        NormalizedPath.New()
            .AppendDirectory("/A")
            .AppendDirectory("//B")
            .AppendDirectory("C/")
            .AppendDirectory("D//")
            .AppendDirectory("///E///")
            .Should().Be("/A/B/C/D/E/");

        NormalizedPath.New("/A/B/")
            .AppendDirectory("/C/D/E")
            .Should().Be("/A/B/C/D/E/");

        this.Invoking(_ => new NormalizedPath(NormalizedPath.New("/MyFile.txt").AppendDirectory("A")))
            .Should().Throw<InvalidOperationException>()
            .WithMessage("This method can only be used for paths that represent a directory, but the '/MyFile.txt' does not.");
    }

    [Test]
    public void Append_File()
    {
        NormalizedPath.New()
            .AppendDirectory("MyDirectory")
            .AppendFile("MyFile.txt")
            .Should().Be("/MyDirectory/MyFile.txt");

        NormalizedPath.New().AppendDirectory("/A").AppendFile("B").Should().Be("/A/B");
        NormalizedPath.New().AppendDirectory("/A").AppendFile("/B").Should().Be("/A/B");
        NormalizedPath.New().AppendDirectory("/A").AppendFile("//B").Should().Be("/A/B");
        NormalizedPath.New().AppendDirectory("/A").AppendFile("///B").Should().Be("/A/B");

        NormalizedPath.New("/A/B/")
            .AppendFile("/C/D/E")
            .Should().Be("/A/B/C/D/E");

        this.Invoking(_ => new NormalizedPath(NormalizedPath.New("/MyFile.txt").AppendFile("A")))
            .Should().Throw<InvalidOperationException>()
            .WithMessage("This method can only be used for paths that represent a directory, but the '/MyFile.txt' does not.");

        this.Invoking(_ => new NormalizedPath(NormalizedPath.New().AppendFile("A/")))
            .Should().Throw<ArgumentException>()
            .WithMessage("The provided value 'A/' more looks like a directory rather than file name. (Parameter 'fileName')")
            .And.ParamName.Should().Be("fileName");

        this.Invoking(_ => new NormalizedPath(NormalizedPath.New().AppendFile("A/B/")))
            .Should().Throw<ArgumentException>()
            .WithMessage("The provided value 'A/B/' more looks like a directory rather than file name. (Parameter 'fileName')")
            .And.ParamName.Should().Be("fileName");
    }

    [Test]
    public void Can_Use_Conversions()
    {
        NormalizedPath path1 = "/MyDirectory/MyFile.txt";
        path1.RawPath.Should().Be("/MyDirectory/MyFile.txt");

        string path2 = new NormalizedPath("/MyDirectory/MyFile.txt");
        path2.Should().Be("/MyDirectory/MyFile.txt");
    }

    [Test]
    public void Can_Apply_Sort()
    {
        var paths = new NormalizedPath[] {
            "/B/C.txt",
            "/A/B/C.txt",
            "/A/B.txt",
            "/A.txt"
        };

        paths.OrderBy(p => p).Should().Equal(
            new NormalizedPath("/A.txt"),
            new NormalizedPath("/A/B.txt"),
            new NormalizedPath("/A/B/C.txt"),
            new NormalizedPath("/B/C.txt")
        );
    }
}
