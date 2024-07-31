using System.Collections;

// ReSharper disable ClassNeverInstantiated.Local
namespace Atom.Tests.Util;

public class CheckTypeTest
{
    private enum TestEnum;

    private class ListOfInt : List<int>;

    private class ListOfSomething<T> : List<T>;

    private class ListOfUnknown : IEnumerable
    {
        public IEnumerator GetEnumerator() => throw new NotImplementedException();
    }

    [Test]
    public void Check_Is_Nullable()
    {
        CheckType.IsNullable<object>().Should().BeTrue();
        CheckType.IsNullable<string>().Should().BeTrue();
        CheckType.IsNullable<Exception>().Should().BeTrue();
        CheckType.IsNullable<List<int>>().Should().BeTrue();
        CheckType.IsNullable<byte[]>().Should().BeTrue();

        CheckType.IsNullable<bool>().Should().BeFalse();
        CheckType.IsNullable<int>().Should().BeFalse();
        CheckType.IsNullable<char>().Should().BeFalse();
        CheckType.IsNullable<ulong>().Should().BeFalse();
        CheckType.IsNullable<TestEnum>().Should().BeFalse();

        CheckType.IsNullable<bool?>().Should().BeTrue();
        CheckType.IsNullable<int?>().Should().BeTrue();
        CheckType.IsNullable<char?>().Should().BeTrue();
        CheckType.IsNullable<ulong?>().Should().BeTrue();
        CheckType.IsNullable<TestEnum?>().Should().BeTrue();

        CheckType.IsNullable(typeof(object)).Should().BeTrue();
        CheckType.IsNullable(typeof(void)).Should().BeFalse();
    }

    [Test]
    public void Check_Is_Enum()
    {
        CheckType.IsEnum<object>().Should().BeFalse();
        CheckType.IsEnum<string>().Should().BeFalse();
        CheckType.IsEnum<bool>().Should().BeFalse();
        CheckType.IsEnum<bool?>().Should().BeFalse();

        CheckType.IsEnum<TestEnum>().Should().BeTrue();
        CheckType.IsEnum<List<TestEnum>>().Should().BeFalse();
        CheckType.IsEnum<TestEnum?>().Should().BeTrue();
        CheckType.IsEnum<List<TestEnum?>>().Should().BeFalse();

        CheckType.IsEnum(typeof(object)).Should().BeFalse();
        CheckType.IsEnum(typeof(void)).Should().BeFalse();
    }

    [Test]
    public void Check_Is_Enumerable_Of_Integer()
    {
        // ReSharper disable once InlineOutVariableDeclaration
        Type itemType;

        CheckType.IsEnumerableOf<int>(out _).Should().BeFalse();
        CheckType.IsEnumerableOf<int?>(out _).Should().BeFalse();

        CheckType.IsEnumerableOf<int[]>(out itemType).Should().BeTrue();
        itemType.Should().Be(typeof(int));
        CheckType.IsEnumerableOf<int?[]>(out itemType).Should().BeTrue();
        itemType.Should().Be(typeof(int?));

        CheckType.IsEnumerableOf<IEnumerable<int>>(out itemType).Should().BeTrue();
        itemType.Should().Be(typeof(int));
        CheckType.IsEnumerableOf<IEnumerable<int?>>(out itemType).Should().BeTrue();
        itemType.Should().Be(typeof(int?));

        CheckType.IsEnumerableOf<List<int>>(out itemType).Should().BeTrue();
        itemType.Should().Be(typeof(int));
        CheckType.IsEnumerableOf<List<int?>>(out itemType).Should().BeTrue();
        itemType.Should().Be(typeof(int?));

        CheckType.IsEnumerableOf<ListOfInt>(out itemType).Should().BeTrue();
        itemType.Should().Be(typeof(int));
        CheckType.IsEnumerableOf<ListOfSomething<int>>(out itemType).Should().BeTrue();
        itemType.Should().Be(typeof(int));
        CheckType.IsEnumerableOf<ListOfSomething<int?>>(out itemType).Should().BeTrue();
        itemType.Should().Be(typeof(int?));
    }

