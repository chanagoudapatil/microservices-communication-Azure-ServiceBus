using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Text.Json;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        ServiceBusClient _serviceBusClient;
        IConfiguration _configuration;
        ServiceBusSender _sender;
        public OrderController(IConfiguration configuration)
        {
            _configuration = configuration;
            _serviceBusClient = new ServiceBusClient(_configuration["ServiceBus:Connection"]);
            _sender = _serviceBusClient.CreateSender(_configuration["ServiceBus:Queue"]);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(Order model)
        {
            model.OrderId = Guid.NewGuid();

            string strData = JsonSerializer.Serialize(model);
            var msg = new ServiceBusMessage(strData);
            await _sender.SendMessageAsync(msg);

            return Ok();
        }
    }
}
