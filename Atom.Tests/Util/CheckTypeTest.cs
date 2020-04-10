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
    }
}
