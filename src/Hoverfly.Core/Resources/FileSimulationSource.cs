namespace Hoverfly.Core.Resources
{
    using System;
    using System.IO;
    using System.Text;

    using Model;

    using Newtonsoft.Json;

    /// <summary>
    /// FileSimulationSource is used for saving and loading hoverfly simulation data.
    /// </summary>
    public class FileSimulationSource : ISimulationSource, ISimulationDestinationSource
    {
        /// <summary>
        /// Creates a source for the hoverfly simulation data.
        /// </summary>
        /// <param name="resoucePath">The file path to the simulation data.</param>
        public FileSimulationSource(string resoucePath)
        {
            ResourcePath = resoucePath;
        }

        /// <summary>
        /// The base path to the simulation data.Will not include any file name.
        /// </summary>
        public string ResourcePath { get; }

        /// <summary>
        /// Gets a simulation data from the ResourcePath.
        /// </summary>
        /// <returns>Returns <see cref="Simulation"/>with the simulation data.</returns>
        public Simulation GetSimulation()
        {
            var filetoLoad = Path.Combine(ResourcePath);

            if (!File.Exists(filetoLoad))
                throw new FileNotFoundException($"Can't find the file '{filetoLoad}'.");

            return JsonConvert.DeserializeObject<Simulation>(File.ReadAllText(filetoLoad));
        }

        /// <summary>
        /// Saves the simulation data.
        /// </summary>
        /// <param name="simulation">The <see cref="Simulation"/> to save.</param>
        /// <remarks>The simulaion data will be saved to the specifed ResourcePath, if the file already exists it will be overwritten.</remarks>
        public void SaveSimulation(Simulation simulation)
        {
            if (string.IsNullOrWhiteSpace(ResourcePath))
                throw new ArgumentNullException($"You need to set the {nameof(ResourcePath)} to a valid file path.");

            File.WriteAllBytes(ResourcePath, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(simulation)));
        }
    }
}