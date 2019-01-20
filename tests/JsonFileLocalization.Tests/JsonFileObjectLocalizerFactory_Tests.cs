using System.Globalization;
using FluentAssertions;
using JsonFileLocalization.Resource;
using JsonFileLocalization.Tests.TestData;
using JsonFileLocalization.Tests.TestData.Models;
using Xunit;

namespace JsonFileLocalization.Tests
{
    public class JsonFileObjectLocalizerFactory_Tests
    {
        [Fact]
        public void Create_WhenCreatedFromType_ReturnsResource()
        {
            //Arrange
            CultureInfo.CurrentUICulture = new CultureInfo("ru-RU");
            var factory = TestJsonFileObjectLocalizerFactory.GetFactory(CultureSuffixStrategy.TwoLetterISO6391AndCountryCode);
            var localizer = factory.Create(typeof(IntArrayObject));

            //Act
            var result = localizer.GetLocalizedObject<int[]>("Value");

            //Assert
            result.Value.Should().Equal(8, 9, 10);
        }
    }
}