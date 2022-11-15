using Forte.EpiserverRedirects.EntityFramework.Model;
using Forte.EpiserverRedirects.EntityFramework.Repository;
using Forte.EpiserverRedirects.Model.RedirectRule;
using System;
using Xunit;


namespace EpiserverRedirects.EntityFramework.Tests.Repository
{
    public class RedirectRuleMapperTest
    {
        private readonly IRedirectRuleMapper _target;
        private readonly DateTime _createdOn;
        private readonly RedirectRule _input;

        public RedirectRuleMapperTest()
        {
            this._target = new RedirectRuleMapper();
            this._createdOn = new DateTime(2020, 10, 25);
            this._input = new RedirectRule
            {
                ContentId = 1111,
                OldPattern = "OLD_PATTERN",
                NewPattern = "NEW_PATTERN",
                RedirectRuleType = RedirectRuleType.ExactMatch,
                RedirectType = RedirectType.Temporary,
                RedirectOrigin = RedirectOrigin.Manual,
                CreatedOn = _createdOn,
                IsActive = true,
                CreatedBy = "CREATED_BY",
                Notes = "NOTES_NOTES_NOTES",
                Priority = 2222
            };
        }

        [Fact]
        public void Given_Rule_Converted_To_Entity()
        {
            var actual = _target.ToNewEntity(_input);

            Assert.NotEqual(Guid.Empty, actual.RuleId);
            Assert.Equal(1111, actual.ContentId);
            Assert.Equal("OLD_PATTERN", actual.OldPattern);
            Assert.Equal("NEW_PATTERN", actual.NewPattern);
            Assert.Equal(RedirectRuleType.ExactMatch, actual.RedirectRuleType);
            Assert.Equal(RedirectType.Temporary, actual.RedirectType);
            Assert.Equal(RedirectOrigin.Manual, actual.RedirectOrigin);
            Assert.Equal(_createdOn, actual.CreatedOn);
            Assert.True(actual.IsActive);
            Assert.Equal("CREATED_BY", actual.CreatedBy);
            Assert.Equal("NOTES_NOTES_NOTES", actual.Notes);
            Assert.Equal(2222, actual.Priority);
        }

        [Fact]
        public void Given_Rule_Mapped_To_Entity()
        {
            var existingRuleId = Guid.NewGuid();
            var existingCreatedOn = new DateTime(2020, 10, 25);
            var actual = new RedirectRuleEntity
            {
                RuleId = existingRuleId,
                CreatedOn = existingCreatedOn,
                CreatedBy = "ACTUAL_CREATED_BY"
            };
            _target.MapForUpdate(_input, actual);

            Assert.Equal(existingRuleId, actual.RuleId);
            Assert.Equal(1111, actual.ContentId);
            Assert.Equal("OLD_PATTERN", actual.OldPattern);
            Assert.Equal("NEW_PATTERN", actual.NewPattern);
            Assert.Equal(RedirectRuleType.ExactMatch, actual.RedirectRuleType);
            Assert.Equal(RedirectType.Temporary, actual.RedirectType);
            Assert.Equal(RedirectOrigin.Manual, actual.RedirectOrigin);
            Assert.Equal(existingCreatedOn, actual.CreatedOn);
            Assert.True(actual.IsActive);
            Assert.Equal("ACTUAL_CREATED_BY", actual.CreatedBy);
            Assert.Equal("NOTES_NOTES_NOTES", actual.Notes);
            Assert.Equal(2222, actual.Priority);
        }
    }
}
