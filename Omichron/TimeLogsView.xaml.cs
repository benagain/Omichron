using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
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

            var appEvents = new ApplicationEvents(Application.Current);

            this.WhenActivated(disposables =>
            {
                this.WhenAnyValue(x => x.ViewModel)
                    .BindTo(this, x => x.DataContext)
                    .AddTo(disposables);
            });
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

namespace System.Reactive.Disposables
{
}
