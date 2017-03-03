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
using System.Linq;
using TooManyRules.DataAccess;

namespace TooManyRules.Engine
{
    internal class RuleEngine : IRuleEngine
    {
        private readonly IRulesRepository rulesRepository;

        public RuleEngine(IRulesRepository rulesRepository)
        {
            this.rulesRepository = rulesRepository;
        }

        public EvaluationResult EvaluateRules(string ns, object input)
        {
            return new EvaluationResult();
        }

        public EvaluationResult EvaluateRule(string ns, string name, object input)
        {
            var result = new EvaluationResult();

            var rule = rulesRepository.FindBy(r =>
                r.Namespace.Equals(ns, StringComparison.OrdinalIgnoreCase) &&
                r.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

            if (rule != null)
            {
                result.Success = true;
            }

            return result;
        }
    }
}