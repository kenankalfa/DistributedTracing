using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ServiceC1.Controllers
{
    [Route("api/servicec1/date")]
    [ApiController]
    public class DateController : ControllerBase
    {
        private readonly HttpClient _client;
        private IConfiguration _configuration;

        public DateController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _client = httpClientFactory.CreateClient();
            _configuration = configuration;
            _client.BaseAddress = new Uri(_configuration.GetSection("OpenTelemetry:Common:ServiceC3:BaseAddress").Get<string>());
        }

        [HttpGet]
        public async Task<string> Get([FromQuery] string yearValue)
        {
            var sendResult = await this.SendData(new WhateverBusinessLib.DateMessage() { Year = int.Parse(yearValue), CustomMessage = $"greeting from {yearValue}" }, _configuration.GetSection("OpenTelemetry:Common:ServiceC3:Source").Get<string>());
            return $"service.c1:{yearValue}-service.c3:{sendResult}";
        }

        private async Task<int> SendData(object json, string url)
        {
            var content = new StringContent(JsonConvert.SerializeObject(json), System.Text.Encoding.UTF8, "application/json");
            var task = await _client.PostAsync(url, content);
            var resultContent = await task.Content.ReadAsStringAsync();
            return int.Parse(resultContent);
        }

        [HttpPost]
        [Route("return-calculate")]
        public async Task<string> DateCalculate([FromBody] WhateverBusinessLib.DateMessage message)
        {
            await Task.Delay(500);

            return (message.Year + 1).ToString();
        }
    }
}
