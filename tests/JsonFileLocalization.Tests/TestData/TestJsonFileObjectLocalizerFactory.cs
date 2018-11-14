using JsonFileLocalization.ObjectLocalization;
using JsonFileLocalization.Resource;
using JsonFileLocalization.StringLocalization;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace JsonFileLocalization.Tests.TestData
{
    public static class TestJsonFileObjectLocalizerFactory
    {
        public static JsonFileObjectLocalizerFactory GetFactory(CultureSuffixStrategy strategy)
        {
            var loggerFactory = Substitute.For<ILoggerFactory>();
            loggerFactory.CreateLogger<JsonFileStringLocalizer>()
                .Returns(Substitute.For<ILogger<JsonFileStringLocalizer>>());
            var settings = TestJsonFileResourceManager.GetSettings(strategy);
            var factory = new JsonFileObjectLocalizerFactory(loggerFactory, settings,
                TestJsonFileResourceManager.GetResourceManager(strategy));
            return factory;
        }
    }
}