namespace Data.WebClient.Enums
{
    public enum ErrorHandlingMode
    {
        /// <summary>
        /// Return null as response on error
        /// </summary>
        Ignore,

        /// <summary>
        /// Return WebClientException on error
        /// </summary>
        Manual,

        /// <summary>
        /// Try return appropriate exception from Common.Kernel.Exceptions project
        /// </summary>
        Auto,

        /// <summary>
        /// Show WebClientException with debug information
        /// </summary>
        Debug
    }
}
