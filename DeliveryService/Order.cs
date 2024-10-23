namespace DeliveryService
{
    /// <summary>
    /// Order data model
    /// </summary>
    public class Order
    {
        public Guid OrderId { get; set; }
        public double Weight { get; set; }
        public int DistrictId { get; set; }
        public DateTime DeliveryTime { get; set; }
    }
}
