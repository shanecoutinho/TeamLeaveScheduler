using System.Text.Json;

namespace LeaveScheduler.API.Services;

public class PublicHolidayService
{
    private readonly HashSet<DateOnly> _holidays;

    // Constructor used in unit tests
    public PublicHolidayService(HashSet<DateOnly> holidays)
    {
        _holidays = holidays;
    }

    // Constructor used by the application
    public PublicHolidayService()
    {
        var filePath = Path.Combine(AppContext.BaseDirectory, "Data", "public_holidays.json");

        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);

            var dates = JsonSerializer.Deserialize<List<DateOnly>>(json);

            _holidays = dates != null
                ? new HashSet<DateOnly>(dates)
                : new HashSet<DateOnly>();
        }
        else
        {
            _holidays = new HashSet<DateOnly>();
        }
    }

    public bool IsPublicHoliday(DateOnly date)
    {
        return _holidays.Contains(date);
    }
}