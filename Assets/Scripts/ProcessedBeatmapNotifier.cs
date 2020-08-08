using System.Collections.Generic;

public static class ProcessedBeatmapNotifier
{
    public static void SendNotification(string message)
    {
        GetNotifications.Add(message);

        if (GetNotifications.Count > 20)
        {
            GetNotifications.RemoveAt(0);
        }
    }

    public static List<string> GetNotifications { get; } = new List<string>();
}