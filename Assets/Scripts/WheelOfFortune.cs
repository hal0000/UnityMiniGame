using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class WheelOfFortune : MonoBehaviour
{
    [SerializeField] private Transform wheelTransform;
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private Button buttonPrefab;
    [SerializeField] private TextMeshProUGUI[] _textsOnWheel;
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _selectedColor;
    [SerializeField] private Button spinButton;

    private int selectedNumber = -1;
    private bool isSpinning;
    private float[] segmentAngles = { -22.5f, 22.5f, 67.5f, 112.5f, 157.5f, 202.5f, 247.5f, 292.5f };
    private Button[] _buttons; 
    private int _spinCount;
    private MainMenu _menu;
    private Coroutine _co;
    private void Awake()
    {
        _menu = ServiceLocator.Get<GameManager>().CurrentScene as MainMenu;
        if (_menu == null)
        {
            Debug.LogError("MainMenu could not be found via the Service Locator.");
        }
        Init();
    }

    void Init()
    {
        _buttons = new Button[8];
        for (int i = 0; i < 8; i++)
        {
            var button = Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity, buttonContainer);
            if (button.transform.GetChild(0).TryGetComponent<TextMeshProUGUI>(out var temp))  temp.text = $"{i + 1}";
            int tempNumber = i + 1;
            button.onClick.AddListener(() =>
            {
                SelectNumber(tempNumber);
            });
            _buttons[i] = button;
        }
    }
    private void OnEnable()
    {
        _spinCount = _menu.PlayerData.WheelSpinCount;//incase of data deletion, i'd like to reset spincount
        selectedNumber = -1;
        ResetTextColors();
        EnableAllButtons();
        spinButton.interactable = false;
        resultText.text = "Please choose a number";
    }

    private void SelectNumber(int num)
    {
        selectedNumber = num;
        resultText.text = $"Selected: {num}";
        ResetTextColors();
        _textsOnWheel[num-1].color = _selectedColor;
        spinButton.interactable = true;
    }

    public void StartSpin()
    {
        if (isSpinning) return;
        if (_menu.PlayerData.Gold < 1)
        {
            resultText.text = "Not enough Gold!";
            return;
        }
        if (selectedNumber == -1)
        {
            resultText.text = "Please choose a number!";
            return;
        }
        resultText.text = "Spinning...";
        _menu.CurrencyManager.SpendGold(1);
        isSpinning = true;
        int selectedSegment = GetSpinResult();
        _co = StartCoroutine(SpinToSegment(selectedSegment));
    }

    private int GetSpinResult()
    {
        _menu.IncrementPlayerSpinCount();
        _spinCount++;
        bool isWin = false;
        if (_spinCount == 2 || _spinCount == 7 || _spinCount == 8)//rigged as requested
        {
            isWin = true;
        }
        else if (_spinCount > 10)
        {
            // Fixed 5% chance of winning after the 10th spin.
            isWin = Random.value < 0.05f;
        }
        return isWin ? (selectedNumber - 1) : RandomLoseSegment();
    }

    private int RandomLoseSegment()
    {
        int randSegment;
        do
        {
            randSegment = Random.Range(0, 8);
        } 
        while (randSegment == selectedNumber - 1);
        return randSegment;
    }

    private IEnumerator SpinToSegment(int segmentIndex)
    {
        DisableAllButtons();
        float targetAngle = segmentAngles[segmentIndex] + (360 * Random.Range(3, 5));
        float currentAngle = wheelTransform.rotation.eulerAngles.z;
        float duration = 3f;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float angle = Mathf.Lerp(currentAngle, targetAngle, elapsedTime / duration);
            wheelTransform.rotation = Quaternion.Euler(0f, 0f, angle);
            AdjustTextRotation(angle);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        wheelTransform.rotation = Quaternion.Euler(0f, 0f, targetAngle);
        AdjustTextRotation(targetAngle);
        if (segmentIndex == (selectedNumber - 1))
        {
            resultText.text = $"You WON! ({selectedNumber})";
            _menu.CurrencyManager.EarnGold(8);
        }
        else
        {
            resultText.text = "You LOST!";
        }
        isSpinning = false;
        selectedNumber = -1;
        spinButton.interactable = false;
        ResetTextColors();
        EnableAllButtons();
    }

    private void AdjustTextRotation(float angle)
    {
        for (int i = 0; i < _textsOnWheel.Length; i++) 
            _textsOnWheel[i].transform.localRotation = Quaternion.Euler(0f, 0f, -angle);

    }

    private void ResetTextColors()
    {
        for (int i = 0; i < _textsOnWheel.Length; i++)  
            _textsOnWheel[i].color = _defaultColor;
    }
    private void DisableAllButtons()
    {
        for (int i = 0; i < _buttons.Length; i++) 
            _buttons[i].interactable = false;
    }

    private void EnableAllButtons()
    {
        for (int i = 0; i < _buttons.Length; i++) 
            _buttons[i].interactable = true;
    }

    public void Close()
    {
        isSpinning = false;
        if (_co != null)
        {
            StopCoroutine(_co);
            _co = null;
        }
        ResetTextColors();
    }
}