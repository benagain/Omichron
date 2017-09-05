using ReactiveUI;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Reactive.Linq;

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

            //this.ViewModel.Logs = new[]
            //////this.DataContext = new[]
            //{
            //    new TimeLog("midas-123", DateTime.UtcNow, TimeSpan.FromHours(3)),
            //};
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
        List<TimeLog> Logs { get; }
    }

    public class TimeLogsViewModel : ReactiveObject, ITimeLogsViewModel
    {
        private IScreen hostScreen;
        private ObservableAsPropertyHelper<List<TimeLog>> logs;

        public TimeLogsViewModel(IScreen screen)
        {
            hostScreen = screen;

            logs = Observable
                .Interval(TimeSpan.FromSeconds(3), RxApp.MainThreadScheduler)
                .StartWith(0)
                .Select(x => AnotherEvent())
                .ToProperty(this, x => x.Logs, new List<TimeLog>());
        }

        private static int id = 123;

        private List<TimeLog> AnotherEvent()
        {
            var current = logs?.Value ?? new List<TimeLog>();
            return new List<TimeLog>(current)
            {
                new TimeLog($"midas-{id++}", DateTime.UtcNow, TimeSpan.FromHours(3))
            };
        }

        public List<TimeLog> Logs => logs.Value;

        string IRoutableViewModel.UrlPathSegment => "timelog";

        IScreen IRoutableViewModel.HostScreen => hostScreen;

    }
}
