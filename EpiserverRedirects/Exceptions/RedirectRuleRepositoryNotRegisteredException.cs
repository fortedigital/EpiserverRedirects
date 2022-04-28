using System;
using Forte.EpiserverRedirects.Repository;

namespace Forte.EpiserverRedirects.Exceptions
{
    public class RedirectRuleRepositoryNotRegisteredException : Exception
    {
        public RedirectRuleRepositoryNotRegisteredException()
            : base(
                $"{nameof(IRedirectRuleRepository)} service was not registered. Please specify type of repository after configuring EpiserverRedirects (either DynamicDataStore or custom one)")
        {
        }
    }
}
