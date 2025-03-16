using System;

public static class TimerUtility
{
    private static readonly DateTime firstDt = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
    public static DateTime CurrentTime => DateTime.UtcNow;
    public static bool CanClaimDailyBonus(double lastClaimTimeStamp, double lastVerifiedTimeStamp)
    {
        
        // For new players (no bonus claimed yet), allow immediate claim.
        if (lastClaimTimeStamp == 0 && lastVerifiedTimeStamp == 0)
        {
            return true;
        }
        // Convert the stored timestamps (last claim and last verified) to UTC DateTime
        DateTime lastClaim = ConvertTimestampToDateTime(lastClaimTimeStamp);
        DateTime lastVerified = ConvertTimestampToDateTime(lastVerifiedTimeStamp);

        // Define today's bonus claim threshold as 13:00 UTC
        DateTime todayClaimTime = new DateTime(CurrentTime.Year, CurrentTime.Month, CurrentTime.Day, 13, 0, 0, DateTimeKind.Utc);

        // SECURITY: Verify that the system's current UTC time is moving forward relative to the last verified time.
        // This helps detect if the user has manually rolled the clock back.
        if (CurrentTime <= lastVerified)
        {
            //Servercall or Punish as System time manipulation detected: current time is not ahead of the last verified time.
            return false;
        }

        // Check if the bonus was already claimed today.
        // Using '>=' ensures that a claim made exactly at 13:00 UTC is considered as already claimed.
        if (lastClaim >= todayClaimTime)
        {
            return false;
        }

        // Allow the bonus claim only if the current time has reached or passed today's threshold.
        return CurrentTime >= todayClaimTime;
    }
    public static void ClaimDailyBonus(PlayerModel playerData)
    {
        // Update both the claim and verified timestamps to the current UTC time
        double currentTimestamp = ConvertDateTimeToTimestamp(CurrentTime);
        playerData.LastClaimTimestamp = currentTimestamp;
        playerData.LastVerifiedTimestamp = currentTimestamp;
        SaveSystem.Save(playerData);
    }
    private static DateTime ConvertTimestampToDateTime(double timestamp) => firstDt.AddSeconds(timestamp);
    private static double ConvertDateTimeToTimestamp(DateTime dateTime) => (dateTime - firstDt).TotalSeconds;
}