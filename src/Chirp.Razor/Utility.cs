using System.Reflection;

public class Utility
{
    public static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

    public static Stream GetResourceStream(string resourcePath)
    {
        FileStream? stream = Assembly.GetExecutingAssembly().GetFile(resourcePath);

        if (stream == null)
        {
            throw new ArgumentException("Unable to find resource");
        }

        return stream;
    }
}