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

using FluentAssertions;
using Newtonsoft.Json;
using System.Threading.Tasks;
using TooManyRules.Engine.Evaluators;
using TooManyRules.Models;
using TooManyRules.Tests.TestObjects;
using Xunit;
using Numerical = TooManyRules.Engine.Evaluators.Numerical;

namespace TooManyRules.Tests
{
    public class EvaluationTests
    {
        [Fact]
        public void EvaulatingRulesThatDoNotExistShouldNotSucceed()
        {
            using (var factory = new ControllerFactory())
            {
                var engine = factory.CreateRuleEngine();

                var result = engine.EvaluateRule("sys", "NotARule", null);
                result.Should().NotBeNull();
                result.Success.Should().BeFalse();
            }
        }

        [Fact]
        public async Task RulesWithEmptyDefinitionShouldSucceed()
        {
            using (var factory = new ControllerFactory())
            {
                var service = factory.CreateRulesService();

                await service.Add(new Rule
                {
                    Name = "ThisIsARule",
                    Definition = JsonConvert.SerializeObject(new {})
                });

                var engine = factory.CreateRuleEngine();

                var result = engine.EvaluateRule("sys", "ThisIsARule", null);
                result.Should().NotBeNull();
                result.Success.Should().BeTrue();
            }
        }

        [Fact]
        public void EqualityEvaluatorShouldEvaluateNumbersCorrectly()
        {
            var evaluator = new Numerical.EqualityEvaluator();

            evaluator.Operator.Should().Be(BinaryOperators.EqualTo);

            evaluator.Evaluate(0, 0).Should().BeTrue();
            evaluator.Evaluate(-1, 0).Should().BeFalse();
            evaluator.Evaluate(1, 0).Should().BeFalse();
            evaluator.Evaluate(999999999999, 999999999999).Should().BeTrue();
            evaluator.Evaluate(99999.9999999m, 99999.9999999m).Should().BeTrue();
            evaluator.Evaluate(199999.9999999m, 99999.9999999m).Should().BeFalse();
            evaluator.Evaluate(199999.9999999m, -99999.9999999m).Should().BeFalse();
            evaluator.Evaluate(0, -99999.9999999m).Should().BeFalse();
        }

        [Fact]
        public void InequalityEvaluatorShouldEvaluateNumbersCorrectly()
        {
            var evaluator = new Numerical.InequalityEvaluator();

            evaluator.Operator.Should().Be(BinaryOperators.NotEqualTo);

            evaluator.Evaluate(0, 0).Should().BeFalse();
            evaluator.Evaluate(-1, 0).Should().BeTrue();
            evaluator.Evaluate(1, 0).Should().BeTrue();
            evaluator.Evaluate(999999999999, 999999999999).Should().BeFalse();
            evaluator.Evaluate(99999.9999999m, 99999.9999999m).Should().BeFalse();
            evaluator.Evaluate(199999.9999999m, 99999.9999999m).Should().BeTrue();
            evaluator.Evaluate(199999.9999999m, -99999.9999999m).Should().BeTrue();
            evaluator.Evaluate(0, -99999.9999999m).Should().BeTrue();
        }

        [Fact]
        public void GreaterThanEvaluatorShouldEvaluateNumbersCorrectly()
        {
            var evaluator = new Numerical.GreaterThanEvaluator();

            evaluator.Operator.Should().Be(BinaryOperators.GreaterThan);

            evaluator.Evaluate(0, 0).Should().BeFalse();
            evaluator.Evaluate(-1, 0).Should().BeFalse();
            evaluator.Evaluate(1, 0).Should().BeTrue();
            evaluator.Evaluate(999999999999, 999999999999).Should().BeFalse();
            evaluator.Evaluate(99999.9999999m, 99999.9999999m).Should().BeFalse();
            evaluator.Evaluate(199999.9999999m, 99999.9999999m).Should().BeTrue();
            evaluator.Evaluate(199999.9999999m, -99999.9999999m).Should().BeTrue();
            evaluator.Evaluate(0, -99999.9999999m).Should().BeTrue();
        }

        [Fact]
        public void GreaterThanOrEqualToEvaluatorShouldEvaluateNumbersCorrectly()
        {
            var evaluator = new Numerical.GreaterThanOrEqualToEvaluator();

            evaluator.Operator.Should().Be(BinaryOperators.GreaterThanOrEqualTo);

            evaluator.Evaluate(0, 0).Should().BeTrue();
            evaluator.Evaluate(-1, 0).Should().BeFalse();
            evaluator.Evaluate(1, 0).Should().BeTrue();
            evaluator.Evaluate(999999999999, 999999999999).Should().BeTrue();
            evaluator.Evaluate(99999.9999999m, 99999.9999999m).Should().BeTrue();
            evaluator.Evaluate(199999.9999999m, 99999.9999999m).Should().BeTrue();
            evaluator.Evaluate(199999.9999999m, -99999.9999999m).Should().BeTrue();
            evaluator.Evaluate(0, -99999.9999999m).Should().BeTrue();
        }

        [Fact]
        public void LessThanEvaluatorShouldEvaluateNumbersCorrectly()
        {
            var evaluator = new Numerical.LessThanEvaluator();

            evaluator.Operator.Should().Be(BinaryOperators.LessThan);

            evaluator.Evaluate(0, 0).Should().BeFalse();
            evaluator.Evaluate(-1, 0).Should().BeTrue();
            evaluator.Evaluate(1, 0).Should().BeFalse();
            evaluator.Evaluate(999999999999, 999999999999).Should().BeFalse();
            evaluator.Evaluate(99999.9999999m, 99999.9999999m).Should().BeFalse();
            evaluator.Evaluate(199999.9999999m, 99999.9999999m).Should().BeFalse();
            evaluator.Evaluate(199999.9999999m, -99999.9999999m).Should().BeFalse();
            evaluator.Evaluate(0, -99999.9999999m).Should().BeFalse();
        }

        [Fact]
        public void LessThanOrEqualToEvaluatorShouldEvaluateNumbersCorrectly()
        {
            var evaluator = new Numerical.LessThanOrEqualToEvaluator();

            evaluator.Operator.Should().Be(BinaryOperators.LessThanOrEqualTo);

            evaluator.Evaluate(0, 0).Should().BeTrue();
            evaluator.Evaluate(-1, 0).Should().BeTrue();
            evaluator.Evaluate(1, 0).Should().BeFalse();
            evaluator.Evaluate(999999999999, 999999999999).Should().BeTrue();
            evaluator.Evaluate(99999.9999999m, 99999.9999999m).Should().BeTrue();
            evaluator.Evaluate(199999.9999999m, 99999.9999999m).Should().BeFalse();
            evaluator.Evaluate(199999.9999999m, -99999.9999999m).Should().BeFalse();
            evaluator.Evaluate(0, -99999.9999999m).Should().BeFalse();
        }
    }
}