using Hoverfly.Core.Configuration;
using System;
using System.IO;

namespace Hoverfly.Core.Tests
{
    public static class HoverFlyTestConfig
    {
        public static string PackagePath64 => Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\.hoverfly\\amd64\\");

        public static string PackagePath32 => Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\.hoverfly\\386\\");

        public static HoverflyConfig GetHoverFlyConfigWIthBasePath(bool amd64 = true)
        {
            return HoverflyConfig.Config().SetHoverflyBasePath(amd64 ? PackagePath64 : PackagePath32);
        }
    }
}
