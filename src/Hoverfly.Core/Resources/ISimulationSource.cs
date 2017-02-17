namespace Hoverfly.Core.Resources
{
    using Model;

    /// <summary>
    /// ISimulationSource is used for load hoverfly simulation data.
    /// </summary>
    public interface ISimulationSource
    {
        /// <summary>
        /// Gets a simulation data from the ResourcePath.
        /// </summary>
        /// <returns>Returns <see cref="Simulation"/>with the simulation data.</returns>
        Simulation GetSimulation();
    }
}