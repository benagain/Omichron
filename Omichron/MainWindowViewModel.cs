using ReactiveUI;
using Splat;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Omichron
{
    public class MainWindowViewModel : ReactiveObject, IScreen, ISupportsActivation
    {
        public MainWindowViewModel(RoutingState testRouter = null)
        {
            Router = testRouter ?? new RoutingState();

            this.WhenActivated((CompositeDisposable disposables) =>
            {
                this.WhenAnyValue(x => x.ViewType)
                    .Do(x => Console.WriteLine($"{x} selected"))
                    .Subscribe();

                var parameter = Locator.Current.GetService<TimeLogsViewModel>();
                Router.Navigate.Execute(parameter);
            });
        }

        public RoutingState Router { get; }

        string viewType;
        public string ViewType
        {
            get { return viewType; }
            set { this.RaiseAndSetIfChanged(ref viewType, value); }
        }


        public ViewModelActivator Activator { get; } = new ViewModelActivator();
    }
}
