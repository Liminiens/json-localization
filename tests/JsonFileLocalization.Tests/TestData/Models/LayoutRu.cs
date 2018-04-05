using System;
using System.Collections.Generic;
using System.Text;

namespace JsonFileLocalization.Tests.TestData.Models
{
    public class LayoutRu
    {
        public string TestString { get; set; }

        public TestObject TestObject { get; set; }

        public InnerArray Inner { get; set; }

        public class InnerArray
        {
            public string[] TestArray { get; set; }
        }
    }
}
