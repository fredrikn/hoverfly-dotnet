namespace Hoverfly.Core.Resources
{
    /// <summary>
    /// The client that works against a hoverfly instance.
    /// </summary>
    public interface IHoverflyClient
    {
        /// <summary>
        /// Imports simulation to hoverfly.
        /// </summary>
        /// <param name="simulationData">The simulation data as a hoverfly json simulation.</param>
        void ImportSimulation(byte[] simulationData);

        /// <summary>
        /// Gets the simulation recorded by hoverfly.
        /// </summary>
        byte[] GetSimulation();

        /// <summary>
        /// Cheks if hoverfly is running and is healty.
        /// </summary>
        /// <returns>Returns true if hoverfly is healthy.</returns>
        bool IsHealthy();
    }
}