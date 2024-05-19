using Microsoft.AspNetCore.Mvc;
using RabbitMQClient.Customers.API.Bus;
using RabbitMQClient.Customers.API.Model;

namespace RabbitMQClient.Customers.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private const string ROUTING_KEY = "customer-created";
        private readonly IBusService _busService;

        public CustomersController(IBusService busService)
        {

            _busService = busService;

        }

        [HttpPost]
        public IActionResult Post(CustomerInputModel model)
        {
            var @event = new Customer(model.Name, model.Cpf, model.BirthDate);

            _busService.Publish(ROUTING_KEY, @event);

            return NoContent();
        }
    }
}