    [Test]
    public void Check_Is_Enumerable_Of_String()
    {
        // ReSharper disable once InlineOutVariableDeclaration
        Type itemType;

        CheckType.IsEnumerableOf<string>(out itemType).Should().BeTrue();
        itemType.Should().Be(typeof(char));
        CheckType.IsEnumerableOf<string?>(out itemType).Should().BeTrue();
        itemType.Should().Be(typeof(char));

        CheckType.IsEnumerableOf<string[]>(out itemType).Should().BeTrue();
        itemType.Should().Be(typeof(string));
        CheckType.IsEnumerableOf<string?[]>(out itemType).Should().BeTrue();
        itemType.Should().Be(typeof(string));

        CheckType.IsEnumerableOf<IEnumerable<string>>(out itemType).Should().BeTrue();
        itemType.Should().Be(typeof(string));
        CheckType.IsEnumerableOf<IEnumerable<string?>>(out itemType).Should().BeTrue();
        itemType.Should().Be(typeof(string));

        CheckType.IsEnumerableOf<List<string>>(out itemType).Should().BeTrue();
        itemType.Should().Be(typeof(string));
        CheckType.IsEnumerableOf<List<string?>>(out itemType).Should().BeTrue();
        itemType.Should().Be(typeof(string));

        CheckType.IsEnumerableOf<ListOfSomething<string>>(out itemType).Should().BeTrue();
        itemType.Should().Be(typeof(string));
        CheckType.IsEnumerableOf<ListOfSomething<string?>>(out itemType).Should().BeTrue();
        itemType.Should().Be(typeof(string));
    }

    [Test]
    public void Check_Is_Enumerable_Of_Object()
    {
        // ReSharper disable once InlineOutVariableDeclaration
        Type itemType;

        CheckType.IsEnumerableOf<object>(out _).Should().BeFalse();
        CheckType.IsEnumerableOf<object?>(out _).Should().BeFalse();

        CheckType.IsEnumerableOf<object[]>(out itemType).Should().BeTrue();
        itemType.Should().Be(typeof(object));
        CheckType.IsEnumerableOf<object?[]>(out itemType).Should().BeTrue();
        itemType.Should().Be(typeof(object));

        CheckType.IsEnumerableOf<IEnumerable<object>>(out itemType).Should().BeTrue();
        itemType.Should().Be(typeof(object));
        CheckType.IsEnumerableOf<IEnumerable<object?>>(out itemType).Should().BeTrue();
        itemType.Should().Be(typeof(object));

        CheckType.IsEnumerableOf<List<object>>(out itemType).Should().BeTrue();
        itemType.Should().Be(typeof(object));
        CheckType.IsEnumerableOf<List<object?>>(out itemType).Should().BeTrue();
        itemType.Should().Be(typeof(object));

        CheckType.IsEnumerableOf<ListOfSomething<object>>(out itemType).Should().BeTrue();
        itemType.Should().Be(typeof(object));
        CheckType.IsEnumerableOf<ListOfSomething<object?>>(out itemType).Should().BeTrue();
        itemType.Should().Be(typeof(object));
    }

    [Test]
    public void Check_Is_Enumerable_Of_Other()
    {
        CheckType.IsEnumerableOf(typeof(IEnumerable), out _).Should().BeFalse();
        CheckType.IsEnumerableOf(typeof(IEnumerable<>), out _).Should().BeFalse();
        CheckType.IsEnumerableOf(typeof(ListOfUnknown), out _).Should().BeFalse();
        CheckType.IsEnumerableOf(typeof(Array), out _).Should().BeFalse();
        CheckType.IsEnumerableOf(typeof(List<>), out _).Should().BeFalse();
        CheckType.IsEnumerableOf(typeof(object), out _).Should().BeFalse();
        CheckType.IsEnumerableOf(typeof(void), out _).Should().BeFalse();
    }
}
