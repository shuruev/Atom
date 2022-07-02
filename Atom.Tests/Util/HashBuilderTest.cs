namespace Atom.Tests.Util;

public class HashBuilderTest
{
    [Test]
    public void Basic_Test()
    {
        var num1 = 0;
        int? num2 = null;
        var str1 = "test";
        string? str2 = null;
        var day1 = DayOfWeek.Monday;
        DayOfWeek? day2 = null;
        var obj1 = new byte[] { 1, 2, 3 };
        byte[]? obj2 = null;

        new HashBuilder()
            .Add(num1)
            .Add(num2)
            .Add(str1)
            .Add(str2)
            .Add((int)day1)
            .Add((int?)day2)
            .Add(obj1)
            .Add(obj2)
            .GetHash()
            .Should().Be("{1e96e4a6-7760-eec5-5545-0afb31d4effe}");

        new HashBuilder()
            .AddMany(num1, num2, str1, str2, (int)day1, (int?)day2, obj1, obj2)
            .GetHash()
            .Should().Be("{1e96e4a6-7760-eec5-5545-0afb31d4effe}");
    }

    [Test]
    public void Basic_Byte_Arrays()
    {
        var data1 = new byte[] { 1, 2, 3, 4, 5 };
        var hash11 = new HashBuilder().Add(data1).GetHash();
        var hash12 = new HashBuilder().AddMany(data1).GetHash();
        var hash13 = new HashBuilder().Add(data1).Add(data1).GetHash();
        var hash14 = new HashBuilder().AddMany(data1, data1).GetHash();

        hash11.Should().Be("{60705137-f215-8b43-2322-8d5c87fd4ae8}");
        hash12.Should().Be("{60705137-f215-8b43-2322-8d5c87fd4ae8}");
        hash13.Should().Be("{ce6128b4-a23b-e40d-fa5e-6fe912b6b90c}");
        hash14.Should().Be("{ce6128b4-a23b-e40d-fa5e-6fe912b6b90c}");

        var data2 = new byte?[] { 1, 2, 3, 4, 5 };
        var hash21 = new HashBuilder().Add(data2).GetHash();
        var hash22 = new HashBuilder().AddMany(data2).GetHash();
        var hash23 = new HashBuilder().Add(data2).Add(data2).GetHash();
        var hash24 = new HashBuilder().AddMany(data2, data2).GetHash();

        hash21.Should().Be(hash11);
        hash22.Should().Be(hash12);
        hash23.Should().Be(hash13);
        hash24.Should().Be(hash14);
    }

    [Test]
    public void List_Of_Nullables()
    {
        var list1 = new List<string?>
        {
            "test1",
            null,
            "test2"
        };
        new HashBuilder().Add(list1).GetHash().Should().Be("{9f21ab4a-8aef-efbc-fd70-fcb226a3a099}");

        var list2 = new[] {
            "test1",
            null,
            "test2"
        };
        new HashBuilder().Add(list2).GetHash().Should().Be("{9f21ab4a-8aef-efbc-fd70-fcb226a3a099}");
    }

    [Test]
    public void Null_Values_Considered_Equal()
    {
        byte? a1 = null;
        string? b1 = null;
        byte[]? c1 = null;

        var hash1 = new HashBuilder()
            .Add(a1)
            .Add(b1)
            .Add(c1)
            .GetHash();

        long? a2 = null;
        object? b2 = null;
        DateTime? c2 = null;

        var hash2 = new HashBuilder()
            .Add(a2)
            .Add(b2)
            .Add(c2)
            .GetHash();

        hash2.Should().Be(hash1);
    }

    [Test]
    public void Non_Null_Values_Considered_Equal()
    {
        byte a1 = 1;
        byte? b1 = 2;
        byte? c1 = null;

        var hash1 = new HashBuilder()
            .Add(a1)
            .Add(b1)
            .Add(c1)
            .GetHash();

        byte a2 = 1;
        byte b2 = 2;
        byte? c2 = null;

        var hash2 = new HashBuilder()
            .Add(a2)
            .Add(b2)
            .Add(c2)
            .GetHash();

        hash2.Should().Be(hash1);
    }

    [Test]
    public void Can_Safely_Cast_To_Object()
    {
        byte a1 = 1;
        byte? b1 = 2;
        byte? c1 = null;

        var hash1 = new HashBuilder()
            .Add(a1)
            .Add(b1)
            .Add(c1)
            .GetHash();

        object a2 = a1;
        object b2 = b1;
        object? c2 = c1;

        var hash2 = new HashBuilder()
            .Add(a2)
            .Add(b2)
            .Add(c2)
            .GetHash();

        hash2.Should().Be(hash1);
    }

