using System.Globalization;
using FluentAssertions;
using JsonFileLocalization.Resource;
using JsonFileLocalization.Tests.TestData;
using JsonFileLocalization.Tests.TestData.Models;
using Xunit;

namespace JsonFileLocalization.Tests
{
    public class JsonFileStringLocalizerFactory_Tests
    {

        [Fact]
        public void Factory_WhenCreatesResourceFromType_ReturnsWorkingResource()
        {
            //Arrange
            var factory = TestJsonFileStringLocalizerFactory.GetFactory(CultureSuffixStrategy.TwoLetterISO6391AndCountryCode);
            CultureInfo.CurrentUICulture = new CultureInfo("ru-RU");

            //Act
            var localizer = factory.Create(typeof(TestStringValue));
            var result = localizer["Value"];

            //Assert
            result.Value.Should().Be("Some value");
            result.ResourceNotFound.Should().BeFalse();
        }
    }
}