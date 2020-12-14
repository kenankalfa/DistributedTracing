using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using zipkin4net;

namespace ServiceB.Controllers
{
    [Route("api/values")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private ILogger<ValuesController> _logger;
        private IConfiguration _configuration;

        public ValuesController(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<ValuesController>();
            _configuration = configuration;
        }

        [HttpGet]
        public int Get([FromQuery] string val1,string val2)
        {
            _logger.LogDebug("lorem ipsum lorem");

            var trace = Trace.Create();

            trace.Record(Annotations.LocalOperationStart("serviceb-get-operation"));
            trace.Record(Annotations.ServiceName(_configuration.GetSection("OpenTelemetry:Common:Trace:ServiceName").Get<string>()));
            trace.Record(Annotations.Tag("vip-person-operation", "hey-elon-musk-hear-me"));
            trace.Record(Annotations.Tag(_configuration.GetSection("OpenTelemetry:Common:Enrich:TagKey").Get<string>(), _configuration.GetSection("OpenTelemetry:Common:Enrich:TagValue").Get<string>()));
            trace.Record(Annotations.Rpc(_configuration.GetSection("OpenTelemetry:Common:Trace:Rpc").Get<string>()));
            trace.Record(Annotations.LocalAddr(new System.Net.IPEndPoint(IPAddress.Parse("192.19.07.28"),1907)));
            trace.Record(Annotations.LocalOperationStop());

            return val1.Length + val2.Length;
        }
    }
}
