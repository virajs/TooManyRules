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

using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TooManyRules.BusinessLayer;
using TooManyRules.Models;

namespace TooManyRules.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class RulesController : Controller
    {
        private readonly IRulesService rulesService;

        public RulesController(IRulesService rulesService)
        {
            this.rulesService = rulesService;
        }

        // GET api/rules
        [HttpGet]
        public IEnumerable<Rule> Get()
        {
            return rulesService.GetAll();
        }

        // GET api/rules/5
        [HttpGet("{id}")]
        public Rule Get(int id)
        {
            return rulesService.Get(id);
        }

        // POST api/rules
        [HttpPost]
        public void Post([FromBody] Rule value)
        {
            rulesService.Add(value);
        }

        // PUT api/rules/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Rule value)
        {
            rulesService.Edit(value);
        }

        // DELETE api/rules/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            rulesService.Delete(id);
        }
    }
}