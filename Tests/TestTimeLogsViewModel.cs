using FluentAssertions;
using NSubstitute;
using Omichron;
using Omichron.Services;
using Ploeh.AutoFixture.Xunit2;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Windows;
using Xunit;

namespace Tests
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
        public void Logs_are_empty_after_construction(TimeLogsViewModel sut, List<TimeLog> logs)
        {
            var api = Substitute.For<TimeLogSource>();
            api.Search().Returns(logs);

            ((ISupportsActivation)sut).Activator.Activate();
            sut.Logs.Count.Should().Be(0);
        }

        [Theory, AutoMockData]
        public void Logs_are_retrieved_when_search_is_executed(TimeLogsViewModel sut, List<TimeLog> logs)
        {
            var api = Substitute.For<TimeLogSource>();
            api.Search().Returns(logs);

            ((ISupportsActivation)sut).Activator.Activate();
            sut.ExecuteSearch.Execute().Subscribe();
            Assert.Equal(logs.Count, sut.Logs.Count);
        }

        [Theory, AutoMockData]
        public void Logs_are_retrieved_when_application_is_activated([Frozen] IApplicationEvents events, TimeLogsViewModel sut, [Frozen] List<TimeLog> logs, TimeLogSource api)
        {
            // Given
            events.Activated.Returns(x => Observable.Return(new EventArgs()));
            // When
            ((ISupportsActivation)sut).Activator.Activate();
            // Then
            Assert.Equal(logs.Count, sut.Logs.Count);
        }
    }
}
