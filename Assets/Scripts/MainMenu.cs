using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu : BaseScene
{
    [SerializeField] BlockingWait blockingWaitPopup;
    [SerializeField] WheelOfFortune wheelOfFortune;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private Button dailyBonusButton;
    private Coroutine _dailyBonusCoroutine;
    private CurrencyManager _currencyManager;
    public CurrencyManager CurrencyManager => _currencyManager;
    public override void Awake()
    {
        base.Awake();
        GameManager.Instance.CurrentScene = this;
        blockingWaitPopup.OnRewardClaimed += HandleExtraCoin;
        _currencyManager = ServiceLocator.Get<CurrencyManager>();
        _currencyManager.OnGoldUpdated += UpdateGoldUI;
    }

    public override void Start()
    {
        base.Start();
        UpdateGoldUI(_currencyManager.Gold);
        _dailyBonusCoroutine = StartCoroutine(CheckDailyBonusAvailability());
    }

#region Button UI Actions

    public void Action_SpendOneCoin()
    {
        if (_currencyManager.Gold > 0)
        {
            _currencyManager.SpendGold(1);
        }
        else
        {
            Debug.Log("Not enough gold!");
        }
    }

    public void Action_GetExtraCoin()
    {
        Debug.Log("MainMenu:Action_GetExtraCoin");
        blockingWaitPopup.gameObject.SetActive(true);
    }
    public void Action_WheelOfFortune()
    {
        Debug.Log("MainMenu:Action_WheelOfFortune");
        wheelOfFortune.gameObject.SetActive(true);
    }
    public void Action_ClaimFreeCoin()
    {
        if (_currencyManager.TryClaimDailyBonus())
        {
            if (_dailyBonusCoroutine != null)
            {
                StopCoroutine(_dailyBonusCoroutine);
                _dailyBonusCoroutine = null;
            }
            _dailyBonusCoroutine = StartCoroutine(CheckDailyBonusAvailability());
        }
        else
        {
            Debug.Log("Daily bonus not available yet!");
        }
    }

    public void Action_DeleteUserData()
    {
        SaveSystem.DeleteSave();
        var playerData = SaveSystem.Load();
        ServiceLocator.Unregister<CurrencyManager>();
        _currencyManager = new CurrencyManager(playerData);
        ServiceLocator.Register(_currencyManager);
        _currencyManager.OnGoldUpdated += UpdateGoldUI;
        UpdateGoldUI(_currencyManager.Gold);
        PlayerData = playerData;
        SaveSystem.Save(playerData);
        if (_dailyBonusCoroutine != null)
        {
            StopCoroutine(_dailyBonusCoroutine);
        }
        _dailyBonusCoroutine = StartCoroutine(CheckDailyBonusAvailability());
        Debug.Log("User data deleted and CurrencyManager has been reset.");
    }
#endregion

    private void HandleExtraCoin()
    {
        _currencyManager.EarnGold(1);
    }
    
    private void UpdateGoldUI(int currentGold)
    {
        goldText.text = $"Gold: {currentGold}";
    }
    public void IncrementPlayerSpinCount()
    {
        PlayerData.WheelSpinCount++;
        SaveSystem.Save(PlayerData);
    }
    
    private IEnumerator CheckDailyBonusAvailability()
    {
        bool isAvailable = TimerUtility.CanClaimDailyBonus(PlayerData.LastClaimTimestamp, PlayerData.LastVerifiedTimestamp);
        SetDailyBonusButtonAttributes(isAvailable);
        if (isAvailable) yield break;
        DateTime now = TimerUtility.CurrentTime;
        DateTime todayClaimTime = new DateTime(now.Year, now.Month, now.Day, 13, 0, 0, DateTimeKind.Utc);
        if (now >= todayClaimTime)
        {
            todayClaimTime = todayClaimTime.AddDays(1);
        }
        double waitTime = (todayClaimTime - now).TotalSeconds;
        yield return new WaitForSeconds((float)waitTime + 10);//+10 for buffer just incase
        SetDailyBonusButtonAttributes(true);
    }

    void SetDailyBonusButtonAttributes(bool value)
    {
        dailyBonusButton.interactable = value;
        if (dailyBonusButton.transform.GetChild(0).TryGetComponent<TextMeshProUGUI>(out var temp))
        {
             temp.text = value ? "Daily Bonus Ready" : "Daily Bonus Not Ready";
        }
    }
    public override void OnDisable()
    {
        base.OnDisable();
        if (_currencyManager != null)
        {
            _currencyManager.OnGoldUpdated -= UpdateGoldUI;
        }
        if (_dailyBonusCoroutine != null)
        {
            StopCoroutine(_dailyBonusCoroutine);
        }
        blockingWaitPopup.OnRewardClaimed -= HandleExtraCoin;
    }
}