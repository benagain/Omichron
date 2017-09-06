using System;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ReactiveUI;

namespace Omichron
{
    /// <summary>
    /// Interaction logic for TimeLogsView.xaml
    /// </summary>
    public partial class TimeLogsView : UserControl, IViewFor<ITimeLogsViewModel>
    {
        public TimeLogsView()
        {
            InitializeComponent();

            this.WhenAnyValue(x => x.ViewModel).BindTo(this, x => x.DataContext);
        }

        public ITimeLogsViewModel ViewModel
        {
            get { return (ITimeLogsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(ITimeLogsViewModel), typeof(TimeLogsView), new PropertyMetadata(null));

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (ITimeLogsViewModel)value; }
        }
    }

    public interface ITimeLogsViewModel : IRoutableViewModel
    {
        IReactiveDerivedList<TimeLog> Logs { get; }
    }

    public class TimeLogsViewModel : ReactiveObject, ITimeLogsViewModel
    {
        private IScreen hostScreen;

        public TimeLogsViewModel(IScreen screen)
        {
            hostScreen = screen;

            Logs = Observable.Interval(TimeSpan.FromSeconds(10), RxApp.MainThreadScheduler)
                .StartWith(-1)
                .SelectMany(AnotherEvent)
                .CreateCollection(RxApp.MainThreadScheduler);

            CollectionViewSource
                .GetDefaultView(Logs)
                .GroupDescriptions
                .Add(new PropertyGroupDescription(nameof(TimeLog.IssueId)));
        }

        private static int id = 123;

        private IObservable<TimeLog> AnotherEvent(long filter)
        {
            var count = filter == -1 ? 2 : 1;
            return Enumerable
                .Range(0, count)
                .Select(_ => new TimeLog($"midas-{id++}", DateTime.UtcNow, TimeSpan.FromHours(3)))
                .ToObservable();
        }

        public IReactiveDerivedList<TimeLog> Logs { get; }

        string IRoutableViewModel.UrlPathSegment => "timelog";

        IScreen IRoutableViewModel.HostScreen => hostScreen;

    }
}
