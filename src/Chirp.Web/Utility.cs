namespace Chirp.Web;

public class Utility
{
    public static string GetTimeStamp(double unixTimeStamp)
    {
        DateTime dateTime = new DateTime(1970, 1, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("yyyy/MM/dd HH:mm:ss");
    }
}