using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockingWait : MonoBehaviour
{
    [SerializeField] Slider sliderRef;
    [SerializeField] GameObject waitingScreen;
    [SerializeField] GameObject resultScreen;
    [SerializeField] Button claimButton;
    [SerializeField] int waitTime = 5;  

    /// <summary>
    /// // Event for claiming 5 seconds free 1 gold reward
    /// </summary>
    public event System.Action OnRewardClaimed; 

    void OnEnable()
    {
        waitingScreen.SetActive(true);
        resultScreen.SetActive(false);
        claimButton.onClick.AddListener(ClaimReward);
        StartCoroutine(StartWaiting());
    }

    IEnumerator StartWaiting()
    {
        float elapsedTime = 0f;
        while (elapsedTime < waitTime)
        {
            sliderRef.value = elapsedTime / waitTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        waitingScreen.SetActive(false);
        resultScreen.SetActive(true);
    }

    void ClaimReward()
    {
        gameObject.SetActive(false);
        OnRewardClaimed?.Invoke();
    }

    void OnDisable()
    {
        claimButton.onClick.RemoveListener(ClaimReward);
    }
}