using Microsoft.AspNetCore.Mvc;

namespace ServiceA.Controllers
{
    [Route("api/telemetry/servicea/calc")]
    [ApiController]
    public class CalcController : ControllerBase
    {
        WhateverBusinessLib.Calculator calculator;

        public CalcController()
        {
            calculator = new WhateverBusinessLib.Calculator();
        }

        [HttpPost]
        [Route("summy")]
        public int Sum([FromBody] WhateverBusinessLib.Val values)
        {
            return calculator.Sum(values);
        }

        [HttpPost]
        [Route("subsy")]
        public int Substract([FromBody] WhateverBusinessLib.Val values)
        {
            return calculator.Substract(values);
        }
    }
}
