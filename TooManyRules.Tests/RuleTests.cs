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
using TooManyRules.Models;
using TooManyRules.Tests.TestObjects;
using Xunit;

namespace TooManyRules.Tests
{
    public class RuleTests
    {
        [Fact]
        public void EmptyTableShouldReturnNoRules()
        {
            // Arrange
            using (var factory = new ControllerFactory())
            {
                // Act
                var rules = factory.RulesController.Get();

                // Assert
                Assert.NotNull(rules);
                Assert.Empty(rules);
            }
        }

        [Fact]
        public void AddingANewRuleShouldSucceed()
        {
            // Arrange
            using (var factory = new ControllerFactory())
            {
                var expectedRule = new Rule { Id = 1, Name = "My First Rules" };
                var controller = factory.RulesController;

                // Act
                controller.Post(expectedRule);

                // Assert
                var rules = controller.Get();

                Assert.NotNull(rules);
                Assert.NotEmpty(rules);
                Assert.Equal(1, rules.Count());

                var actualRule = controller.Get().First();

                Assert.NotNull(actualRule);
                Assert.Equal(expectedRule.Id, actualRule.Id);
                Assert.Equal(expectedRule.Name, actualRule.Name, StringComparer.Ordinal);
            }
        }

        [Fact]
        public void GettingARuleByIdShouldSucceed()
        {
            // Arrange
            using (var factory = new ControllerFactory())
            {
                const int id = 1;
                var expectedRule = new Rule { Id = id, Name = "My First Rules" };
                var controller = factory.RulesController;

                // Act
                controller.Post(expectedRule);

                // Assert
                var actualRule = controller.Get(id);

                Assert.NotNull(actualRule);
                Assert.Equal(expectedRule.Id, actualRule.Id);
                Assert.Equal(expectedRule.Name, actualRule.Name, StringComparer.Ordinal);
            }
        }

        [Fact]
        public void UpdatingAnExistingRuleShouldSucceed()
        {
            // Arrange
            using (var factory = new ControllerFactory())
            {
                const int id = 1;

                var initialRule = new Rule {Id = id, Name = "Another Rule"};
                var controller = factory.RulesController;

                controller.Post(initialRule);

                var expectedRule = new Rule {Name = "Updating Rule!"};

                // Act
                controller.Put(id, expectedRule);

                // Assert
                var rules = controller.Get();

                Assert.NotNull(rules);
                Assert.NotEmpty(rules);
                Assert.Equal(1, rules.Count());

                var actualRule = controller.Get().First();

                Assert.NotNull(actualRule);
                Assert.Equal(id, actualRule.Id);
                Assert.Equal(expectedRule.Name, actualRule.Name, StringComparer.Ordinal);
            }
        }

        [Fact]
        public void DeletingARuleShouldSucceed()
        {
            // Arrange
            using (var factory = new ControllerFactory())
            {
                const int id = 1;

                var initialRule = new Rule { Id = id, Name = "Another Rule" };
                var controller = factory.RulesController;

                controller.Post(initialRule);

                // Act
                controller.Delete(id);

                // Assert
                var rules = controller.Get();

                Assert.NotNull(rules);
                Assert.Empty(rules);
            }
        }
    }
}