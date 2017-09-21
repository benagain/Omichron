using NodaTime;
using NodaTime.Extensions;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using System;

namespace AutoFixture.NodaTime
{
    public class LocalDateTimeFromDateTime : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (!request.Equals(typeof(LocalDateTime)))
                return new NoSpecimen();

            var dateTime = context.Create<DateTime>();
            return dateTime.ToLocalDateTime();
        }
    }
}

