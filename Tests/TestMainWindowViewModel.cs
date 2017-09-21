using FluentAssertions;
using Omichron;
using Ploeh.AutoFixture.Xunit2;
using ReactiveUI;
using Splat;
using System;
using Xunit;

namespace Tests
{
    public class TestMainWindowViewModel
    {
        [Theory, AutoMockData]
        public void Starts_with_list_view(DateTime bob, [Frozen] RoutingState router, MainWindowViewModel sut, TimeLogsViewModel expected)
        {
            Locator.CurrentMutable.RegisterConstant<TimeLogsViewModel>(expected);
            ((ISupportsActivation)sut).Activator.Activate();
            router.NavigationStack.Should().Equal(new[] { (IRoutableViewModel) expected });
        }

    }
}
