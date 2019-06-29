We are so happy that you want to use Hoverfly DotNet :)

Hoverfly binaries are not part of this package. So you need to download Hoverfly first.

1) Create a folder called ".hoverfly" in the root path where your tests will be run.
   If you use 64-bit please create a subfolder to ".hoverfly" with the name "amd64"
   If you use 32-bit please create a subfolder to ".hoverfly" with the name "386"

2) Download Hoverfly from:

32-bit
https://github.com/SpectoLabs/hoverfly/releases/download/v1.0.1/hoverfly_bundle_windows_386.zip

64-bit
https://github.com/SpectoLabs/hoverfly/releases/download/v1.0.1/hoverfly_bundle_windows_amd64.zip

Unzip the correct version into the ".hoverfly\<bit>\" folder.

Note:

You can also just put Hoverfly anywhere on the dist, add the path to the Environment variable "PATH"

By default Hoverfly DotNet will first use the HoverflyConfig.HoverflyBasePath to try to locate where the Hoverfly.exe is located.
If it can't find it, till will try to just run "Hoverfly.exe". If that doesn't work Hoverfly DotNet will search in the subdirectoried of the
specified HoverflyConfig.HoverflyBasePath (if the config is not set, default null, the Environment current directory will be used).