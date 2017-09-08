using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.Reactive.Testing;
using Omichron;
using Ploeh.AutoFixture.Xunit2;
using ReactiveUI;
using ReactiveUI.Testing;
using Tests.Helpers;
using Xunit;

namespace Tests
{
    public class TestThrottleAfter
    {
        [Theory, AutoData]
        public void WhenNoSequence_NothingObserved(TestScheduler scheduler, Subject<char> subject, long delay)
        {
            var source = scheduler.CreateColdObservable<int>();

            var actual = source
                .ThrottleAfter(TimeSpan.FromTicks(delay), scheduler)
                .CreateObserver(scheduler);

            scheduler.AdvanceTo(delay + 1);

            Assert.Empty(actual.Messages);
        }

        [Theory, AutoData]
        public void WhenSingleElement_ItIsObserved(TestScheduler scheduler, long delay, char c)
        {
            var expected = new[] { c };

            var source = scheduler.CreateColdObservable(
                Sequence.OnNext(1, c));

            var actual = source.AsObservable()
                .ThrottleAfter(TimeSpan.FromTicks(delay), scheduler)
                .CreateCollection();

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
            //scheduler.With(x =>
            //{
            //    var actualx = Observable
            //        .Interval(TimeSpan.FromTicks(10), x)
            //        .ThrottleAfter(TimeSpan.FromTicks(7), x)
            //        .CreateCollection(x);

            //    x.AdvanceTo(11);

            //    Assert.Equal(1, actualx.Count);
            //});

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
