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

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TooManyRules.BusinessLayer;
using TooManyRules.Models;

namespace TooManyRules.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class RulesController : Controller
    {
        private readonly ILogger<RulesController> log;
        private readonly IRulesService service;

        public RulesController(IRulesService service, ILogger<RulesController> log)
        {
            this.service = service;
            this.log = log;
        }

        // GET api/rules
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await service.GetAll());
            }
            catch (Exception e)
            {
                log.LogError(new EventId(e.HResult), e, "");
            }

            return StatusCode(500);
        }

        // GET api/rules/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                return Ok(await service.Get(id));
            }
            catch (Exception e)
            {
                log.LogError(new EventId(e.HResult), e, "");
            }

            return StatusCode(500);
        }

        // POST api/rules
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Rule value)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var key = await service.Add(value);

                    return Created($"api/rules/{key}", key);
                }

                return BadRequest(ModelState);
            }
            catch (Exception e)
            {
                log.LogError(new EventId(e.HResult), e, "");
            }

            return StatusCode(500);
        }

        // PUT api/rules/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Rule value)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await service.Edit(id, value);

                    return NoContent();
                }

                return BadRequest(ModelState);
            }
            catch (Exception e)
            {
                log.LogError(new EventId(e.HResult), e, "");
            }

            return StatusCode(500);
        }

        // DELETE api/rules/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await service.Delete(id);

                return NoContent();
            }
            catch (Exception e)
            {
                log.LogError(new EventId(e.HResult), e, "");
            }

            return StatusCode(500);
        }
    }
}