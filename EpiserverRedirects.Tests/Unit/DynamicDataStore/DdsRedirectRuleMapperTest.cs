using EPiServer.Data;
using Forte.EpiserverRedirects.DynamicDataStore;
using Forte.EpiserverRedirects.Model.RedirectRule;
using System;
using Xunit;


namespace Forte.EpiserverRedirects.Tests.Unit.DynamicDataStore
{
    public class DdsRedirectRuleMapperTest
    {
        private readonly IDdsRedirectRuleMapper target;
        private readonly DateTime createdOn;
        private readonly RedirectRuleModel input;

        public DdsRedirectRuleMapperTest()
        {
            this.target = new DdsRedirectRuleMapper();
            this.createdOn = new DateTime(2020, 10, 25);
            this.input = new RedirectRuleModel
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
        public void Given_Null_To_MapForSave_Returns_Null()
        {
            var actual = target.MapForSave(null);
            Assert.Null(actual);
        }

        [Fact]
        public void Given_Rule_To_MapForSave_Returns_MappedEntity()
        {
            var actual = target.MapForSave(input);

            Assert.NotNull(actual.Id);
            Assert.NotEqual(Guid.Empty, actual.Id.ExternalId);
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
        public void Given_Rule_To_MapForUpdate_Maps_To_DdsEntity()
        {
            var existingId = Identity.NewIdentity();
            var existingCreatedOn = new DateTime(2020, 10, 25);
            var actual = new DdsRedirectRule
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

        [Fact]
        public void Given_Entity_To_MapToModel_Returns_Model()
        {
            var existingId = Identity.NewIdentity();
            var input = new DdsRedirectRule
            {
                Id = existingId,
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
            var actual = target.MapToModel(input);
            Assert.Equal(existingId.ExternalId, actual.Id);
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
    }
}
