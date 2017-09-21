using NodaTime;
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
                this.WhenAnyValue(x => x.StartDate)
                    .Do(x => Console.WriteLine($"Start from {x.Date}"))
                    .Subscribe();

                var parameter = Locator.Current.GetService<TimeLogsViewModel>();
                Router.Navigate.Execute(parameter);
            });
        }

        public RoutingState Router { get; }

        LocalDateTime startDate;
        public LocalDateTime StartDate
        {
            get { return startDate; }
            set { this.RaiseAndSetIfChanged(ref startDate, value); }
        }

    public ViewModelActivator Activator { get; } = new ViewModelActivator();
    }
}
