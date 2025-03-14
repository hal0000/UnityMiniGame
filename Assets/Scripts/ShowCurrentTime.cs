using UnityEngine;
using TMPro;

public class ShowCurrentTime : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI currentTimeLabel;

    private void Start()
    {
        InvokeRepeating(nameof(UpdateTime), 0f, 1f); // Update once per second
    }

    private void UpdateTime()
    {
        currentTimeLabel.text = TimerUtility.CurrentTime.ToString("F");
    }
}