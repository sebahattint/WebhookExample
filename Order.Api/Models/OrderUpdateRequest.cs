namespace Order.Api.Models
{
    public class OrderUpdateRequest
    {
        public int OrderId { get; set; }
        public string Status { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
