### New in 0.4.1 (Released 2018/01/22)
* Removed the log factory
* Added support to add a logger to the Hoverfly's config.
* Added support of adding simulation for Hoverfly and not only for the HoverflyRunner.

### New in 0.4.0 (Released 2018/01/13)
* Upgrade to use Hoverfly version 0.15.0
* Added the Spy mode, Hoverfly simulates external APIs if a request match is found in simulation data, otherwise, the request will be passed through to the real API.

### New in 0.3.0 (Released 2017/10/06)
* Upgrade to use Hoverfly version 0.14.2
* Removed the use of Webserver mode.
* Added support of capturing all or specific headers.

### New in 0.2.1 (Released 2017/08/17)
* Fixed a bug where exact field match was always added to the body and query requests.

### New in 0.2.0 (Released 2017/05/16)
* Upgrade to support Hoverfly version 0.11.3
* Schema changes, targetting v2
* NOTE: Older v1 simulation files will not work with this version.

### New in 0.1.3 (Released 2017/02/25)
* Upgrade to support Hoverfly version 0.10.2
* Added support of adding simulations to existing simulation with the HoverflyRunner

### New in 0.1.2 (Released 2017/02/19)
* Added support for specifying Delays while creating services with the hoverfly Dsl.

### New in 0.1.1 (Released 2017/02/19)
* Adding HoverflyRunner to make it faster to create a hoverfly instance.
* Added the possibity to specify if random proxy and/or admin port should be created when a port is already in use by other process.

### New in 0.1.0 (Released 2017/02/18)
* API's against Hoverfly
* DSL to make it easier to create Simulations