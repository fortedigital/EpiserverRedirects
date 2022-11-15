using Forte.EpiserverRedirects.EntityFramework;
using Forte.EpiserverRedirects.EntityFramework.Model;
using Forte.EpiserverRedirects.EntityFramework.Repository;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Repository;
using Moq;
using Moq.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Xunit;


namespace EpiserverRedirects.EntityFramework.Tests.Repository
{
    public class RedirectRulesRepositoryTest
    {
        private readonly IRedirectRuleRepository target;
        private readonly Mock<IRedirectRulesDbContext> dbContext;
        private readonly Mock<IRedirectRuleMapper> mapper;

        private readonly Guid id1;
        private readonly Guid id2;
        private readonly Guid id3;
        private readonly RedirectRuleEntity entity1;
        private readonly RedirectRuleEntity entity2;
        private readonly RedirectRuleEntity entity3;
        private readonly RedirectRuleModel model1;
        private readonly RedirectRuleModel model2;
        private readonly RedirectRuleModel model3;

        public RedirectRulesRepositoryTest()
        {
            dbContext = new Mock<IRedirectRulesDbContext>(MockBehavior.Strict);
            mapper = new Mock<IRedirectRuleMapper>(MockBehavior.Strict);
            target = new RedirectRulesRepository<IRedirectRulesDbContext>(dbContext.Object, mapper.Object);

            id1 = Guid.NewGuid();
            id2 = Guid.NewGuid();
            id3 = Guid.NewGuid();
            entity1 = new RedirectRuleEntity { Id = id1, OldPattern = "One", ContentId = 1111 };
            entity2 = new RedirectRuleEntity { Id = id2, OldPattern = "Two", ContentId = 2222 };
            entity3 = new RedirectRuleEntity { Id = id3, OldPattern = "Three", ContentId = null };
            model1 = new RedirectRuleModel();
            model2 = new RedirectRuleModel();
            model3 = new RedirectRuleModel();
            dbContext.Setup(o => o.RedirectRules).ReturnsDbSet(new List<RedirectRuleEntity> { entity1, entity2, entity3 });
            mapper.Setup(o => o.MapToModel(entity1)).Returns(model1);
            mapper.Setup(o => o.MapToModel(entity2)).Returns(model2);
            mapper.Setup(o => o.MapToModel(entity3)).Returns(model3);
        }

        [Fact]
        public void Given_Id_Returns_RedirectRule()
        {
            var actual = target.GetById(id2);
            Assert.Same(model2, actual);
        }

        [Fact]
        public void Given_GetAll_Returns_All()
        {
            var actual = target.GetAll();
            Assert.Equal(3, actual.Count);
            Assert.Same(model1, actual[0]);
            Assert.Same(model2, actual[1]);
            Assert.Same(model3, actual[2]);
        }

        [Fact]
        // TODO - This should be separate TestSuite with all fields and sort fields applied
        public void Given_Query_IsApplied()
        {
            var query = new RedirectRuleQuery { OldPattern = "Two" };

            var actual = target.Query(out var total, query);
            Assert.Equal(1, actual.Count);
            Assert.Same(model2, actual[0]);
        }

        [Fact]
        public void Given_ContentFilter_WithOneId_Returns_One()
        {
            var filter = new List<int> { 2222 };
            var actual = target.GetByContent(filter);
            Assert.Equal(1, actual.Count);
            Assert.Same(model2, actual[0]);
        }

        [Fact]
        public void Given_ContentFilter_With_TwoIds_Returns_Two()
        {
            var filter = new List<int> { 2222, 1111 };
            var actual = target.GetByContent(filter);
            Assert.Equal(2, actual.Count);
            Assert.Same(model1, actual[0]);
            Assert.Same(model2, actual[1]);
        }

        [Fact]
        public void Given_ContentFilter_With_Zero_Returns_Zero()
        {
            var filter = new List<int> { };
            var actual = target.GetByContent(filter);
            Assert.Equal(0, actual.Count);
        }

        [Fact]
        public void Given_Rule_Add_To_Store()
        {
            var input = new RedirectRuleModel();
            var entity = new RedirectRuleEntity();
            var model = new RedirectRuleModel();
            mapper.Setup(o => o.MapForSave(input)).Returns(entity);
            dbContext.Setup(o => o.SaveChanges()).Returns(0).Verifiable();
            mapper.Setup(o => o.MapToModel(entity)).Returns(model);

            var actual = target.Add(input);
            Assert.Same(model, actual);
            dbContext.Verify();
        }

        [Fact]
        public void Given_Rule_Update_To_Store()
        {
            var input = new RedirectRuleModel { Id = id1 };
            mapper.Setup(o => o.MapForUpdate(input, entity1)).Verifiable();
            dbContext.Setup(o => o.SaveChanges()).Returns(0).Verifiable();

            var actual = target.Update(input);
            Assert.Same(model1, actual);
            mapper.Verify();
            dbContext.Verify();
        }

        [Fact]
        public void Given_Rule_NotFound_During_Update()
        {
            var input = new RedirectRuleModel { Id = Guid.NewGuid() };
            Assert.Throws<ArgumentException>(() => target.Update(input));
        }

        [Fact]
        public void Given_Id_Delete_Rule_In_Store()
        {
            dbContext.Setup(o => o.SaveChanges()).Returns(0).Verifiable();

            var actual = target.Delete(id1);
            Assert.True(actual);
            dbContext.Verify();
        }

        [Fact]
        public void Given_Delete_NonExistent_Returns_True()
        {
            var actual = target.Delete(Guid.NewGuid());
            Assert.True(actual);
        }

        [Fact]
        public void Given_Delete_Fails_Returns_False()
        {
            dbContext.Setup(o => o.SaveChanges()).Throws<InvalidOperationException>();

            var actual = target.Delete(id1);
            Assert.False(actual);
        }

        [Fact]
        public void Given_ClearAll_Fails_Returns_False()
        {
            dbContext.Setup(o => o.SaveChanges()).Throws<InvalidOperationException>();

            var actual = target.ClearAll();
            Assert.False(actual);
        }
    }
}
