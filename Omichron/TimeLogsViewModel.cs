﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Data;
using ReactiveUI;

namespace Omichron
{
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
}