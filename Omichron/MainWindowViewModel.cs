using ReactiveUI;
using Splat;
using System.Reactive.Disposables;

namespace Omichron
{
    public class MainWindowViewModel : ReactiveObject, IScreen, ISupportsActivation
    {
        public MainWindowViewModel(RoutingState testRouter = null)
        {
            Router = testRouter ?? new RoutingState();

            this.WhenActivated((CompositeDisposable disposables) =>
            {
                var parameter = Locator.Current.GetService<TimeLogsViewModel>();
                //Router.Navigate.Execute(parameter);
            });
        }

        public RoutingState Router { get; }

        public ViewModelActivator Activator { get; } = new ViewModelActivator();
    }
}
