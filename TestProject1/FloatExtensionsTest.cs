using System;
using Xunit;
using WhatIsAVector;

namespace TestProject1
{
    public class FloatExtensionsTest
    {
        [Fact]
        public void ShouldReturnScaledValue()
        {
            var result = FloatExtensions.Map(1f, 0, 10, 0, 20);
            Assert.Equal(2f, result);
        }

        [Fact]
        public void ShouldExtrapolateByDefault()
        {
            var foo = FloatExtensions.Map(10f, 0, 1, 10, 11);
            var bar = FloatExtensions.Map(-1f, 0, 1, 10, 11);
            var cux = FloatExtensions.Map(2f, 0, 1, 20, 10);

            Assert.Equal(20f, foo);
            Assert.Equal(9f, bar);
            Assert.Equal(0f, cux);
        }

        [Fact]
        public void ShouldClampCorrectly()
        {
            var foo = FloatExtensions.Map(1f, 0, 10, 0, 20, true);
            var bar = FloatExtensions.Map(10f, 0, 1, 10, 11, true);
            var cux = FloatExtensions.Map(-1f, 0, 1, 10, 11, true);
            var nox = FloatExtensions.Map(2f, 0, 1, 20, 10, true);

            Assert.Equal(2f, foo);
            Assert.Equal(11f, bar);
            Assert.Equal(10f, cux);
            Assert.Equal(10f, nox);
        }
    }
}
