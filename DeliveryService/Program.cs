using Microsoft.Extensions.Configuration;

namespace DeliveryService
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Loading of configuration
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true)
                .Build();

            string logFilePath = config["LogFilePath"];
            string resultFilePath = config["ResultFilePath"];
            string orderDataPath = config["OrderDataPath"];

            string districtIdStr = config["DistrictId"];
            string firstDeliveryTimeStr = config["FirstDeliveryTime"];

            // Check if the path of the log file is passed
            if (logFilePath == null)
            {
                Console.WriteLine("Log Path should be privided in configuration file appsettings.json");

                return;
            }

            // Check if the path of the result file is passed
            if (resultFilePath == null)
            {
                Console.WriteLine("Result Path should be privided in configuration file appsettings.json");

                return;
            }

            // Check if the path of the order data file is passed
            if (orderDataPath == null)
            {
                Console.WriteLine("Order Data Path should be privided in configuration file appsettings.json");

                return;
            }

            // Logger initialization
            Logger logger = new Logger(logFilePath);

            // Сheck if parameters are passed 
            if (args.Length < 2 &&
                districtIdStr == null && 
                firstDeliveryTimeStr == null)
            {
                Console.WriteLine("Usage: DeliveryApp <DistrictId> <FirstDeliveryTime>");

                return;
            }
            else if(args.Length == 2)
            {
                districtIdStr = args[0];
                firstDeliveryTimeStr = args[1];
            }

            // Check if passed district is valid
            if (!int.TryParse(districtIdStr, out var districtId))
            {
                Console.WriteLine($"Error converting DistrictId. DistrictId should be integer. Value: {districtIdStr}");

                return;
            }

            // Check if passed firstDeliveryTime is valid
            if (!DateTime.TryParse(firstDeliveryTimeStr, out var firstDeliveryTime))
            {
                Console.WriteLine($"Error converting FirstDeliveryTime. FirstDeliveryTime should be DateTime. Value: {firstDeliveryTimeStr}");

                return;
            }

            // Reading and filtering orders
            var deliveryProcessor = new DeliveryProcessor(logger);

            Console.WriteLine("Reading orders from disk...");

            List<Order> orders;
            try
            {
                // reading orders data from disk
                orders = deliveryProcessor.ReadOrders(orderDataPath);
            }
            catch (Exception e) 
            {
                Console.WriteLine(e.Message);

                return;
            }

            Console.WriteLine("done.");

            Console.WriteLine("Validating orders...");

            // Validating orders from file
            var validOrders = orders.Where(OrderValidator.Validate).ToList();

            if(validOrders.Count != orders.Count)
            {
                var message = "File contains bad data";

                try
                {
                    logger.Log(message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                Console.WriteLine(message);
            }

            Console.WriteLine("done.");

            Console.WriteLine("Filtering orders...");

            List<Order> filteredOrders = new List<Order>();

            try
            {
                // Filtering order
                filteredOrders = deliveryProcessor.FilterOrders(validOrders, districtId, firstDeliveryTime);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("done.");

            Console.WriteLine("Writing results to disk...");

            // Writing results to disk
            deliveryProcessor.WriteResults(filteredOrders, resultFilePath);

            Console.WriteLine("done.");
        }
    }
}
