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

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using TooManyRules.Engine;
using TooManyRules.Models;
using TooManyRules.Tests.TestObjects;
using Xunit;

namespace TooManyRules.Tests
{
    public class PasswordPolicyScenarioTests : IDisposable
    {
        [Fact]
        public async Task EvaluationMinPasswordLengthRule()
        {
            var definition = new RuleDefinition
            {
                InputOperand = "{password}",
                Operator = ">=",
                ConstantOperand = "8"
            };

            var policy = new Policy {Name = "Password Policy"};
            policy.Rules.Add(new Rule
            {
                Name = "Min Password Length",
                Definition = JsonConvert.SerializeObject(definition)
            });

            var factory = new ControllerFactory();

            var controller = factory.CreatePoliciesController();
            await controller.AddPolicy(policy);

            var engine = factory.CreateRuleEngine();

            var result = engine.EvaluatePolicy(policy.Name, "password");
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
        }
    }
}