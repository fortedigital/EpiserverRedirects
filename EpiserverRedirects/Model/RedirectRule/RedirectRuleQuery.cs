using EPiServer.Shell.Services.Rest;
using Forte.EpiserverRedirects.Menu;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;


namespace Forte.EpiserverRedirects.Model.RedirectRule
{
    [ModelBinder(typeof(QueryModelBinder))]
    public class RedirectRuleQuery
    {
        public string OldPattern { get; set; }
        public string NewPattern { get; set; }
        public int? ContentId { get; set; }
        public RedirectType? RedirectType { get; set; }
        public RedirectRuleType? RedirectRuleType { get; set; }
        public RedirectOrigin? RedirectOrigin { get; set; }

        public bool? IsActive { get; set; }
        public DateTime? CreatedOnFrom { get; set; }
        public DateTime? CreatedOnTo { get; set; }
        public string CreatedBy { get; set; }
        public string Notes { get; set; }
        public int? Priority { get; set; }

        public IEnumerable<SortColumn> SortColumns { get; set; }
        public ItemRange Range { get; set; }
    }
}
