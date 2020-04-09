using System;
using Atom.Util;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atom.Tests.Util
{
    [TestClass]
    public class Base64UrlTest
    {
        [TestMethod]
        public void Base64_Url_Mass_Check()
        {
            var rand = new Random();

            var all = new byte[1000];
            rand.NextBytes(all);

            for (var i = 0; i < all.Length; i++)
            {
                var data = new byte[i];
                Array.Copy(all, data, data.Length);

                var encoded = Base64Url.Encode(data);
                encoded.Should().NotContain("+");
                encoded.Should().NotContain("/");
                encoded.Should().NotContain("=");

                var decoded = Base64Url.Decode(encoded);
                decoded.Should().BeEquivalentTo(data);
            }
        }
    }
}
