namespace Hoverfly.Core.Resources
{
    using System;
    using System.IO;

    /// <summary>
    /// SimulationSource is used for saveing and loading hoverfly simulation data.
    /// </summary>
    public class SimulationSource : ISimulationSource
    {
        /// <summary>
        /// Creates a source for the hoverfly simulation data.
        /// </summary>
        /// <param name="resourcePath">The file path to the simulation data.</param>
        public SimulationSource(string resourcePath)
        {
            if (string.IsNullOrWhiteSpace(resourcePath))
                throw new ArgumentNullException(nameof(resourcePath));

            ResourcePath = resourcePath;
        }

        /// <summary>
        /// The path to the simulation file.
        /// </summary>
        public string ResourcePath { get; }

        /// <summary>
        /// Gets the simulation data for the given name.
        /// </summary>
        /// <returns>Returns a byte array with the simulation data.</returns>
        public byte[] GetSimulation()
        {
            if (File.Exists(ResourcePath))
                throw new FileNotFoundException($"Can't find the file '{ResourcePath}'.");

            return File.ReadAllBytes(ResourcePath);
        }

        /// <summary>
        /// Saves the simulation data.
        /// </summary>
        /// <param name="simulationData">A Byte array that contains the simulation data.</param>
        /// <param name="name">The name of the simulation data.</param>
        public void SaveSimulation(byte[] simulationData, string name)
        {
            File.WriteAllBytes(name, simulationData);
        }
    }
}
