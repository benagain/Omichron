using ReactiveUI;
using Splat;
using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Omichron
{
    public partial class MainWindow : Window, IViewFor<MainWindowViewModel>, IEnableLogger
    {
        public MainWindow()
        {
            InitializeComponent();

            ViewModel = Locator.Current.GetService<MainWindowViewModel>();

            this.WhenActivated(disposables =>
            {
                this.WhenAnyValue(x => x.ViewModel)
                    .BindTo(this, x => x.DataContext)
                    .AddTo(disposables);

                Observable.FromEventPattern<SelectionChangedEventHandler, SelectionChangedEventArgs>(
                    h => ViewType.SelectionChanged += h,
                    h => ViewType.SelectionChanged -= h)
                    .Select(x => x.EventArgs.AddedItems.Cast<ListBoxItem>().First().Content.ToString())
                    .LoggedCatch(this)
                    .BindTo(ViewModel, vm => vm.ViewType);                

            });
        }

        private void ViewType_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public AppBootstrapper Bootstrapper { get; } = new AppBootstrapper();

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(MainWindowViewModel), typeof(MainWindow), new PropertyMetadata(null));

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (MainWindowViewModel)value; }
        }

        public MainWindowViewModel ViewModel
        {
            get { return (MainWindowViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
    }
}
