namespace ClinicApp.Models;

public struct WorkSchedule
{
    public int StartHour { get; }
    public int EndHour { get; }

    public int HoursPerDay => EndHour - StartHour;
    public string Display => $"{StartHour:D2}:00–{EndHour:D2}:00";
    public bool IsNow => DateTime.Now.Hour >= StartHour && DateTime.Now.Hour < EndHour;

    public WorkSchedule(int startHour, int endHour)
    {
        if (startHour < 0 || startHour > 23)
        {
            throw new ArgumentOutOfRangeException(nameof(startHour), "Start hour must be between 0 and 23.");
        }

        if (endHour < 1 || endHour > 24)
        {
            throw new ArgumentOutOfRangeException(nameof(endHour), "End hour must be between 1 and 24.");
        }

        if (startHour >= endHour)
        {
            throw new ArgumentException("Start hour must be earlier than end hour.");
        }

        StartHour = startHour;
        EndHour = endHour;
    }

    public bool Contains(int hour)
    {
        return hour >= StartHour && hour < EndHour;
    }

    public override string ToString()
    {
        return Display;
    }
}
