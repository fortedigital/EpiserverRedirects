using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Forte.EpiserverRedirects.Tests.Repository
{
    public class TestRepository : IRedirectRuleRepository
    {
        private readonly HashSet<RedirectRuleModel> _redirectsHashSet;
        
        public TestRepository()
        {
            _redirectsHashSet = new HashSet<RedirectRuleModel>();
        }

        public TestRepository(HashSet<RedirectRuleModel> redirectsCollection)
        {
            _redirectsHashSet = redirectsCollection;
        }

        public RedirectRuleModel GetById(Guid id)
        {
            var redirectRule =
                _redirectsHashSet.FirstOrDefault(r => r.Id == id);
            return redirectRule;
        }

        public IList<RedirectRuleModel> GetAll() => _redirectsHashSet.ToList();

        public IList<RedirectRuleModel> Query(out int total, RedirectRuleQuery query)
        {
            total = _redirectsHashSet.Count;
            return _redirectsHashSet.ToList();
        }

        public IList<RedirectRuleModel> GetByContent(IList<int> contentIds) => _redirectsHashSet.ToList();

        public RedirectRuleModel FindRegexMatch(string patern)
        {
            return _redirectsHashSet
                .Where(r => r.IsActive && r.RedirectRuleType == RedirectRuleType.Regex)
                .OrderBy(x => x.Priority)
                .AsEnumerable()
                .FirstOrDefault(r => Regex.IsMatch(patern, r.OldPattern, RegexOptions.IgnoreCase));
        }

        public RedirectRuleModel FindExactMatch(string patern)
        {
            return _redirectsHashSet
                .Where(r => r.IsActive && r.RedirectRuleType == RedirectRuleType.ExactMatch)
                .OrderBy(x => x.Priority)
                .FirstOrDefault(r => r.OldPattern == patern);
        }

        public RedirectRuleModel Add(RedirectRuleModel redirectRule)
        {
            redirectRule.Id = Guid.NewGuid();
            _redirectsHashSet.Add(redirectRule);
            return redirectRule;
        }

        public RedirectRuleModel Update(RedirectRuleModel redirectRule)
        {
            var redirectRuleToUpdate =
                _redirectsHashSet.FirstOrDefault(r => r.Id == redirectRule.Id);
            
            if(redirectRuleToUpdate==null)
            {
                throw new KeyNotFoundException("No existing redirect with this GUID");
            }

            WriteToModel(redirectRule, redirectRuleToUpdate);
            return redirectRule;
        }

        public bool Delete(Guid id)
        {
            var redirectRule = _redirectsHashSet.FirstOrDefault(r => r.Id == id);
            
            return redirectRule != null && _redirectsHashSet.Remove(redirectRule);
        }

        public bool ClearAll()
        {
            _redirectsHashSet.Clear();

            return true;
        }

        protected static void WriteToModel(RedirectRuleModel redirectRule, RedirectRuleModel redirectRuleToUpdate)
        {
            redirectRuleToUpdate.Id = redirectRule.Id;
            redirectRuleToUpdate.OldPattern = redirectRule.OldPattern;
            redirectRuleToUpdate.NewPattern = redirectRule.NewPattern;
            redirectRuleToUpdate.RedirectType = redirectRule.RedirectType;
            redirectRuleToUpdate.RedirectRuleType = redirectRule.RedirectRuleType;
            redirectRuleToUpdate.IsActive = redirectRule.IsActive;
            redirectRuleToUpdate.Notes = redirectRule.Notes;
            redirectRuleToUpdate.Priority = redirectRule.Priority;
            redirectRuleToUpdate.ContentId = redirectRule.ContentId;
        }
    }
}
