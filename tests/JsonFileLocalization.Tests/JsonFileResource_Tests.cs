using System;
using System.Globalization;
using System.Linq;
using FluentAssertions;
using JsonFileLocalization.Resource;
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
    }
}
