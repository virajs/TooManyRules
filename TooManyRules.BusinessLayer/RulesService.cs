﻿// Copyright 2017 Ryan Caille
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
using System.Linq;
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

        public IList<Rule> GetAll()
        {
            return rulesRepository.GetAll().ToList();
        }

        public Rule Get(int id)
        {
            return rulesRepository.FindBy(r => r.Id == id).FirstOrDefault();
        }

        public void Add(Rule value)
        {
            rulesRepository.Add(value);
            rulesRepository.Save();
        }

        public void Edit(Rule value)
        {
            rulesRepository.Edit(value);
            rulesRepository.Save();
        }

        public void Delete(int id)
        {
            var entity = Get(id);
            if (entity != null)
            {
                rulesRepository.Delete(entity);
                rulesRepository.Save();
            }
        }
    }
}