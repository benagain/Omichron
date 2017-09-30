using FluentAssertions;
using Microsoft.Reactive.Testing;
using NSubstitute;
using Omichron;
using Omichron.Services;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit2;
using ReactiveUI;
using ReactiveUI.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using Xunit;

namespace Tests
{
    namespace TimeLogsViewModelTests
    {
        public class TestTimeLogsViewModel
        {
            // This isn't apparently needed, leaving it for a while just in case!
            //public TestTimeLogsViewModel()
            //{
            //    var x = RxApp.MainThreadScheduler;
            //    //RxApp.MainThreadScheduler = System.Reactive.Concurrency.ImmediateScheduler.Instance;
            //}

            [Theory, AutoMockData]
            public void Logs_are_empty_after_construction(TimeLogsViewModel sut, [Frozen] List<TimeLog> logs)
            {
                ((ISupportsActivation)sut).Activator.Activate();
                sut.Logs.Should().BeEmpty();
            }

			[Theory, AutoMockData]
			public void Logs_are_retrieved_when_search_is_executed([Frozen] List<TimeLog> expected, Generator<TimeLogsViewModel> sutgen)
			{
				new TestScheduler().With(sched =>
				{
					var sut = sutgen.First();
					// Given
					((ISupportsActivation)sut).Activator.Activate();
					// When
					sut.ExecuteSearch.Execute().Subscribe();
					sched.AdvanceByMs(1);
					// Then
					sut.Logs.Should().BeEquivalentTo(expected);
				});
			}

			[Theory, AutoMockData]
            public void Logs_are_retrieved_when_application_is_activated(
				[Frozen] IApplicationEvents events, 
				Generator<TimeLogsViewModel> sutgen, 
				[Frozen] List<TimeLog> expected)
            {
                new TestScheduler().With(sched =>
                {
					var sut = sutgen.First();
                    // Given
                    events.Activated.Returns(x => Observable.Return(new EventArgs()));
                    // When
                    ((ISupportsActivation)sut).Activator.Activate();
					sched.AdvanceByMs(1);

					// Then
					sut.Logs.Should().BeEquivalentTo(expected);
                });
            }

            [Theory, AutoMockData]
            public void Logs_are_restricted_to_after_start_date([Frozen] IApplicationEvents events, TimeLogsViewModel sut, [Frozen] List<TimeLog> expected, TimeLogSource api)
            {
            }
        }
    }
}
