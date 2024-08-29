namespace Order.Api.Models
{
    public class OrderUpdateMessage
    {
        public int OrderId { get; set; }
        public string Status { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
