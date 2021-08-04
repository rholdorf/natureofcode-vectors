using System;
using Xunit;
using WhatIsAVector;

namespace TestProject1
{
    public class FloatExtensionsTest
    {
        [Fact]
        public void MapShouldReturnScaledValue()
        {
            var result = FloatExtensions.Map(1f, 0, 10, 0, 20);
            Assert.Equal(2f, result);
        }

        [Fact]
        public void MapShouldExtrapolateByDefault()
        {
            var foo = FloatExtensions.Map(10f, 0, 1, 10, 11);
            var bar = FloatExtensions.Map(-1f, 0, 1, 10, 11);
            var cux = FloatExtensions.Map(2f, 0, 1, 20, 10);

            Assert.Equal(20f, foo);
            Assert.Equal(9f, bar);
            Assert.Equal(0f, cux);
        }

        [Fact]
        public void MapShouldClampCorrectly()
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

        [Fact]
        public void ConstrainShouldReturnSameValue()
        {
            var foo = FloatExtensions.Constrain(1f, 3f, 5f);
            Assert.Equal(3f, foo);
        }

        [Fact]
        public void ConstrainReturnLowerBound()
        {
            var foo = FloatExtensions.Constrain(1f, -1f, 5f);
            Assert.Equal(1f, foo);
        }

        [Fact]
        public void ConstrainShouldReturnUperBound()
        {
            var foo = FloatExtensions.Constrain(1f, 10f, 5f);
            Assert.Equal(10f, foo);
        }

        [Fact]
        public void ClampShouldReturnSameValue()
        {
            var value = 1f;
            var foo = FloatExtensions.Clamp(ref value, 3f, 5f);
            Assert.Equal(3f, foo);
            Assert.Equal(3f, value);
        }

        [Fact]
        public void ClampReturnLowerBound()
        {
            var value = 1f;
            var foo = FloatExtensions.Clamp(ref value, -1f, 5f);
            Assert.Equal(1f, foo);
            Assert.Equal(1f, value);
        }

        [Fact]
        public void ClampShouldReturnUperBound()
        {
            var value = 1f;
            var foo = FloatExtensions.Clamp(ref value, 10f, 5f);
            Assert.Equal(10f, foo);
            Assert.Equal(10f, value);
        }
    }
}