using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.Reactive.Testing;
using Omichron;
using Ploeh.AutoFixture.Xunit2;
using ReactiveUI;
using Tests.Helpers;
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
        public void WhenTwoElementsInsideDuration_OnlyFirstIsObserved(TestScheduler scheduler, long delay, int observed, int ignored)
        {
            var source = scheduler.CreateColdObservable(
                Sequence.OnNext(1, observed),
                Sequence.OnNext(delay, ignored));

            var actual = source
                .ThrottleAfter(TimeSpan.FromTicks(delay), scheduler)
                .CreateObserver(scheduler);

            scheduler.AdvanceTo(delay + 1);

            actual.Messages.AssertEqual(
                Sequence.OnNext(1, observed));
        }

        [Theory, AutoData]
        public void WhenTwoElementsOutsideDuration_BothAreObserved(TestScheduler scheduler, long delay, int observed, int alsoObserved)
        {
            var source = scheduler.CreateColdObservable(
                Sequence.OnNext(1, observed),
                Sequence.OnNext(delay + 1, alsoObserved));

            var actual = source
                .ThrottleAfter(TimeSpan.FromTicks(delay), scheduler)
                .CreateObserver(scheduler);

            scheduler.AdvanceTo(delay + 1);

            actual.Messages.AssertEqual(
                Sequence.OnNext(1, observed),
                Sequence.OnNext(delay + 1, alsoObserved));
        }
    }
}
