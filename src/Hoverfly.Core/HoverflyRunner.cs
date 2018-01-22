namespace Hoverfly.Core
{
    using System;
    using Configuration;
    using Model;
    using Resources;

    /// <summary>
    /// HoverflyRunner helps starting Hoverfly in different modes.
    /// </summary>
    public class HoverflyRunner : IDisposable
    {
        private readonly Hoverfly _hoverfly;
        private readonly HoverflyMode _hoverflyMode;
        private ISimulationSource _simulationSource;
        private readonly ISimulationDestinationSource _simulationDestinationSource;

        private HoverflyRunner(ISimulationSource simulationSource, HoverflyConfig hoverflyConfig)
        {
            _hoverflyMode = HoverflyMode.Simulate;;
            _hoverfly = new Hoverfly(_hoverflyMode, hoverflyConfig);
            _simulationSource = simulationSource;
            _simulationDestinationSource = null;
        }

        private HoverflyRunner(ISimulationDestinationSource simulationDestinationSource, HoverflyConfig hoverflyConfig)
        {
            _hoverflyMode = HoverflyMode.Capture;
            _hoverfly = new Hoverfly(_hoverflyMode, hoverflyConfig);
            _simulationSource = null;
            _simulationDestinationSource = simulationDestinationSource;
        }


        private HoverflyRunner(HoverflyMode hoverflyMode, HoverflyConfig hoverflyConfig)
        {
            _hoverflyMode = hoverflyMode;
            _hoverfly = new Hoverfly(_hoverflyMode, hoverflyConfig);
            _simulationSource = null;
        }

        private HoverflyRunner(HoverflyMode hoverflyMode, ISimulationSource simulationSource, HoverflyConfig hoverflyConfig)
        {
            _hoverflyMode = hoverflyMode;
            _hoverfly = new Hoverfly(_hoverflyMode, hoverflyConfig);
            _simulationSource = simulationSource;
            _simulationDestinationSource = null;
        }

        /// <summary>
        /// Instantiates a runner which runs <see cref="Hoverfly"/> in capture mode.
        /// </summary>
        /// <param name="outputFilename">The path and filename where to store recorded simulations.</param>
        /// <returns>Returns <see cref="HoverflyRunner"/>.</returns>
        public static HoverflyRunner StartInCaptureMode(string outputFilename)
        {
            return StartInCaptureMode(outputFilename, HoverflyConfig.Config());
        }

        /// <summary>
        /// Instantiates a runner which runs <see cref="Hoverfly"/> in capture mode.
        /// </summary>
        /// <param name="outputFilename">The path and filename where to store recorded simulations.</param>
        /// <param name="hoverflyConfig">The hoverfly configuration.</param>
        /// <returns>Returns <see cref="HoverflyRunner"/>.</returns>
        public static HoverflyRunner StartInCaptureMode(string outputFilename, HoverflyConfig hoverflyConfig)
        {
            var fileSimulation = new FileSimulationSource(outputFilename) as ISimulationDestinationSource;
            return StartInCaptureMode(fileSimulation, hoverflyConfig);
        }

        /// <summary>
        /// Instantiates a runner which runs <see cref="Hoverfly"/> in capture mode.
        /// </summary>
        /// <param name="output">The <see cref="ISimulationDestinationSource"/> to store recorded simulations.</param>
        /// <returns>Returns <see cref="HoverflyRunner"/>.</returns>
        public static HoverflyRunner StartInCaptureMode(ISimulationDestinationSource output)
        {
            return StartInCaptureMode(output, HoverflyConfig.Config());
        }

        /// <summary>
        /// Instantiates a runner which runs <see cref="Hoverfly"/> in capture mode.
        /// </summary>
        /// <param name="output">The <see cref="ISimulationDestinationSource"/> to store recorded simulations.</param>
        /// <param name="hoverflyConfig">The hoverfly configuration.</param>
        /// <returns>Returns <see cref="HoverflyRunner"/>.</returns>
        public static HoverflyRunner StartInCaptureMode(ISimulationDestinationSource output, HoverflyConfig hoverflyConfig)
        {
            var hoverflyRunner = new HoverflyRunner(output, hoverflyConfig);
            hoverflyRunner.Start();
            return hoverflyRunner;
        }

        /// <summary>
        /// Instantiates a runner which runs <see cref="Hoverfly"/> in simulate mode.
        /// </summary>
        /// <returns>Returns <see cref="HoverflyRunner"/>.</returns>
        public static HoverflyRunner StartInSimulationMode()
        {
            return StartInSimulationMode(HoverflyConfig.Config());
        }

        /// <summary>
        /// Instantiates a runner which runs <see cref="Hoverfly"/> in simulate mode.
        /// </summary>
        /// <param name="hoverflyConfig">The hoverfly configuration.</param>
        /// <returns>Returns <see cref="HoverflyRunner"/>.</returns>
        public static HoverflyRunner StartInSimulationMode(HoverflyConfig hoverflyConfig)
        {
            var hoverflyRunner = new HoverflyRunner(HoverflyMode.Simulate, hoverflyConfig);
            hoverflyRunner.Start();
            return hoverflyRunner;
        }

        /// <summary>
        /// Instantiates a runner which runs <see cref="Hoverfly"/> in simulate mode.
        /// </summary>
        /// <param name="simulationFile">The simulation to import.</param>
        /// <returns>Returns <see cref="HoverflyRunner"/>.</returns>
        public static HoverflyRunner StartInSimulationMode(string simulationFile)
        {
            return StartInSimulationMode(simulationFile, HoverflyConfig.Config());
        }

        /// <summary>
        /// Instantiates a runner which runs <see cref="Hoverfly"/> in simulate mode.
        /// </summary>
        /// <param name="simulationFile">The simulation to import.</param>
        /// <param name="hoverflyConfig">The hoverfly configuration.</param>
        /// <returns>Returns <see cref="HoverflyRunner"/>.</returns>
        public static HoverflyRunner StartInSimulationMode(string simulationFile, HoverflyConfig hoverflyConfig)
        {
            return StartInSimulationMode(new FileSimulationSource(simulationFile), hoverflyConfig);
        }

        /// <summary>
        /// Instantiates a runner which runs <see cref="Hoverfly"/> in simulate mode.
        /// </summary>
        /// <param name="simulationSource">The simulation to import.</param>
        /// <returns>Returns <see cref="HoverflyRunner"/>.</returns>
        public static HoverflyRunner StartInSimulationMode(ISimulationSource simulationSource)
        {
            return StartInSimulationMode(simulationSource, HoverflyConfig.Config());
        }

        /// <summary>
        /// Instantiates a runner which runs <see cref="Hoverfly"/> in simulate mode.
        /// </summary>
        /// <param name="simulationSource">The simulation to import.</param>
        /// <param name="hoverflyConfig">The hoverfly configuration.</param>
        /// <returns>Returns <see cref="HoverflyRunner"/>.</returns>
        public static HoverflyRunner StartInSimulationMode(ISimulationSource simulationSource, HoverflyConfig hoverflyConfig)
        {
            var hoverflyRunner = new HoverflyRunner(simulationSource, hoverflyConfig);
            hoverflyRunner.Start();
            return hoverflyRunner;
        }

        /// <summary>
        /// Instantiates a runner which runs <see cref="Hoverfly"/> in Spy mode.
        /// </summary>
        /// <returns>Returns <see cref="HoverflyRunner"/>.</returns>
        public static HoverflyRunner StartInSpyMode()
        {
            return StartInSpyMode(HoverflyConfig.Config());
        }

        /// <summary>
        /// Instantiates a runner which runs <see cref="Hoverfly"/> in Spy mode.
        /// </summary>
        /// <param name="hoverflyConfig">The hoverfly configuration.</param>
        /// <returns>Returns <see cref="HoverflyRunner"/>.</returns>
        public static HoverflyRunner StartInSpyMode(HoverflyConfig hoverflyConfig)
        {
            var hoverflyRunner = new HoverflyRunner(HoverflyMode.Spy, hoverflyConfig);
            hoverflyRunner.Start();
            return hoverflyRunner;
        }

        /// <summary>
        /// Instantiates a runner which runs <see cref="Hoverfly"/> in Spy mode.
        /// </summary>
        /// <param name="simulationFile">The simulation to import.</param>
        /// <returns>Returns <see cref="HoverflyRunner"/>.</returns>
        public static HoverflyRunner StartInSpyMode(string simulationFile)
        {
            return StartInSpyMode(simulationFile, HoverflyConfig.Config());
        }

        /// <summary>
        /// Instantiates a runner which runs <see cref="Hoverfly"/> in Spy mode.
        /// </summary>
        /// <param name="simulationFile">The simulation to import.</param>
        /// <param name="hoverflyConfig">The hoverfly configuration.</param>
        /// <returns>Returns <see cref="HoverflyRunner"/>.</returns>
        public static HoverflyRunner StartInSpyMode(string simulationFile, HoverflyConfig hoverflyConfig)
        {
            return StartInSpyMode(new FileSimulationSource(simulationFile), hoverflyConfig);
        }

        /// <summary>
        /// Instantiates a runner which runs <see cref="Hoverfly"/> in Spy mode.
        /// </summary>
        /// <param name="simulationSource">The simulation to import.</param>
        /// <returns>Returns <see cref="HoverflyRunner"/>.</returns>
        public static HoverflyRunner StartInSpyMode(ISimulationSource simulationSource)
        {
            return StartInSpyMode(simulationSource, HoverflyConfig.Config());
        }

        /// <summary>
        /// Instantiates a runner which runs <see cref="Hoverfly"/> in Spy mode.
        /// </summary>
        /// <param name="simulationSource">The simulation to import.</param>
        /// <param name="hoverflyConfig">The hoverfly configuration.</param>
        /// <returns>Returns <see cref="HoverflyRunner"/>.</returns>
        public static HoverflyRunner StartInSpyMode(ISimulationSource simulationSource, HoverflyConfig hoverflyConfig)
        {
            var hoverflyRunner = new HoverflyRunner(HoverflyMode.Spy, simulationSource, hoverflyConfig);
            hoverflyRunner.Start();
            return hoverflyRunner;
        }

        /// <summary>
        /// Starts <see cref="Hoverfly"/>.
        /// </summary>
        protected void Start()
        {
            _hoverfly.Start();
            ImportSimulationSource();
        }

        /// <summary>
        /// Stops <see cref="Hoverfly"/>.
        /// </summary>
        public void Stop()
        {
            try
            {
                if (_hoverflyMode == HoverflyMode.Capture && _simulationDestinationSource != null)
                    _hoverfly.ExportSimulation(_simulationDestinationSource);
            }
            finally
            {
                _hoverfly.Stop();
            }
        }

        /// <summary>
        /// Gets the proxy port this runner runs on.
        /// </summary>
        /// <returns>Returns the proxy port.</returns>
        public int GetProxyPort()
        {
            return _hoverfly.GetProxyPort();
        }

        /// <summary>
        /// Gets started Hoverfly mode.
        /// </summary>
        /// <returns>Returns the <see cref="HoverflyMode"/>.</returns>
        public HoverflyMode GetHoverflyMode()
        {
            return _hoverfly.GetMode();
        }

        /// <summary>
        /// Changes the Simulation used by <see cref="Hoverfly"/>.
        /// </summary>
        /// <param name="simulationSource">The simulation source.</param>
        public void Simulate(ISimulationSource simulationSource)
        {
            if (simulationSource == null)
                throw new ArgumentNullException(nameof(simulationSource));

            _simulationSource = simulationSource;
            ImportSimulationSource();
        }

        /// <summary>
        /// Adds simulations.
        /// </summary>
        /// <param name="simulationSource">The simulation to add.</param>
        /// <remarks>This method can be used when a simulation is already loaded, 
        /// and in some tests need to add more simulation without replacing the whole simulation that is already loaded.
        /// </remarks>
        public void AddSimulation(ISimulationSource simulationSource)
        {
            if (simulationSource == null)
                throw new ArgumentNullException(nameof(simulationSource));

            _hoverfly.AddSimulation(simulationSource);
        }


        /// <summary>
        /// Get the <see cref="Hoverfly"/> simulation data if any.
        /// </summary>
        public Simulation GetSimulation()
        {
            return _hoverfly.GetSimulation();
        }

        public void Dispose()
        {
            Stop();
        }

        private void ImportSimulationSource()
        {
            try
            {
                if (_simulationSource != null && (_hoverflyMode == HoverflyMode.Simulate || _hoverflyMode == HoverflyMode.Spy))
                    _hoverfly.ImportSimulation(_simulationSource);
            }
            catch (Exception)
            {
                Stop();                
                throw;
            }
        }
    }
}