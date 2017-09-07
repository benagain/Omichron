using System.Reactive;
using Microsoft.Reactive.Testing;

namespace Tests.Helpers
{
    public static class Sequence
    {
        public static Recorded<Notification<T>> OnNext<T>(long time, T value)
        {
            return new Recorded<Notification<T>>(time, Notification.CreateOnNext(value));
        }
    }
}
