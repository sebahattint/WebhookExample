using Order.Api.Models;

namespace Order.Api.Services
{
    public class WebhookSubscriptionService
    {
        private readonly List<WebhookSubscription> _subscriptions = new List<WebhookSubscription>();

        public IEnumerable<WebhookSubscription> GetAllSubscriptions()
        {
            return _subscriptions;
        }

        public void AddSubscription(WebhookSubscription subscription)
        {
            _subscriptions.Add(subscription);
        }

        public void RemoveSubscription(Guid id)
        {
            var subscription = _subscriptions.FirstOrDefault(s => s.Id == id);
            if (subscription != null)
            {
                _subscriptions.Remove(subscription);
            }
        }
    }
}
