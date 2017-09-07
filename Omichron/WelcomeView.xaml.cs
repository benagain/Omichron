using ReactiveUI;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Omichron
{
    /// <summary>
    /// Interaction logic for Issue.xaml
    /// </summary>
    public partial class WelcomeView : UserControl, IViewFor<IWelcomeViewModel>
    {
        public WelcomeView()
        {
            InitializeComponent();

            this.WhenAnyValue(x => x.ViewModel).BindTo(this, x => x.DataContext);

            ReactiveUI.Legacy.UserError.RegisterHandler(async error =>
            {
                RxApp.MainThreadScheduler.Schedule(error,
                (scheduler, userError) =>
                {
                    // NOTE: this code really shouldn't throw away the MessageBoxResult
                    var result = MessageBox.Show(userError.ErrorMessage);
                    return Disposable.Empty;
                });

                return await Task.Run(() => {
                    return ReactiveUI.Legacy.RecoveryOptionResult.CancelOperation;
                });
            });
        }

        public IWelcomeViewModel ViewModel
        {
            get { return (IWelcomeViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(IWelcomeViewModel), typeof(WelcomeView), new PropertyMetadata(null));

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (IWelcomeViewModel)value; }
        }
    }
}
