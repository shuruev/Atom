using System;
using System.Collections.Generic;
using Atom.Util;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atom.Tests.Util
{
    [TestClass]
    public class CheckTypeTest
    {
        private enum TestEnum
        {
        }

        [TestMethod]
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

        [TestMethod]
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
            CheckType.IsEnum<TestEnum>().Should().BeTrue();
            CheckType.IsEnum(typeof(void)).Should().BeFalse();
        }

        [TestMethod]
        public void Get_Keyword()
        {
            CheckType.GetKeyword<char>().Should().Be("char");
            CheckType.GetKeyword<char?>().Should().Be("char?");
            CheckType.GetKeyword<bool>().Should().Be("bool");
            CheckType.GetKeyword<bool?>().Should().Be("bool?");
            CheckType.GetKeyword<byte>().Should().Be("byte");
            CheckType.GetKeyword<byte?>().Should().Be("byte?");
            CheckType.GetKeyword<sbyte>().Should().Be("sbyte");
            CheckType.GetKeyword<sbyte?>().Should().Be("sbyte?");
            CheckType.GetKeyword<short>().Should().Be("short");
            CheckType.GetKeyword<short?>().Should().Be("short?");
            CheckType.GetKeyword<ushort>().Should().Be("ushort");
            CheckType.GetKeyword<ushort?>().Should().Be("ushort?");
            CheckType.GetKeyword<int>().Should().Be("int");
            CheckType.GetKeyword<int?>().Should().Be("int?");
            CheckType.GetKeyword<uint>().Should().Be("uint");
            CheckType.GetKeyword<uint?>().Should().Be("uint?");
            CheckType.GetKeyword<long>().Should().Be("long");
            CheckType.GetKeyword<long?>().Should().Be("long?");
            CheckType.GetKeyword<ulong>().Should().Be("ulong");
            CheckType.GetKeyword<ulong?>().Should().Be("ulong?");
            CheckType.GetKeyword<float>().Should().Be("float");
            CheckType.GetKeyword<float?>().Should().Be("float?");
            CheckType.GetKeyword<double>().Should().Be("double");
            CheckType.GetKeyword<double?>().Should().Be("double?");
            CheckType.GetKeyword<decimal>().Should().Be("decimal");
            CheckType.GetKeyword<decimal?>().Should().Be("decimal?");

            CheckType.GetKeyword<DateTime>().Should().BeNull();
            CheckType.GetKeyword<DateTime?>().Should().BeNull();
            CheckType.GetKeyword<TimeSpan>().Should().BeNull();
            CheckType.GetKeyword<TimeSpan?>().Should().BeNull();
            CheckType.GetKeyword<Guid>().Should().BeNull();
            CheckType.GetKeyword<Guid?>().Should().BeNull();
            CheckType.GetKeyword<TestEnum>().Should().BeNull();
            CheckType.GetKeyword<TestEnum?>().Should().BeNull();

            CheckType.GetKeyword<Exception>().Should().BeNull();
            CheckType.GetKeyword<List<int>>().Should().BeNull();
            CheckType.GetKeyword<List<int?>>().Should().BeNull();
            CheckType.GetKeyword<List<Guid>>().Should().BeNull();
            CheckType.GetKeyword<List<Guid?>>().Should().BeNull();
            CheckType.GetKeyword<List<TestEnum>>().Should().BeNull();
            CheckType.GetKeyword<List<TestEnum?>>().Should().BeNull();

            CheckType.GetKeyword(typeof(object)).Should().Be("object");
            CheckType.GetKeyword(typeof(string)).Should().Be("string");
            CheckType.GetKeyword(typeof(void)).Should().Be("void");
        }
    }
}
