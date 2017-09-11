namespace System.Windows
{
    public static class ApplicationEventsExtensions
    {
        public static IApplicationEvents ToInterface(this ApplicationEvents events)
        {
            return new Proxy(events);
        }

        private class Proxy : IApplicationEvents
        {
            private ApplicationEvents events;

            public Proxy(ApplicationEvents events)
            {
                this.events = events;
            }

            public IObservable<StartupEventArgs> Startup => events.Startup;

            public IObservable<ExitEventArgs> Exit => events.Exit;

            public IObservable<EventArgs> Activated => events.Activated;

            public IObservable<EventArgs> Deactivated => events.Deactivated;

            public IObservable<SessionEndingCancelEventArgs> SessionEnding => events.SessionEnding;
        }
    }
}
