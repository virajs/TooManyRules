// Copyright 2017 Ryan Caille
//  
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation 
// files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, 
// modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software
// is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//  
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE 
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR 
// IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TooManyRules.BusinessLayer;
using TooManyRules.Models;

namespace TooManyRules.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class RulesController : BaseController
    {
        private readonly ILogger<RulesController> log;
        private readonly IRulesService service;

        public RulesController(IRulesService service, ILogger<RulesController> log)
        {
            this.service = service;
            this.log = log;
        }

        protected override ILogger Log => log;

        // GET api/rules
        [HttpGet]
        public async Task<IActionResult> Get() => await Ok(service.GetAll);

        // GET api/rules/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) => await Ok(async () => await service.Get(id));

        // POST api/rules
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Rule value) => await Created(service.Add, value, "api/rules");

        // PUT api/rules/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Rule value) => await NoContent(service.Edit, id, value);

        // DELETE api/rules/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) => await NoContent(service.Delete, id);
    }
}