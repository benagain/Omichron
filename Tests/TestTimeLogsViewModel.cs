using FluentAssertions;
using Microsoft.Reactive.Testing;
using NSubstitute;
using Omichron;
using Omichron.Services;
using ReactiveUI.Testing;
using System.Collections.Generic;
using Xunit;

namespace Tests
{

    public class TestTimeLogsViewModel
    {
        [Theory, AutoMockData]
        public void Logs_are_empty_after_construction(TimeLogsViewModel sut, List<TimeLog> logs)
        {
            var api = Substitute.For<TimeLogSource>();
            api.Search().Returns(logs);

            sut.Logs.Count.Should().Be(0);
        }

        [Theory, AutoMockData]
        public void Logs_are_retrieved_when_application_is_activated(TimeLogsViewModel sut, List<TimeLog> logs)
        {
            var api = Substitute.For<TimeLogSource>();
            api.Search().Returns(logs);

            new TestScheduler().With(x =>
            {
                //x.AdvanceBy(100);
                Assert.Equal(logs.Count, sut.Logs.Count);
            });
        }
    }
}
