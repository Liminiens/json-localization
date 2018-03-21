using System;
using System.Globalization;
using FluentAssertions;
using JsonFileLocalization.Resources;
using JsonFileLocalization.Tests.TestData;
using JsonFileLocalization.Tests.TestData.Models;
using Xunit;

namespace JsonFileLocalization.Tests
{
    public class JsonFileResourceManager_Tests
    {
        [Theory]
        [InlineData("ru-RU", "Test")]
        [InlineData("en-US", "Test en-US")]
        public void GetResource_WhenCalled_ReturnsCorrectCultureResource(string culture, string testString)
        {
            //Arrange
            var manager = TestJsonFileResourceManager.GetResourceManager(JsonFileCultureSuffixStrategy.TwoLetterISO6391AndCountryCode);

            //Act
            var resource = manager.GetResource("_Layout", String.Empty, new CultureInfo(culture));

            //Assert
            resource.Culture.Name.Should().Be(culture);
            resource.GetValue<string>("TestString").Value.Should().Be(testString);
        }

        [Theory]
        [InlineData(JsonFileCultureSuffixStrategy.TwoLetterISO6391, "ru")]
        [InlineData(JsonFileCultureSuffixStrategy.TwoLetterISO6391AndCountryCode, "ru-RU")]
        public void GetResource_WhenCalled_ReturnsCorrectFileForSuffixStratagy(
            JsonFileCultureSuffixStrategy strategy, string testString)
        {
            //Arrange
            var manager = TestJsonFileResourceManager.GetResourceManager(strategy);

            //Act
            var resource = manager.GetResource("Strategy", String.Empty, new CultureInfo("ru-RU"));

            //Assert
            resource.GetValue<string>("Test").Value.Should().Be(testString);
        }

        [Fact]
        public void GetResource_WhenThereIsNoFileWithLocationInName_FallbacksToNoLocationFile()
        {
            //Arrange
            var manager = TestJsonFileResourceManager.GetResourceManager(JsonFileCultureSuffixStrategy.TwoLetterISO6391AndCountryCode);
            var type = typeof(LocationTest);
            var assemblyName = type.Assembly.GetName().Name;

            //Act
            var resource = manager.GetResource(type.Name, assemblyName, new CultureInfo("ru-RU"));
            var result = resource.GetValue<LocationTest>(String.Empty).Value;

            //Assert
            result.TestArray.Should().BeEquivalentTo(new[] { 3, 2, 1 });
        }

        [Fact]
        public void GetResource_WhenLocationIsPassed_ReturnsCorrectResource()
        {
            //Arrange
            var manager = TestJsonFileResourceManager.GetResourceManager(JsonFileCultureSuffixStrategy.TwoLetterISO6391AndCountryCode);
            var type = typeof(IntArrayObject);
            var assemblyName = type.Assembly.GetName().Name;

            //Act
            var resource = manager.GetResource(type.Name, assemblyName, new CultureInfo("ru-RU"));
            var result = resource.GetValue<IntArrayObject>(String.Empty).Value;

            //Assert
            result.Value.Should().BeEquivalentTo(new[] { 8, 9, 10 });
        }
    }
}