    [Test]
    public void Avoid_Type_Confusion()
    {
        bool? a1 = false;
        var b1 = true;
        var c1 = false;

        var hash1 = new HashBuilder()
            .Add(a1)
            .Add(b1)
            .Add(c1)
            .GetHash();

        var a2 = true;
        var b2 = false;
        bool? c2 = false;

        var hash2 = new HashBuilder()
            .Add(a2)
            .Add(b2)
            .Add(c2)
            .GetHash();

        hash2.Should().NotBe(hash1);
    }

    [Test]
    public void Avoid_Confusion_Between_Integer_Types()
    {
        var hash1 = new HashBuilder().Add((byte)0).GetHash();
        var hash2 = new HashBuilder().Add((short)0).GetHash();
        var hash3 = new HashBuilder().Add(0).GetHash();
        var hash4 = new HashBuilder().Add((long)0).GetHash();
        hash2.Should().NotBe(hash1);
        hash3.Should().NotBe(hash2).And.NotBe(hash1);
        hash4.Should().NotBe(hash3).And.NotBe(hash2).And.NotBe(hash1);
    }

    [Test]
    public void Avoid_Confusion_Between_Floating_Types()
    {
        var hash1 = new HashBuilder().Add((float)0).GetHash();
        var hash2 = new HashBuilder().Add((double)0).GetHash();
        var hash3 = new HashBuilder().Add((decimal)0).GetHash();
        hash2.Should().NotBe(hash1);
        hash3.Should().NotBe(hash2).And.NotBe(hash1);
    }

    [Test]
    public void Avoid_Confusion_Between_Null_And_False()
    {
        var hash1 = new HashBuilder().Add(null).Add(null).GetHash();
        var hash2 = new HashBuilder().Add(false).Add(null).GetHash();
        var hash3 = new HashBuilder().Add(null).Add(false).GetHash();
        var hash4 = new HashBuilder().Add(false).Add(false).GetHash();
        hash2.Should().NotBe(hash1);
        hash3.Should().NotBe(hash2).And.NotBe(hash1);
        hash4.Should().NotBe(hash3).And.NotBe(hash2).And.NotBe(hash1);

        hash1 = new HashBuilder().Add(new bool?[] { null, null }).GetHash();
        hash2 = new HashBuilder().Add(new bool?[] { false, null }).GetHash();
        hash3 = new HashBuilder().Add(new bool?[] { null, false }).GetHash();
        hash4 = new HashBuilder().Add(new bool?[] { false, false }).GetHash();
        hash2.Should().NotBe(hash1);
        hash3.Should().NotBe(hash2).And.NotBe(hash1);
        hash4.Should().NotBe(hash3).And.NotBe(hash2).And.NotBe(hash1);
    }

    [Test]
    public void Avoid_Confusion_Between_List_And_Many_Items()
    {
        var hash1 = new HashBuilder().Add(1).Add(2).Add(3).GetHash();
        var hash2 = new HashBuilder().AddMany(1, 2, 3).GetHash();
        hash2.Should().Be(hash1);

        var hash3 = new HashBuilder().Add(new[] { 1, 2, 3 }).GetHash();
        hash3.Should().NotBe(hash2);

        var hash4 = new HashBuilder().AddMany(new[] { 1, 2, 3 }).GetHash();
        hash4.Should().Be(hash3);
    }

    [Test]
    public void Avoid_Confusion_Between_List_And_Single_Item()
    {
        var hash1 = new HashBuilder().Add(1).GetHash();
        var hash2 = new HashBuilder().Add(new[] { 1 }).GetHash();
        hash2.Should().NotBe(hash1);
    }

    [Test]
    public void Data_Type_Null()
    {
        new HashBuilder().Add(null).GetHash().Should().Be("{ad85b893-0dfe-89a0-cdf6-34904fd59f71}");
    }

    [Test]
    public void Data_Type_Boolean()
    {
        new HashBuilder().Add(false).GetHash().Should().Be("{fb71a730-c283-00f5-0a74-d41689f19c76}");
        new HashBuilder().Add(true).GetHash().Should().Be("{2f80e901-06d9-1a34-2b76-9125c562d95c}");
    }

    [Test]
    public void Data_Type_Byte()
    {
        new HashBuilder().Add(Byte.MinValue).GetHash().Should().Be("{d050dc37-3c20-baa1-9548-ad9a28769fd2}");
        new HashBuilder().Add((byte)0).GetHash().Should().Be("{d050dc37-3c20-baa1-9548-ad9a28769fd2}");
        new HashBuilder().Add(Byte.MaxValue).GetHash().Should().Be("{70091737-fae3-afbd-3b88-d3536255fb42}");
    }

