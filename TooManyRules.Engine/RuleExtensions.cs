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

using Newtonsoft.Json;
using TooManyRules.Engine.Evaluators;
using TooManyRules.Engine.Evaluators.Numerical;
using TooManyRules.Models;

namespace TooManyRules.Engine
{
    public static class RuleExtensions
    {
        public static IRuleDefinition BuildRule(this Rule rule)
        {
            var definition = JsonConvert.DeserializeObject<RuleDefinition>(rule.Definition);


            return definition;
        }
    }

    public class RuleDefinition : IRuleDefinition
    {
        public string InputOperand { get; set; }
        public string Operator { get; set; }
        public string ConstantOperand { get; set; }

        public (bool success, string failedRule) Evaluate(object value)
        {
            var inputValue = value.ToString();
            var evaluator = CreateEvaluator(Operator);

            if (!decimal.TryParse(ConstantOperand, out decimal constantOperand))
            {
                return (false, string.Empty);
            }

            var result = evaluator.Evaluate(inputValue.Length, constantOperand);

            return (result, string.Empty);
        }

        private static IEvaluate<decimal> CreateEvaluator(string op)
        {
            IEvaluate<decimal> result;

            switch (op)
            {
                case "==":
                    result = new EqualityEvaluator();
                    break;
                case "!=":
                    result = new InequalityEvaluator();
                    break;
                case "<=":
                    result = new LessThanOrEqualToEvaluator();
                    break;
                case "<":
                    result = new LessThanEvaluator();
                    break;
                case ">=":
                    result = new GreaterThanOrEqualToEvaluator();
                    break;
                case ">":
                    result = new GreaterThanEvaluator();
                    break;
                default:
                    result = null;
                    break;
            }

            return result;
        }
    }
}