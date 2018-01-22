namespace Hoverfly.Core.Configuration
{
    using global::Hoverfly.Core.Logging;
    using System;

    public class HoverflyConfig
    {
        private const int DEFAULT_PROXY_PORT = 8500;
        private const int DEFAULT_ADMIN_PORT = 8888;

        private const string LOCALHOST = "http://localhost";

        /// <summary>
        /// Gets the admin port number used by hoverfly.
        /// </summary>
        public int AdminPort { get; private set; } = DEFAULT_ADMIN_PORT;

        /// <summary>
        /// Gets the headers to capture.
        /// </summary>
        public string[] CaptureHeaders { get; private set; }

        /// <summary>
        /// Gets the proxy port number used by hoverfly.
        /// </summary>
        public int ProxyPort { get; private set; } = DEFAULT_PROXY_PORT;

        /// <summary>
        /// Gets if the hoverfly uses a remote instance.
        /// </summary>
        /// <remarks>If set to true, no hoverfly process will be started by .Net.</remarks>
        public bool IsRemoteInstance { get; private set; }

        /// <summary>
        /// Gets the base URL of the remote hoverfly instance.
        /// </summary>
        public string RemoteHost { get; private set; } = LOCALHOST;

        /// <summary>
        /// Gets if any request to localhost will be or not be proxied through hoverfly..
        /// </summary>
        public bool ProxyLocalhost { get; private set; }

        /// <summary>
        /// Gets the base path to the hoverfly.exe.
        /// </summary>
        public string HoverflyBasePath { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the logger used to log information. Default is <see cref="OutputLog"/>"
        /// </summary>
        public ILog Logger { get; private set; } = new OutputLog();

        /// <summary>
        /// Create an new instance of <see cref="HoverflyConfig"/>.
        /// </summary>
        /// <returns>Returns an instance of <see cref="HoverflyConfig"/>.</returns>
        public static HoverflyConfig Config() => new HoverflyConfig();

        internal bool UseRandomProxyPort { get; private set; } = false;

        internal bool UseRandomAdminPort { get; private set; } = false;

        /// <summary>
        /// Use this method if there is already a remote instance of hoverfly running. By using this method .Net will not start a hoverfly instance.
        /// </summary>
        /// <param name="remoteHost">The base URL of the remote hoverfly instance. If nothing is specified, localhost, will be used by default.</param>
        /// <param name="proxyPort">The proxy port the remote hoverfly uses. If nothing is specified, 8500, will be used by default.</param>
        /// <param name="adminPort">The admin port the remote hoverfly uses. If nothing is specified, 8888, will be used by default.</param>
        /// <returns>Returns <see cref="HoverflyConfig"/> for further customizations.</returns>
        public HoverflyConfig UseRemoteInstance(
            string remoteHost = null,
            int proxyPort = DEFAULT_PROXY_PORT,
            int adminPort = DEFAULT_ADMIN_PORT)
        {
            IsRemoteInstance = true;
            RemoteHost = remoteHost ?? LOCALHOST;
            return this;
        }

        /// <summary>
        /// Sets the proxy port used by the hoverfly.
        /// </summary>
        /// <param name="port">The proxy port.</param>
        /// <param name="createRandomIfAlreadyInUse">Specify if Hoverfly should try to create a random port if the specfied one is already in use. Default is false.</param>
        /// <returns>Returns <see cref="HoverflyConfig"/> for further customizations.</returns>
        public HoverflyConfig SetProxyPort(int port, bool createRandomIfAlreadyInUse = false)
        {
            ProxyPort = port;
            UseRandomProxyPort = createRandomIfAlreadyInUse;
            return this;
        }

        /// <summary>
        /// Specifies that Hoverfly should try to create a random Proxy port if the default or specified one is already in use.
        /// </summary>
        /// <returns>Returns <see cref="HoverflyConfig"/> for further customizations.</returns>
        public HoverflyConfig AllowToCreateRandomProxyPort()
        {
            UseRandomProxyPort = true;
            return this;
        }


        /// <summary>
        /// Sets the admin used by the hoverfly.
        /// </summary>
        /// <param name="port">The admin port.</param>
        /// <param name="createRandomIfAlreadyInUse">Specify if Hoverfly should try to create a random port if the specfied one is already in use. Default is false.</param>
        /// <returns>Returns <see cref="HoverflyConfig"/> for further customizations.</returns>
        public HoverflyConfig SetAdminPort(int port, bool createRandomIfAlreadyInUse = false)
        {
            AdminPort = port;
            UseRandomAdminPort = createRandomIfAlreadyInUse;
            return this;
        }

        /// <summary>
        /// Specifies that Hoverfly should try to create a random Admin port if the default or specified one is already in use.
        /// </summary>
        /// <returns>Returns <see cref="HoverflyConfig"/> for further customizations.</returns>
        public HoverflyConfig AllowToCreateRandomAdminPort()
        {
            UseRandomAdminPort = true;
            return this;
        }

        /// <summary>
        /// Sets the base remote host of an already running hoverfly instance.
        /// </summary>
        /// <param name="remoteHost">The remote host URL port.</param>
        /// <returns>Returns <see cref="HoverflyConfig"/> for further customizations.</returns>
        public HoverflyConfig SetRemoteHost(string remoteHost)
        {
            if (string.IsNullOrWhiteSpace(remoteHost))
                throw new ArgumentNullException(nameof(remoteHost));

            RemoteHost = remoteHost;
            return this;
        }

        /// <summary>
        /// Sets if localhost should be proxied.
        /// </summary>
        /// <param name="proxyLocalhost">If localhost should be proxied or not.</param>
        /// <returns>Returns <see cref="HoverflyConfig"/> for further customizations.</returns>
        public HoverflyConfig SetProxyLocalhost(bool proxyLocalhost)
        {
            ProxyLocalhost = proxyLocalhost;
            return this;
        }

        /// <summary>
        /// Sets the base path to the hoverfly.exe.
        /// </summary>
        /// <param name="hoverflyBasePath">The base path to the hoverfly.exe.</param>
        /// <returns>Returns <see cref="HoverflyConfig"/> for further customizations.</returns>
        public HoverflyConfig SetHoverflyBasePath(string hoverflyBasePath)
        {
            if (string.IsNullOrWhiteSpace(hoverflyBasePath))
                throw new ArgumentNullException(nameof(hoverflyBasePath));

            HoverflyBasePath = hoverflyBasePath;
            return this;
        }

        /// <summary>
        /// Specifies which request headers to capture.
        /// </summary>
        /// <param name="headers">An array of header names</param>
        /// <returns>Returns <see cref="HoverflyConfig"/> for further customizations.</returns>
        public HoverflyConfig SetCaptureHeaders(params string[] headers)
        {
            CaptureHeaders = headers;
            return this;
        }

        /// <summary>
        /// Sets the logger to be used.
        /// </summary>
        /// <param name="logger">The <see cref="ILog"/> to be used for logging.</param>
        /// <returns>Returns <see cref="HoverflyConfig"/> for further customizations.</returns>
        public HoverflyConfig SetLogger(ILog logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            return this;
        }

        /// <summary>
        /// Set to capture all request headers
        /// </summary>
        /// <returns>Returns <see cref="HoverflyConfig"/> for further customizations.</returns>
        public HoverflyConfig CaptureAllHeaders()
        {
            CaptureHeaders = new[] { "*" };
            return this;
        }
    }
}