using EPiServer.Data;
using Forte.EpiserverRedirects.DynamicData;
using Forte.EpiserverRedirects.Model;
using Forte.EpiserverRedirects.Model.RedirectRule;
using System;
using System.Collections.Generic;
using Xunit;


namespace Forte.EpiserverRedirects.Tests.Unit.DynamicData
{
    public class DynamicDataRedirectRuleMapperTest
    {
        private readonly IDynamicDataRedirectRuleMapper _target;
        private readonly DateTime _createdOn;
        private readonly RedirectRuleModel _input;

        public DynamicDataRedirectRuleMapperTest()
        {
            _target = new DynamicDataRedirectRuleMapper();
            _createdOn = new DateTime(2020, 10, 25);
            _input = new RedirectRuleModel
            {
                ContentId = 1111,
                OldPattern = "OLD_PATTERN",
                NewPattern = "NEW_PATTERN",
                RedirectRuleType = RedirectRuleType.ExactMatch,
                RedirectType = RedirectType.Permanent,
                RedirectOrigin = RedirectOrigin.Manual,
                CreatedOn = _createdOn,
                IsActive = true,
                CreatedBy = "CREATED_BY",
                Notes = "NOTES_NOTES_NOTES",
                Priority = 2222
            };
        }

        [Fact]
        public void Given_Null_To_MapForSave_Returns_Null()
        {
            var actual = _target.MapForSave(null);
            Assert.Null(actual);
        }

        [Fact]
        public void Given_Rule_To_MapForSave_Returns_MappedEntity()
        {
            var actual = _target.MapForSave(_input);

            Assert.NotNull(actual.Id);
            Assert.NotEqual(Guid.Empty, actual.Id.ExternalId);
            Assert.Equal(1111, actual.ContentId);
            Assert.Equal("OLD_PATTERN", actual.OldPattern);
            Assert.Equal("NEW_PATTERN", actual.NewPattern);
            Assert.Equal(RedirectRuleType.ExactMatch, actual.RedirectRuleType);
            Assert.Equal(RedirectType.Permanent, actual.RedirectType);
            Assert.Equal(RedirectOrigin.Manual, actual.RedirectOrigin);
            Assert.Equal(_createdOn, actual.CreatedOn);
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
            var actual = new DynamicDataRedirectRule
            {
                Id = existingId,
                CreatedOn = existingCreatedOn,
                CreatedBy = "ACTUAL_CREATED_BY"
            };
            _target.MapForUpdate(_input, actual);

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
            var input = new DynamicDataRedirectRule
            {
                Id = existingId,
                ContentId = 1111,
                OldPattern = "OLD_PATTERN",
                NewPattern = "NEW_PATTERN",
                RedirectRuleType = RedirectRuleType.ExactMatch,
                RedirectType = RedirectType.Permanent,
                RedirectOrigin = RedirectOrigin.Manual,
                CreatedOn = _createdOn,
                IsActive = true,
                CreatedBy = "CREATED_BY",
                Notes = "NOTES_NOTES_NOTES",
                Priority = 2222
            };
            var actual = _target.MapToModel(input);
            Assert.Equal(existingId.ExternalId, actual.Id);
            Assert.Equal(1111, actual.ContentId);
            Assert.Equal("OLD_PATTERN", actual.OldPattern);
            Assert.Equal("NEW_PATTERN", actual.NewPattern);
            Assert.Equal(RedirectRuleType.ExactMatch, actual.RedirectRuleType);
            Assert.Equal(RedirectType.Permanent, actual.RedirectType);
            Assert.Equal(RedirectOrigin.Manual, actual.RedirectOrigin);
            Assert.Equal(_createdOn, actual.CreatedOn);
            Assert.True(actual.IsActive);
            Assert.Equal("CREATED_BY", actual.CreatedBy);
            Assert.Equal("NOTES_NOTES_NOTES", actual.Notes);
            Assert.Equal(2222, actual.Priority);
        }

        [Fact]
        public void Given_SearchResult_Converted_ToModelQueryResult()
        {
            var id = Identity.NewIdentity();
            var inputRule = new DynamicDataRedirectRule
            {
                Id = id,
                ContentId = 1111,
                OldPattern = "OLD_PATTERN",
                NewPattern = "NEW_PATTERN",
                RedirectRuleType = RedirectRuleType.ExactMatch,
                RedirectType = RedirectType.Permanent,
                RedirectOrigin = RedirectOrigin.Manual,
                CreatedOn = _createdOn,
                IsActive = true,
                CreatedBy = "CREATED_BY",
                Notes = "NOTES_NOTES_NOTES",
                Priority = 2222
            };
            var input = new SearchResult<DynamicDataRedirectRule>
            {
                Total = 9999,
                Items = new List<DynamicDataRedirectRule> { inputRule }
            };
            var actual = _target.MapSearchResult(input);
            Assert.Equal(9999, actual.Total);
            Assert.Equal(1, actual.Items.Count);
            Assert.Equal(id.ExternalId, actual.Items[0].Id);
            Assert.Equal(1111, actual.Items[0].ContentId);
            Assert.Equal("OLD_PATTERN", actual.Items[0].OldPattern);
            Assert.Equal("NEW_PATTERN", actual.Items[0].NewPattern);
            Assert.Equal(RedirectRuleType.ExactMatch, actual.Items[0].RedirectRuleType);
            Assert.Equal(RedirectType.Permanent, actual.Items[0].RedirectType);
            Assert.Equal(RedirectOrigin.Manual, actual.Items[0].RedirectOrigin);
            Assert.Equal(_createdOn, actual.Items[0].CreatedOn);
            Assert.True(actual.Items[0].IsActive);
            Assert.Equal("CREATED_BY", actual.Items[0].CreatedBy);
            Assert.Equal("NOTES_NOTES_NOTES", actual.Items[0].Notes);
            Assert.Equal(2222, actual.Items[0].Priority);
        }
    }
}
