using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoNSubstitute;
using Ploeh.AutoFixture.Xunit2;

namespace Tests
{
    public class AutoMockDataAttribute : AutoDataAttribute
    {
        public AutoMockDataAttribute()
            : base(new Fixture()
                  .Customize(new AutoConfiguredNSubstituteCustomization())
                  .Customize(new AutoFixture.NodaTime.NodaTimeCustomization()))
        {
        }
    }
}