    [Test]
    public void Data_Type_Int16()
    {
        new HashBuilder().Add(Int16.MinValue).GetHash().Should().Be("{4cbaa955-1fce-bfe6-8f7b-d9742ba5f952}");
        new HashBuilder().Add((short)0).GetHash().Should().Be("{98aecfed-4095-42fd-e4b8-556d5b723bb6}");
        new HashBuilder().Add(Int16.MaxValue).GetHash().Should().Be("{20cb7225-aa7b-1998-9405-a992700877c4}");
    }

    [Test]
    public void Data_Type_Int32()
    {
        new HashBuilder().Add(Int32.MinValue).GetHash().Should().Be("{6eae6dc5-5253-0d8e-2f2c-7edfab01b091}");
        new HashBuilder().Add(0).GetHash().Should().Be("{c4dbdd47-b2b3-b712-24fc-ff9aa02cbdcd}");
        new HashBuilder().Add(Int32.MaxValue).GetHash().Should().Be("{5864836c-d573-20e4-9a22-d25313e34250}");
    }

    [Test]
    public void Data_Type_Int64()
    {
        new HashBuilder().Add(Int64.MinValue).GetHash().Should().Be("{ba9a551b-1b61-f47d-d8a9-61687ce07ef3}");
        new HashBuilder().Add((long)0).GetHash().Should().Be("{40c89733-076d-a33b-d205-5d39323d26ad}");
        new HashBuilder().Add(Int64.MaxValue).GetHash().Should().Be("{0263e572-b0a9-2e48-04a3-fec03e6f2bb8}");
    }

    [Test]
    public void Data_Type_Single()
    {
        new HashBuilder().Add(Single.MinValue).GetHash().Should().Be("{68030906-144a-df48-4068-eb82a3580bab}");
        new HashBuilder().Add((float)0).GetHash().Should().Be("{b95ce10e-da30-2e51-9cca-c2b353f65d3f}");
        new HashBuilder().Add(Single.MaxValue).GetHash().Should().Be("{ac890e3c-180a-a7ec-1e0b-15ca2ec805e7}");
    }

    [Test]
    public void Data_Type_Double()
    {
        new HashBuilder().Add(Double.MinValue).GetHash().Should().Be("{cb411e34-eef6-f738-b048-6add0e2f24d9}");
        new HashBuilder().Add((double)0).GetHash().Should().Be("{10e35e5a-10e9-a25f-be3e-3bc05b4f8aa1}");
        new HashBuilder().Add(Double.MaxValue).GetHash().Should().Be("{6fc8e29e-faa8-12c3-586c-8d54049d192f}");
    }

    [Test]
    public void Data_Type_Decimal()
    {
        new HashBuilder().Add(Decimal.MinValue).GetHash().Should().Be("{0d4289e8-c117-b024-3238-988776a8611c}");
        new HashBuilder().Add((decimal)0).GetHash().Should().Be("{db03488c-18d7-6c68-7fe2-6bcd08b3c575}");
        new HashBuilder().Add(Decimal.MaxValue).GetHash().Should().Be("{b4b71579-4be1-c7ee-6636-9b787bb329e3}");
    }

    [Test]
    public void Data_Type_String()
    {
        new HashBuilder().Add(String.Empty).GetHash().Should().Be("{d685a40d-cc27-ecff-45b5-1b91fab8055a}");
        new HashBuilder().Add("🙂").GetHash().Should().Be("{c14500c1-a2f7-79f3-3da1-1e6866c09264}");
        new HashBuilder().Add("Hello").GetHash().Should().Be("{f042785e-dc59-97e6-4fa7-f78a5d64858b}");
    }

    [Test]
    public void Data_Type_Guid()
    {
        new HashBuilder().Add(Guid.Empty).GetHash().Should().Be("{3b9f8da1-04fa-3f87-631f-23e1cc935adc}");
        new HashBuilder().Add(new Guid("{e3e39c20-1400-4bf9-8307-ee5f36c61cce}")).GetHash().Should().Be("{f3fd0866-e251-408a-ce6f-167c7d01b15b}");
        new HashBuilder().Add(new Guid("{ffffffff-ffff-ffff-ffff-ffffffffffff}")).GetHash().Should().Be("{c779e7c4-7592-e2a5-b755-b1bd41295531}");
    }

