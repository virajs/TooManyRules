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
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TooManyRules.DataAccess;
using TooManyRules.Models;

namespace TooManyRules.BusinessLayer
{
    internal class RulesService : IRulesService
    {
        private readonly IRulesRepository rulesRepository;

        public RulesService(IRulesRepository rulesRepository)
        {
            this.rulesRepository = rulesRepository;
        }

        public async Task<IList<Rule>> GetAll()
        {
            return await rulesRepository.GetAll().ToListAsync();
        }

        public async Task<Rule> Get(int id)
        {
            return await rulesRepository.FindBy(r => r.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> Add(Rule value)
        {
            rulesRepository.Add(value);
            await rulesRepository.Save();

            return value.Id;
        }

        public async Task Edit(int id, Rule value)
        {
            var entity = await Get(id);
            entity.Name = value.Name;

            rulesRepository.Edit(entity);
            await rulesRepository.Save();
        }

        public async Task Delete(int id)
        {
            var entity = await Get(id);
            if (entity != null)
            {
                rulesRepository.Delete(entity);
                await rulesRepository.Save();
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            rulesRepository?.Dispose();
        }
    }
}