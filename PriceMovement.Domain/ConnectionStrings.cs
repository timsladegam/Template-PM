namespace PriceMovement.Domain
{
    using System.Collections.Generic;

    /// <summary>
    /// Dat sent to to server from client.
    /// </summary>
    public class ConnectionStrings
    {
        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        public string GTPSContext { get; set; }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        public string GRDBContext { get; set; }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        public string ThinkfolioContext { get; set; }

        /// <summary>
        /// Gets or sets the database timeout.
        /// </summary>
        public int DataBaseCommandTimeout { get; set; }
    }
}
