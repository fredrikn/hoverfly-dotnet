namespace Hoverfly.Dsl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Core.Model;
    using Core.Resources;

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
            var pairs = _serviceBuilder.SelectMany(pair => pair.RequestResponsePairs).ToList();

            var hoverflyData = new HoverflyData(pairs, new GlobalActions(new List<DelaySettings>()));

            return new Simulation(hoverflyData, new HoverflyMetaData());
        }
    }
}
