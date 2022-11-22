using Forte.EpiserverRedirects.EntityFramework.Model;
using Forte.EpiserverRedirects.EntityFramework.Repository;
using Forte.EpiserverRedirects.Model;
using Forte.EpiserverRedirects.Model.RedirectRule;
using System;
using System.Collections.Generic;
using Xunit;


namespace EpiserverRedirects.EntityFramework.Tests.Repository
{
    public class RedirectRuleMapperTest
    {
        private readonly IRedirectRuleMapper _target;
        private readonly DateTime _createdOn;
        private readonly RedirectRuleModel _input;

        public RedirectRuleMapperTest()
        {
            _target = new RedirectRuleMapper();
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

            Assert.NotEqual(Guid.Empty, actual.Id);
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
        public void Given_Rule_To_MapForUpdate_Maps_To_Entity()
        {
            var existingId = Guid.NewGuid();
            var existingCreatedOn = new DateTime(2020, 10, 25);
            var actual = new RedirectRuleEntity
            {
                Id = existingId,
                CreatedOn = existingCreatedOn,
                CreatedBy = "ACTUAL_CREATED_BY"
            };
            _target.MapForUpdate(_input, actual);

            Assert.Equal(existingId, actual.Id);
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
            var existingId = Guid.NewGuid();
            var input = new RedirectRuleEntity
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
            Assert.Equal(existingId, actual.Id);
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
            var id = Guid.NewGuid();
            var inputRule = new RedirectRuleEntity
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
            var input = new SearchResult<RedirectRuleEntity>
            {
                Total = 9999,
                Items = new List<RedirectRuleEntity> { inputRule }
            };
            var actual = _target.MapSearchResult(input);
            Assert.Equal(9999, actual.Total);
            Assert.Equal(1, actual.Items.Count);
            Assert.Equal(id, actual.Items[0].Id);
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
