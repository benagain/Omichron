using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Omichron;
using Microsoft.Reactive.Testing;
using Ploeh.AutoFixture.Xunit2;
using ReactiveUI;
using Xunit;

namespace Tests
{
    public class TestThrottleAfter
    {
        [Theory, AutoData]
        public void WhenNoSequence_NothingObserved(TestScheduler scheduler, Subject<char> subject, long delay)
        {
            var actual = subject.AsObservable()
                .ThrottleAfter(TimeSpan.FromMilliseconds(delay), scheduler)
                .CreateCollection();

            scheduler.AdvanceTo(delay + 1);

            Assert.Empty(actual);
        }

        [Theory, AutoData]
        public void WhenSingleElement_ItIsObserved(TestScheduler scheduler, Subject<char> subject, long delay, char c)
        {
            var expected = new[] { c };

            var actual = subject.AsObservable()
                .ThrottleAfter(TimeSpan.FromMilliseconds(delay), scheduler)
                .CreateCollection();

            scheduler.Schedule(TimeSpan.FromMilliseconds(0), () => subject.OnNext(c));
            scheduler.AdvanceTo(delay + 1);

            Assert.Equal(expected, actual);
        }

        [Theory, AutoData]
        public void WhenTwoElementsThatAreFarApart_BothAreObserved(TestScheduler scheduler, Subject<char> subject, long delay, char a, char b)
        {
            var expected = new[] { a, b };

            var actual = subject.AsObservable()
                .ThrottleAfter(TimeSpan.FromMilliseconds(delay), scheduler)
                .CreateCollection();

            scheduler.Schedule(TimeSpan.FromMilliseconds(0), () => subject.OnNext(a));
            scheduler.Schedule(TimeSpan.FromMilliseconds(delay + 1), () => subject.OnNext(a));
            scheduler.AdvanceTo(delay + 2);

            Assert.Equal(expected, actual);
        }
    }

    static class ScheduleExtensions
    {
        public static IDisposable Schedule<TAbsolute, TRelative, TState>(this VirtualTimeSchedulerBase<TAbsolute, TRelative> scheduler, TState state, Action action)
            where TAbsolute : IComparable<TAbsolute>
        {
            return scheduler.Schedule(state, (a, b) =>
            {
                action();
                return Disposable.Empty;
            });
        }
    }
}
