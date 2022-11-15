using EPiServer.Data;
using Forte.EpiserverRedirects.DynamicData;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Repository;
using Moq;
using System;
using System.Linq;
using Xunit;


namespace Forte.EpiserverRedirects.Tests.Unit.DynamicDataStore
{
    public class DynamicDataRepositoryTest
    {
        private readonly IRedirectRuleRepository target;
        private readonly Mock<IDynamicDataStore<DynamicDataRedirectRule>> ruleStore;
        private readonly Mock<IDynamicDataRedirectRuleMapper> mapper;

        public DynamicDataRepositoryTest()
        {
            ruleStore = new Mock<IDynamicDataStore<DynamicDataRedirectRule>>(MockBehavior.Strict);
            mapper = new Mock<IDynamicDataRedirectRuleMapper>(MockBehavior.Strict);
            target = new DynamicDataRepository(ruleStore.Object, mapper.Object);
        }

        [Fact]
        public void Given_GetAll_Returns_StoreItems()
        {
            var expected = new Mock<IOrderedQueryable<DynamicDataRedirectRule>>();
            ruleStore.Setup(o => o.Items()).Returns(expected.Object);

            var actual = target.GetAll();
            Assert.Same(expected.Object, actual);
        }

        [Fact]
        public void Given_Id_Returns_RedirectRule()
        {
            var RuleId = Guid.NewGuid();
            var expected = new DynamicDataRedirectRule();
            ruleStore.Setup(o => o.GetById(RuleId)).Returns(expected);

            var actual = target.GetById(RuleId);
            Assert.Same(expected, actual);
        }

        [Fact]
        public void Given_Rule_Add_To_Store()
        {
            var inputRule = new RedirectRule();
            var mappedRule = new DynamicDataRedirectRule();
            mapper.Setup(o => o.ToNewEntity(inputRule)).Returns(mappedRule);
            ruleStore.Setup(o => o.Save(mappedRule)).Returns(Identity.NewIdentity()).Verifiable();

            var actual = target.Add(inputRule);
            Assert.Same(mappedRule, actual);
            ruleStore.VerifyAll();
        }

        [Fact]
        public void Given_Rule_Update_To_Store()
        {
            var ruleId = Guid.NewGuid();
            var inputRule = new RedirectRule { RuleId = ruleId };
            var ddsRule = new DynamicDataRedirectRule();
            ruleStore.Setup(o => o.GetById(ruleId)).Returns(ddsRule);
            mapper.Setup(o => o.MapForUpdate(inputRule, ddsRule)).Verifiable();
            ruleStore.Setup(o => o.Save(ddsRule)).Returns(Identity.NewIdentity()).Verifiable();

            var actual = target.Update(inputRule);
            Assert.Same(ddsRule, actual);
            mapper.VerifyAll();
            ruleStore.VerifyAll();
        }

        [Fact]
        public void Given_Rule_NotFound_During_Update()
        {
            var ruleId = Guid.NewGuid();
            var inputRule = new RedirectRule { RuleId = ruleId };
            ruleStore.Setup(o => o.GetById(ruleId)).Returns(null as DynamicDataRedirectRule);

            Assert.Throws<InvalidOperationException>(() => target.Update(inputRule));
        }

        [Fact]
        public void Given_Id_Delete_Rule_In_Store()
        {
            var ruleId = Guid.NewGuid();
            ruleStore.Setup(o => o.Delete(ruleId)).Verifiable();

            var actual = target.Delete(ruleId);
            Assert.True(actual);
            ruleStore.VerifyAll();
        }

        [Fact]
        public void Given_Delete_Fails_Returns_False()
        {
            var ruleId = Guid.NewGuid();
            ruleStore.Setup(o => o.Delete(ruleId)).Throws<InvalidOperationException>();

            var actual = target.Delete(ruleId);
            Assert.False(actual);
        }

        [Fact]
        public void Given_ClearAll_Calls_DeleteAll_In_Store()
        {
            ruleStore.Setup(o => o.DeleteAll());

            var actual = target.ClearAll();
            Assert.True(actual);
        }
    }
}
