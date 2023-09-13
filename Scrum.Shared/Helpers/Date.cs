using Google.Type;

namespace Scrum.Shared.Helpers
{
    public static class GoogleHelpers
    {
        public static DateOnly? ToDateOnly(this Google.Type.Date value)
        => (value.Year == 0 || value.Month == 0 || value.Day == 0)
            ? null
            : new(value.Year, value.Month, value.Day);
        public static Google.Type.Date FromDateOnly(this DateOnly? value)
           => value is null
            ? new() { Day = 0, Month = 0, Year = 0 }
            : new() { Day = value.Value.Day, Month = value.Value.Month, Year = value.Value.Year };
    }

    public class DateComparer : IComparer<Google.Type.Date>
    {
        public int Compare(Date? x, Date? y)
        {
            if (x == null && y == null)
            {
                return 0; 
            }
            else if (x == null)
            {
                return -1;
            }
            else if (y == null)
            {
                return 1; 
            }
            else
            {
                // Compare based on Year, Month, and Day properties
                if (x.Year < y.Year)
                {
                    return -1;
                }
                else if (x.Year > y.Year)
                {
                    return 1;
                }
                else
                {
                    // Years are equal, compare months
                    if (x.Month < y.Month)
                    {
                        return -1;
                    }
                    else if (x.Month > y.Month)
                    {
                        return 1;
                    }
                    else
                    {
                        // Months are equal, compare days
                        if (x.Day < y.Day)
                        {
                            return -1;
                        }
                        else if (x.Day > y.Day)
                        {
                            return 1;
                        }
                        else
                        {
                            return 0; // Years, months, and days are equal.
                        }
                    }
                }
            }
        }
    }
}