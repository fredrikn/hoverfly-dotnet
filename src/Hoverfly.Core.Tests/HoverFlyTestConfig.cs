using Hoverfly.Core.Configuration;
using System;
using System.IO;

namespace Hoverfly.Core.Tests
{
    public static class HoverFlyTestConfig
    {
        public static string PackagePath => Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\packages\\SpectoLabs.Hoverfly.0.11.3\\tools\\");

        public static HoverflyConfig GetHoverFlyConfigWIthBasePath()
        {
            return HoverflyConfig.Config().SetHoverflyBasePath(PackagePath);
        }
    }
}
