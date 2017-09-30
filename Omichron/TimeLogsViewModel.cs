using Omichron.Services;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Data;
using System.ComponentModel;

namespace Omichron
{
    public interface ITimeLogsViewModel
    {
        ReactiveList<TimeLog> Logs { get; }
        ReactiveCommand<Unit, List<TimeLog>> ExecuteSearch { get; }
    }

    public class TimeLogsViewModel : ReactiveObject, ITimeLogsViewModel, IRoutableViewModel, ISupportsActivation
    {
        private IScreen hostScreen;
        private TimeLogSource source;

        public TimeLogsViewModel(IScreen screen, IApplicationEvents applicationEvents, TimeLogSource source)
        {
            hostScreen = screen;
            this.source = source;

            ExecuteSearch = ReactiveCommand.CreateFromTask<Unit, List<TimeLog>>(
              searchTerm => source.Search()
            );

            (this).WhenActivated(disposables =>
            {
                ExecuteSearch.AddTo(disposables);

                ExecuteSearch
                    .Subscribe(x => { DoExecuteSearch(x); })
                    .AddTo(disposables);

                applicationEvents
                    .Activated
                    .SelectMany(_ => ExecuteSearch.Execute())
                    .Subscribe()
                    .AddTo(disposables);
            });

            CollectionViewSource
                .GetDefaultView(Logs)
                .GroupDescriptions
                .Add(new PropertyGroupDescription(nameof(TimeLog.IssueId)));
        }

        private void DoExecuteSearch(List<TimeLog> x)
        {
            Logs.Clear();
            foreach (var y in x) Logs.Add(y);
        }

        public ReactiveCommand<Unit, List<TimeLog>> ExecuteSearch { get; }

        public ReactiveList<TimeLog> Logs { get; } = new ReactiveList<TimeLog>();

        string IRoutableViewModel.UrlPathSegment => "timelog";

        IScreen IRoutableViewModel.HostScreen => hostScreen;

        ViewModelActivator ISupportsActivation.Activator { get; } = new ViewModelActivator();
    }

    public class DesignTimeLogsViewModel : TimeLogsViewModel
    {
        public DesignTimeLogsViewModel()
            : base(null, null, null)
        {
            Logs.Add(new TimeLog("Issue-1", new DateTime(2017, 07, 12, 10, 15, 00), TimeSpan.FromMinutes(15)));
            Logs.Add(new TimeLog("Issue-1", new DateTime(2017, 07, 12, 09, 00, 00), TimeSpan.FromMinutes(75)));
            Logs.Add(new TimeLog("Issue-1", new DateTime(2017, 07, 12, 10, 30, 00), TimeSpan.FromMinutes(45)));
            Logs.Add(new TimeLog("Issue-2", new DateTime(2017, 07, 11, 09, 00, 00), TimeSpan.FromHours(4)));
            Logs.Add(new TimeLog("Issue-3", new DateTime(2017, 07, 11, 13, 00, 00), TimeSpan.FromHours(3)));
            Logs.Add(new TimeLog("Issue-3", new DateTime(2017, 07, 10, 08, 35, 00), TimeSpan.FromMinutes(15)));
        }
    }
}
