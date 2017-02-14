namespace Hoverfly.Core.Resources
{
    public interface IHoverflyClient
    {
        /// <summary>
        /// Imports simulation to hoverfly.
        /// </summary>
        /// <param name="simulationData">The simulation as a hoverfly json simulatnon.</param>
        void ImportSimulation(string simulationData);

        /// <summary>
        /// Cheks if hoverfly is running and is healty.
        /// </summary>
        /// <returns>Returns true if hoverfly is healthy.</returns>
        bool IsHealthy();
    }
}