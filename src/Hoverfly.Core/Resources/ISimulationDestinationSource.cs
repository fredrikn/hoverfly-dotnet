namespace Hoverfly.Core.Resources
{
    using Model;

    /// <summary>
    /// ISimulationDestinationSource is used for saving hoverfly simulation data.
    /// </summary>
    public interface ISimulationDestinationSource
    {
        /// <summary>
        /// Saves the simulation data.
        /// </summary>
        /// <param name="simulation">The <see cref="Simulation"/> to save.</param>
        void SaveSimulation(Simulation simulation);
    }
}
