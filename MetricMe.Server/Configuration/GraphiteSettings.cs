namespace MetricMe.Server.Configuration
{
    /// <summary>
    /// Settings for graphite.
    /// </summary>
    public class GraphiteSettings
    {
        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        /// <value>
        /// The host.
        /// </value>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>
        /// The port.
        /// </value>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the global prefix.
        /// </summary>
        /// <value>
        /// The global prefix.
        /// </value>
        public string GlobalPrefix { get; set; }

        /// <summary>
        /// Gets or sets the global suffix.
        /// </summary>
        /// <value>
        /// The global suffix.
        /// </value>
        public string GlobalSuffix { get; set; }

        /// <summary>
        /// Gets or sets the counter prefix.
        /// </summary>
        /// <value>
        /// The counter prefix.
        /// </value>
        public string CounterPrefix { get; set; }

        /// <summary>
        /// Gets or sets the timer prefix.
        /// </summary>
        /// <value>
        /// The timer prefix.
        /// </value>
        public string TimerPrefix { get; set; }

        /// <summary>
        /// Gets or sets the gauge prefix.
        /// </summary>
        /// <value>
        /// The gauge prefix.
        /// </value>
        public string GaugePrefix { get; set; }

        /// <summary>
        /// Gets or sets the set prefix.
        /// </summary>
        /// <value>
        /// The set prefix.
        /// </value>
        public string SetPrefix { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to flush the counts.
        /// </summary>
        /// <value>
        ///   <c>true</c> if counts should be flushed; otherwise, <c>false</c>.
        /// </value>
        public bool FlushCounts { get; set; }
    }
}