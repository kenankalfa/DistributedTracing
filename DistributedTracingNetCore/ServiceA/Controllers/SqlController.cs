using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceA.Controllers
{
    [Route("api/telemetry/servicea/sql")]
    [ApiController]
    public class SqlController : ControllerBase
    {
        private MyDbContext _context;
        public SqlController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<List<tb_Person>> Get()
        {
            var rnd = new Random();
            var personList = await _context.tb_Person.Where(q => q.PersonRef > 10000 && q.PersonRef < 110000).Skip(rnd.Next(0,9000)).Take(5).ToListAsync();
            return personList;
        }
    }
}
