using System.Windows;
using System.Windows.Controls;
using ReactiveUI;
using System.Reactive.Linq;
using System;

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

            //ReactiveUI.Events.System.Windows.ApplicationEvents
            new ApplicationEvents(Application.Current).Activated
                .Do(x => Console.WriteLine(x))
                //.InvokeCommand(ViewModel?.WindowActivated)
                .Subscribe(x => ViewModel?.WindowActivated?.Execute());
            //this.Events().MouseLeave.Subscribe(_ => Console.WriteLine("Bye!"));
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
}
