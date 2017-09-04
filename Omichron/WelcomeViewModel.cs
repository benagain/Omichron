using System;
using ReactiveUI;
using System.Reactive.Linq;
using System.Reactive.Disposables;

namespace Omichron
{
    // It's usually a good idea to create an interface for every ViewModel and
    // reference that instead of the implementation. This makes creating fake
    // versions or design-time versions of ViewModels much easier.
    public interface IWelcomeViewModel : IRoutableViewModel
    {
        ReactiveCommand HelloWorld { get; }
        Interaction<string, bool> Helloed { get; }
    }

    public class WelcomeViewModel : ReactiveObject, IWelcomeViewModel
    {
        /* COOLSTUFF: What is UrlPathSegment
         * 
         * Imagine that the router state is like the path of the URL - what 
         * would the path look like for this particular page? Maybe it would be
         * the current user's name, or an "id". In this case, it's just a 
         * constant. You can get the whole path via 
         * IRoutingState.GetUrlForCurrentRoute.
         */
        public string UrlPathSegment => "welcome";

        public IScreen HostScreen { get; protected set; }

        public ReactiveCommand HelloWorld { get; }

        public Interaction<string, bool> Helloed { get; } = new Interaction<string, bool>();

        /* COOLSTUFF: Why the Screen here?
         *
         * Every RoutableViewModel has a pointer to its IScreen. This is really
         * useful in a unit test runner, because you can create a dummy screen,
         * invoke Commands / change Properties, then test to see if you navigated
         * to the correct new screen 
         */
        public WelcomeViewModel(IScreen screen)
        {
            HostScreen = screen;

            HelloWorld = ReactiveCommand.Create(async () => await Helloed.Handle("It works!!!"));

            this.WhenNavigatedTo(() => Bar());
        }

        private IDisposable Bar()
        {
            return Disposable.Create(() => Foo());
        }

        private void Foo()
        {
            if (true) { }
        }
    }
}