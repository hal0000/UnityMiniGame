using UnityEngine;

public class BaseScene : MonoBehaviour
{
    public SaveSystem.PlayerData PlayerData;

    public virtual void Awake()
    {
        PlayerData = SaveSystem.Load();
    }

    public virtual void OnEnable()
    {
    }

    public virtual void Start()
    {
    }

    public virtual void OnDisable()
    {
    }

    public virtual void OnDestroy()
    {
    }
}