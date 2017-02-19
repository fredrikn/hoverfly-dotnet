namespace Hoverfly.Core.Dsl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Model;
    using Resources;

    public class DslSimulationSource : ISimulationSource
    {
        private readonly StubServiceBuilder[] _serviceBuilder;

        public DslSimulationSource(params StubServiceBuilder[] serviceBuilder)
        {
            if (serviceBuilder == null)
                throw new ArgumentNullException(nameof(serviceBuilder));

            _serviceBuilder = serviceBuilder;
        }

        public static DslSimulationSource Dsl(params StubServiceBuilder[] serviceBuilder)
        {
            return new DslSimulationSource(serviceBuilder);
        }

        public Simulation GetSimulation()
        {
            var pairs = _serviceBuilder.SelectMany(service => service.RequestResponsePairs).ToList();

            var delaySettings = new List<DelaySettings>();
            delaySettings.AddRange(_serviceBuilder.SelectMany(service => service.Delays));

            var hoverflyData = new HoverflyData(pairs, new GlobalActions(delaySettings));

            return new Simulation(hoverflyData, new HoverflyMetaData());
        }
    }
}