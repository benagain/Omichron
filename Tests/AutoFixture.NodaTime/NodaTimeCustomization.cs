using Ploeh.AutoFixture;

namespace AutoFixture.NodaTime
{
    public class NodaTimeCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(new LocalDateTimeFromDateTime());
        }
    }
}
