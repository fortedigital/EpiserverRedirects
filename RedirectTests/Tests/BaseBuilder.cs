using System;
using System.Collections.Generic;
using Forte.RedirectMiddleware.Model;
using Forte.RedirectMiddleware.Repository;
using RedirectTests.Data;

namespace RedirectTests.Tests
{
    public abstract class BaseBuilder<T>
    {
        private const int DefaultRedirectRulesNumber = 5;
        protected IRedirectRuleRepository RedirectRuleRepository = new TestRedirectRuleRepository();
        protected readonly RedirectRuleTestDataBuilder RedirectRuleTestDataBuilder = RedirectRuleTestDataBuilder.Start();

        internal BaseBuilder() { }

        public BaseBuilder<T> WithRandomExistingRules(int numberOfExistingRandomRules = DefaultRedirectRulesNumber)
        {
            RedirectRuleTestDataBuilder.InitializeWithRandomData(numberOfExistingRandomRules);
            return this;
        }

        public BaseBuilder<T> WithExplicitExistingRules(Dictionary<Guid, RedirectRule> existingRedirects)
        {
            RedirectRuleTestDataBuilder.InitializeData(existingRedirects);
            return this;
        }

        public BaseBuilder<T> WithRule(Func<RedirectRuleTestDataBuilder, RedirectRule> redirectRuleTestDataBuilderFunc,
            out RedirectRule redirectRule)
        {
            redirectRule = redirectRuleTestDataBuilderFunc.Invoke(RedirectRuleTestDataBuilder);
            return this;
        }

        public abstract T Create();
    }
}