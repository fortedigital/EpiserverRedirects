using EPiServer.Data;
using Forte.EpiserverRedirects.DynamicData;
using Forte.EpiserverRedirects.Model.RedirectRule;
using System;
using Xunit;


namespace Forte.EpiserverRedirects.Tests.Unit.DynamicData
{
    public class DynamicDataRedirectRuleMapperTest
    {
        private readonly IDynamicDataRedirectRuleMapper target;
        private readonly DateTime createdOn;
        private readonly RedirectRule input;

        public DynamicDataRedirectRuleMapperTest()
        {
            this.target = new DynamicDataRedirectRuleMapper();
            this.createdOn = new DateTime(2020, 10, 25);
            this.input = new RedirectRule
            {
                ContentId = 1111,
                OldPattern = "OLD_PATTERN",
                NewPattern = "NEW_PATTERN",
                RedirectRuleType = RedirectRuleType.ExactMatch,
                RedirectType = RedirectType.Permanent,
                RedirectOrigin = RedirectOrigin.Manual,
                CreatedOn = createdOn,
                IsActive = true,
                CreatedBy = "CREATED_BY",
                Notes = "NOTES_NOTES_NOTES",
                Priority = 2222
            };
        }

        [Fact]
        public void Given_Rule_Converted_To_DdsEntity()
        {
            var actual = target.ToNewEntity(input);

            Assert.NotNull(actual.Id);
            Assert.NotEqual(Guid.Empty, actual.RuleId);
            Assert.Equal(1111, actual.ContentId);
            Assert.Equal("OLD_PATTERN", actual.OldPattern);
            Assert.Equal("NEW_PATTERN", actual.NewPattern);
            Assert.Equal(RedirectRuleType.ExactMatch, actual.RedirectRuleType);
            Assert.Equal(RedirectType.Permanent, actual.RedirectType);
            Assert.Equal(RedirectOrigin.Manual, actual.RedirectOrigin);
            Assert.Equal(createdOn, actual.CreatedOn);
            Assert.True(actual.IsActive);
            Assert.Equal("CREATED_BY", actual.CreatedBy);
            Assert.Equal("NOTES_NOTES_NOTES", actual.Notes);
            Assert.Equal(2222, actual.Priority);
        }

        [Fact]
        public void Given_Rule_Mapped_To_DdsEntity()
        {
            var existingId = Identity.NewIdentity();
            var existingCreatedOn = new DateTime(2020, 10, 25);
            var actual = new DynamicDataRedirectRule
            {
                Id = existingId,
                CreatedOn = existingCreatedOn,
                CreatedBy = "ACTUAL_CREATED_BY"
            };
            target.MapForUpdate(input, actual);

            Assert.Same(existingId, actual.Id);
            Assert.Equal(1111, actual.ContentId);
            Assert.Equal("OLD_PATTERN", actual.OldPattern);
            Assert.Equal("NEW_PATTERN", actual.NewPattern);
            Assert.Equal(RedirectRuleType.ExactMatch, actual.RedirectRuleType);
            Assert.Equal(RedirectType.Permanent, actual.RedirectType);
            Assert.Equal(RedirectOrigin.Manual, actual.RedirectOrigin);
            Assert.Equal(existingCreatedOn, actual.CreatedOn);
            Assert.True(actual.IsActive);
            Assert.Equal("ACTUAL_CREATED_BY", actual.CreatedBy);
            Assert.Equal("NOTES_NOTES_NOTES", actual.Notes);
            Assert.Equal(2222, actual.Priority);
        }
    }
}
