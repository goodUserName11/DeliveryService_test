using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using CsvHelper.TypeConversion;

namespace DeliveryService
{
    public class DeliveryProcessor
    {
        private Logger _logger;

        public DeliveryProcessor(Logger loger)
        {
            _logger = loger;
        }

        /// <summary>
        /// Reads orders data from file
        /// </summary>
        /// <param name="filePath">Path to file with orders</param>
        /// <returns>List of orders</returns>
        /// <exception cref="IOException"></exception>
        /// <exception cref="TypeConverterException"></exception>
        public List<Order> ReadOrders(string filePath)
        {
            var orders = new List<Order>();
            // csv parser configuration
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                DetectDelimiter = true,
                HasHeaderRecord = false,
            };

            try
            {
                // Data file parsing
                TextReader reader = new StreamReader(filePath);
                using (CsvReader csvReader = new CsvReader(reader, config))
                {
                    orders = csvReader.GetRecords<Order>().ToList();
                }

                _logger.Log($"Readed data. Rows count: {orders.Count}");
            }
            catch(TypeConverterException e)
            {
                _logger.Log($"Error in data. The conversion cannot be performed. Text: {e.Text}");
                throw;
            }
            catch (IOException e) 
            {
                var message = $"Error reading data: {e.Message}";

                _logger.Log(message);
                throw new IOException(message, e);
            }

            return orders;
        }

        /// <summary>
        /// Takes a list of orders and filters it based on a given District ID and Delivery Time
        /// </summary>
        /// <param name="orders">List of orders to filter</param>
        /// <param name="districtId">District ID to filter</param>
        /// <param name="firstDeliveryTime">Initial delivery time from which filtering will be performed</param>
        /// <returns>Filtered order list</returns>
        public List<Order> FilterOrders(List<Order> orders, int districtId, DateTime firstDeliveryTime)
        {
            DateTime filterTime = firstDeliveryTime.AddMinutes(30);
            var filteredOrders = orders.Where(order => order.DistrictId == districtId &&
                                         order.DeliveryTime >= firstDeliveryTime &&
                                         order.DeliveryTime <= filterTime).ToList();

            _logger.Log($"Filtered orders districtId: {districtId}, " +
                $"for time: {firstDeliveryTime}, " +
                $"rows count: {filteredOrders.Count}");
            
            return filteredOrders;
        }

        /// <summary>
        /// Writes orders data to file
        /// </summary>
        /// <param name="orders">Orders to write</param>
        /// <param name="resultFilePath">File to write</param>
        /// <exception cref="IOException"></exception>
        public void WriteResults(List<Order> orders, string resultFilePath)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                DetectDelimiter = true,
                HasHeaderRecord = false,
            };

            try
            {
                using StreamWriter writer = new StreamWriter(resultFilePath);
                using (var csvWriter = new CsvWriter(writer, config))
                {
                    csvWriter.WriteRecords(orders);
                }

                _logger.Log($"The results are written, rows count: {orders.Count}");
            }
            catch (IOException e)
            {
                var message = $"Error writing results: {e.Message}";

                _logger.Log(message);
                throw new IOException(message, e);
            }
        }
    }
}
