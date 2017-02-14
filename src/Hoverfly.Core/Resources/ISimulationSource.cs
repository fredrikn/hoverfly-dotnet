namespace Hoverfly.Core.Resources
{
    /// <summary>
    /// ISimulationSource is used for saveing and loading hoverfly simulation data.
    /// </summary>
    public interface ISimulationSource
    {
        /// <summary>
        /// The path to the simulation file.
        /// </summary>
        string ResourcePath { get; }

        /// <summary>
        /// Gets the simulation data for the given name.
        /// </summary>
        /// <returns>Returns a byte array with the simulation data.</returns>
        /// <remarks>Will use the specified ResourcePath to get the simulation data.</remarks>
        byte[] GetSimulation();

        /// <summary>
        /// Saves the simulation data.
        /// </summary>
        /// <param name="simulationData">A Byte array that contains the simulation data.</param>
        /// <param name="name">The name of the simulation data.</param>
        void SaveSimulation(byte[] simulationData, string name);
    }
}