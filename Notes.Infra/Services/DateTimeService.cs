using Notes.Application.Interfaces;

namespace Notes.Infra.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime Now => DateTime.UtcNow;
}
