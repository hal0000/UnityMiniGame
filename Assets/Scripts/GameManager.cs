using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public BaseScene CurrentScene;
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        ServiceLocator.Register(Instance);
        var playerData = SaveSystem.Load();
        ServiceLocator.Register(playerData);
        var currencyManager = new CurrencyManager(ServiceLocator.Get<PlayerModel>());
        ServiceLocator.Register(currencyManager);
    }
}