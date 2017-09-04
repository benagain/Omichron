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

            this.WhenActivated(d =>
            {
                d(this
                    .ViewModel
                    .Helloed
                    .RegisterHandler(async interraction =>
                    {
                        await Task.Run(() =>
                        {
                            var dialogResult = MessageBox.Show(interraction.Input);
                            interraction.SetOutput(dialogResult == MessageBoxResult.OK);
                        });
                    }));
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
