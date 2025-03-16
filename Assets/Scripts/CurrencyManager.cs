using System;
using UnityEngine;

public class CurrencyManager
{
    private PlayerModel _playerData;
    public event Action<int> OnGoldUpdated;
    /// <summary>
    /// Constructor that takes the player data from the save system
    /// </summary>
    /// <param name="playerData"></param>
    public CurrencyManager(PlayerModel playerData)
    {
        _playerData = playerData;
    }

    /// <summary>
    /// Read-only property to access current gold
    /// </summary>
    public int Gold => _playerData.Gold;

    /// <summary>
    /// Method to spend gold and update the save system
    /// </summary>
    /// <param name="amount"></param>
    public void SpendGold(int amount)
    {
        if (_playerData.Gold >= amount)
        {
            _playerData.Gold -= amount;
            SaveSystem.Save(_playerData);
            // Notify subscribers about the new gold value
            OnGoldUpdated?.Invoke(_playerData.Gold);
        }
        else Debug.Log("Not enough gold!");
    }

    /// <summary>
    /// Method to add gold and update the save system
    /// </summary>
    /// <param name="amount"></param>
    public void EarnGold(int amount)
    {
        _playerData.Gold += amount;
        SaveSystem.Save(_playerData);
        OnGoldUpdated?.Invoke(_playerData.Gold);
    }
    /// <summary>
    /// Attempt to claim the daily bonus
    /// </summary>
    /// <returns></returns>
    public bool TryClaimDailyBonus()
    {
        // Check if the daily bonus can be claimed using the improved TimerUtility
        if (!TimerUtility.CanClaimDailyBonus(_playerData.LastClaimTimestamp, _playerData.LastVerifiedTimestamp)) return false;
        EarnGold(1);
        TimerUtility.ClaimDailyBonus(_playerData);
        return true;
    }
}