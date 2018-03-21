using System;
using System.Globalization;
using System.Linq;
using FluentAssertions;
using JsonFileLocalization.Resources;
using JsonFileLocalization.Tests.TestData;
using Xunit;

namespace JsonFileLocalization.Tests
{
    public class JsonFileStringLocalizer_Tests
    {
        [Fact]
        public void Indexer_WhenPassedCorrectPath_ReturnsString()
        {
            //Arrange
            var factory = TestJsonFileStringLocalizerFactory.GetFactory(JsonFileCultureSuffixStrategy.TwoLetterISO6391AndCountryCode);
            CultureInfo.CurrentUICulture = new CultureInfo("ru-RU");

            //Act
            var localizer = factory.Create("_Layout", String.Empty);
            var result = localizer["TestString"];

            //Assert
            result.Value.Should().Be("Test");
            result.ResourceNotFound.Should().BeFalse();
        }

        [Fact]
        public void GetAllStrings_WhenCalled_ReturnsAllRootStrings()
        {
            //Arrange
            var factory = TestJsonFileStringLocalizerFactory.GetFactory(JsonFileCultureSuffixStrategy.TwoLetterISO6391AndCountryCode);
            CultureInfo.CurrentUICulture = new CultureInfo("ru-RU");

            //Act
            var localizer = factory.Create("AllStrings", String.Empty);
            var result = localizer.GetAllStrings(false).Select(x => x.Value);

            //Assert
            result.Should().Contain(new[] {"Test1", "Test2"});
        }

        [Fact]
        public void GetAllStrings_WhenCalledWithParentCultures_ReturnsAllRootStrings()
        {
            //Arrange
            var factory = TestJsonFileStringLocalizerFactory.GetFactory(JsonFileCultureSuffixStrategy.TwoLetterISO6391AndCountryCode);
            CultureInfo.CurrentUICulture = new CultureInfo("ru-RU-Test");

            //Act
            var localizer = factory.Create("AllStrings", String.Empty);
            var result = localizer.GetAllStrings(true).Select(x => x.Value);

            //Assert
            result.Should().Contain(new[] { "Test1", "Test2", "Test3", "Test4" });
        }

        [Fact]
        public void WithCulture_WhenCalled_ReturnsLocalizerWithCorrectCulture()
        {
            //Arrange
            var factory = TestJsonFileStringLocalizerFactory.GetFactory(JsonFileCultureSuffixStrategy.TwoLetterISO6391);
            CultureInfo.CurrentUICulture = new CultureInfo("en");

            //Act
            var localizer = factory.Create("WithCulture", String.Empty);
            var enValue = localizer["Hey"];
            var etValue = localizer.WithCulture(new CultureInfo("et"))["Hey"];

            //Assert
            enValue.Value.Should().Be("Hello");
            etValue.Value.Should().Be("Something");
        }

        [Fact]
        public void Indexer_WhenCalledWithFormatArguments_ReturnsFormattedString()
        {
            //Arrange
            var factory = TestJsonFileStringLocalizerFactory.GetFactory(JsonFileCultureSuffixStrategy.TwoLetterISO6391);
            CultureInfo.CurrentUICulture = new CultureInfo("en");

            //Act
            var localizer = factory.Create("Format", String.Empty);
            var value = localizer["Test", "Boo"].Value;

            //Assert
            value.Should().Be("Hello, Boo");
        }
    }
}