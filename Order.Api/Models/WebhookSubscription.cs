namespace Order.Api.Models
{
    public class WebhookSubscription
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string WebhookUrl { get; set; }
        public DateTime SubscribedDate { get; set; } = DateTime.UtcNow;
    }
}
