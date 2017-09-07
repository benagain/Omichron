using System;
using Microsoft.Reactive.Testing;

namespace Tests.Helpers
{
    public static class ObservableExtensions
    {
        public static ITestableObserver<T> CreateObserver<T>(this IObservable<T> source, TestScheduler scheduler)
        {
            var subscription = scheduler.CreateObserver<T>();
            source.Subscribe(subscription);
            return subscription;
        }
    }
}
