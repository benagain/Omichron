using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows.Data;
using ReactiveUI;
using Omichron.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Omichron
{
    public interface ITimeLogsViewModel : IRoutableViewModel
    {
        ReactiveList<TimeLog> Logs { get; }
        ReactiveCommand<Unit, Unit> WindowActivated { get; }
        ReactiveCommand<Unit, List<TimeLog>> ExecuteSearch { get; }
    }

    public class TimeLogsViewModel : ReactiveObject, ITimeLogsViewModel
    {
        private IScreen hostScreen;
        private TimeLogSource source;

        public TimeLogsViewModel(IScreen screen, TimeLogSource source)
        {
            hostScreen = screen;
            this.source = source;

            ExecuteSearch = ReactiveCommand.CreateFromTask<Unit, List<TimeLog>>(
              searchTerm => source.Search()
            );

            WindowActivated = ReactiveCommand.Create(
                () => { ExecuteSearch.Execute(); }
            );

            ExecuteSearch.CreateCollection();
            ExecuteSearch.Subscribe(x =>
            {
                Logs.Clear();
                foreach (var y in x) Logs.Add(y);
            });

            CollectionViewSource
                .GetDefaultView(Logs)
                .GroupDescriptions
                .Add(new PropertyGroupDescription(nameof(TimeLog.IssueId)));
        }

        public ReactiveCommand<Unit, Unit> WindowActivated { get; }

        public ReactiveCommand<Unit, List<TimeLog>> ExecuteSearch { get; }

        public ReactiveList<TimeLog> Logs { get; } = new ReactiveList<TimeLog>();

        string IRoutableViewModel.UrlPathSegment => "timelog";

        IScreen IRoutableViewModel.HostScreen => hostScreen;
    }
}
