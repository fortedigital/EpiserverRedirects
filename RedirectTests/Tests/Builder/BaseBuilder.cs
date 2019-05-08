using System;
using System.Collections.Generic;
using Forte.RedirectMiddleware.Model.RedirectRule;
using Forte.RedirectMiddleware.Repository;
using RedirectTests.Data;

namespace RedirectTests.Tests.Builder
{
    public abstract class BaseBuilder<TResolver, TBuilder>
        where TBuilder : BaseBuilder<TResolver, TBuilder>
    {
        private const int DefaultRedirectRulesNumber = 5;
        protected IRedirectRuleRepository RedirectRuleRepository = new TestRepository();
        private readonly RedirectRuleTestDataBuilder _redirectRuleTestDataBuilder = RedirectRuleTestDataBuilder.Start();

        protected abstract TBuilder ThisBuilder { get; }
        internal BaseBuilder() { }

        public TBuilder WithRandomExistingRules(int numberOfExistingRandomRules = DefaultRedirectRulesNumber)
        {
            _redirectRuleTestDataBuilder.InitializeWithRandomData(numberOfExistingRandomRules);
            return ThisBuilder;
        }

        public TBuilder WithExplicitExistingRules(HashSet<RedirectRule> existingRedirects)
        {
            _redirectRuleTestDataBuilder.InitializeData(existingRedirects);
            return ThisBuilder;
        }

        public TBuilder WithRule(Func<RedirectRuleTestDataBuilder, RedirectRule> redirectRuleTestDataBuilderFunc,
            out RedirectRule redirectRule)
        {
            redirectRule = redirectRuleTestDataBuilderFunc.Invoke(_redirectRuleTestDataBuilder);
            return ThisBuilder;
        }

        public abstract TResolver Create();

        protected void CreateRepository()
        {
            var existingRules = _redirectRuleTestDataBuilder.GetData();
            RedirectRuleRepository = new TestRepository(existingRules);
        }
    }
}