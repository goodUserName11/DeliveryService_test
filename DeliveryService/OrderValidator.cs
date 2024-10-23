namespace DeliveryService
{
    /// <summary>
    /// Class for order valiadtion
    /// </summary>
    public static class OrderValidator
    {
        /// <summary>
        /// Validate order
        /// </summary>
        /// <param name="order">Order to validate</param>
        /// <returns>Is order correct</returns>
        public static bool Validate(Order order)
        {
            return order.Weight > 0 && 
                order.DeliveryTime != DateTime.MinValue && 
                order.DistrictId > 0;//?
        }
    }
}
