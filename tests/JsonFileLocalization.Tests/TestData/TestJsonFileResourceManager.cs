using System;
using System.Collections.Generic;
using System.IO;
using JsonFileLocalization.Resource;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.Core;

namespace JsonFileLocalization.Tests.TestData
{
    public static class TestJsonFileResourceManager
    {
        public static JsonFileLocalizationSettings GetSettings(JsonFileCultureSuffixStrategy strategy)
        {
            var environment = Substitute.For<IHostingEnvironment>();
            environment.ContentRootPath.Returns(Directory.GetCurrentDirectory());
            var settings = new JsonFileLocalizationSettings(environment, strategy, "Resources");
            return settings;
        }

        public static (ILoggerFactory Factory, Func<IEnumerable<ICall>> LoggerCalls) GetLoggerFactory()
        {
            var loggerFactory = Substitute.For<ILoggerFactory>();
            var logger = Substitute.For<ILogger<JsonFileResource>>();
            loggerFactory.CreateLogger<JsonFileResource>().Returns(logger);
            return (loggerFactory, () => logger.ReceivedCalls());
        }
        public static JsonFileResourceManager GetResourceManager(JsonFileCultureSuffixStrategy strategy)
        {
            return new JsonFileResourceManager(GetSettings(strategy), GetLoggerFactory().Factory);
        }
    }
}