using Microsoft.Reactive.Testing;
using ReactiveUI.Testing;
using System;

namespace Tests.AutoFixture
{
	public class AutoTestScheduler : TestScheduler , IDisposable
	{
		private readonly IDisposable testSchedule;

		public AutoTestScheduler()
		{
			testSchedule = TestUtils.WithScheduler(this);
		}

		public void Dispose()
		{
			testSchedule.Dispose();
		}
	}
}
