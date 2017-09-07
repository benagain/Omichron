using System;
using System.Reactive;
using Microsoft.Reactive.Testing;

namespace Tests.Helpers
{
    public static class Sequence
    {
        public static Recorded<Notification<T>> OnNext<T>(long time, T value)
            => new Recorded<Notification<T>>(time, Notification.CreateOnNext(value));

        public static Recorded<Notification<T>> OnCompleted<T>(long time)
            => new Recorded<Notification<T>>(time, Notification.CreateOnCompleted<T>());

        public static Recorded<Notification<T>> OnError<T>(long time, Exception error)
            => new Recorded<Notification<T>>(time, Notification.CreateOnError<T>(error));
    }
}
