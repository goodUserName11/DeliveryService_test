appsettings.json - file with configuration
	Path to log file
	LogFilePath: "path/to/file",
	Path to file that will contains result
	ResultFilePath: "path/to/file",
	Path to orders data file
	OrderDataPath: "path/to/file",

	Optional parameters:
		District Id to filter
		DistrictId: integer,
		Time to filter
		FirstDeliveryTime: "yyyy-mm-dd hh:MM:ss"

orders.csv - default orders data file
	OrderId,		Weight,	DistrictId, DeliveryTime
	Guid string,	double,	integer,	"yyyy-mm-dd hh:MM:ss"


delivery_log.txt - default log file
"yyyy-mm-dd hh:MM:ss": message

filtered_orders.csv - default result file with filtered orders
	OrderId,		Weight,	DistrictId, DeliveryTime
	Guid string,	double,	integer,	"mm/dd/yyyy hh:MM:ss"