using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using zipkin4net;
using zipkin4net.Middleware;
using zipkin4net.Tracers.Zipkin;
using zipkin4net.Transport.Http;

namespace ServiceC2
{
    class Program
    {
        private const string CONFIG_QUEUE = "service_c_queue";
        private static IModel rabbitMqChannel;
        private static IHost host;
        
        static void Main(string[] args)
        {
            Console.WriteLine("--------------- service.c2 listening -------------------");

            var builder = new HostBuilder()
               .ConfigureServices((hostContext, services) =>
               {
                   services.AddHttpClient();
                   services.AddTransient<WebApi>();
                   services.AddLogging();
               })
               .UseConsoleLifetime();

            host = builder.Build();

            var configurationBuilder = new ConfigurationBuilder()
                                            .AddJsonFile($"appsettings.json", true, true)
                                            .AddEnvironmentVariables();

            var _configuration = configurationBuilder.Build();

            IStatistics statistics = new Statistics();
            TraceManager.SamplingRate = 1.0f;
            var logger = new TracingLogger(host.Services.GetRequiredService<ILoggerFactory>(), "zipkin4net");
            var httpSender = new HttpZipkinSender(_configuration.GetSection("OpenTelemetry:Common:Trace:ServiceName").Get<string>(), "application/json");
            var tracer = new ZipkinTracer(httpSender, new JSONSpanSerializer(), statistics);
            TraceManager.RegisterTracer(tracer);
            TraceManager.Start(logger);

            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = ConnectionFactory.DefaultUser,
                Password = ConnectionFactory.DefaultPass,
                Port = AmqpTcpEndpoint.UseDefaultPort
            };

            var rabbitMqConnection = factory.CreateConnection();
            rabbitMqChannel = rabbitMqConnection.CreateModel();

            //declare the queue  
            rabbitMqChannel.QueueDeclare(queue: CONFIG_QUEUE,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

            rabbitMqChannel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new EventingBasicConsumer(rabbitMqChannel);
            consumer.Received += Recieved;
            rabbitMqChannel.BasicConsume(queue: CONFIG_QUEUE,
                                         autoAck: false,
                                         consumer: consumer);

            Console.ReadLine();
            Console.ReadLine();
        }

        static async void Recieved(object model, BasicDeliverEventArgs basicDeliverEventArgs)
        {
            var body = basicDeliverEventArgs.Body;
            var jsonMessage = Encoding.UTF8.GetString(body.ToArray());
            rabbitMqChannel.BasicAck(deliveryTag: basicDeliverEventArgs.DeliveryTag, multiple: false);

            Console.WriteLine("Recieved with : {0}", jsonMessage);

            var dateMessageItem = JsonConvert.DeserializeObject<WhateverBusinessLib.DateMessage>(jsonMessage);

            if (dateMessageItem != null)
            {
                using (var serviceScope = host.Services.CreateScope())
                {
                    var services = serviceScope.ServiceProvider;

                    try
                    {
                        var api = services.GetRequiredService<WebApi>();
                        await api.SendData(dateMessageItem, "api/servicec1/date/return-calculate");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error Occured");
                    }
                }
            }

            Thread.Sleep(1000);
        }
    }

    public class WebApi
    {
        private HttpClient _client;
        public WebApi(IHttpClientFactory httpFactory)
        {
            _client = httpFactory.CreateClient();
            _client.BaseAddress = new Uri("https://localhost:32031");
        }

        public async Task SendData(WhateverBusinessLib.DateMessage dateMessage, string url)
        {
            var trace = Trace.Create();
            trace.Record(Annotations.LocalOperationStart("servicec1-post-operation"));
            trace.Record(Annotations.ServiceName("api.service-c2"));
            trace.Record(Annotations.Tag("vip-person-operation", "hey-elon-musk-hear-me"));
            trace.Record(Annotations.Tag("hr.api.code", "api-9999"));
            trace.Record(Annotations.Rpc(url));
            trace.Record(Annotations.LocalAddr(new System.Net.IPEndPoint(IPAddress.Parse("192.19.07.28"), 1907)));
            trace.Record(Annotations.LocalOperationStop());

            dateMessage.Year = dateMessage.Year + 1;
            dateMessage.CustomMessage = $"greeting from {dateMessage.Year}";
            var content = new StringContent(JsonConvert.SerializeObject(dateMessage), System.Text.Encoding.UTF8, "application/json");
            var task = await _client.PostAsync(url, content);
            var resultContent = await task.Content.ReadAsStringAsync();
        }
    }
}
