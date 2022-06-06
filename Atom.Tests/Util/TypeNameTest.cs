namespace Atom.Tests.Util;

public class TypeNameTest
{
    private enum TestEnum
    {
    }

    [Test]
    public void Get_Keyword()
    {
        TypeName.GetKeyword<char>().Should().Be("char");
        TypeName.GetKeyword<char?>().Should().Be("char?");
        TypeName.GetKeyword<bool>().Should().Be("bool");
        TypeName.GetKeyword<bool?>().Should().Be("bool?");
        TypeName.GetKeyword<byte>().Should().Be("byte");
        TypeName.GetKeyword<byte?>().Should().Be("byte?");
        TypeName.GetKeyword<sbyte>().Should().Be("sbyte");
        TypeName.GetKeyword<sbyte?>().Should().Be("sbyte?");
        TypeName.GetKeyword<short>().Should().Be("short");
        TypeName.GetKeyword<short?>().Should().Be("short?");
        TypeName.GetKeyword<ushort>().Should().Be("ushort");
        TypeName.GetKeyword<ushort?>().Should().Be("ushort?");
        TypeName.GetKeyword<int>().Should().Be("int");
        TypeName.GetKeyword<int?>().Should().Be("int?");
        TypeName.GetKeyword<uint>().Should().Be("uint");
        TypeName.GetKeyword<uint?>().Should().Be("uint?");
        TypeName.GetKeyword<long>().Should().Be("long");
        TypeName.GetKeyword<long?>().Should().Be("long?");
        TypeName.GetKeyword<ulong>().Should().Be("ulong");
        TypeName.GetKeyword<ulong?>().Should().Be("ulong?");
        TypeName.GetKeyword<float>().Should().Be("float");
        TypeName.GetKeyword<float?>().Should().Be("float?");
        TypeName.GetKeyword<double>().Should().Be("double");
        TypeName.GetKeyword<double?>().Should().Be("double?");
        TypeName.GetKeyword<decimal>().Should().Be("decimal");
        TypeName.GetKeyword<decimal?>().Should().Be("decimal?");

        TypeName.GetKeyword<DateTime>().Should().BeNull();
        TypeName.GetKeyword<DateTime?>().Should().BeNull();
        TypeName.GetKeyword<TimeSpan>().Should().BeNull();
        TypeName.GetKeyword<TimeSpan?>().Should().BeNull();
        TypeName.GetKeyword<Guid>().Should().BeNull();
        TypeName.GetKeyword<Guid?>().Should().BeNull();
        TypeName.GetKeyword<TestEnum>().Should().BeNull();
        TypeName.GetKeyword<TestEnum?>().Should().BeNull();

        TypeName.GetKeyword<Exception>().Should().BeNull();
        TypeName.GetKeyword<List<int>>().Should().BeNull();
        TypeName.GetKeyword<List<int?>>().Should().BeNull();
        TypeName.GetKeyword<List<Guid>>().Should().BeNull();
        TypeName.GetKeyword<List<Guid?>>().Should().BeNull();
        TypeName.GetKeyword<List<TestEnum>>().Should().BeNull();
        TypeName.GetKeyword<List<TestEnum?>>().Should().BeNull();

        TypeName.GetKeyword(typeof(object)).Should().Be("object");
        TypeName.GetKeyword(typeof(string)).Should().Be("string");
        TypeName.GetKeyword(typeof(void)).Should().Be("void");
    }

    [Test]
    public void Get_Friendly_Name()
    {
        TypeName.GetFriendlyName<int>().Should().Be("int");
        TypeName.GetFriendlyName<byte?>().Should().Be("byte?");
        TypeName.GetFriendlyName<DateTime>().Should().Be("DateTime");
        TypeName.GetFriendlyName<Guid?>().Should().Be("Guid?");
        TypeName.GetFriendlyName<TestEnum>().Should().Be("TestEnum");
        TypeName.GetFriendlyName<TestEnum?>().Should().Be("TestEnum?");
        TypeName.GetFriendlyName<Exception>().Should().Be("Exception");

        TypeName.GetFriendlyName<int[]>().Should().Be("int[]");
        TypeName.GetFriendlyName<byte?[]>().Should().Be("byte?[]");
        TypeName.GetFriendlyName<DateTime[]>().Should().Be("DateTime[]");
        TypeName.GetFriendlyName<Guid?[]>().Should().Be("Guid?[]");
        TypeName.GetFriendlyName<TestEnum[]>().Should().Be("TestEnum[]");
        TypeName.GetFriendlyName<TestEnum?[]>().Should().Be("TestEnum?[]");
        TypeName.GetFriendlyName<Exception[]>().Should().Be("Exception[]");

        TypeName.GetFriendlyName<List<int>>().Should().Be("List<int>");
        TypeName.GetFriendlyName<List<byte?>>().Should().Be("List<byte?>");
        TypeName.GetFriendlyName<List<DateTime>>().Should().Be("List<DateTime>");
        TypeName.GetFriendlyName<List<Guid?>>().Should().Be("List<Guid?>");
        TypeName.GetFriendlyName<List<TestEnum>>().Should().Be("List<TestEnum>");
        TypeName.GetFriendlyName<List<TestEnum?>>().Should().Be("List<TestEnum?>");
        TypeName.GetFriendlyName<List<Exception>>().Should().Be("List<Exception>");

        TypeName.GetFriendlyName<Dictionary<string, List<int>>>().Should().Be("Dictionary<string, List<int>>");
        TypeName.GetFriendlyName<Dictionary<string, List<byte?>>>().Should().Be("Dictionary<string, List<byte?>>");
        TypeName.GetFriendlyName<Dictionary<string, List<DateTime>>>().Should().Be("Dictionary<string, List<DateTime>>");
        TypeName.GetFriendlyName<Dictionary<string, List<Guid?>>>().Should().Be("Dictionary<string, List<Guid?>>");
        TypeName.GetFriendlyName<Dictionary<string, List<TestEnum>>>().Should().Be("Dictionary<string, List<TestEnum>>");
        TypeName.GetFriendlyName<Dictionary<string, List<TestEnum?>>>().Should().Be("Dictionary<string, List<TestEnum?>>");
        TypeName.GetFriendlyName<Dictionary<string, List<Exception>>>().Should().Be("Dictionary<string, List<Exception>>");

        TypeName.GetFriendlyName(typeof(object)).Should().Be("object");
        TypeName.GetFriendlyName(typeof(string)).Should().Be("string");
        TypeName.GetFriendlyName(typeof(void)).Should().Be("void");
    }
}
