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
        void SaveSimulation(Simulation simulation);
    }
}