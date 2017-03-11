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

using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using TooManyRules.Models;
using TooManyRules.Tests.TestObjects;
using Xunit;

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
    }
}