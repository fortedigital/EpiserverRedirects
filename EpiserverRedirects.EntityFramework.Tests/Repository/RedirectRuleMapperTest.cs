using Forte.EpiserverRedirects.EntityFramework.Model;
using Forte.EpiserverRedirects.EntityFramework.Repository;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Repository;
using System;
using Xunit;


namespace Forte.EpiserverRedirects.EntityFramework.Tests.Repository
{
    public class RedirectRuleMapperTest
    {
        private readonly IRedirectRuleMapper<RedirectRuleEntity> _target;
        private readonly Guid _entityId;
        private readonly RedirectRuleEntity _entity;
        private readonly RedirectRuleModel _model;

        public RedirectRuleMapperTest()
        {
            _target = new RedirectRuleMapper();
            _entityId = Guid.NewGuid();
            _entity = new RedirectRuleEntity
            {
                RuleId = _entityId,
                ContentId = 1111,
                OldPattern = "OLD_PATTERN_ORIGINAL",
                NewPattern = "NEW_PATTERN_ORIGINAL",
                RedirectRuleType = RedirectRuleType.ExactMatch,
                RedirectType = RedirectType.Temporary,
                RedirectOrigin = RedirectOrigin.Import,
                CreatedOn = new DateTime(2005, 05, 05),
                IsActive = true,
                CreatedBy = "CREATED_BY_ORIGINAL",
                Notes = "NOTES_NOTES_ORIGINAL",
                Priority = 2222
            };
            _model = new RedirectRuleModel
            {
                RuleId = Guid.NewGuid(),
                ContentId = 7777,
                OldPattern = "OLD_PATTERN_MODIFIED",
                NewPattern = "NEW_PATTERN_MODIFIED",
                RedirectRuleType = RedirectRuleType.Regex,
                RedirectType = RedirectType.Permanent,
                RedirectOrigin = RedirectOrigin.System,
                CreatedOn = new DateTime(2010, 10, 10),
                IsActive = false,
                CreatedBy = "CREATED_BY_MODIFIED",
                Notes = "NOTES_NOTES_MODIFIED",
                Priority = 8888
            };
        }

        [Fact]
        public void Given_Rule_MapForCreate_MapsCorrectly()
        {
            var actual = _target.ToNewEntity(_model);

            Assert.NotEqual(_model.RuleId, _entity.RuleId);
            Assert.Equal(7777, actual.ContentId);
            Assert.Equal("OLD_PATTERN_MODIFIED", actual.OldPattern);
            Assert.Equal("NEW_PATTERN_MODIFIED", actual.NewPattern);
            Assert.Equal(RedirectRuleType.Regex, actual.RedirectRuleType);
            Assert.Equal(RedirectType.Permanent, actual.RedirectType);
            Assert.Equal(RedirectOrigin.System, actual.RedirectOrigin);
            Assert.Equal(new DateTime(2010, 10, 10), actual.CreatedOn);
            Assert.False(actual.IsActive);
            Assert.Equal("CREATED_BY_MODIFIED", actual.CreatedBy);
            Assert.Equal("NOTES_NOTES_MODIFIED", actual.Notes);
            Assert.Equal(8888, actual.Priority);
        }

        [Fact]
        public void Given_Rule_MapForUpdate_MapsCorrectly()
        {
            _target.MapForUpdate(_model, _entity);

            Assert.Equal(_entityId, _entity.RuleId);
            Assert.Equal(7777, _entity.ContentId);
            Assert.Equal("OLD_PATTERN_MODIFIED", _entity.OldPattern);
            Assert.Equal("NEW_PATTERN_MODIFIED", _entity.NewPattern);
            Assert.Equal(RedirectRuleType.Regex, _entity.RedirectRuleType);
            Assert.Equal(RedirectType.Permanent, _entity.RedirectType);
            Assert.Equal(RedirectOrigin.Import, _entity.RedirectOrigin);
            Assert.Equal(new DateTime(2005, 05, 05), _entity.CreatedOn);
            Assert.False(_entity.IsActive);
            Assert.Equal("CREATED_BY_ORIGINAL", _entity.CreatedBy);
            Assert.Equal("NOTES_NOTES_MODIFIED", _entity.Notes);
            Assert.Equal(8888, _entity.Priority);
        }
    }
}
