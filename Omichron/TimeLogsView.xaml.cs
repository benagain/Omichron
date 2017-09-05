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
        ReactiveList<TimeLog> Logs { get; }
    }

    public class TimeLogsViewModel : ReactiveObject, ITimeLogsViewModel
    {
        private IScreen hostScreen;
        private ObservableAsPropertyHelper<ReactiveList<TimeLog>> logs;

        public TimeLogsViewModel(IScreen screen)
        {
            hostScreen = screen;

            //logs = Observable
            //    .Interval(TimeSpan.FromSeconds(3), RxApp.MainThreadScheduler)
            //    .StartWith(0)
            //    .Select(x => AnotherEvent())
            //    .ToProperty(this, x => x.Logs, new ReactiveList<TimeLog>());

            var otherLogs = new ReactiveList<TimeLog>();

            GLogs = new ListCollectionView(otherLogs);
            GLogs.GroupDescriptions.Add(new PropertyGroupDescription("IssueId"));

            var up = Observable.Interval(TimeSpan.FromSeconds(10), RxApp.MainThreadScheduler)
                .StartWith(0)
                .Select(x => AnotherEvent())
                .Subscribe(d =>
                {
                    otherLogs.Clear();
                    foreach (var x in d) otherLogs.Add(x);
                });
        }

        private static int id = 123;

        private ReactiveList<TimeLog> AnotherEvent()
        {
            var current = logs?.Value ?? new ReactiveList<TimeLog>();
            var news = Enumerable
                .Range(0, current.Any() ? 1 : 3)
                .Select(_ => new TimeLog($"midas-{id++}", DateTime.UtcNow, TimeSpan.FromHours(3)));

            var l = new ReactiveList<TimeLog>(current);
            l.AddRange(news);
            return l;
        }

        public ReactiveList<TimeLog> Logs => logs.Value;

        public ListCollectionView GLogs { get; }

        string IRoutableViewModel.UrlPathSegment => "timelog";

        IScreen IRoutableViewModel.HostScreen => hostScreen;

    }
}
