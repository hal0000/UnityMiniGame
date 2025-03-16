using System;

[Serializable]
public class PlayerModel
{
    /// <summary>
    /// Default gold on first launch
    /// </summary>
    public int Gold = 5;
    /// <summary>
    /// UTC Timestamp of last claim
    /// </summary>
    public double LastClaimTimestamp;
    /// <summary>
    /// for tracking verified time
    /// </summary>
    public double LastVerifiedTimestamp;
    /// <summary>
    /// WheelOfFortune Spin count
    /// </summary>
    public int WheelSpinCount = 0;
}
