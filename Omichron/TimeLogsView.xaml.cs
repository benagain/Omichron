using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Concurrency;
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

            var periodic = Observable.Interval(TimeSpan.FromSeconds(10), RxApp.MainThreadScheduler);

            var updateOnWindowFocus = Observable.FromEventPattern(
                    h => Application.Current.Activated += h,
                    h => Application.Current.Activated -= h)
                .Do(_ => Debug.WriteLine("Application activated!"))
                .Select(_ => 1L);

            Logs = periodic
                .Merge(updateOnWindowFocus)
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

    public static class ObservableExtensions
    {
        /// <summary>
        /// Ignores elements of an observable sequence that follow other elements within a specified relative time duration.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Sequence to throttle.</param>
        /// <param name="sampleDuration">Throttling </param>
        /// <returns>The throttled sequence.</returns>
        /// <exception cref="ArgumentNullException">source is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">sampleDuration is less than TimeSpan.Zero.</exception>
        /// <remarks>This operator throttles the source sequence by creating a <see cref="Window"/> over the sequence
        /// and taking only the first element in the window.  As the element is produced immediately, even sequences
        /// with gaps equal or larger than <paramref name="sampleDuration"/> between elements will produce the element.
        /// This is unlike <see cref="Throttle"/> which does not produce any elements in that case.
        /// </remarks>
        public static IObservable<T> ThrottleAfter<T>(this IObservable<T> source, TimeSpan sampleDuration)
        {
            return source
                .Window(() => Observable.Interval(sampleDuration))
                .SelectMany(x => x.Take(1));
        }

        public static IObservable<T> ThrottleAfter<T>(this IObservable<T> source, TimeSpan sampleDuration, IScheduler scheduler)
        {
            return source
                .Window(() => Observable.Interval(sampleDuration, scheduler))
                .SelectMany(x => x.Take(1));
        }
    }
}
