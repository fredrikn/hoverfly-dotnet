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
        /// Changes the hoverfly mode.
        /// </summary>
        /// <param name="mode">The <see cref="HoverflyMode"/> to change to.</param>
        void ChangeMode(HoverflyMode mode);

        /// <summary>
        /// Gets the hoverfly mode.
        /// </summary>
        /// <returns>Return the <see cref="HoverflyMode"/> the current hoverfly process uses.</returns>
        HoverflyMode GetMode();

        /// <summary>
        /// Cheks if hoverfly is running and is healty.
        /// </summary>
        /// <returns>Returns true if hoverfly is healthy.</returns>
        bool IsHealthy();
    }
}