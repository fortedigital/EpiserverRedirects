using EPiServer.Data;
using EPiServer.Shell.Search;
using Forte.EpiserverRedirects.DynamicData;
using Forte.EpiserverRedirects.Model;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;


namespace Forte.EpiserverRedirects.Tests.Unit.DynamicData
{
    public class DynamicDataRepositoryTest
    {
        private readonly IRedirectRuleRepository _target;
        private readonly Mock<IDynamicDataStore<DynamicDataRedirectRule>> _store;
        private readonly Mock<IDynamicDataRedirectRuleMapper> _mapper;

        private readonly Guid _id1;
        private readonly Guid _id2;
        private readonly Guid _id3;
        private readonly DynamicDataRedirectRule _entity1;
        private readonly DynamicDataRedirectRule _entity2;
        private readonly DynamicDataRedirectRule _entity3;
        private readonly RedirectRuleModel _model1;
        private readonly RedirectRuleModel _model2;
        private readonly RedirectRuleModel _model3;

        public DynamicDataRepositoryTest()
        {
            _store = new Mock<IDynamicDataStore<DynamicDataRedirectRule>>(MockBehavior.Strict);
            _mapper = new Mock<IDynamicDataRedirectRuleMapper>(MockBehavior.Strict);
            _target = new DynamicDataRepository(_store.Object, _mapper.Object);

            _id1 = Guid.NewGuid();
            _id2 = Guid.NewGuid();
            _id3 = Guid.NewGuid();
            _entity1 = new DynamicDataRedirectRule { Id = _id1, OldPattern = "One", ContentId = 1111 };
            _entity2 = new DynamicDataRedirectRule { Id = _id2, OldPattern = "Two", ContentId = 2222 };
            _entity3 = new DynamicDataRedirectRule { Id = _id3, OldPattern = "Three", ContentId = null };
            _model1 = new RedirectRuleModel();
            _model2 = new RedirectRuleModel();
            _model3 = new RedirectRuleModel();
            _store.Setup(o => o.Items()).Returns((new List<DynamicDataRedirectRule> { _entity1, _entity2, _entity3 }).AsQueryable());
            _mapper.Setup(o => o.MapToModel(_entity1)).Returns(_model1);
            _mapper.Setup(o => o.MapToModel(_entity2)).Returns(_model2);
            _mapper.Setup(o => o.MapToModel(_entity3)).Returns(_model3);
            _store.Setup(o => o.GetById(_id1)).Returns(_entity1);
            _store.Setup(o => o.GetById(_id2)).Returns(_entity2);
            _store.Setup(o => o.GetById(_id3)).Returns(_entity3);
        }

        [Fact]
        public void Given_Id_Returns_RedirectRule()
        {
            var actual = _target.GetById(_id2);
            Assert.Same(_model2, actual);
        }

        [Fact]
        public void Given_GetAll_Returns_All()
        {
            var actual = _target.GetAll();
            Assert.Equal(3, actual.Count);
            Assert.Same(_model1, actual[0]);
            Assert.Same(_model2, actual[1]);
            Assert.Same(_model3, actual[2]);
        }

        [Fact]
        public void Given_ContentFilter_WithOneId_Returns_One()
        {
            var filter = new List<int> { 2222 };
            var actual = _target.GetByContent(filter);
            Assert.Equal(1, actual.Count);
            Assert.Same(_model2, actual[0]);
        }

        [Fact]
        public void Given_ContentFilter_With_TwoIds_Returns_Two()
        {
            var filter = new List<int> { 2222, 1111 };
            var actual = _target.GetByContent(filter);
            Assert.Equal(2, actual.Count);
            Assert.Same(_model1, actual[0]);
            Assert.Same(_model2, actual[1]);
        }

        [Fact]
        public void Given_ContentFilter_With_Zero_Returns_Zero()
        {
            var filter = new List<int> { };
            var actual = _target.GetByContent(filter);
            Assert.Equal(0, actual.Count);
        }

        [Fact]
        public void Given_Rule_Add_To_Store()
        {
            var input = new RedirectRuleModel();
            var entity = new DynamicDataRedirectRule();
            var model = new RedirectRuleModel();
            _mapper.Setup(o => o.MapForSave(input)).Returns(entity);
            _store.Setup(o => o.Save(entity)).Returns(Identity.NewIdentity()).Verifiable();
            _mapper.Setup(o => o.MapToModel(entity)).Returns(model);

            var actual = _target.Add(input);
            Assert.Same(model, actual);
            _store.Verify();
        }

        [Fact]
        public void Given_Rule_Update_To_Store()
        {
            var input = new RedirectRuleModel { Id = _id1 };
            _mapper.Setup(o => o.MapForUpdate(input, _entity1)).Verifiable();
            _store.Setup(o => o.Save(_entity1)).Returns(Identity.NewIdentity()).Verifiable();

            var actual = _target.Update(input);
            Assert.Same(_model1, actual);
            _mapper.Verify();
            _store.Verify();
        }

        [Fact]
        public void Given_Rule_NotFound_During_Update()
        {
            var id = Guid.NewGuid();
            _store.Setup(o => o.GetById(id)).Returns(null as DynamicDataRedirectRule);
            var input = new RedirectRuleModel { Id = id };
            Assert.Throws<InvalidOperationException>(() => _target.Update(input));
        }

        [Fact]
        public void Given_Id_Delete_Rule_In_Store()
        {
            _store.Setup(o => o.Delete(_id1)).Verifiable();

            var actual = _target.Delete(_id1);
            Assert.True(actual);
            _store.Verify();
        }

        [Fact]
        public void Given_Delete_Fails_Returns_False()
        {
            _store.Setup(o => o.Delete(_id1)).Throws<InvalidOperationException>();

            var actual = _target.Delete(_id1);
            Assert.False(actual);
        }

        [Fact]
        public void Given_ClearAll_Returns_True()
        {
            _store.Setup(o => o.DeleteAll()).Verifiable();

            var actual = _target.ClearAll();
            Assert.True(actual);
            _store.Verify();
        }

        [Fact]
        public void Given_ClearAll_Fails_Returns_False()
        {
            _store.Setup(o => o.DeleteAll()).Throws<InvalidOperationException>();

            var actual = _target.ClearAll();
            Assert.False(actual);
        }
    }
}
