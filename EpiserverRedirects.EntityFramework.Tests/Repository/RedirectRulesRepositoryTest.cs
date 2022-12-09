using Forte.EpiserverRedirects.EntityFramework;
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
using System.Linq;
using Xunit;


namespace EpiserverRedirects.EntityFramework.Tests.Repository
{
    public class RedirectRulesRepositoryTest
    {
        private readonly IRedirectRuleRepository _target;
        private readonly Mock<IRedirectRulesDbContext> _dbContext;
        private readonly Mock<IRedirectRuleMapper<RedirectRuleEntity>> _mapper;
        private readonly Mock<DbSet<RedirectRuleEntity>> _dbSet;

        private readonly Guid _id1;
        private readonly Guid _id2;
        private readonly Guid _id3;
        private readonly RedirectRuleEntity _entity1;
        private readonly RedirectRuleEntity _entity2;
        private readonly RedirectRuleEntity _entity3;

        public RedirectRulesRepositoryTest()
        {
            _dbContext = new Mock<IRedirectRulesDbContext>(MockBehavior.Strict);
            _mapper = new Mock<IRedirectRuleMapper<RedirectRuleEntity>>(MockBehavior.Strict);
            _target = new RedirectRulesRepository(_dbContext.Object, _mapper.Object);
            _dbSet = new Mock<DbSet<RedirectRuleEntity>>(MockBehavior.Strict);

            _id1 = Guid.NewGuid();
            _id2 = Guid.NewGuid();
            _id3 = Guid.NewGuid();
            _entity1 = new RedirectRuleEntity { RuleId = _id1, OldPattern = "One", ContentId = 1111 };
            _entity2 = new RedirectRuleEntity { RuleId = _id2, OldPattern = "Two", ContentId = 2222 };
            _entity3 = new RedirectRuleEntity { RuleId = _id3, OldPattern = "Three", ContentId = null };
            _dbContext.Setup(o => o.RedirectRules).ReturnsDbSet(new List<RedirectRuleEntity> { _entity1, _entity2, _entity3 }, _dbSet);
        }

        [Fact]
        public void Given_Id_Returns_RedirectRule()
        {
            var actual = _target.GetById(_id2);
            Assert.Same(_entity2, actual);
        }

        [Fact]
        public void Given_GetAll_Returns_All()
        {
            var actual = _target.GetAll();
            Assert.Equal(3, actual.Count());
            Assert.Contains(_entity1, actual);
            Assert.Contains(_entity2, actual);
            Assert.Contains(_entity3, actual);
        }

        [Fact]
        public void Given_Rule_Add_To_Store()
        {
            var input = new RedirectRuleModel();
            var entity = new RedirectRuleEntity();
            _mapper.Setup(o => o.ToNewEntity(input)).Returns(entity);
            _dbSet.Setup(o => o.Add(entity)).Returns(null as EntityEntry<RedirectRuleEntity>).Verifiable();
            _dbContext.Setup(o => o.SaveChanges()).Returns(0).Verifiable();

            var actual = _target.Add(input);
            Assert.Same(entity, actual);
            _dbSet.Verify();
            _dbContext.Verify();
        }

        [Fact]
        public void Given_Rule_Update_To_Store()
        {
            var input = new RedirectRuleModel { RuleId = _id1 };
            _mapper.Setup(o => o.MapForUpdate(input, _entity1)).Verifiable();
            _dbSet.Setup(o => o.Update(_entity1)).Returns(null as EntityEntry<RedirectRuleEntity>).Verifiable();
            _dbContext.Setup(o => o.SaveChanges()).Returns(0).Verifiable();

            var actual = _target.Update(input);
            Assert.Same(_entity1, actual);
            _mapper.Verify();
            _dbSet.Verify();
            _dbContext.Verify();
        }

        [Fact]
        public void Given_Rule_NotFound_During_Update()
        {
            var input = new RedirectRuleModel { RuleId = Guid.NewGuid() };
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
            _dbContext.Setup(o => o.RedirectRules).ReturnsDbSet(new List<RedirectRuleEntity>(), _dbSet);
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
