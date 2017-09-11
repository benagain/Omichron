using System;
using ReactiveUI;
using Splat;
using Omichron.Services;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Omichron
{
    /* COOLSTUFF: What is the AppBootstrapper?
     * 
     * The AppBootstrapper is like a ViewModel for the WPF Application class.
     * Since Application isn't very testable (just like Window / UserControl), 
     * we want to create a class we can test. Since our application only has
     * one "screen" (i.e. a place we present Routed Views), we can also use 
     * this as our IScreen.
     * 
     * An IScreen is a ViewModel that contains a Router - practically speaking,
     * it usually represents a Window (or the RootFrame of a WinRT app). We 
     * should technically create a MainWindowViewModel to represent the IScreen,
     * but there isn't much benefit to split those up unless you've got multiple
     * windows.
     * 
     * AppBootstrapper is a good place to implement a lot of the "global 
     * variable" type things in your application. It's also the place where
     * you should configure your IoC container. And finally, it's the place 
     * which decides which View to Navigate to when the application starts.
     */

    public class AppBootstrapper : ReactiveObject, IScreen
    {
        public RoutingState Router { get; private set; }

        public AppBootstrapper(IMutableDependencyResolver dependencyResolver = null, RoutingState testRouter = null)
        {
            Router = testRouter ?? new RoutingState();
            dependencyResolver = dependencyResolver ?? Locator.CurrentMutable;

            // Bind 
            RegisterParts(dependencyResolver);

            // TODO: This is a good place to set up any other app 
            // startup tasks, like setting the logging level
            LogHost.Default.Level = LogLevel.Debug;

            // Navigate to the opening page of the application
            Router.Navigate.Execute(dependencyResolver.GetService<TimeLogsViewModel>());
        }

        private void RegisterParts(IMutableDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterConstant(this, typeof(IScreen));

            dependencyResolver.Register(() => 
                new TimeLogsViewModel(
                    this,
                    new ApplicationEvents(Application.Current).ToInterface(),
                    dependencyResolver.GetService<TimeLogSource>()), 
                typeof(TimeLogsViewModel));
            dependencyResolver.Register(() => new TimeLogsView(), typeof(IViewFor<TimeLogsViewModel>));
            dependencyResolver.Register(() => new JiraTimeLog(), typeof(TimeLogSource));
        }

        private class JiraTimeLog : TimeLogSource
        {
            private static int count = 1;

            public Task<List<TimeLog>> Search()
            {
                var random = new Random();
                return Task.FromResult(
                    Enumerable
                        .Range(0, count++)
                        .Select(x => new TimeLog($"hello {++x}", DateTime.UtcNow, TimeSpan.FromMinutes(random.Next(240))))
                        .ToList());
            }
        }
    }
}