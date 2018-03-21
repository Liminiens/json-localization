using System;
using System.Globalization;
using System.Linq;
using FluentAssertions;
using JsonFileLocalization.Resources;
using JsonFileLocalization.Tests.TestData;
using JsonFileLocalization.Tests.TestData.Models;
using Xunit;

namespace JsonFileLocalization.Tests
{
    public class JsonFileResource_Tests
    {
        [Fact]
        public void GetValue_WhenCalledWithCorrectPath_ReturnsCorrectValue()
        {
            //Arrange
            var manager = TestJsonFileResourceManager.GetResourceManager(JsonFileCultureSuffixStrategy.TwoLetterISO6391AndCountryCode);

            //Act
            var resource = manager.GetResource("_Layout", String.Empty, new CultureInfo("ru-RU"));
            var testString = resource.GetValue<string>("TestString").Value;
            var testArray = resource.GetValue<string[]>("Inner.TestArray").Value;
            var testObject = resource.GetValue<TestObject>("TestObject").Value;

            //Assert
            testString.Should().Be("Test");
            testArray.Should().Contain(new[] { "One", "Two" });
            testObject.Value.Should().Be("Something");
            testObject.Id.Should().Be(1);
        }

        [Fact]
        public void GetRootPropertyNames_WhenCalled_ReturnsCorrectEnumeration()
        {
            //Arrange
            var manager = TestJsonFileResourceManager.GetResourceManager(JsonFileCultureSuffixStrategy.TwoLetterISO6391AndCountryCode);

            //Act
            var resource = manager.GetResource("_Layout", String.Empty, new CultureInfo("ru-RU"));
            var result = resource.GetRootPropertyNames().ToList();

            //Assert
            var expected = new[] { "TestString", "Inner", "TestObject" };
            result.Should().Contain(expected);
        }

        [Fact]
        public void GetValue_WhenCalledWithIncorrectType_ReturnsDefaultValueAndWritesInLog()
        {
            //Arrange
            var loggerFactory = TestJsonFileResourceManager.GetLoggerFactory();
            var settings = TestJsonFileResourceManager.GetSettings(JsonFileCultureSuffixStrategy.TwoLetterISO6391AndCountryCode);
            var manager = new JsonFileResourceManager(settings, loggerFactory.Factory);

            //Act
            var resource = manager.GetResource("_Layout", String.Empty, new CultureInfo("ru-RU"));
            var value = resource.GetValue<int>("TestObject").Value;

            //Assert
            value.Should().Be(default(int));
            var loggerCalls = loggerFactory.LoggerCalls();
            loggerCalls.Count().Should().Be(1);
        }

        [Theory]
        [InlineData("_Layout", "", "_Layout")]
        [InlineData("IntArrayObject", "JsonFileLocalization.Tests", "JsonFileLocalization.Tests.IntArrayObject")]
        public void ResourceName_WhenCalled_ReturnsCorrectResourceName(string baseName, string location, string expected)
        {
            //Arrange
            var manager = TestJsonFileResourceManager.GetResourceManager(JsonFileCultureSuffixStrategy.TwoLetterISO6391AndCountryCode);

            //Act
            var resource = manager.GetResource(baseName, location, new CultureInfo("ru-RU"));

            //Assert
            resource.ResourceName.Should().Be(expected);
        }
    }
}