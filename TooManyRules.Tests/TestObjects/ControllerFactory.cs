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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TooManyRules.BusinessLayer;
using TooManyRules.DataAccess;
using TooManyRules.Engine;
using TooManyRules.Models;
using TooManyRules.WebApi.Controllers;

namespace TooManyRules.Tests.TestObjects
{
    internal class ControllerFactory : IDisposable
    {
        private readonly TooManyRulesContext context;
        private readonly Mock<ILogger<RulesController>> mockRulesLogger = new Mock<ILogger<RulesController>>();
        private readonly Mock<ILogger<PoliciesController>> mockPoliciesLogger = new Mock<ILogger<PoliciesController>>();

        public ControllerFactory()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<TooManyRulesContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            context = new TooManyRulesContext(options);
        }

        public void Dispose()
        {
            context?.Dispose();
        }

        public RulesRepository CreateRulesRepository()
        {
            return new RulesRepository(context);
        }

        public RulesService CreateRulesService()
        {
            return new RulesService(CreateRulesRepository());
        }

        public RulesController CreateRulesController()
        {
            return new RulesController(CreateRulesService(), mockRulesLogger.Object);
        }

        public PolicyEngine CreateRuleEngine()
        {
            return new PolicyEngine(new PoliciesRepository(context));
        }

        public PoliciesController CreatePoliciesController()
        {
            return new PoliciesController(CreatePoliciesService(), mockPoliciesLogger.Object);
        }

        private PoliciesService CreatePoliciesService()
        {
            return new PoliciesService(CreatePoliciesRepository());
        }

        private PoliciesRepository CreatePoliciesRepository()
        {
            return new PoliciesRepository(context);
        }
    }
}