using System;

public static class UnixTimeConverter
{
    public static DateTime FromUnixTimeSeconds(long unixTime)
    {
        // Unix epoch starts at 1970-01-01T00:00:00Z
        return DateTimeOffset.FromUnixTimeSeconds(unixTime).UtcDateTime;
    }

    public static long ToUnixTimeSeconds(DateTime dateTime)
    {
        // Convert DateTime to Unix timestamp
        return new DateTimeOffset(dateTime).ToUnixTimeSeconds();
    }
}
