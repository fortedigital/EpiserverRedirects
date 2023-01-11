using System;
using System.Linq;
using EPiServer.Data;
using Forte.EpiserverRedirects.DynamicData;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Repository;
using Moq;
using Xunit;

namespace Forte.EpiserverRedirects.Tests.Unit.DynamicData
{
    public class DynamicDataRepositoryTest
    {
        private readonly IRedirectRuleRepository _target;
        private readonly Mock<IDynamicDataStore<RedirectRule>> _store;
        private readonly Mock<IRedirectRuleMapper<RedirectRule>> _mapper;

        public DynamicDataRepositoryTest()
        {
            _store = new Mock<IDynamicDataStore<RedirectRule>>(MockBehavior.Strict);
            _mapper = new Mock<IRedirectRuleMapper<RedirectRule>>(MockBehavior.Strict);
            _target = new DynamicDataRepository(_store.Object, _mapper.Object);
        }

        [Fact]
        public void Given_GetAll_Returns_StoreItems()
        {
            var expected = new Mock<IQueryable<RedirectRule>>();
            _store.Setup(o => o.Items()).Returns(expected.Object);

            var actual = _target.GetAll();
            Assert.Same(expected.Object, actual);
        }

        [Fact]
        public void Given_Id_Returns_RedirectRule()
        {
            var ruleId = Guid.NewGuid();
            var expected = new RedirectRule();
            _store.Setup(o => o.GetById(ruleId)).Returns(expected);

            var actual = _target.GetById(ruleId);
            Assert.Same(expected, actual);
        }

        [Fact]
        public void Given_Rule_Add_To_Store()
        {
            var input = new RedirectRuleModel();
            var entity = new RedirectRule();
            _mapper.Setup(o => o.ToNewEntity(input)).Returns(entity);
            _store.Setup(o => o.Save(entity)).Returns(Identity.NewIdentity()).Verifiable();

            var actual = _target.Add(input);
            Assert.Same(entity, actual);
            _store.Verify();
        }

        [Fact]
        public void Given_Rule_Update_To_Store()
        {
            var ruleId = Guid.NewGuid();
            var input = new RedirectRuleModel { RuleId = ruleId };
            var entity = new RedirectRule();
            _store.Setup(o => o.GetById(ruleId)).Returns(entity);
            _mapper.Setup(o => o.MapForUpdate(input, entity)).Verifiable();
            _store.Setup(o => o.Save(entity)).Returns(Identity.NewIdentity()).Verifiable();

            var actual = _target.Update(input);
            Assert.Same(entity, actual);
            _mapper.Verify();
            _store.Verify();
        }

        [Fact]
        public void Given_Rule_NotFound_During_Update()
        {
            var ruleId = Guid.NewGuid();
            var input = new RedirectRuleModel { RuleId = ruleId };
            _store.Setup(o => o.GetById(ruleId)).Returns(null as RedirectRule);

            Assert.Throws<InvalidOperationException>(() => _target.Update(input));
        }

        [Fact]
        public void Given_Id_Delete_Rule_In_Store()
        {
            var ruleId = Guid.NewGuid();
            _store.Setup(o => o.Delete(ruleId)).Verifiable();

            var actual = _target.Delete(ruleId);
            Assert.True(actual);
            _store.Verify();
        }

        [Fact]
        public void Given_Delete_Fails_Returns_False()
        {
            var ruleId = Guid.NewGuid();
            _store.Setup(o => o.Delete(ruleId)).Throws<InvalidOperationException>();

            var actual = _target.Delete(ruleId);
            Assert.False(actual);
        }

        [Fact]
        public void Given_ClearAll_Calls_DeleteAll_In_Store()
        {
            _store.Setup(o => o.DeleteAll());

            var actual = _target.ClearAll();
            Assert.True(actual);
        }
    }
}
