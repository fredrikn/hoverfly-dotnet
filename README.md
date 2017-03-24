# hoverfly-dotnet
A .Net Library for Hoverfly (http://hoverfly.io/)

Hoverfly is maintained by SpectoLabs (https://specto.io/)

Hoverfly DotNet on NuGet https://www.nuget.org/packages/Hoverfly-DotNet

# What is Hoverfly DotNet?
Hoverfly is a lightweight service virtualisation tool which allows you to stub / simulate HTTP(S) services. It is a proxy written in Go which responds to HTTP(S) requests with stored responses, pretending to be it’s real counterpart.

It enables you to get around common testing problems caused by external dependencies, such as non-deterministic data, flakiness, not yet implemented API’s, licensing fees, slow tests and more.

Hoverfly .Net is a native language binding which gives you an expressive API for managing Hoverfly in .Net. It gives you a Hoverfly class which abstracts away the binary and API calls.

***NOTE: If you run the tests in parallel you can only at the moment have one Hoverfly process started. In those cases you need to start the process before all tests will be running, and stop when all tests are done. BUT you can only have one simulation loaded, or you can first use hoverfly.GetSimulation method to get the current Simulation, then modify the Simulation object and then use hoverfly.ImportSimulation to import the “new” Simulation.***

### Example of using simulation:

```cs
var hoverfly = new Hoverfly(HoverflyMode.Simulate);

hoverfly.Start();

var result = //Use HttpClient to get content, e.g. "http://time.jsontest.com";

// This call will not reach the destination URL, insted Hoverfly will return a first call that is recorded.
var result2 = //Use HttpClient to get content, e.g. "http://time.jsontest.com";

hoverfly.Stop();
```

### Example of using HoverflyRunner for simulation:

```cs
using (var runner = HoverflyRunner.StartInSimulationMode())
{
}
```

### Capture and export simulations:

```cs
var hoverfly = new Hoverfly(HoverflyMode.Capture);

hoverfly.Start();

// use for example HttpClient to make a call to a URL, e.g. "http://echo.jsontest.com/key/value/one/two");

hoverfly.ExportSimulation("simulation.json");

hoverfly.Stop();
```

### Example of using HoverflyRunner to capture automatically export simulations to a file:

```cs
using (var runner = HoverflyRunner.StartInCaptureMode("simulation.json"))
{
   // All HTTP calls will be captured, and then saved to "simnulation.json"
}
```

### Import recorded simulations into hoverfly:

```cs
var hoverfly = new Hoverfly(HoverflyMode.Simulate);

hoverfly.Start();

hoverfly.ImportSimulation(new FileSimulationSource("simulation.json"));

// use for example HttpClient to make a call to a URL, e.g. "http://echo.jsontest.com/key/value/one/two");
// and it will return the result from the imported recorded simulation

hoverfly.Stop();
```

### Example of using HoverflyRunner to import simulation at start:

```cs
using (var runner = HoverflyRunner.StartInSimulationMode("simulation.json"))
{
}
```

### Specify your own simulation for a call to specific URL with Hoverfly Dsl:

```cs
using static Hoverfly.Core.Dsl.HoverflyDsl;
using static Hoverfly.Core.Dsl.ResponseCreators;
using static Hoverfly.Core.Dsl.DslSimulationSource;
    
...

var hoverfly = new Hoverfly(HoverflyMode.Simulate);

hoverfly.Start();

hoverfly.ImportSimulation(Dsl(
                Service("http://myservice.something.com")
                    .Get("/key/value/three/four")
                    .WillReturn(Success("Hello World!", "plain/text"))));

// Every call (using for example HttpClient) to the URL http://echo.jsontest.com/key/value/three/four
// will now return the specified success response in the imported simulation. This will happen as 
// long as hoverfly is running.
var result = <Http Get Content From "http://myservice.something.com/key/value/three/four">

hoverfly.Stop();
```

### Specify your own simulation for a call to specific URL with Hoverfly Dsl and HoverflyRunner:

```cs
using (var runner = HoverflyRunner.StartInSimulationMode())
{
   runner.Simulate(Dsl(
                    Service("http://mysuper.microservice.com")
                            .Get("/key/value/three/four")
                            .WillReturn(Success("Hello World!", "text/plain"))));

   var result = // Get result from "http://mysuper.microservice.com/key/value/three/four" 
                // using for example HttpClient will return "Hello World!"
}
```

### Example of using delays for a request (Can be used to simulate timeouts etc.):

```cs
 hoverfly.ImportSimulation(
                    Dsl(
                        Service("http://mysuper.microservice.com")
                            .Get("/key/value/three/four")
                            .WithDelay(2000)
                            .WillReturn(Success("Test", "application/json"))));
```

### Example of using hoverfly in a integration test:

```cs
Hoverfly hoverfly;

void StartUp()
{
    hoverfly = new Hoverfly(HoverflyMode.Simulate);
    hoverfly.ImportSimulation("simulation.json");
    hoverfly.Start();
}

void MyTest()
{
    var myController = new MyController();
    
    var result = myController.Get("10");
    
    Assert.Equals(result.StatusCode, 200);
}

void TearDown()
{
   hoverfly.Stop();
}
```

MyController, using ASP.Net Core

```cs
[Route("api/[controller]")]
public class MyController : Controller
{
   [HttpGet]
   public IActionResult Get(string userId)
   {
       // Assume this code integrates with another service (maybe a Microservice :P) over HTTP with HttpClient
       // httpClient.GetAsync("http://mysuper.microservice.com/foo");
       
       ...
       
       return result;
   }
}
```

Hoverfly will add a Proxy and all Http calls will go through that proxy. Hoverfly will in simulation mode return a recorded result for a given Http call, so no external call will be made. With the use of hoverfly in our integration/component test we don't need to create fakes to avoid external calls. With hoverfly we can also in our tests simulate delays on certain external calls etc. For example to test timeouts etc.
