using JsonFileLocalization.Resource;
using JsonFileLocalization.StringLocalization;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace JsonFileLocalization.Tests.TestData
{
    public static class TestJsonFileStringLocalizerFactory
    {
        public static JsonFileStringLocalizerFactory GetFactory(CultureSuffixStrategy strategy)
        {
            var loggerFactory = Substitute.For<ILoggerFactory>();
            loggerFactory.CreateLogger<JsonFileStringLocalizer>()
                .Returns(Substitute.For<ILogger<JsonFileStringLocalizer>>());
            var factory = new JsonFileStringLocalizerFactory(loggerFactory,
                TestJsonFileResourceManager.GetResourceManager(strategy));
            return factory;
        }
    }
}