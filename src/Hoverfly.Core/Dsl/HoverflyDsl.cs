namespace Hoverfly.Core.Dsl
{
    using System;

    public static class HoverflyDsl
    {
        public static StubServiceBuilder Service(Uri baseUrl)
        {
            if (baseUrl == null)
                throw new ArgumentNullException(nameof(baseUrl));

            return new StubServiceBuilder(baseUrl);
        }

        public static StubServiceBuilder Service(string baseUrl)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new ArgumentNullException(nameof(baseUrl));

            if (!baseUrl.Contains("http"))
                baseUrl = $"http://{baseUrl}";

            return new StubServiceBuilder(baseUrl);
        }
    }
}