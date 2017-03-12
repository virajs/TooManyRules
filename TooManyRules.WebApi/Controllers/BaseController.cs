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

namespace TooManyRules.WebApi.Controllers
{
    public abstract class BaseController : Controller
    {
        protected abstract ILogger Log { get; }

        public async Task<IActionResult> Ok<T>(Func<Task<T>> callback, string failureMessage = "")
        {
            try
            {
                return Ok(await callback());
            }
            catch (Exception e)
            {
                Log.LogError(new EventId(500), e, failureMessage);
            }

            return StatusCode(500);
        }

        public async Task<IActionResult> Created<T, TKey>(Func<T, Task<TKey>> callback, T value, string uri,
            string failureMessage = "")
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var key = await callback(value);

                    return Created($"{uri}/{key}", key);
                }

                return BadRequest(ModelState);
            }
            catch (Exception e)
            {
                Log.LogError(new EventId(500), e, failureMessage);
            }

            return StatusCode(500);
        }

        public async Task<IActionResult> NoContent<T, TKey>(Func<TKey, T, Task> callback, TKey key, T value,
            string failureMessage = "")
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await callback(key, value);

                    return NoContent();
                }

                return BadRequest(ModelState);
            }
            catch (Exception e)
            {
                Log.LogError(new EventId(500), e, failureMessage);
            }

            return StatusCode(500);
        }

        public async Task<IActionResult> NoContent<TKey>(Func<TKey, Task> callback, TKey key, string failureMessage = "")
        {
            try
            {
                await callback(key);

                return NoContent();
            }
            catch (Exception e)
            {
                Log.LogError(new EventId(500), e, failureMessage);
            }

            return StatusCode(500);
        }
    }
}