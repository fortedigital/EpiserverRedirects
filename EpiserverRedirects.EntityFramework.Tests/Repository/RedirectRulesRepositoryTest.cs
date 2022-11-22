﻿using Forte.EpiserverRedirects.EntityFramework;
using Forte.EpiserverRedirects.EntityFramework.Model;
using Forte.EpiserverRedirects.EntityFramework.Repository;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using Moq.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Xunit;


namespace EpiserverRedirects.EntityFramework.Tests.Repository
{
    public class RedirectRulesRepositoryTest
    {
        private readonly IRedirectRuleRepository _target;
        private readonly Mock<IRedirectRulesDbContext> _dbContext;
        private readonly Mock<IRedirectRuleMapper> _mapper;
        private readonly Mock<DbSet<RedirectRuleEntity>> _dbSet;

        private readonly Guid _id1;
        private readonly Guid _id2;
        private readonly Guid _id3;
        private readonly RedirectRuleEntity _entity1;
        private readonly RedirectRuleEntity _entity2;
        private readonly RedirectRuleEntity _entity3;
        private readonly RedirectRuleModel _model1;
        private readonly RedirectRuleModel _model2;
        private readonly RedirectRuleModel _model3;

        public RedirectRulesRepositoryTest()
        {
            _dbContext = new Mock<IRedirectRulesDbContext>(MockBehavior.Strict);
            _mapper = new Mock<IRedirectRuleMapper>(MockBehavior.Strict);
            _target = new RedirectRulesRepository(_dbContext.Object, _mapper.Object);
            _dbSet = new Mock<DbSet<RedirectRuleEntity>>(MockBehavior.Strict);

            _id1 = Guid.NewGuid();
            _id2 = Guid.NewGuid();
            _id3 = Guid.NewGuid();
            _entity1 = new RedirectRuleEntity { Id = _id1, OldPattern = "One", ContentId = 1111 };
            _entity2 = new RedirectRuleEntity { Id = _id2, OldPattern = "Two", ContentId = 2222 };
            _entity3 = new RedirectRuleEntity { Id = _id3, OldPattern = "Three", ContentId = null };
            _model1 = new RedirectRuleModel();
            _model2 = new RedirectRuleModel();
            _model3 = new RedirectRuleModel();
            _dbContext.Setup(o => o.RedirectRules).ReturnsDbSet(new List<RedirectRuleEntity> { _entity1, _entity2, _entity3 }, _dbSet);
            _mapper.Setup(o => o.MapToModel(_entity1)).Returns(_model1);
            _mapper.Setup(o => o.MapToModel(_entity2)).Returns(_model2);
            _mapper.Setup(o => o.MapToModel(_entity3)).Returns(_model3);
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
            Assert.Contains(_model1, actual);
            Assert.Contains(_model2, actual);
            Assert.Contains(_model3, actual);
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
            Assert.Contains(_model1, actual);
            Assert.Contains(_model2, actual);
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
            var entity = new RedirectRuleEntity();
            var model = new RedirectRuleModel();
            _mapper.Setup(o => o.MapForSave(input)).Returns(entity);
            _dbSet.Setup(o => o.Add(entity)).Returns(null as EntityEntry<RedirectRuleEntity>).Verifiable();
            _dbContext.Setup(o => o.SaveChanges()).Returns(0).Verifiable();
            _mapper.Setup(o => o.MapToModel(entity)).Returns(model);

            var actual = _target.Add(input);
            Assert.Same(model, actual);
            _dbSet.Verify();
            _dbContext.Verify();
        }

        [Fact]
        public void Given_Rule_Update_To_Store()
        {
            var input = new RedirectRuleModel { Id = _id1 };
            _mapper.Setup(o => o.MapForUpdate(input, _entity1)).Verifiable();
            _dbSet.Setup(o => o.Update(_entity1)).Returns(null as EntityEntry<RedirectRuleEntity>).Verifiable();
            _dbContext.Setup(o => o.SaveChanges()).Returns(0).Verifiable();

            var actual = _target.Update(input);
            Assert.Same(_model1, actual);
            _mapper.Verify();
            _dbSet.Verify();
            _dbContext.Verify();
        }

        [Fact]
        public void Given_Rule_NotFound_During_Update()
        {
            var input = new RedirectRuleModel { Id = Guid.NewGuid() };
            Assert.Throws<ArgumentException>(() => _target.Update(input));
        }

        [Fact]
        public void Given_Id_Delete_Rule_In_Store()
        {
            _dbSet.Setup(o => o.Remove(_entity1)).Returns(null as EntityEntry<RedirectRuleEntity>).Verifiable();
            _dbContext.Setup(o => o.SaveChanges()).Returns(0).Verifiable();

            var actual = _target.Delete(_id1);
            Assert.True(actual);
            _dbSet.Verify();
            _dbContext.Verify();
        }

        [Fact]
        public void Given_Delete_NonExistent_Returns_True()
        {
            var actual = _target.Delete(Guid.NewGuid());
            Assert.True(actual);
        }

        [Fact]
        public void Given_Delete_Fails_Returns_False()
        {
            _dbContext.Setup(o => o.SaveChanges()).Throws<InvalidOperationException>();

            var actual = _target.Delete(_id1);
            Assert.False(actual);
        }

        [Fact]
        public void Given_ClearAll_OnEmptySet()
        {
            _dbContext.Setup(o => o.RedirectRules).ReturnsDbSet(new List<RedirectRuleEntity> {}, _dbSet);
            var actual = _target.ClearAll();
            Assert.True(actual);
        }

        [Fact]
        public void Given_ClearAll_Fails_Returns_False()
        {
            _dbContext.Setup(o => o.SaveChanges()).Throws<InvalidOperationException>();

            var actual = _target.ClearAll();
            Assert.False(actual);
        }
    }
}
