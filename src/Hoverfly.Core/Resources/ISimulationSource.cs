namespace Hoverfly.Core.Resources
{
    using Model;

    /// <summary>
    /// ISimulationSource is used for saveing and loading hoverfly simulation data.
    /// </summary>
    public interface ISimulationSource
    {
        /// <summary>
        /// Gets a simulation data from the ResourcePath.
        /// </summary>
        /// <returns>Returns <see cref="Simulation"/>with the simulation data.</returns>
        Simulation GetSimulation();

        /// <summary>
        /// Saves the simulation data.
        /// </summary>
        /// <param name="simulation">The <see cref="Simulation"/> to save.</param>
        /// <param name="fileName">The path and file name to where he simulation data should be saved. If the file exists, it will be overwritten.</param>
        void SaveSimulation(Simulation simulation, string fileName);
    }
}