    [Test]
    public void Data_Type_DateTime()
    {
        new HashBuilder().Add(DateTime.MinValue).GetHash().Should().Be("{0d77b4b1-3b42-2cc2-82e3-dbcaa26b86e5}");
        new HashBuilder().Add(DateTime.MaxValue).GetHash().Should().Be("{c6d96a5e-e399-b8a6-f439-28327f24257b}");

        var date1 = 25.May(1983);
        date1.Kind.Should().Be(DateTimeKind.Unspecified);
        date1.TimeOfDay.Should().Be(TimeSpan.Zero);
        new HashBuilder().Add(date1).GetHash().Should().Be("{a760d47c-73fd-bdea-5a39-d39aad24ca7f}");

        var date2 = date1.ToLocalTime();
        date2.Kind.Should().Be(DateTimeKind.Local);
        date2.TimeOfDay.Should().NotBe(date1.TimeOfDay);
        new HashBuilder().Add(date2).GetHash().Should().Be("{8f243170-d7ff-3c41-358c-bc164d5a4c33}");

        var date3 = DateTime.SpecifyKind(date2, DateTimeKind.Utc);
        date3.Kind.Should().Be(DateTimeKind.Utc);
        date3.TimeOfDay.Should().Be(date2.TimeOfDay);
        new HashBuilder().Add(date3).GetHash().Should().Be("{8f243170-d7ff-3c41-358c-bc164d5a4c33}");
    }

    [Test]
    public void Data_Type_TimeSpan()
    {
        new HashBuilder().Add(TimeSpan.MinValue).GetHash().Should().Be("{0b9339dc-0c45-a443-bd53-f504248e1029}");
        new HashBuilder().Add(TimeSpan.Zero).GetHash().Should().Be("{d9f9fdeb-cc45-f9e4-8442-5d06fa540f5f}");
        new HashBuilder().Add(TimeSpan.MaxValue).GetHash().Should().Be("{c07868be-127d-7f8a-b7a5-b8485d208eec}");
    }

    [Test]
    public void Data_Type_DateTimeOffset()
    {
        new HashBuilder().Add(DateTimeOffset.MinValue).GetHash().Should().Be("{504247b8-9246-3de2-7d78-53cf3a77addf}");
        new HashBuilder().Add(DateTimeOffset.MaxValue).GetHash().Should().Be("{f04fdc3d-22ad-da5d-252b-16c9a073b1c6}");

        var date1 = new DateTimeOffset(25.May(1983), TimeSpan.Zero);
        date1.TimeOfDay.Should().Be(TimeSpan.Zero);
        new HashBuilder().Add(date1).GetHash().Should().Be("{f2135d49-30be-48b7-892e-b1325417c130}");

        var date2 = date1.ToLocalTime();
        date2.TimeOfDay.Should().NotBe(date1.TimeOfDay);
        new HashBuilder().Add(date2).GetHash().Should().Be("{0d0b8811-50e0-27a7-93b9-2bb9115a2280}");

        var date3 = date1.ToUniversalTime();
        date3.TimeOfDay.Should().Be(date1.TimeOfDay);
        new HashBuilder().Add(date3).GetHash().Should().Be("{f2135d49-30be-48b7-892e-b1325417c130}");
    }

    [Test]
    public void Data_Type_DateOnly()
    {
        new HashBuilder().Add(DateOnly.MinValue).GetHash().Should().Be("{a6ea7fdf-5697-afc6-c3e3-53e7a0ee139a}");
        new HashBuilder().Add(DateOnly.MaxValue).GetHash().Should().Be("{713fab53-4611-cc10-d5b1-a5fcd9204b30}");

        var date = new DateOnly(1983, 5, 25);
        new HashBuilder().Add(date).GetHash().Should().Be("{47a85899-1d94-1bc3-a367-097b9216cf64}");
    }

    [Test]
    public void Data_Type_TimeOnly()
    {
        new HashBuilder().Add(TimeOnly.MinValue).GetHash().Should().Be("{abe11b52-2c1b-08cd-8b0e-dec4c52469bb}");
        new HashBuilder().Add(TimeOnly.Parse("12:34")).GetHash().Should().Be("{05f5112f-2bac-73ea-0072-8ce0c890650a}");
        new HashBuilder().Add(TimeOnly.MaxValue).GetHash().Should().Be("{e9eb33dc-dcb0-5d8b-a73f-290d02feb498}");
    }

    [Test]
    public void Data_Type_Unknown()
    {
        this.Invoking(_ => new HashBuilder().Add(new object()).GetHash())
            .Should().Throw<InvalidOperationException>()
            .WithMessage("Hashing is not supported for type 'Object', use standard .NET types or collections.");

        this.Invoking(_ => new HashBuilder().Add(new Exception()).GetHash())
            .Should().Throw<InvalidOperationException>()
            .WithMessage("Hashing is not supported for type 'Exception', use standard .NET types or collections.");

        this.Invoking(_ => new HashBuilder().Add(new List<Exception>()).GetHash())
            .Should().Throw<InvalidOperationException>()
            .WithMessage("Hashing is not supported for type 'Exception', use standard .NET types or collections.");
    }
}
