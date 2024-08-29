using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Notification.Api.Models;

namespace Notification.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : Controller
    {
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(ILogger<NotificationController> logger)
        {
            _logger = logger;
        }

        [Route("Send")]
        [HttpPost]
        public async Task<IActionResult> Send(OrderUpdateMessage message)
        {
            //Bildirim g�nderim i�lemleri.
            //await _notificationService.Send(message);
            Console.WriteLine(JsonConvert.SerializeObject(message));

            return Ok();
        }
    }
}
