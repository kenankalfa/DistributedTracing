using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Tasks;

namespace ServiceC3.Controllers
{
    [Route("api/servicec3/date")]
    [ApiController]
    public class DateController : ControllerBase
    {
        private const string CONFIG_QUEUE = "service_c_queue";

        public DateController()
        {
            
        }

        [HttpPost]
        [Route("calculate")]
        public async Task<string> DateCalculate([FromBody] WhateverBusinessLib.DateMessage message)
        {
            await Task.Delay(500);

            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = ConnectionFactory.DefaultUser,
                Password = ConnectionFactory.DefaultPass,
                Port = AmqpTcpEndpoint.UseDefaultPort
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: CONFIG_QUEUE,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                channel.BasicPublish(exchange: "",
                                     routingKey: CONFIG_QUEUE,
                                     basicProperties: null,
                                     body: body);
            }

            return (message.Year+1).ToString();
        }
    }
}
