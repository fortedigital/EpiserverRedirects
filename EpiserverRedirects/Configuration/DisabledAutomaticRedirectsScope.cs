using System;

namespace Forte.EpiserverRedirects.Configuration
{
    public sealed class DisabledAutomaticRedirectsScope : IDisposable
    {
        private readonly bool _previousValue;

        public DisabledAutomaticRedirectsScope()
        {
            _previousValue = EventsHandlersScopeConfiguration.IsAutomaticRedirectsDisabled;
            EventsHandlersScopeConfiguration.IsAutomaticRedirectsDisabled = true;
        }

        public void Dispose()
        {
            EventsHandlersScopeConfiguration.IsAutomaticRedirectsDisabled = _previousValue;
        }
    }
}
