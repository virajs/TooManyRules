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
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TooManyRules.Models;
using TooManyRules.Tests.TestObjects;
using Xunit;

namespace TooManyRules.Tests
{
    public class RuleTests
    {
        private const string Name = "My First Rule";
        private const string Definition = "{}";

        private static Rule CreateTestRule()
        {
            return new Rule {Name = Name, Definition = Definition};
        }

        [Fact]
        public async Task AddingANewRuleShouldSucceed()
        {
            // Arrange
            using (var factory = new ControllerFactory())
            {
                var expectedRule = CreateTestRule();
                var controller = factory.CreateRulesController();

                // Act
                var result = await controller.Post(expectedRule);

                // Assert
                var which = result.Should().NotBeNull()
                    .And.BeOfType<CreatedResult>()
                    .Which;

                which.Value.Should().BeOfType<int>().And.NotBeNull().And.NotBe(0);
                which.Location.Should().Be($"api/rules/{which.Value}");
            }
        }

        [Fact]
        public async Task DeletingARuleShouldSucceed()
        {
            // Arrange
            using (var factory = new ControllerFactory())
            {
                var rule = CreateTestRule();
                var controller = factory.CreateRulesController();

                var id = (int) ((CreatedResult) await controller.Post(rule)).Value;

                // Act
                var result = await controller.Delete(id);

                // Assert
                result.Should().NotBeNull()
                    .And.BeOfType<NoContentResult>();

                var getResult = await controller.Get();
                getResult.Should().NotBeNull()
                    .And.BeOfType<OkObjectResult>()
                    .Which.Value.As<IList<Rule>>().Should().BeEmpty();
            }
        }

        [Fact]
        public async Task EmptyTableShouldReturnNoRules()
        {
            // Arrange
            using (var factory = new ControllerFactory())
            {
                var controller = factory.CreateRulesController();

                // Act
                var result = await controller.Get();

                // Assert
                result.Should().NotBeNull()
                    .And.BeOfType<OkObjectResult>()
                    .Which.Value.As<IList<Rule>>().Should().BeEmpty();
            }
        }

        [Fact]
        public async Task GettingARuleByIdShouldSucceed()
        {
            // Arrange
            using (var factory = new ControllerFactory())
            {
                var expectedRule = CreateTestRule();
                var controller = factory.CreateRulesController();

                var id = (int) ((CreatedResult) await controller.Post(expectedRule)).Value;

                expectedRule.Id = id;

                // Act
                var result = await controller.Get(id);

                // Assert
                result.Should().NotBeNull()
                    .And.BeOfType<OkObjectResult>()
                    .Which.Value.ShouldBeEquivalentTo(expectedRule);
            }
        }

        [Fact]
        public async Task UpdatingAnExistingRuleShouldSucceed()
        {
            // Arrange
            using (var factory = new ControllerFactory())
            {
                var initialRule = CreateTestRule();
                var controller = factory.CreateRulesController();

                var id = (int) ((CreatedResult) await controller.Post(initialRule)).Value;

                var fakeDefinition = JsonConvert.SerializeObject(new
                {
                    InputOperand = "$value",
                    Operator = "=",
                    ConstantOperand = "0"
                });

                var updateRule = new Rule {Definition = fakeDefinition};
                var expectedRule = new Rule {Id = initialRule.Id, Name = initialRule.Name, Definition = updateRule.Definition};

                // Act
                var putResult = await controller.Put(id, updateRule);

                // Assert
                putResult.Should().BeOfType<NoContentResult>();

                var result = await controller.Get(id);

                result.Should().NotBeNull()
                    .And.BeOfType<OkObjectResult>()
                    .Which.Value.ShouldBeEquivalentTo(expectedRule);
            }
        }
    }
}