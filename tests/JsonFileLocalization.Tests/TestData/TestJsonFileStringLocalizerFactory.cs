using JsonFileLocalization.Resources;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace JsonFileLocalization.Tests.TestData
{
    public static class TestJsonFileStringLocalizerFactory
    {
        public static JsonFileStringLocalizerFactory GetFactory(JsonFileCultureSuffixStrategy strategy)
        {
            var loggerFactory = Substitute.For<ILoggerFactory>();
            loggerFactory.CreateLogger<JsonFileStringLocalizer>()
                .Returns(Substitute.For<ILogger<JsonFileStringLocalizer>>());
            var settings = TestJsonFileResourceManager.GetSettings(strategy);
            var factory = new JsonFileStringLocalizerFactory(loggerFactory, settings,
                TestJsonFileResourceManager.GetResourceManager(strategy));
            return factory;
        }
    }
}