namespace Hoverfly.Core.Resources
{
    using System;
    using System.IO;

    /// <summary>
    /// JsonFileSimulationSource is used for saveing and loading hoverfly simulation data.
    /// </summary>
    public class JsonFileSimulationSource : ISimulationSource
    {
        /// <summary>
        /// Creates a source for the hoverfly simulation data.
        /// </summary>
        /// <param name="resourcePath">The file path to the simulation data.</param>
        public JsonFileSimulationSource(string resourcePath)
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
        /// <param name="name">The name of the simulation file. Will be the name of a .json file.</param>
        /// <returns>Returns a byte array with the simulation data.</returns>
        public byte[] GetSimulation(string name)
        {
            var jsonFile = Path.Combine(ResourcePath, name);

            if (!jsonFile.Contains(".json"))
                jsonFile = jsonFile + ".json";

            if (!File.Exists(jsonFile))
                throw new FileNotFoundException($"Can't find the file '{jsonFile}'.");

            return File.ReadAllBytes(jsonFile);
        }

        /// <summary>
        /// Saves the simulation data.
        /// </summary>
        /// <param name="simulationData">A Byte array that contains the simulation data.</param>
        /// <param name="name">The name of the simulation data file.</param>
        public void SaveSimulation(byte[] simulationData, string name)
        {
            var jsonFile = Path.Combine(ResourcePath, name);

            if (!jsonFile.Contains(".json"))
                jsonFile = jsonFile + ".json";

            File.WriteAllBytes(jsonFile, simulationData);
        }
    }
}