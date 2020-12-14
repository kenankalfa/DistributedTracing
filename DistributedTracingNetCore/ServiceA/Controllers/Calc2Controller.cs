using Microsoft.AspNetCore.Mvc;

namespace ServiceA.Controllers
{
    [Route("api/servicea/calc-no-traced")]
    [ApiController]
    public class Calc2Controller : ControllerBase
    {
        WhateverBusinessLib.Calculator calculator;

        public Calc2Controller()
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
