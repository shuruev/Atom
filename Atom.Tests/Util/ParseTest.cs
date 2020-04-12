using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Atom.Util;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atom.Tests.Util
{
    [TestClass]
    public class ParseStringTest
    {
        private enum TestEnum
        {
            Zero = 0,
            One = 1,
            Two = 2
        }

        private static readonly string[] _empty = { String.Empty, "   ", "\t", "\r\n", null };

        [TestMethod]
        public void Cannot_Parse_Values_Of_Unknown_Type()
        {
            "xml".Invoking(s => s.As<XDocument>())
                .Should().Throw<InvalidOperationException>()
                .WithMessage("Parsing as type <XDocument> is not supported.");
        }

        [TestMethod]
        public void Can_Parse_String_Values()
        {
            "text".As<string>().Should().Be("text");
            String.Empty.As<string>().Should().BeEmpty();
            "   ".As<string>().Should().Be("   ");
            ((string)null).As<string>().Should().BeNull();
        }

        [TestMethod]
        public void Can_Parse_Boolean_Values()
        {
            "true".As<bool>().Should().BeTrue();
            "True".As<bool>().Should().BeTrue();
            "TRUE".As<bool>().Should().BeTrue();
            "TrUe".As<bool>().Should().BeTrue();
            "true   ".As<bool>().Should().BeTrue();

            "false".As<bool>().Should().BeFalse();
            "False".As<bool>().Should().BeFalse();
            "FALSE".As<bool>().Should().BeFalse();
            "FaLsE".As<bool>().Should().BeFalse();
            "   false".As<bool>().Should().BeFalse();

            "yes".Invoking(s => s.As<bool>())
                .Should().Throw<ArgumentException>()
                .WithMessage("Cannot parse value of type <bool> from string 'yes'.");

            foreach (var value in _empty)
            {
                value.Invoking(s => s.As<bool>())
                    .Should().Throw<ArgumentException>()
                    .WithMessage($"Cannot parse value of type <bool> from string '{value}'.");
            }
        }

        [TestMethod]
        public void Can_Parse_Boolean_Nullable_Values()
        {
            "true".As<bool?>().Should().BeTrue();
            "True".As<bool?>().Should().BeTrue();
            "TRUE".As<bool?>().Should().BeTrue();
            "TrUe".As<bool?>().Should().BeTrue();
            "true   ".As<bool?>().Should().BeTrue();

            "false".As<bool?>().Should().BeFalse();
            "False".As<bool?>().Should().BeFalse();
            "FALSE".As<bool?>().Should().BeFalse();
            "FaLsE".As<bool?>().Should().BeFalse();
            "   false".As<bool?>().Should().BeFalse();

            "yes".Invoking(s => s.As<bool?>())
                .Should().Throw<ArgumentException>()
                .WithMessage("Cannot parse value of type <bool?> from string 'yes'.");

            foreach (var value in _empty)
            {
                value.As<bool?>().Should().BeNull();
            }
        }

        [TestMethod]
        public void Can_Parse_Byte_Values()
        {
            "25".As<byte>().Should().Be(25);
            "   25   ".As<byte>().Should().Be(25);

            foreach (var value in new[] { "-38", "1000", "1e2", "NaN" })
            {
                value.Invoking(s => s.As<byte>())
                    .Should().Throw<ArgumentException>()
                    .WithMessage($"Cannot parse value of type <byte> from string '{value}'.");
            }

            foreach (var value in _empty)
            {
                value.Invoking(s => s.As<byte>())
                    .Should().Throw<ArgumentException>()
                    .WithMessage($"Cannot parse value of type <byte> from string '{value}'.");
            }
        }

        [TestMethod]
        public void Can_Parse_Byte_Nullable_Values()
        {
            "25".As<byte?>().Should().Be(25);
            "   25   ".As<byte?>().Should().Be(25);

            foreach (var value in new[] { "-38", "1000", "1e2", "NaN" })
            {
                value.Invoking(s => s.As<byte?>())
                    .Should().Throw<ArgumentException>()
                    .WithMessage($"Cannot parse value of type <byte?> from string '{value}'.");
            }

            foreach (var value in _empty)
                value.As<byte?>().Should().BeNull();
        }

        [TestMethod]
        public void Can_Parse_Int16_Values()
        {
            "25".As<short>().Should().Be(25);
            "   25   ".As<short>().Should().Be(25);
            "-38".As<short>().Should().Be(-38);
            "1000".As<short>().Should().Be(1000);

            foreach (var value in new[] { "100000", "1e2", "NaN" })
            {
                value.Invoking(s => s.As<short>())
                    .Should().Throw<ArgumentException>()
                    .WithMessage($"Cannot parse value of type <short> from string '{value}'.");
            }

            foreach (var value in _empty)
            {
                value.Invoking(s => s.As<short>())
                    .Should().Throw<ArgumentException>()
                    .WithMessage($"Cannot parse value of type <short> from string '{value}'.");
            }
        }

        [TestMethod]
        public void Can_Parse_Int16_Nullable_Values()
        {
            "25".As<short?>().Should().Be(25);
            "   25   ".As<short?>().Should().Be(25);
            "-38".As<short?>().Should().Be(-38);
            "1000".As<short?>().Should().Be(1000);

            foreach (var value in new[] { "100000", "1e2", "NaN" })
            {
                value.Invoking(s => s.As<short?>())
                    .Should().Throw<ArgumentException>()
                    .WithMessage($"Cannot parse value of type <short?> from string '{value}'.");
            }

            foreach (var value in _empty)
                value.As<short?>().Should().BeNull();
        }

        [TestMethod]
        public void Can_Parse_Int32_Values()
        {
            "25".As<int>().Should().Be(25);
            "   25   ".As<int>().Should().Be(25);
            "-38".As<int>().Should().Be(-38);
            "1000".As<int>().Should().Be(1000);
            "100000".As<int>().Should().Be(100000);

            foreach (var value in new[] { "10000000000", "1e2", "NaN" })
            {
                value.Invoking(s => s.As<int>())
                    .Should().Throw<ArgumentException>()
                    .WithMessage($"Cannot parse value of type <int> from string '{value}'.");
            }

            foreach (var value in _empty)
            {
                value.Invoking(s => s.As<int>())
                    .Should().Throw<ArgumentException>()
                    .WithMessage($"Cannot parse value of type <int> from string '{value}'.");
            }
        }

        [TestMethod]
        public void Can_Parse_Int32_Nullable_Values()
        {
            "25".As<int?>().Should().Be(25);
            "   25   ".As<int?>().Should().Be(25);
            "-38".As<int?>().Should().Be(-38);
            "1000".As<int?>().Should().Be(1000);
            "100000".As<int?>().Should().Be(100000);

            foreach (var value in new[] { "10000000000", "1e2", "NaN" })
            {
                value.Invoking(s => s.As<int?>())
                    .Should().Throw<ArgumentException>()
                    .WithMessage($"Cannot parse value of type <int?> from string '{value}'.");
            }

            foreach (var value in _empty)
                value.As<int?>().Should().BeNull();
        }

        [TestMethod]
        public void Can_Parse_Int64_Values()
        {
            "25".As<long>().Should().Be(25);
            "   25   ".As<long>().Should().Be(25);
            "-38".As<long>().Should().Be(-38);
            "1000".As<long>().Should().Be(1000);
            "100000".As<long>().Should().Be(100000);
            "10000000000".As<long>().Should().Be(10000000000);

            foreach (var value in new[] { "10000000000000000000", "1e2", "NaN" })
            {
                value.Invoking(s => s.As<long>())
                    .Should().Throw<ArgumentException>()
                    .WithMessage($"Cannot parse value of type <long> from string '{value}'.");
            }

            foreach (var value in _empty)
            {
                value.Invoking(s => s.As<long>())
                    .Should().Throw<ArgumentException>()
                    .WithMessage($"Cannot parse value of type <long> from string '{value}'.");
            }
        }

        [TestMethod]
        public void Can_Parse_Int64_Nullable_Values()
        {
            "25".As<long?>().Should().Be(25);
            "   25   ".As<long?>().Should().Be(25);
            "-38".As<long?>().Should().Be(-38);
            "1000".As<long?>().Should().Be(1000);
            "100000".As<long?>().Should().Be(100000);
            "10000000000".As<long?>().Should().Be(10000000000);

            foreach (var value in new[] { "10000000000000000000", "1e2", "NaN" })
            {
                value.Invoking(s => s.As<long?>())
                    .Should().Throw<ArgumentException>()
                    .WithMessage($"Cannot parse value of type <long?> from string '{value}'.");
            }

            foreach (var value in _empty)
                value.As<long?>().Should().BeNull();
        }

        [TestMethod]
        public void Can_Parse_Single_Values()
        {
            Parse.AllowNonNumericValues = false;

            "2.5".As<float>().Should().Be(2.5f);
            "   2.5   ".As<float>().Should().Be(2.5f);
            "-0.2".As<float>().Should().Be(-0.2f);
            "-.2".As<float>().Should().Be(-0.2f);
            "1000".As<float>().Should().Be(1000f);
            "3.4e-12".As<float>().Should().Be(3.4e-12f);

            "0".As<float>().Should().Be(0f);
            "+0".As<float>().Should().Be(0f);
            "-0".As<float>().Should().Be(0f);

            foreach (var value in new[] { "NaN", "-Infinity", "Infinity", "+Infinity", "1e39", "2,5" })
            {
                value.Invoking(s => s.As<float>())
                    .Should().Throw<ArgumentException>()
                    .WithMessage($"Cannot parse value of type <float> from string '{value}'.");
            }

            foreach (var value in _empty)
            {
                value.Invoking(s => s.As<float>())
                    .Should().Throw<ArgumentException>()
                    .WithMessage($"Cannot parse value of type <float> from string '{value}'.");
            }
        }

        [TestMethod]
        public void Can_Parse_Single_Nullable_Values()
        {
            Parse.AllowNonNumericValues = false;

            "2.5".As<float?>().Should().Be(2.5f);
            "   2.5   ".As<float?>().Should().Be(2.5f);
            "-0.2".As<float?>().Should().Be(-0.2f);
            "-.2".As<float?>().Should().Be(-0.2f);
            "1000".As<float?>().Should().Be(1000f);
            "3.4e-12".As<float?>().Should().Be(3.4e-12f);

            "0".As<float?>().Should().Be(0f);
            "+0".As<float?>().Should().Be(0f);
            "-0".As<float?>().Should().Be(0f);

            foreach (var value in new[] { "NaN", "-Infinity", "Infinity", "+Infinity", "1e39", "2,5" })
            {
                value.Invoking(s => s.As<float?>())
                    .Should().Throw<ArgumentException>()
                    .WithMessage($"Cannot parse value of type <float?> from string '{value}'.");
            }

            foreach (var value in _empty)
                value.As<float?>().Should().BeNull();
        }

        [TestMethod]
        public void Can_Parse_Single_Non_Numeric_Values()
        {
            Parse.AllowNonNumericValues = true;

            "NaN".As<float>().Should().Be(Single.NaN);
            "-Infinity".As<float>().Should().Be(Single.NegativeInfinity);
            "Infinity".As<float>().Should().Be(Single.PositiveInfinity);
            "+Infinity".As<float>().Should().Be(Single.PositiveInfinity);
            "1e39".As<float>().Should().Be(Single.PositiveInfinity);

            "NaN".As<float?>().Should().Be(Single.NaN);
            "-Infinity".As<float?>().Should().Be(Single.NegativeInfinity);
            "Infinity".As<float?>().Should().Be(Single.PositiveInfinity);
            "+Infinity".As<float?>().Should().Be(Single.PositiveInfinity);
            "1e39".As<float?>().Should().Be(Single.PositiveInfinity);
        }

        [TestMethod]
        public void Can_Parse_Double_Values()
        {
            Parse.AllowNonNumericValues = false;

            "2.5".As<double>().Should().Be(2.5d);
            "   2.5   ".As<double>().Should().Be(2.5d);
            "-0.2".As<double>().Should().Be(-0.2d);
            "-.2".As<double>().Should().Be(-0.2d);
            "1000".As<double>().Should().Be(1000d);
            "3.4e-12".As<double>().Should().Be(3.4e-12d);
            "1e39".As<double>().Should().Be(1e39d);
            "0.12345678901234567890".As<double>().Should().Be(0.12345678901234568d);

            "0".As<double>().Should().Be(0d);
            "+0".As<double>().Should().Be(0d);
            "-0".As<double>().Should().Be(0d);

            foreach (var value in new[] { "NaN", "-Infinity", "Infinity", "+Infinity", "1e309", "2,5" })
            {
                value.Invoking(s => s.As<double>())
                    .Should().Throw<ArgumentException>()
                    .WithMessage($"Cannot parse value of type <double> from string '{value}'.");
            }

            foreach (var value in _empty)
            {
                value.Invoking(s => s.As<double>())
                    .Should().Throw<ArgumentException>()
                    .WithMessage($"Cannot parse value of type <double> from string '{value}'.");
            }
        }

        [TestMethod]
        public void Can_Parse_Double_Nullable_Values()
        {
            Parse.AllowNonNumericValues = false;

            "2.5".As<double?>().Should().Be(2.5d);
            "   2.5   ".As<double?>().Should().Be(2.5d);
            "-0.2".As<double?>().Should().Be(-0.2d);
            "-.2".As<double?>().Should().Be(-0.2d);
            "1000".As<double?>().Should().Be(1000d);
            "3.4e-12".As<double?>().Should().Be(3.4e-12d);
            "1e39".As<double?>().Should().Be(1e39d);

            "0".As<double?>().Should().Be(0d);
            "+0".As<double?>().Should().Be(0d);
            "-0".As<double?>().Should().Be(0d);

            foreach (var value in new[] { "NaN", "-Infinity", "Infinity", "+Infinity", "1e309", "2,5" })
            {
                value.Invoking(s => s.As<double?>())
                    .Should().Throw<ArgumentException>()
                    .WithMessage($"Cannot parse value of type <double?> from string '{value}'.");
            }

            foreach (var value in _empty)
                value.As<double?>().Should().BeNull();
        }

        [TestMethod]
        public void Can_Parse_Double_Non_Numeric_Values()
        {
            Parse.AllowNonNumericValues = true;

            "NaN".As<double>().Should().Be(Double.NaN);
            "-Infinity".As<double>().Should().Be(Double.NegativeInfinity);
            "Infinity".As<double>().Should().Be(Double.PositiveInfinity);
            "+Infinity".As<double>().Should().Be(Double.PositiveInfinity);
            "1e309".As<double>().Should().Be(Double.PositiveInfinity);

            "NaN".As<double?>().Should().Be(Double.NaN);
            "-Infinity".As<double?>().Should().Be(Double.NegativeInfinity);
            "Infinity".As<double?>().Should().Be(Double.PositiveInfinity);
            "+Infinity".As<double?>().Should().Be(Double.PositiveInfinity);
            "1e309".As<double?>().Should().Be(Double.PositiveInfinity);
        }

        [TestMethod]
        public void Can_Parse_Decimal_Values()
        {
            "2.5".As<decimal>().Should().Be(2.5m);
            "   2.5   ".As<decimal>().Should().Be(2.5m);
            "-0.2".As<decimal>().Should().Be(-0.2m);
            "-.2".As<decimal>().Should().Be(-0.2m);
            "1000".As<decimal>().Should().Be(1000m);
            "3.4e-12".As<decimal>().Should().Be(3.4e-12m);
            "0.12345678901234567890".As<decimal>().Should().Be(0.12345678901234567890m);

            "0".As<decimal>().Should().Be(0m);
            "+0".As<decimal>().Should().Be(0m);
            "-0".As<decimal>().Should().Be(0m);

            foreach (var value in new[] { "NaN", "Infinity", "2,5", "100000000000000000000000000000" })
            {
                value.Invoking(s => s.As<decimal>())
                    .Should().Throw<ArgumentException>()
                    .WithMessage($"Cannot parse value of type <decimal> from string '{value}'.");
            }

            foreach (var value in _empty)
            {
                value.Invoking(s => s.As<decimal>())
                    .Should().Throw<ArgumentException>()
                    .WithMessage($"Cannot parse value of type <decimal> from string '{value}'.");
            }
        }

        [TestMethod]
        public void Can_Parse_Decimal_Nullable_Values()
        {
            "2.5".As<decimal?>().Should().Be(2.5m);
            "   2.5   ".As<decimal?>().Should().Be(2.5m);
            "-0.2".As<decimal?>().Should().Be(-0.2m);
            "-.2".As<decimal?>().Should().Be(-0.2m);
            "1000".As<decimal?>().Should().Be(1000m);
            "3.4e-12".As<decimal?>().Should().Be(3.4e-12m);
            "0.12345678901234567890".As<decimal?>().Should().Be(0.12345678901234567890m);

            "0".As<decimal?>().Should().Be(0m);
            "+0".As<decimal?>().Should().Be(0m);
            "-0".As<decimal?>().Should().Be(0m);

            foreach (var value in new[] { "NaN", "Infinity", "2,5", "100000000000000000000000000000" })
            {
                value.Invoking(s => s.As<decimal?>())
                    .Should().Throw<ArgumentException>()
                    .WithMessage($"Cannot parse value of type <decimal?> from string '{value}'.");
            }

            foreach (var value in _empty)
                value.As<decimal?>().Should().BeNull();
        }

        [TestMethod]
        public void Can_Parse_TimeSpan_Values()
        {
            "15:20:57".As<TimeSpan>().Should().Be(TimeSpan.Parse("15:20:57"));
            "   15:20:57   ".As<TimeSpan>().Should().Be(TimeSpan.Parse("15:20:57"));
            "15:20:57.123".As<TimeSpan>().Should().Be(TimeSpan.Parse("15:20:57.123"));
            "39:20:57".As<TimeSpan>().Should().Be(TimeSpan.Parse("39:20:57"));
            "1:15:20:57".As<TimeSpan>().Should().Be(TimeSpan.Parse("1:15:20:57"));

            "unknown".Invoking(s => s.As<TimeSpan>())
                .Should().Throw<ArgumentException>()
                .WithMessage("Cannot parse value of type <TimeSpan> from string 'unknown'.");

            foreach (var value in _empty)
            {
                value.Invoking(s => s.As<TimeSpan>())
                    .Should().Throw<ArgumentException>()
                    .WithMessage($"Cannot parse value of type <TimeSpan> from string '{value}'.");
            }
        }

        [TestMethod]
        public void Can_Parse_TimeSpan_Nullable_Values()
        {
            "15:20:57".As<TimeSpan?>().Should().Be(TimeSpan.Parse("15:20:57"));
            "   15:20:57   ".As<TimeSpan?>().Should().Be(TimeSpan.Parse("15:20:57"));
            "15:20:57.123".As<TimeSpan?>().Should().Be(TimeSpan.Parse("15:20:57.123"));
            "39:20:57".As<TimeSpan?>().Should().Be(TimeSpan.Parse("39:20:57"));
            "1:15:20:57".As<TimeSpan?>().Should().Be(TimeSpan.Parse("1:15:20:57"));

            "unknown".Invoking(s => s.As<TimeSpan?>())
                .Should().Throw<ArgumentException>()
                .WithMessage("Cannot parse value of type <TimeSpan?> from string 'unknown'.");

            foreach (var value in _empty)
                value.As<TimeSpan?>().Should().BeNull();
        }

        [TestMethod]
        public void Can_Parse_DateTime_Values()
        {
            "1983-05-25".As<DateTime>().Should().Be(new DateTime(1983, 05, 25));
            "   1983-05-25   ".As<DateTime>().Should().Be(new DateTime(1983, 05, 25));
            "2000-11-21 15:20:57".As<DateTime>().Should().Be(new DateTime(2000, 11, 21, 15, 20, 57));
            "2000/11/21 15:20:57".As<DateTime>().Should().Be(new DateTime(2000, 11, 21, 15, 20, 57));

            "unknown".Invoking(s => s.As<DateTime>())
                .Should().Throw<ArgumentException>()
                .WithMessage("Cannot parse value of type <DateTime> from string 'unknown'.");

            foreach (var value in _empty)
            {
                value.Invoking(s => s.As<DateTime>())
                    .Should().Throw<ArgumentException>()
                    .WithMessage($"Cannot parse value of type <DateTime> from string '{value}'.");
            }
        }

        [TestMethod]
        public void Can_Parse_DateTime_Nullable_Values()
        {
            "1983-05-25".As<DateTime?>().Should().Be(new DateTime(1983, 05, 25));
            "   1983-05-25   ".As<DateTime?>().Should().Be(new DateTime(1983, 05, 25));
            "2000-11-21 15:20:57".As<DateTime?>().Should().Be(new DateTime(2000, 11, 21, 15, 20, 57));
            "2000/11/21 15:20:57".As<DateTime?>().Should().Be(new DateTime(2000, 11, 21, 15, 20, 57));

            "unknown".Invoking(s => s.As<DateTime?>())
                .Should().Throw<ArgumentException>()
                .WithMessage("Cannot parse value of type <DateTime?> from string 'unknown'.");

            foreach (var value in _empty)
                value.As<DateTime?>().Should().BeNull();
        }

        [TestMethod]
        public void Can_Parse_Guid_Values()
        {
            var id = new Guid("2DC0B86E-3E16-445A-88B8-C39EFF611331");
            "{2DC0B86E-3E16-445A-88B8-C39EFF611331}".As<Guid>().Should().Be(id);
            "   {2DC0B86E-3E16-445A-88B8-C39EFF611331}   ".As<Guid>().Should().Be(id);
            "2DC0B86E-3E16-445A-88B8-C39EFF611331".As<Guid>().Should().Be(id);
            "2dc0b86e-3e16-445a-88b8-c39eff611331".As<Guid>().Should().Be(id);
            "2dc0b86e3e16445a88b8c39eff611331".As<Guid>().Should().Be(id);

            "unknown".Invoking(s => s.As<Guid>())
                .Should().Throw<ArgumentException>()
                .WithMessage("Cannot parse value of type <Guid> from string 'unknown'.");

            foreach (var value in _empty)
            {
                value.Invoking(s => s.As<Guid>())
                    .Should().Throw<ArgumentException>()
                    .WithMessage($"Cannot parse value of type <Guid> from string '{value}'.");
            }
        }

        [TestMethod]
        public void Can_Parse_Guid_Nullable_Values()
        {
            var id = new Guid("2DC0B86E-3E16-445A-88B8-C39EFF611331");
            "{2DC0B86E-3E16-445A-88B8-C39EFF611331}".As<Guid>().Should().Be(id);
            "   {2DC0B86E-3E16-445A-88B8-C39EFF611331}   ".As<Guid>().Should().Be(id);
            "2DC0B86E-3E16-445A-88B8-C39EFF611331".As<Guid>().Should().Be(id);
            "2dc0b86e-3e16-445a-88b8-c39eff611331".As<Guid>().Should().Be(id);
            "2dc0b86e3e16445a88b8c39eff611331".As<Guid>().Should().Be(id);

            "unknown".Invoking(s => s.As<Guid?>())
                .Should().Throw<ArgumentException>()
                .WithMessage("Cannot parse value of type <Guid?> from string 'unknown'.");

            foreach (var value in _empty)
                value.As<Guid?>().Should().BeNull();
        }

        [TestMethod]
        public void Can_Parse_Enum_Values()
        {
            "Zero".As<TestEnum>().Should().Be(TestEnum.Zero);
            "   Zero   ".As<TestEnum>().Should().Be(TestEnum.Zero);
            "one".As<TestEnum>().Should().Be(TestEnum.One);
            "TWO".As<TestEnum>().Should().Be(TestEnum.Two);

            "0".As<TestEnum>().Should().Be(TestEnum.Zero);
            "1".As<TestEnum>().Should().Be(TestEnum.One);
            "2".As<TestEnum>().Should().Be(TestEnum.Two);

            "3".As<TestEnum>().Should().NotBe(TestEnum.Zero)
                .And.Should().NotBe(TestEnum.One)
                .And.Should().NotBe(TestEnum.Two);

            "unknown".Invoking(s => s.As<TestEnum>())
                .Should().Throw<ArgumentException>()
                .WithMessage("Cannot parse value of type <TestEnum> from string 'unknown'.");

            foreach (var value in _empty)
            {
                value.Invoking(s => s.As<TestEnum>())
                    .Should().Throw<ArgumentException>()
                    .WithMessage($"Cannot parse value of type <TestEnum> from string '{value}'.");
            }
        }

        [TestMethod]
        public void Can_Parse_Enum_Nullable_Values()
        {
            "Zero".As<TestEnum?>().Should().Be(TestEnum.Zero);
            "   Zero   ".As<TestEnum?>().Should().Be(TestEnum.Zero);
            "one".As<TestEnum?>().Should().Be(TestEnum.One);
            "TWO".As<TestEnum?>().Should().Be(TestEnum.Two);

            "0".As<TestEnum?>().Should().Be(TestEnum.Zero);
            "1".As<TestEnum?>().Should().Be(TestEnum.One);
            "2".As<TestEnum?>().Should().Be(TestEnum.Two);

            "3".As<TestEnum?>().Should().NotBe(TestEnum.Zero)
                .And.Should().NotBe(TestEnum.One)
                .And.Should().NotBe(TestEnum.Two);

            "unknown".Invoking(s => s.As<TestEnum?>())
                .Should().Throw<ArgumentException>()
                .WithMessage("Cannot parse value of type <TestEnum?> from string 'unknown'.");

            foreach (var value in _empty)
                value.As<TestEnum?>().Should().BeNull();
        }

        [TestMethod]
        public void Exception_Contains_Inner_Exception()
        {
            Parse.AllowNonNumericValues = false;

            " ".Invoking(s => s.As<int>())
                .Should().ThrowExactly<ArgumentException>()
                .WithMessage("Cannot parse value of type <int> from string ' '.")
                .WithInnerExceptionExactly<ArgumentException>()
                .WithMessage("Cannot parse non-nullable value type from null or whitespace string.");

            "-".Invoking(s => s.As<int>())
                .Should().ThrowExactly<ArgumentException>()
                .WithMessage("Cannot parse value of type <int> from string '-'.")
                .WithInnerExceptionExactly<ArgumentException>()
                .WithMessage("- is not a valid value for Int32. (Parameter 'value')");

            "0+".Invoking(s => s.As<float>())
                .Should().ThrowExactly<ArgumentException>()
                .WithMessage("Cannot parse value of type <float> from string '0+'.")
                .WithInnerExceptionExactly<ArgumentException>()
                .WithMessage("0+ is not a valid value for Single. (Parameter 'value')");

            "NaN".Invoking(s => s.As<double>())
                .Should().ThrowExactly<ArgumentException>()
                .WithMessage("Cannot parse value of type <double> from string 'NaN'.")
                .WithInnerExceptionExactly<ArgumentException>()
                .WithMessage("Value 'NaN' is not allowed to be parsed. Change [Parse.AllowNonNumericValues] flag if you want to use such a value.");
        }

        [TestMethod]
        public void Exception_Uses_Type_Keywords()
        {
            "bad".Invoking(s => s.As<int>())
                .Should().Throw<ArgumentException>()
                .WithMessage("Cannot parse value of type <int> from string 'bad'.");

            "bad".Invoking(s => s.As<int?>())
                .Should().Throw<ArgumentException>()
                .WithMessage("Cannot parse value of type <int?> from string 'bad'.");

            "bad".Invoking(s => s.As<DateTime>())
                .Should().Throw<ArgumentException>()
                .WithMessage("Cannot parse value of type <DateTime> from string 'bad'.");

            "bad".Invoking(s => s.As<DateTime?>())
                .Should().Throw<ArgumentException>()
                .WithMessage("Cannot parse value of type <DateTime?> from string 'bad'.");

            "bad".Invoking(s => s.As<TestEnum>())
                .Should().Throw<ArgumentException>()
                .WithMessage("Cannot parse value of type <TestEnum> from string 'bad'.");

            "bad".Invoking(s => s.As<TestEnum?>())
                .Should().Throw<ArgumentException>()
                .WithMessage("Cannot parse value of type <TestEnum?> from string 'bad'.");

            "bad".Invoking(s => s.As<object>())
                .Should().Throw<InvalidOperationException>()
                .WithMessage("Parsing as type <object> is not supported.");

            "bad".Invoking(s => s.As<List<int>>())
                .Should().Throw<InvalidOperationException>()
                .WithMessage("Parsing as type <List<int>> is not supported.");

            "bad".Invoking(s => s.As<Dictionary<string, bool>>())
                .Should().Throw<InvalidOperationException>()
                .WithMessage("Parsing as type <Dictionary<string, bool>> is not supported.");
        }
    }
}
