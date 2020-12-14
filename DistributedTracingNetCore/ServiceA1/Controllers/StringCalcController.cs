using Microsoft.AspNetCore.Mvc;

namespace ServiceA1.Controllers
{
    [Route("api/telemetry/servicea1/stringcalc")]
    [ApiController]
    public class StringCalcController : ControllerBase
    {
        WhateverBusinessLib.StringCalculator calculator;
        public StringCalcController()
        {
            calculator = new WhateverBusinessLib.StringCalculator();
        }

        [HttpPost]
        [Route("uppy")]
        public string ToUpper([FromBody] WhateverBusinessLib.StringVal values)
        {
            return calculator.ToUpper(values);
        }

        [HttpPost]
        [Route("suby")]
        public string SubString([FromBody] WhateverBusinessLib.StringVal values)
        {
            return calculator.SubString(values);
        }
    }
}
