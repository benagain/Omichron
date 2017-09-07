using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Omichron
{
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
