namespace Hoverfly.Core.Resources
{
    /// <summary>
    /// ISimulationSource is used for saveing and loading hoverfly simulation data.
    /// </summary>
    public interface ISimulationSource
    {
        /// <summary>
        /// Gets the simulation data for the given name.
        /// </summary>
        /// <param name="name">The resource name of the simulation if any.</param>
        /// <returns>Returns a byte array with the simulation data.</returns>
        /// <remarks>Will use the specified ResourcePath to get the simulation data.</remarks>
        byte[] GetSimulation(string name);

        /// <summary>
        /// Saves the simulation data.
        /// </summary>
        /// <param name="simulationData">A Byte array that contains the simulation data.</param>
        /// <param name="name">The name of the simulation data.</param>
        void SaveSimulation(byte[] simulationData, string name);
    }
}