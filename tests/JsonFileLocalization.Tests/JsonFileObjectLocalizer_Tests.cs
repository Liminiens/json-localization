using System;
using System.Globalization;
using FluentAssertions;
using JsonFileLocalization.Resource;
using JsonFileLocalization.Tests.TestData;
using JsonFileLocalization.Tests.TestData.Models;
using Xunit;

namespace JsonFileLocalization.Tests
{
    public class JsonFileObjectLocalizer_Tests
    {
        [Fact]
        public void GetLocalizedObject_WhenCalled_ReturnsCorrectData()
        {
            //Arrange
            CultureInfo.CurrentUICulture = new CultureInfo("ru-RU");
            var factory = TestJsonFileObjectLocalizerFactory.GetFactory(JsonFileCultureSuffixStrategy.TwoLetterISO6391AndCountryCode);
            var localizer = factory.Create("_Layout", String.Empty);

            //Act
            var result = localizer.GetLocalizedObject<LayoutRu>(String.Empty);
            var value = result.Value;

            //Assert
            value.TestString.Should().Be("Test");
            value.Inner.TestArray.Should().Contain(new[] { "One", "Two" });
            value.TestObject.Id.Should().Be(1);
            value.TestObject.Value.Should().Be("Something");
        }

        [Fact]
        public void WithCulture_WhenCalled_UsesCorrectCultureResource()
        {
            //Arrange
            CultureInfo.CurrentUICulture = new CultureInfo("ru-RU");
            var factory = TestJsonFileObjectLocalizerFactory.GetFactory(JsonFileCultureSuffixStrategy.TwoLetterISO6391AndCountryCode);
            var localizer = factory.Create("_Layout", String.Empty);

            //Act
            var result = localizer.GetLocalizedObject<string>("TestObject.Value");
            var value = result.Value;
            var usLocalizer = localizer.WithCulture(new CultureInfo("en-US"));
            var usLocalizerResult = usLocalizer.GetLocalizedObject<string>("TestObject.Value");
            var usValue = usLocalizerResult.Value;

            //Assert
            value.Should().Be("Something");
            usValue.Should().Be("Something en-US");
        }


    }
}