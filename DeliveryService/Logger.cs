namespace DeliveryService
{
    /// <summary>
    /// Simple file logger
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// Path to log file
        /// </summary>
        private string _logFilePath;

        public Logger(string logFilePath)
        {
            _logFilePath = logFilePath;
        }

        /// <summary>
        /// Log message to file
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <exception cref="IOException"></exception>
        public void Log(string message)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(_logFilePath, true))
                {
                    writer.WriteLine($"{DateTime.Now}: {message}");
                }
            }
            catch (IOException e) 
            {
                throw new IOException($"Error logging to file: {e.Message}", e);
            }
        }
    }
}