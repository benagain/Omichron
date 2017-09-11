namespace System.Windows
{
    public interface IApplicationEvents
    {
        IObservable<StartupEventArgs> Startup { get; }
        IObservable<ExitEventArgs> Exit { get; }
        IObservable<EventArgs> Activated { get; }
        IObservable<EventArgs> Deactivated { get; }
        IObservable<SessionEndingCancelEventArgs> SessionEnding { get; }
        //IObservable<DispatcherUnhandledExceptionEventArgs> DispatcherUnhandledException { get; }
        //IObservable<NavigatingCancelEventArgs> Navigating { get; }
        //IObservable<NavigationEventArgs> Navigated { get; }
        //IObservable<NavigationProgressEventArgs> NavigationProgress { get; }
        //IObservable<NavigationFailedEventArgs> NavigationFailed { get; }
        //IObservable<NavigationEventArgs> LoadCompleted { get; }
        //IObservable<NavigationEventArgs> NavigationStopped { get; }
        //IObservable<FragmentNavigationEventArgs> FragmentNavigation { get; }
    }
}
