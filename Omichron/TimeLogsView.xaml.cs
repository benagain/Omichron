using ReactiveUI;
using System;
using System.Windows;
using System.Windows.Controls;

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
        TimeLog[] Logs { get; set;  }
    }

    public class TimeLogsViewModel : ReactiveObject, ITimeLogsViewModel
    {
        private IScreen hostScreen;

        public TimeLogsViewModel(IScreen screen)
        {
            hostScreen = screen;
            Logs = new[]
            {
                new TimeLog("midas-123", DateTime.UtcNow, TimeSpan.FromHours(3)),
            };
        }

        public TimeLog[] Logs { get; set;  }

        string IRoutableViewModel.UrlPathSegment => "timelog";

        IScreen IRoutableViewModel.HostScreen => hostScreen;

    }
}
