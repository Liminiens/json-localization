using FluentAssertions;
using JsonFileLocalization.Resource;
using JsonFileLocalization.Tests.TestData;
using JsonFileLocalization.Tests.TestData.Models;
using System;
using System.Globalization;
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
            var factory = TestJsonFileObjectLocalizerFactory.GetFactory(CultureSuffixStrategy.TwoLetterISO6391AndCountryCode);
            var localizer = factory.Create("_Layout", String.Empty);

            //Act
            var result = localizer.GetLocalizedObject<LayoutRu>(String.Empty);
            var value = result.Value;

            //Assert
            result.SearchedLocation.Should().Be("_Layout", "View name");
            result.Name.Should().Be("", "Because root object");
            result.ResourceNotFound.Should().BeFalse();
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
            var factory = TestJsonFileObjectLocalizerFactory.GetFactory(CultureSuffixStrategy.TwoLetterISO6391AndCountryCode);
            var localizer = factory.Create("_Layout", String.Empty);

            //Act
            var result = localizer.GetLocalizedObject<string>("TestObject.Value");
            var usLocalizer = localizer.WithCulture(new CultureInfo("en-US"));
            var usLocalizerResult = usLocalizer.GetLocalizedObject<string>("TestObject.Value");

            //Assert
            result.Value.Should().Be("Something");
            usLocalizerResult.Value.Should().Be("Something en-US");
        }
    }
}